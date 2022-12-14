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

    // moved from PlayerFreeLookState
    protected Vector3 CalculateMovement()
    {
        Vector3 camForward = stateMachine.MainCameraTransform.forward;
        Vector3 camRight = stateMachine.MainCameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        return camForward * stateMachine.InputReader.MovementValue.y + camRight * stateMachine.InputReader.MovementValue.x;
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
            stateMachine.transform.rotation = Quaternion.LookRotation(lookAt);
        }
    }

    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget != null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
        else if (stateMachine.WeaponHandler.IsWeaponEquipped())
        {
            stateMachine.SwitchState(new PlayerEquipState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}
