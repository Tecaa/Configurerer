using UnityEngine;
using System.Collections;

public class LocomotionUpdate : StateMachineBehaviour {

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
	{
		animator.GetComponent<MecFootPlacer>().EnablePlant(AvatarIKGoal.LeftFoot, 2);
		animator.GetComponent<MecFootPlacer>().EnablePlant(AvatarIKGoal.RightFoot, 2);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
	{

		float lCurrentSpeedFactor = animator.GetFloat("speed");
		float lTime = animatorStateInfo.normalizedTime - Mathf.Floor(animatorStateInfo.normalizedTime);
		float lBlendedTime = 0.5f - 0.25f * lCurrentSpeedFactor;

		FootPlacementData[] lFeet = animator.GetComponents<FootPlacementData>();
		FootPlacementData lFoot;

		//First foot setup start
		if(!lFeet[0].IsLeftFoot)
		{
			lFoot = lFeet[0];
		}
		else
		{
			lFoot = lFeet[1];
		}

		//Setting up transition time
		lFoot.mTransitionTime = 0.15f - (0.1f * lCurrentSpeedFactor);

		//Setting up raycast extra ray dist
		if(lTime < lBlendedTime)
		{
			lFoot.mExtraRayDistanceCheck = 0.7f;
		}
		else
		{
			lFoot.mExtraRayDistanceCheck = -0.2f;
		}
		//First foot setup end


		//Second foot setup start
		if(lFeet[0].IsLeftFoot)
		{
			lFoot = lFeet[0];
		}
		else
		{
			lFoot = lFeet[1];
		}

		//Setting up transition time
		lFoot.mTransitionTime = 0.15f - (0.1f * lCurrentSpeedFactor);

		//Setting up raycast extra ray dist
		if(lTime > 0.5 && lTime < 0.5f + lBlendedTime)
		{
			lFoot.mExtraRayDistanceCheck = 0.7f;
		}
		else
		{
			lFoot.mExtraRayDistanceCheck = -0.2f;
		}
		//Second foot setup end
	}
}
