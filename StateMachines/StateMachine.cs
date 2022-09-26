using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
        //if (transform.CompareTag("Enemy")) { Debug.Log(currentState); }
    }

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
}
