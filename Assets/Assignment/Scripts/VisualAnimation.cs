using UnityEngine;

namespace Assignment.Scripts
{
    public class FloatingRotateAnimation : MonoBehaviour
    {
        [Header("Floating")]
        [SerializeField] private float floatHeight = 0.25f;
        [SerializeField] private float floatDuration = 1.5f;

        [Header("Rotation")]
        [SerializeField] private Vector3 rotationAxis = new Vector3(0f, 1f, 0f);
        [SerializeField] private float rotationSpeed = 90f;

        [Header("Tilt")]
        [SerializeField] private Vector3 tiltAngles = new Vector3(10f, 0f, 10f);

        private Vector3 _startPos;

        private void Start()
        {
            _startPos = transform.localPosition;

            // Apply slight tilt
            transform.localRotation = Quaternion.Euler(tiltAngles);

            // Bob up and down
            LeanTween.moveLocalY(
                    gameObject,
                    _startPos.y + floatHeight,
                    floatDuration)
                .setEaseInOutSine()
                .setLoopPingPong();

            // Continuous rotation
            LeanTween.rotateAroundLocal(
                    gameObject,
                    rotationAxis,
                    360f,
                    360f / rotationSpeed)
                .setEaseLinear()
                .setLoopClamp();
        }
    }
}