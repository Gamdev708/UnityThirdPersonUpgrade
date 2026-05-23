using UnityEngine;

public class Player : MonoBehaviour
{
    public InputReader input;
    public PlayerCamera camera;
    public PlayerParameters parameters;

    public Animator animator;
    public Rigidbody rigidbody;
    public SphereCollider sphereCollider;

    public Vector3 CenterPosition => sphereCollider.bounds.center;

    float lastUpdateRotation, nextUpdateRotation;
    float rotation;

    Vector2 targetTilt;
    Vector2 tilt;

    public enum State
    {
        idle, move, 
    }

    public State state;

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.idle:
                Idle();
                break;
            case State.move:
                Move();
                break;
        }
    }

    float timeOfFixedUpdate;

    private void Update()
    {
        float rotationLerp = Mathf.Clamp01((Time.time - timeOfFixedUpdate) / Time.fixedDeltaTime);

        tilt = Vector2.MoveTowards(tilt, targetTilt, Time.deltaTime * parameters.tiltSpeed);

        transform.rotation =

            Quaternion.RotateTowards(Quaternion.identity, 
            Quaternion.FromToRotation(Vector3.up, new Vector3(tilt.x, 0, tilt.y))
            , tilt.magnitude) 

            * Quaternion.Euler(0, Mathf.LerpAngle(lastUpdateRotation, nextUpdateRotation, rotationLerp), 0);
    }

    void Idle()
    {
        if (input.MovementValue.sqrMagnitude > 0)
        {
            state = State.move;
            animator.CrossFade("Move", 0.1f);
            Move();
            return;
        }

        lastUpdateRotation = nextUpdateRotation;

        targetTilt = Vector2.zero;

        Vector3 force = -new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z) * parameters.drag;

        rigidbody.AddForce(force);
    }
    void Move()
    {
        if (input.MovementValue.sqrMagnitude == 0)
        {
            state = State.idle;
            animator.CrossFade("Idle", 0.2f);
            Idle();
            return;
        }

        Vector3 movementInput = camera.MovementRotation * new Vector3(input.MovementValue.x, 0, input.MovementValue.y);

        lastUpdateRotation = nextUpdateRotation;
        timeOfFixedUpdate = Time.time;

        float targetRotation = Vector3.SignedAngle(Vector3.forward, movementInput, Vector3.up);
        nextUpdateRotation = Mathf.MoveTowardsAngle(lastUpdateRotation, targetRotation, Time.fixedDeltaTime * parameters.turnSpeed);

        targetTilt = new Vector2(movementInput.x, movementInput.z) * parameters.tilt;

        Vector3 force = movementInput
            // uncomment for more robotic forward only movement
            //* Mathf.Max(0, Vector3.Dot(movementInput.normalized, Quaternion.Euler(0, nextUpdateRotation, 0) * Vector3.forward)) 
            * parameters.speed;

        force -= new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z) * parameters.drag;

        rigidbody.AddForce(force);
    }
    void Melee()
    {

    }
    void Shoot()
    {

    }
}
