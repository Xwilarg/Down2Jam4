using UnityEngine;

namespace Down2Jam.Prop
{
    public record InputInfo
    {
        public float Timer;
        public Vector2 Movement;

        // For adjustement
        public bool IsAdjustement;
        public Vector2 Position;
        public float Rotation;
    }
}
