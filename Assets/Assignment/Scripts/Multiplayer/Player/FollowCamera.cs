using UnityEngine;

namespace Assignment.Scripts.Player.Camera
{
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Camera")]
        [SerializeField] private Transform cameraTransform;

        [Header("Follow")]
        [SerializeField] private Vector3 offset =
            new Vector3(0, 3, -6);

        [SerializeField] private float followSmoothness = 10f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 120f;

        [SerializeField] private float rotationSmoothness = 12f;

        [Header("Pitch")]
        [SerializeField] private float minPitch = -30f;

        [SerializeField] private float maxPitch = 60f;

        private float _yaw;
        private float _pitch;

        private Quaternion _currentRotation;

        private void LateUpdate()
        {
            HandleRotation();

            HandleFollow();
        }

        private void HandleRotation()
        {
            _pitch = Mathf.Clamp(
                _pitch,
                minPitch,
                maxPitch);

            Vector3 upDirection =
                target.up;

            Quaternion yawRotation =
                Quaternion.AngleAxis(
                    _yaw,
                    upDirection);

            Quaternion pitchRotation =
                Quaternion.AngleAxis(
                    _pitch,
                    Vector3.right);

            Quaternion targetRotation =
                yawRotation *
                target.rotation *
                pitchRotation;

            _currentRotation =
                Quaternion.Slerp(
                    _currentRotation,
                    targetRotation,
                    rotationSmoothness * Time.deltaTime);
        }

        private void HandleFollow()
        {
            Vector3 desiredPosition =
                target.position +
                _currentRotation * offset;

            transform.position =
                Vector3.Lerp(
                    transform.position,
                    desiredPosition,
                    followSmoothness * Time.deltaTime);

            Vector3 lookTarget =
                target.position +
                target.up * 1.5f;

            transform.rotation =
                Quaternion.LookRotation(
                    lookTarget - transform.position,
                    target.up);
        }

        public void RotateCamera(Vector2 lookInput)
        {
            _yaw +=
                lookInput.x *
                rotationSpeed *
                Time.deltaTime;

            _pitch -=
                lookInput.y *
                rotationSpeed *
                Time.deltaTime;
        }
    }
}