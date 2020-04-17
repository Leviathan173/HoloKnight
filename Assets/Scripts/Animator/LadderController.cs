using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger(AParameters.CLIMB_STAT, 1);
        Debug.Log("ladder stat enter");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Space)) {
            animator.SetTrigger(AParameters.FALL_DOWN_LADDER);
            animator.SendMessage("FallDownLadder", SendMessageOptions.DontRequireReceiver);
        }
        float delteY = Input.GetAxis("Vertical");
        if (!Mathf.Approximately(delteY, 0) && NotInExitStat(animator)){
            if (delteY > 0) {
                //animator.SetInteger(AParameters.ANIME_PLAY_DELTA, 1); // 默认就是1
                animator.SetInteger(AParameters.CLIMB_STAT, 2);
                animator.SendMessage("LadderMoveUp", SendMessageOptions.DontRequireReceiver);
                
            } else {
                animator.SetFloat(AParameters.ANIME_PLAY_DELTA, -1);
                animator.SetInteger(AParameters.CLIMB_STAT, 2);
                animator.SendMessage("LadderMoveDown", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private bool NotInExitStat(Animator animator) {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != Astat.LADDER_BOTTOM ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != Astat.CLIMB_TO_LADDER_TOP_END ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != Astat.MOVE_IN_LADDER) {
            return true;
        } else {
            return false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Debug.Log("exit ladder");
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
