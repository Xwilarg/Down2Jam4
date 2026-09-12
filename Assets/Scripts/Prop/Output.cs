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

        private void Awake()
        {
            _hint.gameObject.SetActive(false);
        }

        public void ShowHint()
        {
            _hint.gameObject.SetActive(true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Forklift"))
            {
                if (IsDeprecated) _hint.color = Color.green;
                _insideCount++;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Forklift"))
            {
                if (IsDeprecated && !IsInside) _hint.color = Color.black;
                _insideCount--;
            }
        }

        public void Shrink()
        {
            GetComponent<CircleCollider2D>().radius /= 2f;
        }

        public void Grow()
        {
            GetComponent<CircleCollider2D>().radius *= 2f;
        }
    }
}
