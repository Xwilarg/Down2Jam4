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

        [SerializeField]
        private Transform[] _inputs, _outputs;

        private int _orderIndex;

        public OrderInfo CurrentOrder => _level.Orders[_orderIndex];
        public Transform CurrentInput { private set; get; }
        public Output CurrentOutput { private set; get; }

        public Transform GetInput(OrderInfo order) => _inputs[order.Input];

        private void Awake()
        {
            Instance = this;
            UpdateInternal();
        }

        private void Start()
        {
            VNManager.Instance.PlayStory(_level.Intro);
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
