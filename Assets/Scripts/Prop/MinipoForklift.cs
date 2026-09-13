using Down2Jam.Manager;
using Down2Jam.SO;
using System;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class MinipoForklift : ForkliftController
    {
        private CargoType _aiCargo;
        private Vector2 _basePos;

        protected override void Awake()
        {
            base.Awake();

            _basePos = transform.position;

            AssignedOrder = new()
            {
                Input = -1,
                Output = -1,
                Cargo = _aiCargo
            };
        }

        protected override void Update()
        {
            base.Update();

            if (!TimerManager.Instance.IsActive) return;

            float maxDist = -1f;
            float bestAngle = 0f;

            for (var angle = -Mathf.PI / 4; angle <= Mathf.PI / 4; angle += MathF.PI / 20f)
            {
                var finalAngle = angle + Mathf.Atan2(transform.up.y, transform.up.x);
                var hit = Physics2D.CircleCast(transform.position, .3f, new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle)), float.MaxValue, LayerMask.GetMask("Map"));

                if (hit.distance > maxDist)
                {
                    maxDist = hit.distance;
                    bestAngle = angle;
                }
            }

            if (bestAngle == 0f)
            {
                _rb.angularVelocity = 0f;
            }
            else if (bestAngle < 0f)
            {
                _rb.angularVelocity = -AngularSpeed;
            }
            else if (bestAngle > 0f)
            {
                _rb.angularVelocity = AngularSpeed;
            }
            _rb.linearVelocity = transform.up * LinearSpeed;
        }

        public override void ReceiveRawInput(Vector2 mov)
        { } // We do nothing here

        public override void ReceiveInput(Vector2 mov, bool isAdjustement)
        { } // We do nothing here

        public override void Stop()
        {
            base.Stop();

            transform.position = _basePos;
        }

        private void OnDrawGizmos()
        {
            for (var angle = - Mathf.PI / 4; angle <= Mathf.PI / 4; angle += MathF.PI / 20f)
            {
                Gizmos.color = Color.blue;
                var finalAngle = angle + Mathf.Atan2(transform.up.y, transform.up.x);
                var hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle)), float.MaxValue, LayerMask.GetMask("Map"));
                Gizmos.DrawLine(transform.position, hit.point);
                Gizmos.color = Color.red;
                hit = Physics2D.CircleCast(transform.position, .3f, new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle)), float.MaxValue, LayerMask.GetMask("Map"));
                Gizmos.DrawLine(transform.position, hit.point);
            }
        }
    }
}
