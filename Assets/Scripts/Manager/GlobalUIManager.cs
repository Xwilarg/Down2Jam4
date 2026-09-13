using Assets.Scripts.Manager;
using Down2Jam.Manager.Achievement;
using Down2Jam.Manager.Persistency;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class GlobalUIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _achievementPanel, _settingsPanel;

        [SerializeField]
        private Transform _achievementPanelContainer;

        [SerializeField]
        private GameObject _achievementPrefab;

        private void Awake()
        {
            _achievementPanel.SetActive(false);
            _settingsPanel.SetActive(false);
        }

        public void ToggleAchievement()
        {
            _achievementPanel.SetActive(!_achievementPanel.activeInHierarchy);

            if (_achievementPanel.activeInHierarchy)
            {
                for (int i = _achievementPanelContainer.childCount - 1; i >= 0; i--) Destroy(_achievementPanelContainer.GetChild(i).gameObject);
                foreach (var ach in AchievementManager.Instance.Achievements)
                {
                    var inst = Instantiate(_achievementPrefab, _achievementPanelContainer);
                    inst.GetComponent<AchievementPopup>().InitUI(ach.Value, PersistencyManager.Instance.SaveData.HasAchievement((int)ach.Key));
                }
            }
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void ToggleSettings()
        {
            _settingsPanel.SetActive(!_settingsPanel.activeInHierarchy);
        }
    }
}
