using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParameters", menuName = "Scriptable Objects/Player Parameters")]
public class PlayerParameters : ScriptableObject
{
    public float speed;
    public float drag;

    public float turnSpeed;

    public float tilt;
    public float tiltSpeed;

    public float jumpDuration;
    public float jumpForce;
    public float jumpExponent;

    public float meleeDuration;
}
