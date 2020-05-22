using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackD : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //Managers.Player.AddFrontForce(0);
        Managers.Player.AttackDCheck();
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
        Managers.Player.currentStamina -= Managers.Player.AttackCost;
        Managers.Player.bar.UpdateSp();
        Managers.Player.AddFrontForce();
        Managers.Player.audio.PlayOneShot(Managers.Player.attack);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger(PAParameters.ATTACKSTAT, -1);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
