using Down2Jam.Manager.Persistency;
using Down2Jam.SO;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class TutorialOnly : MonoBehaviour
    {
        [SerializeField]
        private LevelInfo _firstLevel;

        private void Awake()
        {
            if (PersistencyManager.Instance.SaveData.GetBestScore(_firstLevel.name) > 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
