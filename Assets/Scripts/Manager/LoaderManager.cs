using NsfwDelivery.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class LoaderManager : MonoBehaviour
    {
        public static LevelInfo CurrentLevel { set; get; } = null;

        [SerializeField]
        private LevelInfo _debugInfo;

        private void Awake()
        {
            SceneManager.LoadScene(CurrentLevel == null ? _debugInfo.Level.Name : CurrentLevel.Level.Name, LoadSceneMode.Additive);
        }
    }
}
