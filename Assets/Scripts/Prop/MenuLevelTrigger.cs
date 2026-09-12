using Down2Jam.Manager;
using Down2Jam.Manager.Persistency;
using Down2Jam.SO;
using TMPro;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class MenuLevelTrigger : MonoBehaviour
    {
        [SerializeField]
        private LevelInfo _level;

        [SerializeField]
        private LevelInfo _requirement;

        [SerializeField]
        private TMP_Text _bestScore;

        private void Awake()
        {
            if (_requirement != null && PersistencyManager.Instance.SaveData.GetBestScore(_requirement.name) < 500)
                Destroy(gameObject);
            else
                _bestScore.text = $"Best Score: {PersistencyManager.Instance.SaveData.GetBestScore(_level.name)}";
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            MenuManager.Instance.LoadLevel(_level);
        }
    }
}
