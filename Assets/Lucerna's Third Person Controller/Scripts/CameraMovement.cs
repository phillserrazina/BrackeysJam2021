using UnityEngine;

namespace Lucerna.Movement.ThirdPerson
{
    public class CameraMovement : MonoBehaviour
    {
        // VARIABLES
        [Header("References")]
        [SerializeField] private Transform playerTransform = null;
        [SerializeField] private Vector3 offset = Vector3.zero;
        
        [Header("Settings")]
        [SerializeField] private float mouseSensitivity = 1f;
        [Tooltip("X = Minimum; Y = Maximum")]
        [SerializeField] private Vector2 yClampValues = Vector2.zero;

        private float mouseX = 0f;
        private float mouseY = 0f;

        private Camera cam;

        // EXECUTION FUNCTIONS
        private void Start() => cam = Camera.main;

        private void FixedUpdate() {
            // Camera position based on mouse movement
            mouseX += Input.GetAxis ("Mouse X") * mouseSensitivity;
            mouseY += Input.GetAxis ("Mouse Y") * mouseSensitivity;

            // Clamp the mouse Y position
            mouseY = Mathf.Clamp (mouseY, yClampValues.x, yClampValues.y);

            var targetRot = Quaternion.Euler (mouseY, mouseX, 0.0f);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, 0.125f);
            
            var targetPos = new Vector3 (playerTransform.position.x, playerTransform.position.y, playerTransform.position.z) + offset;
            transform.position = Vector3.Slerp(transform.position, targetPos, 0.125f);

            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                Quaternion turnAngle = Quaternion.Euler (0.0f, transform.eulerAngles.y, 0.0f);

                playerTransform.localRotation = Quaternion.Slerp (playerTransform.rotation, turnAngle, 5f * Time.deltaTime);
            }
        }
    }
}
