using UnityEngine;

namespace Containers
{
    public class PrefabFactory : MonoBehaviour
    {
        public static PrefabFactory Instance;
        public GameObject rightClickCanvas;
        public GameObject doneCanvas;

        private void Awake()
        {
            Instance = this;
        }
    }
}
