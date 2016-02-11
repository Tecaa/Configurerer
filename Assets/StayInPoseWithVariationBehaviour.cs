using UnityEngine;
using System.Collections;

public class StayInPoseWithVariationBehaviour : AnimationBehaviour {

	private StayInPoseWithVariationBehaviour _centralNode;
	public StayInPoseWithVariationBehaviour CentralNode
	{
		get
		{
			if (_centralNode == null)
				_centralNode = (StayInPoseWithVariationBehaviour)AnimationBehaviour.GetCentralBehaviour(this.movement);
			return _centralNode;
		}
	}

	#region implemented abstract members of AnimationBehaviour

	public override void Prepare (BehaviourParams bp)
	{
		BehaviourParams lp = (BehaviourParams)bp;
		this.CentralNode._RealLerpParams = lp;
		this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
		this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
		//if (IsInterleaved)
			//this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;

	}

	protected override void PrepareWebInternal ()
	{
		throw new System.NotImplementedException ();
	}

	public override void Run ()
	{
		Debug.Log ("hola");
	}

	public override void RunWeb ()
	{
		throw new System.NotImplementedException ();
	}

	public override void RunWeb (BehaviourParams lerpParams)
	{
		throw new System.NotImplementedException ();
	}

	public override void Stop ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
