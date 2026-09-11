using UnityEngine;

namespace Down2Jam.Prop
{
    public class Output : MonoBehaviour
    {
        public bool IsInside => _insideCount > 0;

        private int _insideCount = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Forklift")) _insideCount++;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Forklift")) _insideCount--;
        }
    }
}
