using NsfwDelivery.SO;
using UnityEngine;

namespace Down2Jam.Manager
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager Instance { private set; get; }

        [SerializeField]
        private Transform[] _inputs, _outputs;

        [SerializeField]
        private OrderInfo[] _orders;

        private int _orderIndex;

        public OrderInfo CurrentOrder => _orders[_orderIndex];
        public Transform CurrentInput { private set; get; }
        public Transform CurrentOutput { private set; get; }

        public Transform GetInput(OrderInfo order) => _inputs[order.Input];

        private void Awake()
        {
            Instance = this;
            UpdateInternal();
        }

        public void FulfillOrder()
        {
            _orderIndex++;
            UpdateInternal();
        }

        private void UpdateInternal()
        {
            CurrentInput = _inputs[CurrentOrder.Input];
            CurrentOutput = _outputs[CurrentOrder.Output];
        }
    }
}
