using UnityEngine;

namespace Down2Jam.Manager
{
    public class ObjectiveManager : MonoBehaviour
    {
        public static ObjectiveManager Instance { private set; get; }

        [SerializeField]
        private Transform[] _inputs, _outputs;

        private void Awake()
        {
            Instance = this;
        }
    }
}
