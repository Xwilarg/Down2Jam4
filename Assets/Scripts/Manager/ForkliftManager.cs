using Down2Jam.Prop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Down2Jam.Manager
{
    public class ForkliftManager : MonoBehaviour
    {
        public static ForkliftManager Instance { private set; get; }

        private ForkliftController _currentForklift;
        private Vector2 _lastRecordedMove;

        private readonly List<ForkliftController> _oldForklifts = new();

        [SerializeField]
        private GameObject _forkliftPrefab;

        private float _refTimer;

        private float _totalTime;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (TimerManager.Instance.IsActive)
            {
                if (_lastRecordedMove != InputManager.Instance.Mov)
                {
                    _lastRecordedMove = InputManager.Instance.Mov;
                    _currentForklift.ReceiveInput(_lastRecordedMove, false);
                }
                else
                {
                    var newTimer = TimerManager.Instance.Timer;
                    if (newTimer - _refTimer > .05f)
                    {
                        _currentForklift.ReceiveInput(_lastRecordedMove, true);
                        _refTimer = newTimer;
                    }
                }

                foreach (var fl in _oldForklifts)
                {
                    fl.TryActAI(TimerManager.Instance.Timer);
                }
            }

            if (TimerManager.Instance.DidTimerExpired ||
                (TimerManager.Instance.IsActive && ObjectiveManager.Instance.CurrentOutput.IsInside && _oldForklifts.All(x => x.TargetOutput.IsInside)))
            {
                _currentForklift.Stop();
                foreach (var fl in _oldForklifts) fl.Stop();

                TimerManager.Instance.StopTimer();
                InputManager.Instance.ResetMov();
                _oldForklifts.Add(_currentForklift);
                _lastRecordedMove = Vector2.zero;

                _totalTime += TimerManager.Instance.Timer;

                if (ObjectiveManager.Instance.IsLastOrder)
                {
                    VictoryManager.Instance.ShowVictory(_totalTime, _oldForklifts.Count(x => x.TargetOutput.IsInside), _oldForklifts.Count);
                }
                else
                {
                    ObjectiveManager.Instance.FulfillOrder();

                    SpawnForklifts();
                }
            }
        }

        public void SpawnForklifts()
        {
            var go = Instantiate(_forkliftPrefab, transform.position, Quaternion.identity);
            go.transform.position = ObjectiveManager.Instance.CurrentInput.position;
            _currentForklift = go.GetComponent<ForkliftController>();
            _currentForklift.AssignedOrder = ObjectiveManager.Instance.CurrentOrder;

            foreach (var fl in _oldForklifts)
            {
                fl.transform.position = ObjectiveManager.Instance.GetInput(fl.AssignedOrder).transform.position;
                fl.transform.rotation = Quaternion.identity;
            }
        }
    }
}
