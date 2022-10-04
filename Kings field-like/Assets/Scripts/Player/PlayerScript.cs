using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] Rigidbody player;
    [SerializeField] float playerSpeed;
    [SerializeField] float gravity;
    Vector3 velocity;

    [SerializeField] float mouseSensitivity;

    PlayerState currentState;
    GroundedState groundedState = new GroundedState();
    AirborneState airborneState = new AirborneState();

    void Start()
    {
        currentState = groundedState;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        velocity = currentState.MovePlayer(transform);

        player.velocity = new Vector3(velocity.x * playerSpeed, player.velocity.y, velocity.z * playerSpeed);
    }
}
