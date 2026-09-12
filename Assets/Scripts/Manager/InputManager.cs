using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { private set; get; }

        public Vector2 Mov { private set; get; }

        private bool _isReadyToStart = true;

        private void Awake()
        {
            Instance = this;
        }

        public void ResetMov()
        {
            Mov = Vector2.zero;
            _isReadyToStart = false;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            Mov = value.ReadValue<Vector2>();

            if (Mov.magnitude > 0f)
            {
                if (!VNManager.Instance.IsPlayingStory && _isReadyToStart && !TimerManager.Instance.IsActive)
                {
                    TimerManager.Instance.StartTimer();
                }
            }
            else
            {
                _isReadyToStart = true;
            }
        }

        public void OnClick(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && VNManager.Instance.IsPlayingStory)
            {
                VNManager.Instance.DisplayNextDialogue();
            }
        }

        public void OnRetry(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started) SceneManager.LoadScene("Main");
        }
    }
}
