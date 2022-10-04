using UnityEngine;

public class AirborneState : PlayerState
{
    public override Vector3 MovePlayer(Transform playerTransform)
    {
        float x = Input.GetAxis("Horizontal") * .3f;
        float z = Input.GetAxis("Vertical") * .3f;

        return playerTransform.right * x + playerTransform.forward * z;
    }
}
