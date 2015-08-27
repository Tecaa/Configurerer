using UnityEngine;
using System.Collections;

public class FootPlacementData : MonoBehaviour 
{
	public enum Target
	{
		FOOT = 0,
		TOE = 1,
		HEEL = 2
	}

	public bool IsLeftFoot;
	public bool mIgnoreCharacterControllerCollision = true;
	public bool mPlantFoot = true;

	public Vector3 mForwardVector = new Vector3(0, 0, 1);
	public Vector3 mIKHintOffset = new Vector3(0, 0, 0);
	public Vector3 mUpVector = new Vector3(0, 1, 0);

	public float mFootOffsetDist = 0.5f;
	public float mFootLength = 0.22f;
	public float mFootHalfWidth = 0.05f;
	public float mFootHeight = 0.1f;
	public float mFootRotationLimit = 45;
	public float mTransitionTime = 0.2f;
	public float mExtraRayDistanceCheck = 0.0f;
	

	protected bool mFootPlantIsOnTransition = false;
	protected float mFootPlantBlendSpeed;

	protected Vector3 mTargetPos = new Vector3(0,0,0);
	protected Vector3 mTargetToePos = new Vector3(0,0,0);
	protected Vector3 mTargetHeelPos = new Vector3(0,0,0);
	
	protected Vector3 mRotatedFwdVec;
	protected Vector3 mRotatedIKHintOffset;

	protected float mTargetFootWeight = 0;
	protected float mCurrentFootWeight = 0;
	protected float mGoalBlendSpeed = 0;
	protected float mPlantBlendFactor = 0;

	private bool mFootPlanted = false;
	private Vector3 mFootPlantedPos;
	private Quaternion mFootPlantedRot;

	/*****************************************/
	public void SetTargetPos(Target target, Vector3 target_pos)
	{
		switch(target)
		{
		case Target.FOOT:
			mTargetPos = target_pos;
			break;
		
		case Target.TOE:
			mTargetToePos = target_pos;
			break;

		case Target.HEEL:
			mTargetHeelPos = target_pos;
			break;
		}
	}

	/*****************************************************/
	public Vector3 GetTargetPos(Target target)
	{
		switch(target)
		{
		case Target.FOOT:
			return mTargetPos;
			
		case Target.TOE:
			return mTargetToePos;
			
		case Target.HEEL:
			return mTargetHeelPos;
		}

		return Vector3.zero;
	}

	/**********************************/
	public void CalculateRotatedFwdVec()
	{
		AvatarIKGoal lFootID = AvatarIKGoal.LeftFoot;

		if(!IsLeftFoot)
		{
			lFootID = AvatarIKGoal.RightFoot;
		}

		float lAngle = 0;
		Quaternion lYawRotation;

		lAngle = GetComponent<Animator>().GetIKRotation(lFootID).eulerAngles.y * Mathf.PI/180;
		lYawRotation = new Quaternion(0, Mathf.Sin(lAngle * 0.5f), 0, Mathf.Cos(lAngle * 0.5f));
		

		if(mFootPlanted && mPlantFoot)
		{
			lAngle = mFootPlantedRot.eulerAngles.y * Mathf.PI/180;
			lYawRotation = Quaternion.Slerp(lYawRotation, new Quaternion(0, Mathf.Sin(lAngle * 0.5f), 0, Mathf.Cos(lAngle * 0.5f)), mPlantBlendFactor);
		}
		mRotatedFwdVec = lYawRotation * mForwardVector.normalized;

	}

	/*******************************/
	public Vector3 GetRotatedFwdVec()
	{
		return mRotatedFwdVec;
	}

	/*******************************************/
	public void CalculateRotatedIKHint()
	{
		float lAngle = transform.rotation.eulerAngles.y * Mathf.PI/180;
		Quaternion lYawRotation = new Quaternion(0, Mathf.Sin(lAngle * 0.5f), 0, Mathf.Cos(lAngle * 0.5f));

		mRotatedIKHintOffset = lYawRotation * mIKHintOffset;
	}

	/*******************************************/
	public Vector3 GetRotatedIKHint()
	{
		return mRotatedIKHintOffset;
	}

	/*******************************************/
	public void SetTargetFootWeight(float weight)
	{
		mTargetFootWeight = weight;
	}

	/*********************************/
	public float GetTargetFootWeight()
	{
		return mTargetFootWeight;
	}

	/*******************************************/
	public void SetCurrentFootWeight(float weight)
	{
		mCurrentFootWeight = weight;
	}
	
	/*********************************/
	public float GetCurrentFootWeight()
	{
		return mCurrentFootWeight;
	}

	/*******************************************/
	public void SetGoalBlendSpeed(float speed)
	{
		mGoalBlendSpeed = speed;
	}
	
	/*********************************/
	public float GetGoalBlendSpeed()
	{
		return mGoalBlendSpeed;
	}

	/*********************************/
	public float GetPlantBlendFactor()
	{
		return mPlantBlendFactor;
	}
	
	/*******************************************/
	public void SetPlantBlendFactor(float factor)
	{
		mPlantBlendFactor = factor;
	}

	/******************************************************************/
	public void EnablePlantBlend(float blend_speed)
	{
		mFootPlantBlendSpeed = Mathf.Abs(blend_speed);
		mFootPlantIsOnTransition = true;
	}

	/******************************************************************/
	public void DisablePlantBlend(float blend_speed)
	{
		mFootPlantBlendSpeed = -Mathf.Abs(blend_speed);
		mFootPlantIsOnTransition = true;
	}

	/**********************************/
	public float GetFootPlantBlendSpeed()
	{
		return mFootPlantBlendSpeed;
	}

	/***********************************/
	public void PlantBlendTransitionEnded()
	{
		mFootPlantIsOnTransition = false;
	}

	/***********************************/
	public bool IsPlantOnTransition()
	{
		return mFootPlantIsOnTransition;
	}
	
	/**************************************/
	public void SetFootPlanted(bool planted)
	{
		mFootPlanted = planted;
	}

	/***************************/
	public bool GetFootPlanted()
	{
		return mFootPlanted;
	}

	/********************************************/
	public void SetPlantedPos(Vector3 planted_pos)
	{
		mFootPlantedPos = planted_pos;
	}

	/*****************************/
	public Vector3 GetPlantedPos()
	{
		return mFootPlantedPos;
	}

	/*******************************************/
	public void SetPlantedRot(Quaternion planted_rot)
	{
		mFootPlantedRot = planted_rot;
	}

	/*******************************/
	public Quaternion GetPlantedRot()
	{
		return mFootPlantedRot;
	}

}
