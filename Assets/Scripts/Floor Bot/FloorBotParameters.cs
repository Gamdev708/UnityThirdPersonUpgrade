using UnityEngine;

[CreateAssetMenu(fileName = "FloorBotParameters", menuName = "Scriptable Objects/Floor Bot Parameters")]
public class FloorBotParameters : ScriptableObject
{
    public float speed;
    public float drag;

    public float turnSpeed;
}
