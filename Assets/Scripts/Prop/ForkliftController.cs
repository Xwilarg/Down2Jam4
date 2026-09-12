using Down2Jam.Manager;
using NsfwDelivery.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class ForkliftController : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<CargoType, Sprite> _cargos;

        [SerializeField]
        private Sprite _forkliftCurrent, _forkliftAI;

        private const float LinearSpeed = 5f;
        private const float AngularSpeed = 200f;
        private const float ExplosionForce = 5f;
        private const float ExplosionRange = 2f;

        private readonly List<InputInfo> _inputs = new();
        private Rigidbody2D _rb;
        [SerializeField]
        private SpriteRenderer _sr, _cargoSr;

        private Vector2 _mov;
        private bool _skipAdjustements;

        private bool _isExploded;

        private OrderInfo _assignedOrder;
        public Output TargetOutput { private set; get; }
        public OrderInfo AssignedOrder
        {
            set
            {
                _assignedOrder = value;
                TargetOutput = ObjectiveManager.Instance.CurrentOutput;
                TargetOutput.Shrink();

                _cargoSr.sprite = _cargos[value.Cargo];
            }
            get => _assignedOrder;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr.sprite = _forkliftCurrent;
        }

        private void Update()
        {
            if (_isExploded) return;

            _rb.linearVelocity = transform.up * _mov.y * LinearSpeed;
        }

        public void TryActAI(float timer)
        {
            if (_isExploded) return;

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

        public void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;

            ReceiveInput(Vector2.zero, false);
            TargetOutput.Grow();
            TargetOutput.Grow();
            _sr.color = Color.white;
            _skipAdjustements = false;
            TargetOutput.IsDeprecated = true;
            _sr.sprite = _forkliftAI;

            TargetOutput.Clear();
        }

        public void ReceiveInput(Vector2 mov, bool isAdjustement)
        {
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
            _isExploded = true;

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
                _skipAdjustements = true;
            }

            if (AssignedOrder.Cargo == CargoType.Explosive && !_isExploded)
            {
                _skipAdjustements = true;

                var contact = collision.contacts[0].point;
                foreach (var fl in Physics2D.OverlapCircleAll(contact, ExplosionRange, LayerMask.GetMask("Forklift")))
                {
                    var controller = fl.GetComponent<ForkliftController>();
                    controller.Explode(((Vector2)controller.transform.position).normalized - contact);
                }
            }
        }
    }
}
