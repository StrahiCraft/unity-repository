using UnityEngine;

public class GroundedState : PlayerState
{
    public override Vector3 MovePlayer(Transform playerTransform)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        return playerTransform.right * x + playerTransform.forward * z;
    }
}
