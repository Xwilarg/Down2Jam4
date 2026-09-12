using Down2Jam.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Down2Jam.Manager.Achievement
{
    public class AchievementPopup : MonoBehaviour
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private TMP_Text _title;

        public void Init(AchievementInfo data)
        {
            _image.sprite = data.Icon;
            _title.text = data.Name;

            Destroy(gameObject, 3f);
        }
    }
}
