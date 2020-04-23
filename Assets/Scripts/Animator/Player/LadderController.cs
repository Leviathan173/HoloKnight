using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger(PAParameters.CLIMB_STAT, 1);
        //Debug.Log("ladder stat enter");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger(PAParameters.FALL_DOWN_LADDER);
            Managers.Player.FallDownLadder();
        }
        float delteY = Input.GetAxis("Vertical");
        if (!Mathf.Approximately(delteY, 0) && NotInExitOrStartStat(animator)){
            if (delteY > 0) {
                //animator.SetInteger(PAParameters.ANIME_PLAY_DELTA, 1); // 默认就是1
                animator.SetInteger(PAParameters.CLIMB_STAT, 2);
                Managers.Player.LadderMoveUp();
                
            } else {
                animator.SetFloat(PAParameters.ANIME_PLAY_DELTA, -1);
                animator.SetInteger(PAParameters.CLIMB_STAT, 2);
                Managers.Player.LadderMoveDown();
            }
        }
    }

    public static bool NotInExitOrStartStat(Animator animator) {
        return (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != PAStat.LADDER_BOTTOM &&
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != PAStat.CLIMB_TO_LADDER_TOP_END &&
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != PAStat.MOVE_IN_LADDER);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //    Debug.Log("exit ladder");
    //}

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
