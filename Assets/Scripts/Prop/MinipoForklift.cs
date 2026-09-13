using System;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class MinipoForklift : ForkliftController
    {
        protected override void Update()
        {
            base.Update();

            float maxDist = -1f;

            for (var i = 0f; i < 2f * MathF.PI; i += MathF.PI / 20f)
            {
                Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(i), Mathf.Sin(i)), float.MaxValue, LayerMask.GetMask("Map"));
            }
        }
    }
}
