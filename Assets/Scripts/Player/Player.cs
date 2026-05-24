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
        move, jump, melee, shoot
    }

    public State state;

    float timer;

    public bool isIdle;
    public bool jumpWasReleased;
    public bool attackWasReleased;
    public bool shootWasReleased;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.move:
                Move();
                break;
            case State.jump:
                Jump();
                break;
            case State.melee:
                Melee();
                break;
            case State.shoot:
                Shoot();
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

    void Move()
    {
        if (!isIdle && input.MovementValue.sqrMagnitude == 0)
        {
            isIdle = true;
            animator.CrossFade("Idle", 0.2f);
        }
        else if (isIdle && input.MovementValue.sqrMagnitude > 0)
        {
            isIdle = false;
            animator.CrossFade("Move", 0.1f);
        }

        if (input.IsHoldingJump && jumpWasReleased && Physics.SphereCast(transform.position, 0.3f, Vector3.down, out RaycastHit hit, 0.1f, 1))
        {
            state = State.jump;
            jumpWasReleased = false;
            timer = 0;
            animator.CrossFade("Move", 0.1f);
            Jump();
            return;
        }

        if (input.IsAttacking && attackWasReleased)
        {
            state = State.melee;
            attackWasReleased = false;
            timer = 0;
            animator.CrossFade("Melee", 0.1f);
            Melee();
            return;
        }

        Vector3 movementInput = camera.MovementRotation * new Vector3(input.MovementValue.x, 0, input.MovementValue.y);

        lastUpdateRotation = nextUpdateRotation;
        timeOfFixedUpdate = Time.time;

        if (!isIdle)
        {
            float targetRotation = Vector3.SignedAngle(Vector3.forward, movementInput, Vector3.up);
            nextUpdateRotation = Mathf.MoveTowardsAngle(lastUpdateRotation, targetRotation, Time.fixedDeltaTime * parameters.turnSpeed);
        }

        targetTilt = new Vector2(movementInput.x, movementInput.z) * parameters.tilt;

        Vector3 force = movementInput
            // uncomment for more robotic forward only movement
            //* Mathf.Max(0, Vector3.Dot(movementInput.normalized, Quaternion.Euler(0, nextUpdateRotation, 0) * Vector3.forward)) 
            * parameters.speed;

        force -= new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z) * parameters.drag;

        rigidbody.AddForce(force);

        jumpWasReleased = !input.IsHoldingJump;
        attackWasReleased = !input.IsAttacking;
        shootWasReleased = true;
    }
    void Jump()
    {
        timer += Time.fixedDeltaTime;
        float phase = timer / parameters.jumpDuration;

        if (phase > 1)
        {
            state = State.move;
            animator.CrossFade("Move", 0.1f);
            Move();
            return;
        }

        Vector3 movementInput = camera.MovementRotation * new Vector3(input.MovementValue.x, 0, input.MovementValue.y);

        lastUpdateRotation = nextUpdateRotation;
        timeOfFixedUpdate = Time.time;

        float targetRotation = Vector3.SignedAngle(Vector3.forward, movementInput, Vector3.up);
        nextUpdateRotation = Mathf.MoveTowardsAngle(lastUpdateRotation, targetRotation, Time.fixedDeltaTime * parameters.turnSpeed);

        targetTilt = new Vector2(movementInput.x, movementInput.z) * parameters.tilt * 0.5f;

        Vector3 force = movementInput * parameters.speed;

        if (input.IsHoldingJump)
        {
            force.y += parameters.jumpForce * Mathf.Pow(1 - phase, parameters.jumpExponent);
        }

        force -= new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z) * parameters.drag;

        rigidbody.AddForce(force);
    }
    void Melee()
    {
        timer += Time.fixedDeltaTime;

        float phase = timer / parameters.meleeDuration;

        if (phase > 1)
        {
            state = State.move;
            animator.CrossFade("Move", 0.1f);
            Move();
            return;
        }

        Vector3 movementInput = camera.MovementRotation * new Vector3(input.MovementValue.x, 0, input.MovementValue.y);

        lastUpdateRotation = nextUpdateRotation;
        timeOfFixedUpdate = Time.time;

        // uncomment to enable turning while attacking
        //if (input.MovementValue.sqrMagnitude > 0)
        //{
        //    float targetRotation = Vector3.SignedAngle(Vector3.forward, movementInput, Vector3.up);
        //    nextUpdateRotation = Mathf.MoveTowardsAngle(lastUpdateRotation, targetRotation, Time.fixedDeltaTime * parameters.turnSpeed);
        //}

        targetTilt = Vector2.zero;

        Vector3 force = movementInput * parameters.speed * 0.5f;

        force -= new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z) * parameters.drag;

        rigidbody.AddForce(force);
    }
    void Shoot()
    {

    }
}
