using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public InputReader input;
    public CameraParameters parameters;
    public Player player;

    public Vector2 targetEuler;

    private void LateUpdate()
    {
        targetEuler += input.CameraTurnValue() * parameters.sensitivity;

        targetEuler.x = Mathf.Clamp(targetEuler.x, -89, 89);

        Quaternion targetRotation = Quaternion.Euler(targetEuler);

        transform.rotation = targetRotation;

        Vector3 playerOrigin = player.CenterPosition + Vector3.up;

        transform.position = playerOrigin + targetRotation * parameters.localPosition;

        Vector3 playerToCamera = transform.position - playerOrigin;
        if (Physics.SphereCast(playerOrigin, 0.4f, playerToCamera, out RaycastHit hit, playerToCamera.magnitude, 1))
        {
            transform.position = hit.point + hit.normal * 0.4f;
        }
    }

    public Quaternion MovementRotation => Quaternion.Euler(0, targetEuler.y, 0);
}
