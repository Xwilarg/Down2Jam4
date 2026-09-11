using Down2Jam.Manager;
using System.Collections.Generic;
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

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rb.linearVelocity = transform.up * _mov.y * LinearSpeed;
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
