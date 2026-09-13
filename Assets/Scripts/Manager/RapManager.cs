using Assets.Scripts.Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class RapManager : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(LoadGlobalUI());
            StartCoroutine(WaitAndReloadMain());
        }

        private IEnumerator LoadGlobalUI()
        {
            yield return SceneManager.LoadSceneAsync("GlobalUI", LoadSceneMode.Additive);
            AchievementManager.Instance.Unlock(AchievementType.SecretRap);
            GlobalUIManager.Instance.StopBGM();
        }

        private IEnumerator WaitAndReloadMain()
        {
            yield return new WaitForSeconds(51.5f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
