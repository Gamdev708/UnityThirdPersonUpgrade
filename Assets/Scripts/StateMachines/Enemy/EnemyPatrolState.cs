using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private int currentWaypointIndex = 0;
    private bool isReversing = false;
    private bool isIdling = false;
    private float idleTimer = 0f;

    public EnemyPatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        isIdling = false;
        idleTimer = 0f;
        Debug.Log("Entering Patrol State");
        MoveToCurrentWaypoint();
    }

    public override void Tick(float deltaTime)
    {
        if (IsInChaseRange())
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            return;
        }

        if (isIdling)
        {
            Move(deltaTime); // Apply gravity while idling
            idleTimer -= deltaTime;
            if (idleTimer <= 0f)
            {
                isIdling = false;
                AdvanceWaypoint();
                MoveToCurrentWaypoint();
            }
            return;
        }

        // Physically move the enemy using CharacterController
        if (!stateMachine.Agent.pathPending &&
            stateMachine.Agent.remainingDistance > stateMachine.Agent.stoppingDistance)
        {
            Vector3 direction = (stateMachine.Agent.steeringTarget -
                stateMachine.transform.position).normalized;

            Move(direction * stateMachine.MovementSpeed, deltaTime);
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
            FaceWaypoint();
        }
        else if (!stateMachine.Agent.pathPending &&
            stateMachine.Agent.remainingDistance <= stateMachine.Agent.stoppingDistance)
        {
            isIdling = true;
            idleTimer = stateMachine.PatrolIdleDuration;
            stateMachine.Animator.SetFloat("Speed", 0f);
        }
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    // ------------------------------------------------------------------

    private void MoveToCurrentWaypoint()
    {
        if (stateMachine.PatrolWaypoints == null || stateMachine.PatrolWaypoints.Length == 0)
        {
            Debug.Log("No waypoints found!");
            return;
        }

        Debug.Log($"Moving to waypoint {currentWaypointIndex}: {stateMachine.PatrolWaypoints[currentWaypointIndex].position}");
        Debug.Log($"Agent enabled: {stateMachine.Agent.enabled}");
        Debug.Log($"Agent isOnNavMesh: {stateMachine.Agent.isOnNavMesh}");
        Debug.Log($"Agent updatePosition: {stateMachine.Agent.updatePosition}");

        stateMachine.Agent.SetDestination(stateMachine.PatrolWaypoints[currentWaypointIndex].position);
        stateMachine.Animator.SetFloat("Speed", stateMachine.MovementSpeed);
    }

    private void AdvanceWaypoint()
    {
        if (stateMachine.PatrolWaypoints == null || stateMachine.PatrolWaypoints.Length == 0)
            return;

        int last = stateMachine.PatrolWaypoints.Length - 1;

        if (!isReversing)
        {
            if (currentWaypointIndex < last)
            {
                currentWaypointIndex++;
            }
            else
            {
                // Reached the final waypoint — randomly loop or ping-pong
                if (Random.value < 0.5f)
                {
                    // Loop: jump straight back to the start
                    currentWaypointIndex = 0;
                }
                else
                {
                    // Ping-pong: reverse direction
                    isReversing = true;
                    currentWaypointIndex--;
                }
            }
        }
        else
        {
            if (currentWaypointIndex > 0)
            {
                currentWaypointIndex--;
            }
            else
            {
                // Reached the start while reversing — go forward again
                isReversing = false;
                currentWaypointIndex++;
            }
        }
    }

    private void FaceWaypoint()
    {
        Vector3 lookPos = stateMachine.PatrolWaypoints[currentWaypointIndex].position -
            stateMachine.transform.position;
        lookPos.y = 0f;

        if (lookPos.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(lookPos);

        stateMachine.transform.rotation = Quaternion.RotateTowards(
            stateMachine.transform.rotation,
            targetRotation,
            stateMachine.Agent.angularSpeed * Time.deltaTime
        );
    }
}