using Down2Jam.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class ForkliftController : MonoBehaviour
    {
        private const float Speed = 5f;

        private List<InputInfo> _inputs = new();
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void ReceiveInput(Vector2 mov)
        {
            _inputs.Add(new()
            {
                Movement = mov,
                Timer = TimerManager.Instance.Timer,
                Position = transform.position
            });
            _rb.linearVelocity = mov * Speed;
        }
    }
}
