using UnityEngine;

public class FloorBot : MonoBehaviour
{
    public FloorBotParameters parameters;

    public Rigidbody rigidbody;

    public float angle;

    public float rotation;

    public bool randomStartAngle;
    public bool randomizeBounceAngle;

    private void Start()
    {
        if (randomStartAngle) angle = Random.Range(0, 360);
    }

    private void FixedUpdate()
    {
        rotation = Mathf.MoveTowardsAngle(rotation, angle, Time.fixedDeltaTime * parameters.turnSpeed);

        transform.rotation = Quaternion.Euler(0, rotation, 0);

        Vector3 force = Quaternion.Euler(0, angle, 0) * Vector3.forward * parameters.speed;

        force -= new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z) * parameters.drag;

        rigidbody.AddForce(force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.Destroyed();
        }

        ContactPoint contact = collision.GetContact(0);

        if (contact.normal.y > 0.95f) return;

        Vector3 direction = new Vector3(contact.normal.x, 0, contact.normal.z);

        angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

        if (randomizeBounceAngle) angle += Random.Range(-15, 15);
    }
}
