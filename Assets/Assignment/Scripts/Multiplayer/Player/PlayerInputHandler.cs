using UnityEngine;
using System;

namespace Assignment.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 GravityInput { get; private set; }

        public event Action OnJumpPressed;
        public event Action InteractPressed;
        public event Action UninteractPressed;
        public event Action EscPressed;

        public Vector2 CurrentLookInput { get; private set; }
        
        private PlayerControls controls;

        private void Awake()
        {
            controls = new PlayerControls();

            controls.Gameplay.Interact.performed += _ =>
                InteractPressed?.Invoke();
            
            controls.Gameplay.Uninteract.performed += _ =>
                UninteractPressed?.Invoke();
            
            controls.Gameplay.Uninteract.performed += _ =>
                EscPressed?.Invoke();

            controls.Gameplay.Move.performed += ctx =>
                MoveInput = ctx.ReadValue<Vector2>();

            controls.Gameplay.Move.canceled += ctx =>
                MoveInput = Vector2.zero;

            controls.Gameplay.Jump.performed += _ =>
                OnJumpPressed?.Invoke();

            controls.Gameplay.GravityDirection.performed += ctx =>
            {
                GravityInput = ctx.ReadValue<Vector2>();
            };

            controls.Gameplay.GravityDirection.canceled += _ =>
                GravityInput = Vector2.zero;

            controls.Gameplay.Look.performed += ctx =>
            {
                CurrentLookInput = ctx.ReadValue<Vector2>();
            };

            controls.Gameplay.Look.canceled += _ =>
            {
                CurrentLookInput = Vector2.zero;
            };
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }
    }
}