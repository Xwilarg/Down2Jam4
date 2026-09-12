using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class VictoryManager : MonoBehaviour
    {
        public static VictoryManager Instance { private set; get; }

        [SerializeField]
        private GameObject _victoryPanel;

        [SerializeField]
        private TMP_Text _finalText;

        private void Awake()
        {
            Instance = this;

            _victoryPanel.SetActive(false);
        }

        public void ShowVictory(float totalTime, int minipiCount)
        {
            _victoryPanel.SetActive(true);

            var avrTime = totalTime / minipiCount;
            float timeScore = Mathf.FloorToInt(500 - (avrTime * 500f / 10f));
            float finalScore = timeScore + 500;

            _finalText.text =
                $"Minipi Delivered: {minipiCount}\n" +
                $"+500\n" +
                $"\n" +
                $"Average Time: {avrTime:0.0}s\n" +
                $"+{timeScore}\n" +
                $"\n" +
                $"<b>Final Score: {finalScore}</b>";
        }

        public void Retry()
        {
            SceneManager.LoadScene("Main");
        }

        public void NextLevel()
        {
            LoaderManager.CurrentLevel = ObjectiveManager.Instance.NextLevel;
            VNManager.SkipIntro = false;
            SceneManager.LoadScene("Main");
        }
    }
}
