using UnityEngine;

namespace Lucerna.Movement.ThirdPerson
{
    public class PlayerMovement : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";

        [Space(10)]
        [SerializeField] private LayerMask groundLayer = new LayerMask();

        [Header("Animator Variables")]
        [SerializeField] private Animator animator = null;
        [SerializeField] private string horizontalParameter = "Horizontal";
        [SerializeField] private string verticalParameter = "Vertical";


        private float horizontalDirection = 0f;
        private float verticalDirection = 0f;

        private Rigidbody rb = null;

        private bool grounded = false;

        // EXECUTION FUNCTIONS
        private void Start() => rb = GetComponent<Rigidbody>(); 

        private void Update() {
            GetInput();

            if (animator != null)
                UpdateAnimator();
        }

        private void FixedUpdate() => Move();

        private void OnCollisionStay(Collision other) {
            RaycastHit hit;
            Ray landingRay = new Ray(transform.position, Vector3.down * 0.2f);

            Debug.DrawRay(transform.position, Vector3.down * 0.2f);

            if (Physics.Raycast(landingRay, out hit, 1f))
            {
                grounded = true;
            }
        }

        private void OnCollisionExit(Collision other) {
            grounded = false;
        }

        // METHODS
        private void GetInput() {
            horizontalDirection = Input.GetAxis(horizontalAxis);
            verticalDirection = Input.GetAxis(verticalAxis);

            if (Input.GetKeyDown(KeyCode.Space)) {
                if (grounded) {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
                    grounded = false;
                }
            }
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
