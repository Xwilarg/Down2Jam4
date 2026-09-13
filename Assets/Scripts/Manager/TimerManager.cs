using TMPro;
using UnityEngine;

namespace Down2Jam.Manager
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { private set; get; }

        private const float TimerMax = 10f;

        [SerializeField]
        private TMP_Text _timerText;

        public bool IsActive { private set; get; }
        public float Timer { private set; get; }

        public bool DidTimerExpired { private set; get; }

        private void Awake()
        {
            Instance = this;

            _timerText.text = TimerMax.ToString();
        }

        private void Update()
        {
            if (IsActive)
            {
                Timer += Time.deltaTime;
                if (Timer > TimerMax)
                {
                    Timer = TimerMax;
                    DidTimerExpired = true;
                    IsActive = false;
                }
                _timerText.text = $"{TimerMax - Timer:00}";
            }
        }

        public void StartTimer()
        {
            IsActive = true;
            Timer = 0f;
        }

        public void StopTimer()
        {
            DidTimerExpired = false;
            IsActive = false;
            _timerText.text = TimerMax.ToString();
        }
    }
}
