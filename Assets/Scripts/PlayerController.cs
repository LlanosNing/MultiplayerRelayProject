using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 6;

    private Rigidbody rb;

    private Vector3 input;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Cambiar el color del jugador local
        if(IsLocalPlayer == true)
        {
            GetComponent<Renderer>().material.color = Color.hotPink;
        }
    }
    private void Update()
    {
        if(IsLocalPlayer == false) return;

        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer == false) return;

        Vector3 velocity = input.normalized * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }
}
