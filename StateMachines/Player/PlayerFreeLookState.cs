using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int EquipHash = Animator.StringToHash("EquipFromBack");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    private bool shouldFade; // for the climbing animation
    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine) 
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.EquipEvent += OnEquip;

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);
        if (shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }
        else
        {
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }
    }

    public override void Tick(float deltaTime)
    {
        var attackStamina = stateMachine.WeaponHandler.Weapon.Attacks[0].StaminaUsage;

        if (stateMachine.InputReader.IsAttacking && stateMachine.Stamina.GetStamina() >= attackStamina)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, stateMachine.InputReader.MovementValue.magnitude, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.EquipEvent -= OnEquip;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationDamping * deltaTime);
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SetTarget()) { return; }
        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }
    private void OnEquip()
    {
        //stateMachine.SwitchState(new PlayerEquipState(stateMachine));
        stateMachine.WeaponHandler.SetWeaponEquip(true);
        stateMachine.Animator.CrossFadeInFixedTime(EquipHash, CrossFadeDuration);
    }
}
