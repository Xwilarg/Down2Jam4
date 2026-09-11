using UnityEngine;
using UnityEngine.InputSystem;

namespace Down2Jam.Manager
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { private set; get; }

        public Vector2 Mov { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Mov.magnitude > 0f)
            {
                if (!TimerManager.Instance.IsActive)
                {
                    TimerManager.Instance.StartTimer();
                }
            }
        }

        public void ResetMov()
        {
            Mov = Vector2.zero;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            Mov = value.ReadValue<Vector2>();
        }
    }
}
