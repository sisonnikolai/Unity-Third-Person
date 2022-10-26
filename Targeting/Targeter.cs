using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup CinemachineTargetGroup;
    private Camera mainCamera;
    private List<Target> targets = new List<Target>();
    public Target CurrentTarget { get; private set; }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
            target.OnDestroyedEvent += RemoveTarget;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            RemoveTarget(target);
        }
    }

    public bool SetTarget()
    {
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        Vector2 centerScreen = new Vector2(0.5f, 0.5f);

        foreach (Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            Vector2 toCenter = viewPos - centerScreen;
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        CinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        return true;
    }

    public void CancelTarget()
    {
        if (CurrentTarget == null) { return; }
        CinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    public void SwitchTarget()
    {
        if (CurrentTarget == null) { return; }
        if (targets.Count <= 1) { return; }

        Target previousTarget = CurrentTarget;
        Target nextTarget = null;
        float closestTargetDistance = Mathf.Infinity;
        Vector2 centerScreen = new Vector2(0.5f, 0.5f);

        foreach (Target target in targets)
        {
            if (target == previousTarget) { continue; }
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            Vector2 toCenter = viewPos - centerScreen;
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                nextTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }
        CurrentTarget = nextTarget;
        CinemachineTargetGroup.RemoveMember(previousTarget.transform);
        CinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
    }

    private void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            CinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }
        target.OnDestroyedEvent -= RemoveTarget;
        targets.Remove(target);
    }
}
