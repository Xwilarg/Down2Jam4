using Down2Jam.Prop;
using NUnit.Framework;
using System.Collections.Generic;
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

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SpawnForklifts();
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

                foreach (var fl in _oldForklifts)
                {
                    fl.TryActAI(TimerManager.Instance.Timer);
                }

                if (Vector2.Distance(_currentForklift.transform.position, ObjectiveManager.Instance.CurrentOutput.position) < .5f)
                {
                    _currentForklift.Stop();
                    TimerManager.Instance.StopTimer();
                    InputManager.Instance.ResetMov();
                    ObjectiveManager.Instance.FulfillOrder();
                    _oldForklifts.Add(_currentForklift);
                    _lastRecordedMove = Vector2.zero;

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
                fl.transform.position = ObjectiveManager.Instance.GetInput(fl.AssignedOrder).position;
                fl.transform.rotation = Quaternion.identity;
            }
        }
    }
}
