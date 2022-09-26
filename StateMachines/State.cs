using UnityEngine;

public abstract class State //abstract classes can only be inherited. The abstract functions are required when inheriting from an abstract class
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextState.IsTag(tag))
        {
            return nextState.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentState.IsTag(tag))
        {
            return currentState.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
