using UnityEngine;
using System.Collections;




public class MecFootPlacer : MonoBehaviour
{

	//Protected member variables
	protected FootPlacementData mLeftFoot = null;
	protected FootPlacementData mRightFoot = null;
	protected Animator mAnim;

	//private meber variables
	private const float mEpsilon = 0.005f;

	private Vector3 mLeftFootContact_Ontransition_Disable;
	private Vector3 mLeftToeContact_Ontransition_Disable;
	private Vector3 mLeftHeelContact_Ontransition_Disable;


	private Vector3 mRightFootContact_Ontransition_Disable;
	private Vector3 mRightToeContact_Ontransition_Disable;
	private Vector3 mRightHeelContact_Ontransition_Disable;

	private bool mLeftFootActive = true;
	private bool mRightFootActive = true;


	//Functions
	/******************************************************/
	public void SetActive(AvatarIKGoal foot_id, bool active)
	{

		if(foot_id == AvatarIKGoal.LeftFoot)
		{
			if(mLeftFoot == null)
			{
				return;
			}

			if(active)
			{
				if(!mLeftFootActive)
				{
					ResetIKParams(foot_id);
				}
			}
			mLeftFootActive = active;
		}

		if(foot_id == AvatarIKGoal.RightFoot)
		{
			if(mRightFoot == null)
			{
				return;

			}

			if(active)
			{
				if(!mRightFootActive)
				{
					ResetIKParams(foot_id);
				}
			}
			mRightFootActive = active;
		}
	}
	

	/****************************************/
	public bool IsActive(AvatarIKGoal foot_id)
	{

		if(foot_id == AvatarIKGoal.LeftFoot)
		{
			return mLeftFootActive;
		}
		
		if(foot_id == AvatarIKGoal.RightFoot)
		{
			return mRightFootActive;
		}

		return false;
	}

	/*****************************************************/
	public float GetPlantBlendWeight(AvatarIKGoal foot_id)
	{
		FootPlacementData lFoot;
		
		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;
			
		default:
			return -1;
		}

