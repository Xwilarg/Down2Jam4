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

        private Vector2 _mov;

        public OrderInfo AssignedOrder { set; get; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rb.linearVelocity = transform.up * _mov.y * LinearSpeed;
        }

        public void TryActAI(float timer)
        {
            var inputTarget = _inputs.FirstOrDefault(x => x.Timer >= timer);
            if (inputTarget == null) return;

            _mov = inputTarget.Movement;
            _rb.angularVelocity = _mov.x * -AngularSpeed;
        }

        public void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
            ReceiveInput(Vector2.zero);
        }

        public void ReceiveInput(Vector2 mov)
        {
            _inputs.Add(new()
            {
                Movement = mov,
                Timer = TimerManager.Instance.Timer,
                Position = transform.position
            });
            _mov = mov;
            _rb.angularVelocity = mov.x * -AngularSpeed;
        }
    }
}
