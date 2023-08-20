using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Juiciness Settings")]
    public float rotationSmoothness = 10f;
    public Transform playerCamera;
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;

    CharacterController controller;
    Vector3 playerVelocity;
    bool isGrounded;

    float defaultCameraPosY;
    float timer;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultCameraPosY = playerCamera.localPosition.y;
    }

    private void Update()
    {
        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        // Apply gravity
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        playerVelocity.y += gravity * Time.deltaTime;

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply movement
        controller.Move(moveDirection * moveSpeed * Time.deltaTime + playerVelocity * Time.deltaTime);

        // Camera rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * rotationSmoothness);

        Vector3 newCameraPos = playerCamera.localPosition;
        newCameraPos.y = defaultCameraPosY + Mathf.Sin(timer) * bobbingAmount;
        playerCamera.localPosition = newCameraPos;

        timer += bobbingSpeed * Time.deltaTime;

        // Prevent camera flipping
        float cameraRotationX = playerCamera.localEulerAngles.x;
        if (cameraRotationX > 180f)
            cameraRotationX -= 360f;
        cameraRotationX = Mathf.Clamp(cameraRotationX - mouseY * rotationSmoothness, -90f, 90f);
        playerCamera.localEulerAngles = new Vector3(cameraRotationX, 0f, 0f);
    }
}
