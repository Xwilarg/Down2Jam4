using UnityEngine;

namespace Down2Jam.Manager
{
    [ExecuteInEditMode]
    public class DebugManager : MonoBehaviour
    {
        [SerializeField]
        private bool _resetStatic;

        private void Update()
        {
            if (_resetStatic)
            {
                LoaderManager.CurrentLevel = null;
                VNManager.SkipIntro = false;
                _resetStatic = false;
            }
        }
    }
}
