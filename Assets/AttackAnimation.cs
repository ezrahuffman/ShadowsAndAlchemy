using UnityEngine;

public class AttackAnimation : StateMachineBehaviour
{
    bool ended = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("started animation");
        ended = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log($"normalized time: {stateInfo.normalizedTime}");
        if (stateInfo.normalizedTime >= 1 && !ended)
        {
            Debug.Log("Finnished attack anim in animator");
            animator.gameObject.GetComponentInParent<MageController>().FinishAttack();
            ended = true;
        } 
    }
}
