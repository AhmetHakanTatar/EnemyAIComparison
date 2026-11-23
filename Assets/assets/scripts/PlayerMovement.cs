


using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;

        input = Vector2.ClampMagnitude(input, 1f);

        bool running = Keyboard.current.leftShiftKey.isPressed;
        float speed = running ? runSpeed : walkSpeed;

        Vector3 movement =
            new Vector3(input.x, 0f, input.y);

        if (movement.sqrMagnitude > 0.01f)
        {
            transform.forward = movement.normalized;
        }

        if (controller.isGrounded &&
            verticalVelocity.y < 0f)
        {
            verticalVelocity.y = -2f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        // Tek Move çaðrýsý: yatay + dikey hareket
        Vector3 totalMovement =
            movement * speed + verticalVelocity;

        controller.Move(totalMovement * Time.deltaTime);
    }
}
