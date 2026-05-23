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

        transform.position = player.CenterPosition + targetRotation * parameters.localPosition;

        Vector3 playerToCamera = transform.position - player.CenterPosition;
        if (Physics.SphereCast(player.CenterPosition, 0, playerToCamera, out RaycastHit hit, playerToCamera.magnitude, 1))
        {
            transform.position = hit.point;
        }
    }

    public Quaternion MovementRotation => Quaternion.Euler(0, targetEuler.y, 0);
}
