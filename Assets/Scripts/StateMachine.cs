using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : StateMachineBehaviour
{

    public bool isAttacking = false;
    string previousState = "none";
    string currentState = "none";
    string run = "Player_Run";
    string idle = "Player_Idle";
    string jump = "Player_Jump";

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        previousState = currentState;
        if (stateInfo.IsName(idle))
            currentState = idle;
        else if (stateInfo.IsName(run))
            currentState = run;
        else if (stateInfo.IsName(jump))
            currentState = jump;
        else
            currentState = "other";

        //Debug.Log(currentState);

        if (previousState.Equals(jump) && currentState.Equals(idle))
            FindObjectOfType<PlayerModelFlip>().Footstep();

        if (previousState.Equals(jump) && currentState.Equals(run))
            FindObjectOfType<PlayerModelFlip>().Footstep();

        if (stateInfo.IsTag("attack"))
            animator.transform.parent.GetComponent<Player1Controller>().isAttacking = true;
        else
            animator.transform.parent.GetComponent<Player1Controller>().isNotAttacking();
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMachineEnter is called when entering a statemachine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
    //
    //}

    // OnStateMachineExit is called when exiting a statemachine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
    //
    //}
}
