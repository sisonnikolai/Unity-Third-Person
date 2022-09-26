using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        var movement = (motion + stateMachine.ForceReceiver.Movement);
        stateMachine.CharacterController.Move(movement * deltaTime);
    }

    protected void FaceTarget()
    {
        if (stateMachine.Targeter.CurrentTarget != null)
        {
            var lookAt = (stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position);
            lookAt.y = 0f;
            //stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(lookAt), stateMachine.RotationDamping * Time.deltaTime);
            stateMachine.transform.rotation = Quaternion.LookRotation(lookAt);
        }
    }

    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
