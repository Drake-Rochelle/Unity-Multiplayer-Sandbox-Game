using Photon.Pun;
using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody rigidbodyCompnent;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;
    PhotonView view;



        void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        view = GetComponent<PhotonView>();
        // Get rigidbody.
        rigidbodyCompnent = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (view.IsMine)
        {
            // Jump when the Jump button is pressed and we are on the ground.
            if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
            {
                rigidbodyCompnent.AddForce(Vector3.up * 100 * jumpStrength);
                Jumped?.Invoke();
            }
        }
    }
}
