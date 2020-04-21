using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollController : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        animator.gameObject.GetComponent<Collider2D>().isTrigger = true;
        Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("Enemy"), LayerMask.GetMask(""));
        animator.gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
        animator.gameObject.GetComponent<Collider2D>().isTrigger = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Managers.Player.OnRollGoing();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Managers.Player.OnRollExit();
        Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("Enemy"), LayerMask.GetMask("Default","Player"));
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
