﻿using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StayInPoseWithVariationBehaviour : AnimationBehaviour {

    private uint storedNextExercice;
    private bool hasEnteredBefore = false;
    private event EventHandler StayInPoseWithVariationRoundTripEnd;
    private bool isRewinding = false;
    private bool IsRewinding
    {
        get
        {

            if (this.IsCentralNode)
                return isRewinding;
            else
                return CentralNode.isRewinding;
        }
        set
        {

            if (this.IsCentralNode)
                isRewinding = value;
            else
                CentralNode.IsRewinding = value;

        }
    }
	public new StayInPoseWithVariationBehaviour CentralNode
	{
		get
		{
            
            if( this.IsCentralNode)
            {
                return this;
            }
            else
            {
                if (_centralNode == null)
                    _centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement);
                return (StayInPoseWithVariationBehaviour)_centralNode;
            }
            
            
		}
	}

    private float SecondsBetweenRepetitions;
    public float secondsBetweenRepetitions
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._realParams.SecondsBetweenRepetitions;
            }
            else
            {
                return CentralNode._realParams.SecondsBetweenRepetitions;
            }

        }
    }

    private bool IsExecutingMovement = false;
    public bool isExecutingMovement
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this.IsExecutingMovement;
            }
            else
            {
                return CentralNode.IsExecutingMovement;
            }

        }
        set
        {
            if (this.IsCentralNode)
            {
                this.IsExecutingMovement = value;
            }
            else
            {
                CentralNode.IsExecutingMovement = value;
            }

        }
    }


    //
    //==== Controlar tiempos de repeticiones
    //


    public ClockBehaviour _ClockBehaviour;
    public ClockBehaviour clockBehaviour
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._ClockBehaviour;
            }
            else
            {
                return CentralNode._ClockBehaviour;
            }

        }
        set
        {
            if (this.IsCentralNode)
            {
                this._ClockBehaviour = value;
            }
            else
            {
                CentralNode._ClockBehaviour = value;
            }
        }
    }

    public float secondsInPose
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._realParams.SecondsInPose; 
            }
            else
            {
                return CentralNode._realParams.SecondsInPose;
            }

        }
    }



    //
    //==== FIN Controlar tiempos de repeticiones
    //


    //===== FIN RELOJ




    #region implemented abstract members of AnimationBehaviour

    public override void Prepare (BehaviourParams bp)
	{
		BehaviourParams lp = (BehaviourParams)bp;
		this.CentralNode._RealParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
		this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
	}

	protected override void PrepareWebInternal ()
	{
        this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
    }

	public override void Run ()
	{

		this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;

	}

	public override void RunWeb ()
    {

        this.CentralNode._RealParams = new BehaviourParams();
        Debug.Log("cayendo en el run web sin parametros");
        this.initializeRandomAnimations();
        this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
    }

	public override void RunWeb (BehaviourParams lerpParams)
	{
        

        if (_behaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
        {
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
            clockBehaviour.stopExecutionTimer();
            clockBehaviour.stopTimeBetweenRepetitionsTimer();
            isRewinding = true;
            this.CentralNode.actualRandomAnimationIndex = 0;
            startNewExecution();
        }
        else
        {
            this.CentralNode._RealParams = lerpParams;
            
            this.initializeRandomAnimations();
            this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
        }





    }

	/// <summary>
	/// Detiene la interpolación que actualmente se está ejecutando
	/// </summary>
	override public void Stop()
	{
        exerciceMovement = (int)Movement.Iddle;
		this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
		animator.speed = 1;
	}
    #endregion


    private void executionTimerStart()
    {
        Debug.Log("comienza ejecucion| HORA: " + DateTime.Now.ToString());
        isExecutingMovement = true;
    }

    private void executionTimerFinish()
    {
        Debug.Log("termina ejecucion| HORA: " + DateTime.Now.ToString());
        isExecutingMovement = false;

        if (_isAnimationRunning)
        {
            finishRepetitionExecution();
        }

        if(IsWeb == true)
        {
            this.ResumeAnimation();
        }

    }


    /**
    Cuando se acaba el tiempo de pausa entre repeticiones
    **/
    private void pauseBetweenRepetitionsFinish()
    {
        clockBehaviour.stopTimeBetweenRepetitionsTimer();
        Debug.Log("termina pausa entre repeticiones| HORA: " + DateTime.Now.ToString());
        SetNextVariation();
        animator.speed = 1;

    }

    private void pauseBetweenRepetitionsStart()
    {
        Debug.Log("comienza pausa entre repeticiones| HORA: " + DateTime.Now.ToString());
    }

    protected void OnStayInPoseWithVariationRoundTripEnd()
	{
		
		if (!IsCentralNode)
		{
			this.CentralNode.OnStayInPoseWithVariationRoundTripEnd();
		}
		else
		{
			EventHandler eh = StayInPoseWithVariationRoundTripEnd;
			if (eh != null)
			{
				eh(this, new EventArgs());
			}
		}
	}
	



    private void startNewExecution()
    {

        clockBehaviour.executeRepetitionTime(secondsInPose);
    }


	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        IsRewinding = false;
        if (this.IsCentralNode && hasEnteredBefore == false)
        {
            hasEnteredBefore = true;

            clockBehaviour = new ClockBehaviour();
            clockBehaviour.executionTimerFinish += executionTimerFinish;
            clockBehaviour.executionTimerStart += executionTimerStart;
            clockBehaviour.pauseBetweenRepetitionsStart += pauseBetweenRepetitionsStart;
            clockBehaviour.pauseBetweenRepetitionsFinish += pauseBetweenRepetitionsFinish;
            
        }


            

        if (_isAnimationPreparing)
		{
			Debug.Log ("esta preparando");
			OnRepetitionEnd();
			Stop();
            clockBehaviour.stopExecutionTimer();
        }
		else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS
		         || this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
		{
			//Como en este behaviour se utiliza animation.Play para cada repetición, se entra más de una vez al metodo OnStateEnter, 
			//por lo que si ya se ha entrado alguna vez, la velocidad se asigna como 0 para que se respete el tiempo entre ejecución 
			//antes de comenzar la siguiente repetición.
			
			//Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.
			
			animator.speed = this.CentralNode._RealParams.ForwardSpeed;
			
		}
		
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!IsCentralNode)
		{
			beginRep = false;

			if ( _isAnimationRunning == true )
			{

                if (isExecutingMovement == true && exerciceMovement != -1 && !IsRewinding)
                {
                    SetNextVariation();
                }
               
			}


		}
		else
		{


            if(animator.GetInteger("Movement") > 0)
            {
                    startNewExecution();
            }
                

        }

	}




    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        clockBehaviour.Update();

        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            animator.speed = 0;
            return;
        }

        if (_isAnimationPreparing || _isAnimationStopped)
            return;

    }


    protected override void OnRepetitionEnd()
	{
        
		if (!IsCentralNode)
        {
            this.CentralNode.OnRepetitionEnd();
        }	
		else
        {
            base.OnRepetitionEnd();
            
        }
			
	}

	public BehaviourParams GetParams()
	{
		return this._currentParams;
	}

    private void finishRepetitionExecution()
    {
        Debug.Log("se termina la ejecucion");
        exerciceMovement = -1;
        OnRepetitionEnd();
        this.PauseAnimation();
        clockBehaviour.stopExecutionTimer();
    }



    public override void ResumeAnimation()
    {
        base.ResumeAnimation();
        clockBehaviour.executeTimeBetweenRepetitions(secondsBetweenRepetitions);
    }


    public override void PauseAnimation()
    {
        originalABS = this._BehaviourState;


        if (this.CentralNode != null)
        {
            this.CentralNode.originalABS = this._BehaviourState;
        }

        this._BehaviourState = AnimationBehaviourState.STOPPED;
        
        if (IsInterleaved)
        {
            if (this.limb == Limb.Right)
                this._Opposite.PauseAnimation();
        }
    }

    new void OnDestroy()
    {
        /**
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
         **/

        //animatorClock.pauseBetweenRepetitionsStart -= pauseBetweenRepetitionsStart;
        //animatorClock.pauseBetweenRepetitionsFinish -= pauseBetweenRepetitionsFinish;

        base.OnDestroy();
    }

    

}






