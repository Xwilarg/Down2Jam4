using Down2Jam.Prop;
using UnityEngine;

namespace Down2Jam.Manager
{
    public class ForkliftManager : MonoBehaviour
    {
        public static ForkliftManager Instance { private set; get; }

        private ForkliftController _currentForklift;
        private Vector2 _lastRecordedMove;

        [SerializeField]
        private GameObject _forkliftPrefab;

        private void Awake()
        {
            Instance = this;

            SpawnForklift();
        }

        private void Update()
        {
            if (TimerManager.Instance.IsActive)
            {
                if (_lastRecordedMove != InputManager.Instance.Mov)
                {
                    _lastRecordedMove = InputManager.Instance.Mov;
                    _currentForklift.ReceiveInput(_lastRecordedMove);
                }
            }
        }

        public void SpawnForklift()
        {
            var go = Instantiate(_forkliftPrefab, transform.position, Quaternion.identity);
            _currentForklift = go.GetComponent<ForkliftController>();
        }
    }
}
