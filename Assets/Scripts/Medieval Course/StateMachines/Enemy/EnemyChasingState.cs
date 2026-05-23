using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    private readonly int LocomotionSpeedHash = Animator.StringToHash("Speed");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.SetDestination(stateMachine.Player.transform.position);
        FacePlayer();
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

        if (!IsInChaseRange())
        {
            Debug.Log("Out Of Range");
            // Transition to Idle State
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        stateMachine.Animator.SetFloat(LocomotionSpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;

            if (!stateMachine.Agent.pathPending)
            {
                // Calculate direction manually from the agent's path
                // rather than relying on desiredVelocity which is affected by agent speed
                Vector3 direction = (stateMachine.Agent.steeringTarget - stateMachine.transform.position).normalized;

                Move(direction * stateMachine.MovementSpeed, deltaTime);
            }

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        }
    }

    private bool IsInAttackRange()
    {
        if (stateMachine.Player.isDead)
        {
            return false;
        }

        float playerDistanceSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
