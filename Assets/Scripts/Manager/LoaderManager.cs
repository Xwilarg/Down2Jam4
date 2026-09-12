using NsfwDelivery.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class LoaderManager : MonoBehaviour
    {
        public static LoaderManager Instance { private set; get; }

        public static LevelInfo CurrentLevel { set; get; } = null;

        [SerializeField]
        private LevelInfo _debugInfo;

        private void Awake()
        {
            Instance = this;

            if (CurrentLevel == null)
            {
                CurrentLevel = _debugInfo;
                VNManager.SkipIntro = false;
            }
            SceneManager.LoadScene(CurrentLevel.Level.Name, LoadSceneMode.Additive);
        }
    }
}
