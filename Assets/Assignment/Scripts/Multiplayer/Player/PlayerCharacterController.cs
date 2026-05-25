using Assignment.Scripts.Player;
using Assignment.Scripts.Player.Camera;
using Fusion;
using UnityEngine;

namespace Assignment.Scripts.Multiplayer.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerCharacterController : NetworkBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform visuals;

        [SerializeField] private Transform cameraTransform;

        private ThirdPersonCameraController _cameraController;
        private CharacterController _characterController;
        private PlayerInputHandler _inputHandler;

        [Header("Movement")] [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float groundedGravityForce = 2f;
        [SerializeField] private float moveSpeed = 5f;

        private bool _isJumping;
        private Vector3 _velocity;
        private Vector3 _moveVelocity;
        private Vector3 _gravityVelocity;

        private void Awake()
        {
            _cameraController =
                GetComponentInChildren<ThirdPersonCameraController>();

            _characterController =
                GetComponent<CharacterController>();

            _inputHandler =
                GetComponent<PlayerInputHandler>();
        }

        public override void FixedUpdateNetwork()
        {
            HandleMovement();
            MoveCharacter();
        }

        private void LateUpdate()
        {
            UpdateCameraPosition();
        }

        private void OnEnable()
        {
            AddMovementInputListeners();
        }

        private void OnDisable()
        {
            RemoveMovementInputListeners();
        }


        private void AddMovementInputListeners()
        {
            _inputHandler.OnJumpPressed += Jump;
        }

        private void RemoveMovementInputListeners()
        {
            _inputHandler.OnJumpPressed -= Jump;
        }

        private void HandleMovement()
        {
            Vector2 input =
                _inputHandler.MoveInput;

            Vector3 cameraForward = cameraTransform.forward;

            Vector3 cameraRight = cameraTransform.right;

            Vector3 moveDirection =
                cameraForward * input.y +
                cameraRight * input.x;

            moveDirection.Normalize();

            _moveVelocity =
                moveDirection * moveSpeed;
        }

        private void MoveCharacter()
        {
            Vector3 finalVelocity =
                _moveVelocity + _gravityVelocity;

            _characterController.Move(
                finalVelocity * Time.deltaTime);
        }

        private void Jump()
        {
            _isJumping = true;
        }

        private void UpdateCameraPosition()
        {
            _cameraController.RotateCamera(
                _inputHandler.CurrentLookInput);
        }
    }
}