using UnityEngine;

namespace Down2Jam.Prop
{
    public class Explosion : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject, .4f);
        }
    }
}
