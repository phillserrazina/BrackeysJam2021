using UnityEngine;

namespace Lucerna.Movement.ThirdPerson
{
    public class PlayerMovement : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";

        [Header("Animator Variables")]
        [SerializeField] private Animator animator = null;
        [SerializeField] private string horizontalParameter = "Horizontal";
        [SerializeField] private string verticalParameter = "Vertical";


        private float horizontalDirection = 0f;
        private float verticalDirection = 0f;

        private Rigidbody rb = null;

        // EXECUTION FUNCTIONS
        private void Start() => rb = GetComponent<Rigidbody>(); 

        private void Update() {
            GetInput();
            UpdateAnimator();
        }

        private void FixedUpdate() => Move();

        // METHODS
        private void GetInput() {
            horizontalDirection = Input.GetAxis(horizontalAxis);
            verticalDirection = Input.GetAxis(verticalAxis);
        }

        private void UpdateAnimator() {
            animator.SetFloat(horizontalParameter, horizontalDirection);
            animator.SetFloat(verticalParameter, verticalDirection);
        }

        private void Move() {
            rb.MovePosition(transform.position +
                            transform.forward * verticalDirection * movementSpeed * Time.fixedDeltaTime +
                            transform.right * horizontalDirection * movementSpeed * Time.fixedDeltaTime);
        }
    }
}
