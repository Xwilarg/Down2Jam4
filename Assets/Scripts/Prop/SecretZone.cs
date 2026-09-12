using UnityEngine;
using UnityEngine.SceneManagement;

namespace Down2Jam.Prop
{
    public class SecretZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Forklift"))
            {
                SceneManager.LoadScene("SecretRap");
            }
        }
    }
}
