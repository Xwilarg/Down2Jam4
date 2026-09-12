using Down2Jam.Prop;
using NsfwDelivery.SO;
using UnityEngine;

namespace Down2Jam.Manager
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager Instance { private set; get; }

        [SerializeField]
        private LevelInfo _level;
        public LevelInfo NextLevel => _level.Next;

        [SerializeField]
        private Transform[] _inputs, _outputs;

        private int _orderIndex;

        public OrderInfo CurrentOrder => _level.Orders[_orderIndex];
        public Transform CurrentInput { private set; get; }
        public Output CurrentOutput { private set; get; }

        public bool IsLastOrder => _orderIndex == _level.Orders.Length - 1;

        public Transform GetInput(OrderInfo order) => _inputs[order.Input];

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateInternal();
            VNManager.Instance.PlayStory(_level.Intro);
            ForkliftManager.Instance.SpawnForklifts();
        }

        public void FulfillOrder()
        {
            _orderIndex++;
            UpdateInternal();
        }

        private void UpdateInternal()
        {
            CurrentInput = _inputs[CurrentOrder.Input];
            CurrentOutput = _outputs[CurrentOrder.Output].GetComponent<Output>();
            CurrentOutput.ShowHint();
        }
    }
}
