using System;
using Assignment.Scripts.Multiplayer.UI;
using Fusion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assignment.Scripts.Multiplayer.Interactables
{
    public class NetworkedInteractable : NetworkBehaviour, IStateAuthorityChanged
    {
        private bool initialized = false;
        private UnityEvent AuthorityRecieved = new();
        [SerializeField] private InteractableUI fullscreenUI;
        [SerializeField] private GameObject intrestArea;

        [Networked, OnChangedRender(nameof(OnStateChanged))]
        public EInteratableStates SyncedState { get; private set; } = EInteratableStates.CUBE;

        [Networked] public bool IsLocked { get; private set; } = false;
        
        void Start()
        {
            fullscreenUI.DropdownValueChanged += SetState;
        }

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (!initialized)
            {
                Debug.Log("initializing...");
                fullscreenUI.SetAuthorityText(Object.StateAuthority.PlayerId.ToString());
                UpdateStateVisuals();
                initialized = true;
            }
        }

        private void OnStateChanged()
        {
            Debug.Log("State Changed :" + SyncedState);
            UpdateUI();
            UpdateStateVisuals();
        }

        private void SetState(EInteratableStates state)
        {
            SyncedState = state;
        }

        public void StateAuthorityChanged()
        {
            Debug.Log("State Authority Changed : " + Object.StateAuthority.PlayerId);
            fullscreenUI.SetAuthorityText(Object.StateAuthority.PlayerId.ToString());
            AuthorityRecieved?.Invoke();
            AuthorityRecieved?.RemoveAllListeners();
        }

        private void RequestStateAuthority()
        {
            Debug.Log("Request State Authority Change" + Object.StateAuthority.PlayerId);
            Object.RequestStateAuthority();
        }

        private void UpdateStateVisuals()
        {
            var visuals = transform.GetComponentsInChildren<ObjectVisual>();

            foreach (var visual in visuals)
            {
                if (visual.State == SyncedState)
                    visual.EnableVisual();
                else
                    visual.DisableVisual();
            }
        }

        private void UpdateUI()
        {
            fullscreenUI.SetStateText(SyncedState.ToString());
        }

        public void Interact()
        {
            Debug.Log("Interact" + HasStateAuthority);

            if (IsLocked) return;

            if (!HasStateAuthority)
            {
                AuthorityRecieved.AddListener(() => EnableFullScreen());
                RequestStateAuthority();
            }

            EnableFullScreen();
        }

        public void Uninteract()
        {
            Debug.Log("Un-interact" + HasStateAuthority);

            if (!HasStateAuthority)
            {
                AuthorityRecieved.AddListener(() => DisableFullScreen());
                RequestStateAuthority();
            }

            DisableFullScreen();
        }

        private void EnableFullScreen()
        {
            IsLocked = true;
            fullscreenUI.EnableFullScreenUI();
        }

        private void DisableFullScreen()
        {
            IsLocked = false;
            fullscreenUI.DisableFullScreenUI();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                intrestArea.SetActive(false);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                intrestArea.SetActive(true);
            }
        }
    }
}