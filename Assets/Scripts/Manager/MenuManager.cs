using Down2Jam.Manager.Persistency;
using Down2Jam.Prop;
using Down2Jam.SO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance { private set; get; }

        [SerializeField]
        private ForkliftController _player;

        [SerializeField]
        private TMP_Text _globalScore;

        [SerializeField]
        private LevelInfo _firstLevel;

        private bool _isLoading = false;

        private void Awake()
        {
            Instance = this;

            SceneManager.LoadScene("GlobalUI", LoadSceneMode.Additive);

            int totalScore = 0;
            LevelInfo it = _firstLevel;
            do
            {
                totalScore  += PersistencyManager.Instance.SaveData.GetBestScore(it.name);

                it = it.Next;
            } while (it != null);

            _globalScore.text = $"Final Score: {totalScore}";
        }

        public void LoadLevel(LevelInfo level)
        {
            VNManager.SkipIntro = false;
            LoaderManager.CurrentLevel = level;
            _isLoading = true;
            SceneManager.LoadScene("Main");
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            if (!_isLoading) _player.ReceiveRawInput(value.ReadValue<Vector2>());
        }
    }
}