		return lFoot.GetPlantBlendFactor();
	}

	/******************************************************************/
	public void SetPlantBlendWeight(AvatarIKGoal foot_id, float weight)
	{
		FootPlacementData lFoot;
		
		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;
			
		default:
			return;
		}
		
		lFoot.SetPlantBlendFactor(weight);
	}
	
	/***************************************************************/
	public void EnablePlant(AvatarIKGoal foot_id, float blend_speed)
	{
		FootPlacementData lFoot;

		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;
			
		default:
			return;
		}

		lFoot.EnablePlantBlend(blend_speed);
	}

	/***************************************************************/
	public void DisablePlant(AvatarIKGoal foot_id, float blend_speed)
	{
		FootPlacementData lFoot;

		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;
			
		default:
			return;
		}
		
		lFoot.DisablePlantBlend(blend_speed);
	}

	/**********************************************/
	private void ResetIKParams(AvatarIKGoal foot_id)
	{

		if(foot_id == AvatarIKGoal.LeftFoot)
		{
			Vector3 lFootPos = mAnim.GetBoneTransform(HumanBodyBones.LeftFoot).position;

			mLeftFoot.SetTargetFootWeight(0);
			mLeftFoot.SetCurrentFootWeight(0);
			mLeftFoot.SetGoalBlendSpeed(0);
			mLeftFoot.SetFootPlanted(false);

			mLeftFootContact_Ontransition_Disable = lFootPos;
			mLeftToeContact_Ontransition_Disable = mLeftFoot.mUpVector * mLeftFoot.mFootOffsetDist + mLeftFoot.GetRotatedFwdVec() * mLeftFoot.mFootLength;
			mLeftHeelContact_Ontransition_Disable = lFootPos + new Quaternion(0, 0.7071f, 0, 0.7071f) * mLeftFoot.GetRotatedFwdVec() * mLeftFoot.mFootHalfWidth;

		}

		if(foot_id == AvatarIKGoal.RightFoot)
		{
			Vector3 lFootPos = mAnim.GetBoneTransform(HumanBodyBones.RightFoot).position;

			mRightFoot.SetTargetFootWeight(0);
			mRightFoot.SetCurrentFootWeight(0);
			mRightFoot.SetGoalBlendSpeed(0);
			mRightFoot.SetFootPlanted(false);

			mRightFootContact_Ontransition_Disable = lFootPos;
			mRightToeContact_Ontransition_Disable = mRightFoot.mUpVector * mLeftFoot.mFootOffsetDist + mRightFoot.GetRotatedFwdVec() * mRightFoot.mFootLength;
			mRightHeelContact_Ontransition_Disable = lFootPos + new Quaternion(0, 0.7017f, 0, 0.7071f) * mRightFoot.GetRotatedFwdVec() * mRightFoot.mFootHalfWidth;
		}
	}


	/*************************************************************************************************/
	protected void SetIKWeight(AvatarIKGoal foot_id, float target_weight, float transition_time)
	{
		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			if(Mathf.Abs(target_weight - mLeftFoot.GetTargetFootWeight()) > mEpsilon)//to create linear motion and the blend speed should not be calculated if the condition is not true
			{
				if(transition_time != 0)
				{
					mLeftFoot.SetGoalBlendSpeed((target_weight - mLeftFoot.GetCurrentFootWeight())/transition_time);
				}
				else
				{
					mLeftFoot.SetGoalBlendSpeed(0.1f);
					mLeftFoot.SetCurrentFootWeight(target_weight);
				}
			}
			mLeftFoot.SetTargetFootWeight(target_weight);
			break;


		case AvatarIKGoal.RightFoot:
			if(Mathf.Abs(target_weight - mRightFoot.GetTargetFootWeight()) > mEpsilon)//to create linear motion and the blend speed should not be calculated if the condition is not true
			{
				if(transition_time != 0)
				{
					mRightFoot.SetGoalBlendSpeed((target_weight - mRightFoot.GetCurrentFootWeight())/transition_time);
				}
				else
				{
					mRightFoot.SetGoalBlendSpeed(0.1f);
					mRightFoot.SetCurrentFootWeight(target_weight);
				}
			}
			mRightFoot.SetTargetFootWeight(target_weight);
			break;
		}
	}

	/*********************************************************/
	protected void CalculateIKGoalWeights(AvatarIKGoal foot_id)
	{
		FootPlacementData lFoot;

		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
		
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;

		default:
			return;
		}
		

		float lSign = Mathf.Sign(lFoot.GetTargetFootWeight() - lFoot.GetCurrentFootWeight());
		lFoot.SetCurrentFootWeight( lFoot.GetCurrentFootWeight() + lFoot.GetGoalBlendSpeed() * Time.deltaTime);

		if(lSign * Mathf.Sign(lFoot.GetTargetFootWeight() - lFoot.GetCurrentFootWeight()) < 1 ||
		  Mathf.Abs(lFoot.GetCurrentFootWeight() - lFoot.GetTargetFootWeight()) < mEpsilon)
		{
			lFoot.SetCurrentFootWeight(lFoot.GetTargetFootWeight());
			return;
		}

		if(lFoot.GetCurrentFootWeight() > 1 || lFoot.GetCurrentFootWeight() < 0)
		{
			lFoot.SetCurrentFootWeight(lFoot.GetTargetFootWeight());
		}
	}

	/*************************************************/
	protected void CheckForLimits(AvatarIKGoal foot_id)
	{
		FootPlacementData lFoot;

		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;
			
		default:
			return;
		}

		//To check pitch limit of foot
		Vector3 lExtraOffset = Vector3.zero;
		Vector3 lRotatedFwdVec = lFoot.GetRotatedFwdVec();
		
		if(Vector3.Angle(lRotatedFwdVec, lFoot.GetTargetPos(FootPlacementData.Target.TOE)) > lFoot.mFootRotationLimit)
		{
			lExtraOffset = lFoot.mUpVector * lFoot.mFootLength * Mathf.Tan(lFoot.mFootRotationLimit * (Mathf.PI/180));

			if(Vector3.Angle(lFoot.mUpVector, lFoot.GetTargetPos(FootPlacementData.Target.TOE)) > 90)
			{
				lExtraOffset = -lExtraOffset;
			}

			lFoot.SetTargetPos(FootPlacementData.Target.TOE, (lRotatedFwdVec * lFoot.mFootLength) + lExtraOffset);
		}

		Quaternion lQuat90 = new Quaternion(0, 0.7071f, 0, 0.7071f);

		//To check roll limit of foot
		if(Vector3.Angle(lQuat90 * lRotatedFwdVec, lFoot.GetTargetPos(FootPlacementData.Target.HEEL)) > lFoot.mFootRotationLimit)
		{
			lExtraOffset = lFoot.mUpVector * lFoot.mFootHalfWidth * Mathf.Tan(lFoot.mFootRotationLimit * (Mathf.PI/180));
			
			if(Vector3.Angle(lFoot.mUpVector, lFoot.GetTargetPos(FootPlacementData.Target.HEEL)) > 90)
			{
				lExtraOffset = -lExtraOffset;
			}
			lFoot.SetTargetPos(FootPlacementData.Target.HEEL, (lQuat90 * lRotatedFwdVec * lFoot.mFootHalfWidth) + lExtraOffset);
		}
	}

	/***************************************************************/
	protected void UpdateFootPlantBlendWeights(AvatarIKGoal foot_id)
	{
		FootPlacementData lFoot;

		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;
			
		default:
			return;
		}

		if(lFoot.IsPlantOnTransition())
		{
			lFoot.SetPlantBlendFactor(lFoot.GetPlantBlendFactor() + lFoot.GetFootPlantBlendSpeed() * Time.deltaTime);
			
			if(lFoot.GetFootPlantBlendSpeed() > 0) //enbale foot plant
			{
				if(lFoot.GetPlantBlendFactor() > 1)
				{
					lFoot.SetPlantBlendFactor(1);
					lFoot.PlantBlendTransitionEnded();
				}
			}
			else
			{
				if(lFoot.GetPlantBlendFactor() < 0) // disable foot plant
				{
					lFoot.SetPlantBlendFactor(0);
					lFoot.PlantBlendTransitionEnded();
				}
			}
			
		}
	}

	/****************************************************/
	protected void FindContactPoints(AvatarIKGoal foot_id)
	{
		FootPlacementData lFoot;
		Vector3 lContactPos;
		Vector3 lFootPos = mAnim.GetIKPosition(foot_id);
		bool lContactDetected = true;


		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			lFoot = mRightFoot;
			break;
			
		default:
			return;
		}
	
		RaycastHit lHit;

		bool lHasCharacterController = false;
		if(GetComponent<CharacterController>() != null)
		{
			if(GetComponent<CharacterController>().enabled)
			{
				lHasCharacterController = true;
			}
		}

		if(lHasCharacterController && lFoot.mIgnoreCharacterControllerCollision)
		{
			GetComponent<CharacterController>().enabled = false;
		}

		if(Physics.Raycast(lFootPos + lFoot.mUpVector * lFoot.mFootOffsetDist, -lFoot.mUpVector, out lHit, lFoot.mFootOffsetDist + lFoot.mFootHeight + lFoot.mExtraRayDistanceCheck))
		{
			SetIKWeight(foot_id, 1, lFoot.mTransitionTime);

			{//Scope Start For lResult Var
				Vector3 lResult = lHit.point;

				if(lFoot.mPlantFoot && lFoot.GetFootPlanted())
				{
					if(Physics.Raycast(lFoot.GetPlantedPos() + lFoot.mUpVector * lFoot.mFootOffsetDist, -lFoot.mUpVector, out lHit, lFoot.mFootOffsetDist + lFoot.mFootHeight + lFoot.mExtraRayDistanceCheck))
					{
						lResult = Vector3.Lerp(lResult, lHit.point, lFoot.GetPlantBlendFactor());
					}
				}

				lFoot.SetTargetPos(FootPlacementData.Target.FOOT, lResult);
			}//Scope End

			if(foot_id == AvatarIKGoal.LeftFoot)
			{
				mLeftFootContact_Ontransition_Disable = lFoot.GetTargetPos(FootPlacementData.Target.FOOT);
			}
			else
			{
				mRightFootContact_Ontransition_Disable = lFoot.GetTargetPos(FootPlacementData.Target.FOOT);
			}
		
			lContactDetected = true;
		}
		else
		{
			SetIKWeight(foot_id, 0, lFoot.mTransitionTime);
			if(foot_id == AvatarIKGoal.LeftFoot)
			{
				lContactPos = mLeftFootContact_Ontransition_Disable;
			}
			else
			{
				lContactPos = mRightFootContact_Ontransition_Disable;
			}

			lFoot.SetTargetPos(FootPlacementData.Target.FOOT, Vector3.Lerp(lFootPos, lContactPos, lFoot.GetCurrentFootWeight()));
			lContactDetected = false;
		}

		if(Physics.Raycast(lFootPos + (lFoot.mUpVector * lFoot.mFootOffsetDist) + (lFoot.GetRotatedFwdVec() * lFoot.mFootLength), -lFoot.mUpVector, out lHit, 
		                   lFoot.mFootOffsetDist + lFoot.mFootLength + lFoot.mExtraRayDistanceCheck) && lContactDetected)
		{

			{//Scope Start For lResult Var
				Vector3 lResult = lHit.point;
				if(lFoot.mPlantFoot && lFoot.GetFootPlanted())
				{
					if(Physics.Raycast(lFoot.GetPlantedPos() + (lFoot.mUpVector * lFoot.mFootOffsetDist) + (lFoot.GetRotatedFwdVec() * lFoot.mFootLength), -lFoot.mUpVector, out lHit, 
					                lFoot.mFootOffsetDist + lFoot.mFootLength + lFoot.mExtraRayDistanceCheck))
					{
						lResult = Vector3.Lerp(lResult, lHit.point, lFoot.GetPlantBlendFactor());
					}
				}

				lFoot.SetTargetPos(FootPlacementData.Target.TOE, lResult - lFoot.GetTargetPos(FootPlacementData.Target.FOOT));
			}//Scope End
			
			if(foot_id == AvatarIKGoal.LeftFoot)             
			{
				mLeftToeContact_Ontransition_Disable = lFoot.GetTargetPos(FootPlacementData.Target.TOE);
			}
			else
			{
				mRightToeContact_Ontransition_Disable = lFoot.GetTargetPos(FootPlacementData.Target.TOE);
			}
		}
		else
		{
			if(foot_id == AvatarIKGoal.LeftFoot)
			{
				lContactPos = mLeftToeContact_Ontransition_Disable;
			}
			else
			{
				lContactPos = mRightToeContact_Ontransition_Disable;

			}

			lFoot.SetTargetPos(FootPlacementData.Target.TOE, 
			                   Vector3.Slerp(lFoot.GetRotatedFwdVec() * lFoot.mFootLength , lContactPos, lFoot.GetCurrentFootWeight()));
		}

		Quaternion lQuat90 = new Quaternion(0, 0.7071f, 0, 0.7071f);
	
		if(Physics.Raycast(lFootPos + (lFoot.mUpVector * lFoot.mFootOffsetDist) + ((lQuat90 * lFoot.GetRotatedFwdVec()).normalized * lFoot.mFootHalfWidth), -lFoot.mUpVector, out lHit, 
		                   lFoot.mFootOffsetDist + lFoot.mFootLength + lFoot.mExtraRayDistanceCheck) && lContactDetected)
		{
			{//Scope Start For lResult Var
				Vector3 lResult = lHit.point;

				if(lFoot.mPlantFoot && lFoot.GetFootPlanted())
				{
					if(Physics.Raycast(lFoot.GetPlantedPos() + (lFoot.mUpVector * lFoot.mFootOffsetDist) + ((lQuat90 * lFoot.GetRotatedFwdVec()).normalized * lFoot.mFootHalfWidth), -lFoot.mUpVector, out lHit, 
					                lFoot.mFootOffsetDist + lFoot.mFootLength + lFoot.mExtraRayDistanceCheck))
					{
						lResult = Vector3.Lerp(lResult, lHit.point, lFoot.GetPlantBlendFactor());
					}
				}

				lFoot.SetTargetPos(FootPlacementData.Target.HEEL, lResult - lFoot.GetTargetPos(FootPlacementData.Target.FOOT));
			}//Scope End

			if(foot_id == AvatarIKGoal.LeftFoot)
			{
				mLeftHeelContact_Ontransition_Disable = lFoot.GetTargetPos(FootPlacementData.Target.HEEL);

			}
			else
			{
				mRightHeelContact_Ontransition_Disable = lFoot.GetTargetPos(FootPlacementData.Target.HEEL);
			}
		}
		else
		{
			if(foot_id == AvatarIKGoal.LeftFoot)
			{
				lContactPos = mLeftHeelContact_Ontransition_Disable;
			}
			else
			{
				lContactPos = mRightHeelContact_Ontransition_Disable;
			}

			lFoot.SetTargetPos(FootPlacementData.Target.HEEL, 
			                   Vector3.Slerp((lQuat90 * lFoot.GetRotatedFwdVec() * lFoot.mFootHalfWidth), lContactPos, lFoot.GetCurrentFootWeight()));
		}

		if(lHasCharacterController && lFoot.mIgnoreCharacterControllerCollision)
		{
			GetComponent<CharacterController>().enabled = true;
		}
	}

	/*********************************************/
	public void FootPlacement(AvatarIKGoal foot_id)
	{

		FootPlacementData lFoot = null;

		switch(foot_id)
		{
		case AvatarIKGoal.LeftFoot:
			if(!mLeftFootActive)
			{
				return;
			}
			lFoot = mLeftFoot;
			break;
			
		case AvatarIKGoal.RightFoot:
			if(!mRightFootActive)
			{
				return;
			}
			lFoot = mRightFoot;
			break;
			
		default:
			return;
		}

		lFoot.mUpVector.Normalize();
		lFoot.mForwardVector.Normalize();

		//Update forward vec and IKHintoffset based on cahracter foot and body rotation
		lFoot.CalculateRotatedIKHint();
		lFoot.CalculateRotatedFwdVec();

		//Updating Plant Foot Blend Transitions
		UpdateFootPlantBlendWeights(foot_id);

		//Find exact contact points
		FindContactPoints(foot_id);
		
		//Check for feet limits
		CheckForLimits(foot_id);

		//Update IK goal weights
		CalculateIKGoalWeights(foot_id);
		
		/*Setup Final IK Rotaiton*/
		mAnim.SetIKRotationWeight(foot_id, lFoot.GetCurrentFootWeight());


		//converting the foot yaw rotation to degree
		float lAngle = Vector3.Angle(lFoot.GetRotatedFwdVec(), lFoot.mForwardVector);

		Quaternion lQuat90 = new Quaternion(0, 0.7071f, 0, 0.7071f);
		Vector3 lEulerRot = Quaternion.FromToRotation(lFoot.mForwardVector, lFoot.GetTargetPos(FootPlacementData.Target.TOE)).eulerAngles;
		lEulerRot.z = 0;
		Quaternion lRot = Quaternion.Euler(lEulerRot);
		if((lAngle > 90 && lAngle < 180)  || (lAngle > 270 && lAngle < 90))
		{
			lEulerRot = Quaternion.FromToRotation(lFoot.GetTargetPos(FootPlacementData.Target.HEEL), lQuat90 *  lFoot.GetRotatedFwdVec()).eulerAngles;
		}
		else
		{
			lEulerRot = Quaternion.FromToRotation(lQuat90 *  lFoot.GetRotatedFwdVec(), lFoot.GetTargetPos(FootPlacementData.Target.HEEL)).eulerAngles;
		}

		lEulerRot.x = 0;
		lEulerRot.y = 0;
		lRot = lRot * Quaternion.Euler(lEulerRot);
		mAnim.SetIKRotation(foot_id, lRot);

		/*Setup Final IK Swivel Angle*/
		Vector3 lIKHintDir = Vector3.zero;

		if(foot_id == AvatarIKGoal.LeftFoot)
		{
			lIKHintDir = mAnim.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position - mAnim.GetIKPosition(AvatarIKGoal.LeftFoot);//mAnim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
			mAnim.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, lFoot.GetCurrentFootWeight());

			mAnim.SetIKHintPosition(AvatarIKHint.LeftKnee, lFoot.GetTargetPos(FootPlacementData.Target.TOE) + lFoot.GetTargetPos(FootPlacementData.Target.FOOT)
			                        + lFoot.GetRotatedIKHint() + lIKHintDir);

		}

		if(foot_id == AvatarIKGoal.RightFoot)
		{
			lIKHintDir = mAnim.GetBoneTransform(HumanBodyBones.RightLowerLeg).position - mAnim.GetBoneTransform(HumanBodyBones.RightFoot).position;
			mAnim.SetIKHintPositionWeight(AvatarIKHint.RightKnee, lFoot.GetCurrentFootWeight());
			mAnim.SetIKHintPosition(AvatarIKHint.RightKnee, lFoot.GetTargetPos(FootPlacementData.Target.TOE) + lFoot.GetTargetPos(FootPlacementData.Target.FOOT)
			                        + lFoot.GetRotatedIKHint() + lIKHintDir);
		}

		/*Setup Final IK Position*/
		mAnim.SetIKPositionWeight(foot_id, lFoot.GetCurrentFootWeight());
		mAnim.SetIKPosition(foot_id, lFoot.GetTargetPos(FootPlacementData.Target.FOOT) + (lFoot.mUpVector * lFoot.mFootHeight));


		/*Update planted foot position and rotation*/
		if(lFoot.GetCurrentFootWeight() <= 0)
		{
			lFoot.SetFootPlanted(false);
		}
		else
		{
			if(lFoot.mPlantFoot && !lFoot.GetFootPlanted())
			{
				if(Mathf.Abs(lFoot.GetTargetFootWeight() - lFoot.GetCurrentFootWeight()) < mEpsilon)
				{
					lFoot.SetPlantedPos(mAnim.GetIKPosition(foot_id));//
					lFoot.SetPlantedRot(mAnim.GetIKRotation(foot_id));
					lFoot.SetFootPlanted(true);
				}
			}
		}
	}

	/***********/
	void Start () 
	{
		mAnim = GetComponent<Animator>();

		bool lLeftFootSet = false;
		bool lRightFootSet = false;
		FootPlacementData[] lFeet = GetComponents<FootPlacementData>();

		for(byte i = 0; i < lFeet.Length; i++)
		{
			if(lFeet[i].IsLeftFoot)
			{
				if(!lLeftFootSet)
				{
					mLeftFoot = lFeet[i];
					lLeftFootSet = true;
				}
			}
			else
			{
				if(!lRightFootSet)
				{
					mRightFoot = lFeet[i];
					lRightFootSet = true;
				}
			}

			if(lLeftFootSet && lRightFootSet)
			{
				break;
			}
		}
	}
	

	/*****************/
	void OnAnimatorIK()
	{
		if(mLeftFoot != null)
		{
			FootPlacement(AvatarIKGoal.LeftFoot);
		}

		if(mRightFoot != null)
		{
			FootPlacement(AvatarIKGoal.RightFoot);
		}
	}
}
