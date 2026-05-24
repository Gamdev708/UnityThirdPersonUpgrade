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

    public Vector3 cannonPosition;
    public Vector3 cannonRotation;

    public float shootDuration;
    public AnimationCurve shootAimCurve;

    public float shotLifetime;
    public float shotDistance;
    public AnimationCurve shotScale;
    public AnimationCurve shotAlpha;

    public float getCannonDuration;
}
