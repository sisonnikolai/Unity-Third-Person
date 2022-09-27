using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    //properties
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Stamina Stamina { get; private set; }
    [field: SerializeField] public WeaponHandler WeaponHandler { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeDistance { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }

    public Transform MainCameraTransform { get; private set; }
    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        SwitchState(new PlayerFreeLookState(this));
        MainCameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new PlayerDeadState(this));
    }

    public void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        SwitchState(new PlayerHangingState(this, ledgeForward, closestPoint));
    }

    #region Animation Events
    private void HandleEquip()
    {
        SwitchState(new PlayerEquipState(this));
    }

    private void HandleUnequip()
    {
        SwitchState(new PlayerFreeLookState(this));
    }

    private void FootL() { }
    private void FootR() { }
    private void Hit() { }
    #endregion
}
