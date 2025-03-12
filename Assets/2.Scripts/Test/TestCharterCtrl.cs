using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class TestCharterCtrl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float gravity = 9.81f;
    public float jumpForce = 5f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Rotate();
        ApplyGravity();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal"); // A, D (좌우)
        float z = Input.GetAxis("Vertical");   // W, S (앞뒤)

        Vector3 move = transform.right * x + transform.forward * z; 
        moveDirection = move * moveSpeed;

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
        }

        controller.Move((moveDirection + Vector3.up * verticalVelocity) * Time.deltaTime);
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        transform.Rotate(Vector3.up * mouseX);
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
    }
}
