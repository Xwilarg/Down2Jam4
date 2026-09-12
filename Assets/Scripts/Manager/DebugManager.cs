using NsfwDelivery.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField]
        private LevelInfo _debugInfo;

        private void Awake()
        {
            SceneManager.LoadScene(_debugInfo.Level.Name, LoadSceneMode.Additive);
        }
    }
}
