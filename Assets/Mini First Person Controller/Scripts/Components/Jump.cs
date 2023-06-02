using System.Collections;
using UnityEngine;


public class Jump : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpStrength = 5;
    public event System.Action Jumped;

    private bool ableToJump = true;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;

    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        // Jump when the Jump button is pressed and we are on the ground.
       
        if (Input.GetButton("Jump") && (!groundCheck || groundCheck.isGrounded) && ableToJump)
        {
            ableToJump = false;
            StartCoroutine(JumpTimer());
            rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
    }

    IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(1f);
        ableToJump = true;
    }
}
