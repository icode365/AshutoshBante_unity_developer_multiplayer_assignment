using TMPro;
using UnityEngine;

namespace Assignment.Scripts.Multiplayer.Interactables
{
    public class ObjectLabelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text labelText;
        [SerializeField] private TMP_Text authText;
        
        public void SetLabel(string label)
        {
            labelText.text = label;
        }
        
        public void SetAuthority(string authId)
        {
            authText.text = authId;
        }
    }
}
