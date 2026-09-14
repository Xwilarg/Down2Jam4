using Assets.Scripts.Manager;
using Down2Jam.Prop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class ForkliftManager : MonoBehaviour
    {
        public static ForkliftManager Instance { private set; get; }

        private ForkliftController _currentForklift;
        private Vector2 _lastRecordedMove;

        private readonly List<ForkliftController> _oldForklifts = new();
        private MinipoForklift[] _aiForklifts;

        [SerializeField]
        private GameObject _forkliftPrefab;

        public Train Train { private set; get; }

        private float _refTimer;

        public bool GotMinipoTouch { set; get; }

        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("GlobalUI", LoadSceneMode.Additive);
        }

        public void LoadMinipos()
        {
            _aiForklifts = GameObject.FindObjectsByType<MinipoForklift>();
        }

        public void LoadTrain(Train train)
        {
            Train = train;
        }

        private void Update()
        {
            if (TimerManager.Instance.IsActive)
            {
                if (_currentForklift != null)
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
                }

                foreach (var fl in _oldForklifts)
                {
                    fl.TryActAI(TimerManager.Instance.Timer);
                }
            }

            var requireTimerRestart = false;

            if (TimerManager.Instance.DidTimerExpired ||
                (TimerManager.Instance.IsActive && ObjectiveManager.Instance.CurrentOutput.IsInside && _oldForklifts.All(x => x.TargetOutput.IsInside)))
            {
                TimerManager.Instance.StopTimer();

                if (_currentForklift != null)
                {
                    _currentForklift.Stop();
                    _oldForklifts.Add(_currentForklift);
                    _lastRecordedMove = Vector2.zero;
                }

                if (ObjectiveManager.Instance.IsLastOrder)
                {
                    if (!VictoryManager.Instance.IsGameFinished)
                    {
                        var finalScore = VictoryManager.Instance.ShowVictory(_oldForklifts.Select(x => x.TargetOutput.ValidationTimer).Where(x => x.HasValue).Sum(x => x.Value), _oldForklifts.Count(x => x.TargetOutput.IsInside), _oldForklifts.Count);
                        if (finalScore >= 500 && _oldForklifts.Count(x => x.DidExplode && x.TargetOutput.IsInside) >= 2) AchievementManager.Instance.Unlock(AchievementType.WinAfter2Explosions);

                        if (_aiForklifts.Length > 0 && _oldForklifts.All(x => x.TargetOutput.IsInside))
                        {
                            if (!GotMinipoTouch) AchievementManager.Instance.Unlock(AchievementType.DodgeMinipo);
                        }
                    }
                    requireTimerRestart = true;
                    _currentForklift = null;
                }
                else
                {
                    ObjectiveManager.Instance.FulfillOrder();

                    SpawnForklifts();
                }

                InputManager.Instance.ResetMov();
                Train?.ResetPos();

                foreach (var fl in _oldForklifts)
                {
                    fl.Stop();
                    fl.TargetOutput.gameObject.SetActive(false); // Unity shitting itself with physics running asynchroniously or smth
                }
                foreach (var po in _aiForklifts) po.Stop();

                MoveBackForklifts();

                foreach (var fl in _oldForklifts)
                {
                    fl.TargetOutput.Clear();
                    fl.TargetOutput.gameObject.SetActive(true);
                }

                if (requireTimerRestart)
                {
                    TimerManager.Instance.StartTimer();
                }
                GotMinipoTouch = false;
            }
        }

        public void SpawnForklifts()
        {
            var go = Instantiate(_forkliftPrefab, transform.position, Quaternion.identity);
            go.transform.position = ObjectiveManager.Instance.CurrentInput.position;
            _currentForklift = go.GetComponent<ForkliftController>();
            _currentForklift.AssignedOrder = ObjectiveManager.Instance.CurrentOrder;
        }

        public void MoveBackForklifts()
        {
            foreach (var fl in _oldForklifts)
            {
                fl.transform.position = ObjectiveManager.Instance.GetInput(fl.AssignedOrder).transform.position;
                fl.transform.rotation = Quaternion.identity;
            }
        }
    }
}
