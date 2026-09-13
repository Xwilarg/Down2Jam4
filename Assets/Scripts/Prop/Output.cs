using Down2Jam.Manager;
using UnityEngine;

namespace Down2Jam.Prop
{
    public class Output : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _hint;

        public bool IsInside => _insideCount > 0;

        private int _insideCount = 0;

        public bool IsDeprecated { set; get; }
        private Color _highlightColor;

        public float? ValidationTimer { set; get; }

        private void Awake()
        {
            _highlightColor = _hint.color;
            _hint.gameObject.SetActive(false);
        }

        public void ShowHint()
        {
            _hint.gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Forklift") && collision.GetComponent<ForkliftController>().TargetOutput.gameObject.GetEntityId() == gameObject.GetEntityId())
            {
                if (_insideCount == 0) ValidationTimer = TimerManager.Instance.Timer;
                _insideCount++;
                _hint.color = Color.green;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Forklift") && collision.GetComponent<ForkliftController>().TargetOutput.gameObject.GetEntityId() == gameObject.GetEntityId())
            {
                _insideCount--;
                if (!IsInside)
                {
                    _hint.color = IsDeprecated ? Color.white : _highlightColor;
                    ValidationTimer = null;
                }
            }
        }

        public void Clear()
        {
            _insideCount = 0;
            _hint.color = Color.white;
            ValidationTimer = null;
        }

        public void Shrink()
        {
            GetComponent<CircleCollider2D>().radius /= 2f;
        }

        public void Grow()
        {
            GetComponent<CircleCollider2D>().radius *= 1.5f;
        }

        public override string ToString()
        {
            return $"Output (inside: {_insideCount})";
        }
    }
}
