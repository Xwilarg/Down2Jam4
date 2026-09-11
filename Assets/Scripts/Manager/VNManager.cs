using Ink.Runtime;
using Ink.UnityIntegration;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Down2Jam.Manager
{
    public class VNManager : MonoBehaviour
    {
        public static VNManager Instance { private set; get; }

        [SerializeField]
        private InkFile _intro;

        [SerializeField]
        private GameObject _vnContainer;

        [SerializeField]
        private GameObject _nameContainer;

        [SerializeField]
        private TMP_Text _mainText;

        [SerializeField]
        private TMP_Text _name, _nameSubtitle;

        [SerializeField]
        private GameObject _sticker;

        private Story _story;

        public bool IsPlayingStory => _vnContainer.activeInHierarchy;

        private void Awake()
        {
            _story = new(_intro.storyJson);
            _nameContainer.SetActive(false);
            _sticker.SetActive(false);
            UpdateDisplay();
        }

        private void SetSpeaker(string name)
        {
            _nameContainer.SetActive(true);
            if (name == "zirk")
            {
                _name.text = "Mr. Z";
                _nameSubtitle.text = "Vice President";
            }
            else if (name == "nano")
            {
                _name.text = "Mr. N";
                _nameSubtitle.text = "President";
            }
            else if (name == "minipi")
            {
                _name.text = "Minipi";
                _nameSubtitle.text = "Devoted Worker";
            }
            else if (name == "Estalia")
            {
                _name.text = "Estalia";
                _nameSubtitle.text = "Improvised Postal Worker";
            }
            else
                _nameContainer.SetActive(false);
        }

        private void UpdateDisplay()
        {
            _mainText.text = _story.currentText;

            foreach (var tag in _story.currentTags)
            {
                var parts = tag.ToLowerInvariant().Split(' ');
                var body = string.Join(" ", parts.Skip(1));
                switch (parts[0])
                {
                    case "speaker":
                        SetSpeaker(body);
                        break;

                    case "time": break;

                    default:
                        Debug.LogWarning($"Unknown story tag {parts[0]}");
                        break;
                }
            }
        }

        public void DisplayNextDialogue()
        {
            if (_story.canContinue)
            {
                _story.Continue();
                UpdateDisplay();
            }
            else
            {
                _vnContainer.SetActive(false);
            }
        }
    }
}
