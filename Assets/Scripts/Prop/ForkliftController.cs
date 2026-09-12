using Down2Jam.Manager;
using NsfwDelivery.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class ForkliftController : MonoBehaviour
    {
        private const float LinearSpeed = 5f;
        private const float AngularSpeed = 200f;

        private readonly List<InputInfo> _inputs = new();
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;

        private Vector2 _mov;
        private bool _skipAdjustements;

        private OrderInfo _assignedOrder;
        public Output TargetOutput { private set; get; }
        public OrderInfo AssignedOrder
        {
            set
            {
                _assignedOrder = value;
                TargetOutput = ObjectiveManager.Instance.CurrentOutput;
                TargetOutput.Shrink();
            }
            get => _assignedOrder;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            _rb.linearVelocity = transform.up * _mov.y * LinearSpeed;
        }

        public void TryActAI(float timer)
        {
            var inputTarget = _inputs.Where(x => !_skipAdjustements || !x.IsAdjustement).LastOrDefault(x => timer >= x.Timer);
            if (inputTarget == null) return;

            _mov = inputTarget.Movement;
            _rb.angularVelocity = _mov.x * -AngularSpeed;
        }

        public void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
            ReceiveInput(Vector2.zero, false);
            TargetOutput.Grow();
            TargetOutput.Grow();
            _sr.color = Color.white;
            _skipAdjustements = false;
            TargetOutput.IsDeprecated = true;
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
            _rb.angularVelocity = mov.x * -AngularSpeed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Forklift"))
            {
                _skipAdjustements = true;
            }
        }
    }
}
