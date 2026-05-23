using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("PullUp");

    private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);

    private const float CrossFadeDuration = 0.1f;

    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Climbing") < 1f)
        {
            return;
        }

        stateMachine.Controller.enabled = false; //deactivate character controller when updating player position
        stateMachine.transform.Translate(Offset, Space.Self); //Change player position after pullup animation is finished
        stateMachine.Controller.enabled = true; // reactivate the character controller

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero); // stops the character controler from unwanted forward movement
        stateMachine.ForceReceiver.Reset(); // resets gravity and stops velocity build up from hanging
    }
}
