using UnityEngine;

namespace Down2Jam.Manager
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { private set; get; }

        public bool IsActive { private set; get; }
        public float Timer { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            Timer += Time.deltaTime;
            if (Timer > 10f)
            {
                Timer = 10f;
                IsActive = false;
            }
        }

        public void StartTimer()
        {
            IsActive = true;
            Timer = 0f;
        }
    }
}
