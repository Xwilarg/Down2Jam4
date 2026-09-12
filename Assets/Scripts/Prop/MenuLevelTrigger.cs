using Down2Jam.Manager;
using Down2Jam.Manager.Persistency;
using Down2Jam.SO;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class MenuLevelTrigger : MonoBehaviour
    {
        [SerializeField]
        private LevelInfo _level;

        [SerializeField]
        private LevelInfo _requirement;

        private void Awake()
        {
            if (_requirement != null && PersistencyManager.Instance.SaveData.GetBestScore(_requirement.name) == 0)
                Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            MenuManager.Instance.LoadLevel(_level);
        }
    }
}
