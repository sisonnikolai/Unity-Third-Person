using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    private float time = 5f;

    public override void Enter()
    {
        //stateMachine.InputReader.JumpEvent += OnJump;
        //Debug.Log("Enter");
    }
    public override void Tick(float deltaTime)
    {
        //time -= deltaTime;
        //int timeRemaining = Mathf.RoundToInt(time);
        //Debug.Log(timeRemaining);
        //if (time <= 0f)
        //{
        //    stateMachine.SwitchState(new PlayerTestState(stateMachine));
        //}

        Vector3 movement = CalculateMovement();

        stateMachine.CharacterController.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0f, 0.1f, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat("FreeLookSpeed", 1f, 0.1f, deltaTime);
        stateMachine.transform.rotation = Quaternion.LookRotation(movement);
        //stateMachine.transform.Translate(movement * deltaTime);
    }
    public override void Exit()
    {
        //stateMachine.InputReader.JumpEvent -= OnJump;
        //Debug.Log("Exit");
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerTestState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 camForward = stateMachine.MainCameraTransform.forward;
        Vector3 camRight = stateMachine.MainCameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        return camForward * stateMachine.InputReader.MovementValue.y + camRight * stateMachine.InputReader.MovementValue.x;
    }
}
