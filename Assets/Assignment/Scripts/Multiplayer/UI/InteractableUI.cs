using System;
using Assignment.Scripts.Multiplayer.Interactables;
using TMPro;
using UnityEngine;

namespace Assignment.Scripts.Multiplayer.UI
{
    public class InteractableUI : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private GameObject fullScreenPanel;
        [SerializeField] private TMP_Dropdown optionsDropdown;
        [SerializeField] private TMP_Text currentStateText;
        [SerializeField] private TMP_Text currentAuthorityText;

        [SerializeField] private ObjectLabelUI objectLabel;

        public event Action<EInteratableStates> DropdownValueChanged;

        public void Start()
        {
            AddEventListeners();
        }

        public void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddEventListeners()
        {
            optionsDropdown.onValueChanged.AddListener((optionIndex) => OnDropdownValueChanged(optionIndex));
        }

        private void RemoveListeners()
        {
            optionsDropdown.onValueChanged.RemoveAllListeners();
        }

        private void OnDropdownValueChanged(int optionIndex)
        {
            EInteratableStates selectedState = (EInteratableStates)optionIndex;

            DropdownValueChanged?.Invoke(selectedState);
        }

        public void SetAuthorityText(string authId)
        {
            currentAuthorityText.text = "Authority : " + authId;
            objectLabel.SetAuthority(authId);
        }

        public void SetStateText(string text)
        {
            currentStateText.text = text;
            objectLabel.SetLabel(text);
        }

        public void EnableFullScreenUI()
        {
            fullScreenPanel.SetActive(true);
        }

        public void DisableFullScreenUI()
        {
            fullScreenPanel.SetActive(false);
        }
    }
}