using Down2Jam.Prop;
using Down2Jam.SO;
using UnityEngine;

namespace Down2Jam.Manager
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager Instance { private set; get; }

        public LevelInfo NextLevel => LoaderManager.CurrentLevel.Next;

        [SerializeField]
        private Transform[] _inputs, _outputs;

        private int _orderIndex;

        public OrderInfo CurrentOrder => LoaderManager.CurrentLevel.Orders[_orderIndex];
        public Transform CurrentInput { private set; get; }
        public Output CurrentOutput { private set; get; }

        public bool IsLastOrder => _orderIndex == LoaderManager.CurrentLevel.Orders.Length - 1;

        public Transform GetInput(OrderInfo order) => _inputs[order.Input];

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateInternal();
            VNManager.Instance.PlayStory(LoaderManager.CurrentLevel.Intro);
            ForkliftManager.Instance.SpawnForklifts();
            ForkliftManager.Instance.LoadMinipos();
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
