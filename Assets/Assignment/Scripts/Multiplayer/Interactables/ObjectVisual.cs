using UnityEngine;

namespace Assignment.Scripts.Multiplayer.Interactables
{
    public class ObjectVisual : MonoBehaviour
    {
        [SerializeField] private EInteratableStates state;

        public EInteratableStates State => state;

        private MeshRenderer renderer = null;

        private void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
        }

        public void DisableVisual()
        {
            renderer.enabled = false;
        }

        public void EnableVisual()
        {
            renderer.enabled = true;
        }
    }
}