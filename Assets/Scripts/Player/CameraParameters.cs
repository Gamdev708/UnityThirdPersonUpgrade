using UnityEngine;

[CreateAssetMenu(fileName = "CameraParameters", menuName = "Scriptable Objects/Camera Parameters")]
public class CameraParameters : ScriptableObject
{
    public Vector3 localPosition;

    public float sensitivity;
}
