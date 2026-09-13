using Down2Jam.Manager;
using Down2Jam.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.U2D.Physics.PhysicsShape;

namespace Down2Jam.Prop
{
    public class ForkliftController : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<CargoType, Sprite> _cargos;

        [SerializeField]
        private Sprite _forkliftCurrent, _forkliftAI;

        [SerializeField]
        private bool _isMainMenu;

        protected const float LinearSpeed = 5f;
        protected const float AngularSpeed = 200f;
        protected const float ExplosionForce = 10f;
        protected const float ExplosionRange = 2f;

        private readonly List<InputInfo> _inputs = new();
        protected Rigidbody2D _rb;
        [SerializeField]
        private SpriteRenderer _sr, _cargoSr;

        private Vector2 _mov;
        private bool _skipAdjustements;

        public bool DidExplode { private set; get; }
        protected bool _isExploded;

        private float? _breakAdjustementTime;

        private OrderInfo _assignedOrder;
        public Output TargetOutput { private set; get; }
        public OrderInfo AssignedOrder
        {
            set
            {
                _assignedOrder = value;
                TargetOutput = ObjectiveManager.Instance.CurrentOutput;
                TargetOutput?.Shrink();

                _cargoSr.sprite = _cargos[value.Cargo];
            }
            get => _assignedOrder;
        }

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr.sprite = _forkliftCurrent;
        }

        protected virtual void Update()
        {
            if (!_isMainMenu && !TimerManager.Instance.IsActive) return;

            if (_isExploded) return;

            if (TargetOutput != null && TargetOutput.IsInside) ReceiveRawInput(Vector2.zero);
            _rb.linearVelocity = transform.up * _mov.y * LinearSpeed;
        }

        public void BreakAdjustement()
        {
            if (TargetOutput != null && _skipAdjustements && !TargetOutput.IsDeprecated && _breakAdjustementTime == null)
            {
                _breakAdjustementTime = TimerManager.Instance.Timer;
            }
        }

        public void TryActAI(float timer)
        {
            if (_isExploded || (TargetOutput != null && TargetOutput.IsInside)) return;

            if (_breakAdjustementTime != null && timer > _breakAdjustementTime.Value)
            {
                _skipAdjustements = true;
            }

            var inputTarget = _inputs.Where(x => !_skipAdjustements || !x.IsAdjustement).LastOrDefault(x => timer >= x.Timer);
            if (inputTarget == null) return;

            _mov = inputTarget.Movement;
            _rb.angularVelocity = _mov.x * -AngularSpeed;

            if (!_skipAdjustements)
            {
                var timeRef = inputTarget.Timer;
                var next = _inputs.Where(x => !_skipAdjustements || !x.IsAdjustement).FirstOrDefault(x => timer < x.Timer);

                if (next == null) return;

                var nextTimeRef = next.Timer;
                var delta = nextTimeRef - timeRef;

                var me = timer - timeRef;
                var me01 = me / delta;

                transform.SetPositionAndRotation(
                    Vector2.Lerp(inputTarget.Position, next.Position, me01),
                    Quaternion.Euler(0f, 0f, Mathf.LerpAngle(inputTarget.Rotation, next.Rotation, me01)));
            }
        }

        public virtual void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            if (TargetOutput != null)
            {
                if (!TargetOutput.IsDeprecated)
                {
                    TargetOutput.Grow();
                    //TargetOutput.Grow();
                }
                TargetOutput.IsDeprecated = true;
            }
            _sr.color = Color.white;
            _skipAdjustements = false;
            _sr.sprite = _forkliftAI;
            DidExplode = false;
        }

        public virtual void ReceiveRawInput(Vector2 mov)
        {
            _mov = mov;
            _rb.angularVelocity = mov.x * -AngularSpeed;
        }

        public virtual void ReceiveInput(Vector2 mov, bool isAdjustement)
        {
            if (_skipAdjustements && isAdjustement)
            {
                return;
            }

            _inputs.Add(new()
            {
                Movement = mov,
                Timer = TimerManager.Instance.Timer,
                IsAdjustement = isAdjustement,
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles.z
            });
            _mov = mov;
            
            if (_isExploded) return;
            _rb.angularVelocity = mov.x * -AngularSpeed;
        }

        private void Explode(Vector2 dir)
        {
            BreakAdjustement();
            _skipAdjustements = true;

            _isExploded = true;
            DidExplode = true;

            _rb.angularDamping = .2f;
            _rb.linearDamping = .2f;

            _rb.linearVelocity = dir * ExplosionForce;

            StartCoroutine(RecoverExplosion());
        }

        private IEnumerator RecoverExplosion()
        {
            yield return new WaitForSeconds(3f);
            _isExploded = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Forklift"))
            {
                BreakAdjustement();
                _skipAdjustements = true;
            }

            if (collision.collider.CompareTag("Train"))
            {
                Explode(((Vector2)transform.position - collision.contacts[0].point).normalized);
            }

            if (AssignedOrder.Cargo == CargoType.Explosive && !_isExploded)
            {
                var contact = collision.contacts[0].point;
                foreach (var fl in Physics2D.OverlapCircleAll(contact, ExplosionRange, LayerMask.GetMask("Forklift")))
                {
                    var controller = fl.GetComponent<ForkliftController>();
                    controller.Explode(((Vector2)controller.transform.position - contact).normalized);
                }
            }
        }
    }
}
