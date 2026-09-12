using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Manager
{
    public class RapManager : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(WaitAndReloadMain());
        }

        private IEnumerator WaitAndReloadMain()
        {
            yield return new WaitForSeconds(45f);
            SceneManager.LoadScene("Main");
        }
    }
}
