using System;
using Assignment.Scripts.Multiplayer.Interactables;
using Assignment.Scripts.Multiplayer.Managers;
using Assignment.Scripts.Player;
using UnityEngine;

namespace Assignment.Scripts.Multiplayer.Player
{
    public class Interactor : MonoBehaviour
    {
        private PlayerInputHandler inputHandler;
        private NetworkedInteractable _networkedInteractable;

        [SerializeField] private GameObject hudUI;

        private bool _isInteracting = true;

        public void Start()
        {
            inputHandler = GetComponent<PlayerInputHandler>();

            inputHandler.InteractPressed += OnInteract;
            inputHandler.UninteractPressed += OnUninteract;
        }

        private void LateUpdate()
        {
            if (_networkedInteractable)
                hudUI.gameObject.SetActive(true);
            else
                hudUI.gameObject.SetActive(false);
        }

        private void OnInteract()
        {
            Debug.Log("Interact Pressed");

            if (_networkedInteractable)
            {
                _networkedInteractable.Interact();
                CursorManager.SetUIMode();
            }
        }

        private void OnUninteract()
        {
            Debug.Log("Un-Interact Pressed");

            if (_networkedInteractable)
            {
                _networkedInteractable.Uninteract();
                CursorManager.SetGameplayMode();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Interactable Entered");
            other.TryGetComponent(out _networkedInteractable);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Interactable Exit");
            OnUninteract();
            _networkedInteractable = null;
        }
    }
}