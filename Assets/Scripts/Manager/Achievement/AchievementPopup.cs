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

        public void InitEarned(AchievementInfo data)
        {
            _image.sprite = data.Icon;
            _title.text = data.Name;

            Destroy(gameObject, 3f);
        }

        public void InitUI(AchievementInfo data, bool unlocked)
        {
            _image.sprite = data.Icon;
            _image.color = unlocked ? Color.white : Color.black;
            _title.text = unlocked ? data.Name : data.UnlockExpl;
        }
    }
}
