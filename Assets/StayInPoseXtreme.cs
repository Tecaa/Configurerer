﻿using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StayInPoseXtreme : AnimationBehaviour {

	private StayInPoseState stayInPoseState;

	/// <summary>
	/// Esta variable se utiliza para comprobar si es primera vez que se entra al este behaviour
	/// </summary>
	[HideInInspector]
	private event EventHandler LerpRoundTripEnd;
	public new StayInPoseXtreme CentralNode
	{
		get
		{
			if (_centralNode == null)
				_centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement);
			return (StayInPoseXtreme)_centralNode;
		}
	}
	/// <summary>
	/// Se utiliza para enviar datos a ExerciseDataGenerator en intervalos de tiempo determinados.
	/// El tiempo que ha pasado desde que se hizo la última captura de datos.
	/// </summary>
	private float timeSinceCapture = 0;
	protected void OnLerpRoundTripEnd()
	{
		
		if (!IsCentralNode)
		{
			this.CentralNode.OnLerpRoundTripEnd();
		}
		else
		{
			EventHandler eh = LerpRoundTripEnd;
			if (eh != null)
			{
				eh(this, new EventArgs());
			}
		}
	}
	
	void LerpBehaviour_LerpRoundTripEnd(object sender, EventArgs e)
	{
		if (_BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
		{
			this.CentralNode.endRepTime = DateTime.Now;
			
		}/*
        if (IsInterleaved && this.limb == Limb.Right)
        {
            (this._Opposite as FiniteBehaviour).endRepTime = endRepTime;
        }*/
	}
	
	public BehaviourParams GetParams()
	{
		return this._currentParams;
	}
	protected override AnimationBehaviourState _BehaviourState
	{
		get
		{
			
			if (this.IsCentralNode)
				return this._behaviourState;
			else
				return this.CentralNode._behaviourState;
		}
		set
		{
			if (this.IsCentralNode)
				this._behaviourState = value;
			else
				this.CentralNode._behaviourState = value;
			switch (value)
			{
			case AnimationBehaviourState.RUNNING_DEFAULT:
			case AnimationBehaviourState.RUNNING_WITH_PARAMS:
				animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
				break;
			case AnimationBehaviourState.PREPARING_WITH_PARAMS:
				//StartLerp();
				break;
			}
		}
	}
	
	override public void Prepare(BehaviourParams bp)
	{
		BehaviourParams lp = (BehaviourParams)bp;
		this.CentralNode._RealParams = lp;
		this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
		this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
		//if (IsInterleaved)
		//	this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;
		
	}
	
	
	private void _Opposite_RepetitionEnd(object sender, EventArgs e)
	{
        Debug.Log("_Opposite_RepetitionEnd");
		OnRepetitionEnd();
	}
	override protected void PrepareWebInternal()
	{
		this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
	}
	override public void Run()
	{
		this.CentralNode.endRepTime = null;
		if (this.IsInterleaved)
		{
			this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
		}
		this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;

        //Debug.Log("cambiando a " + _BehaviourState + " " + this.IsCentralNode + "  " + this.GetInstanceID());
		this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
		this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
	}
	
	
	override public void RunWeb()
	{
		this.CentralNode.endRepTime = null;
		if (this.IsInterleaved)
		{
			this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
		}
		this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
		if (this.randomAnimations == null)
		{
			this.randomAnimations = new List<Exercise>();
			List<AnimationBehaviour> friends = AnimationBehaviour.getFriendBehaviours(this.movement);
			foreach(AnimationBehaviour a in friends)
			{
				randomAnimations.Add(new Exercise(a.movement, a.laterality, a.limb));
			}
		}
		
		this.CentralNode.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
		this.CentralNode.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
	}
	override public void RunWeb(BehaviourParams bp)
	{
		
		BehaviourParams p = (BehaviourParams)bp;
		this.CentralNode.endRepTime = null;
		this.CentralNode._RealParams = p;
		this.initializeRandomAnimations(bp.Variations);
		this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
		this.CentralNode.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
		this.CentralNode.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
	}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this._BehaviourState == AnimationBehaviourState.PREPARING_WEB || this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
        {
            Debug.Log("OnStateEnter");
            OnRepetitionEnd();
            Stop();
        }
        else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS
                 || this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            //Como en este behaviour se utiliza animation.Play para cada repetición, se entra más de una vez al metodo OnStateEnter, 
            //por lo que si ya se ha entrado alguna vez, la velocidad se asigna como 0 para que se respete el tiempo entre ejecución 
            //antes de comenzar la siguiente repetición.

            //Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.

            animator.speed = 1f;

        }

    }

    float startHoldTime;
	float startRestTime;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

        if (this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)//Testear si esto funciona en este behaviour.
		{
			animator.speed = 0;
			return;
		}
		
		float DELTA = 0.05f;
		
		DateTime temp = DateTime.Now;
        //Debug.Log("state!!: " + stateInfo.normalizedTime);
        //if(endRepTime == null)
        //{
        //    DebugLifeware.Log("asddsa", DebugLifeware.Developer.Alfredo_Gallardo    );
        //}
        if (IsCentralNode)
        {
            Debug.Log("pausa entre repeticiones");
            //SetNextVariation();
        }

        if (this._BehaviourState != AnimationBehaviourState.STOPPED && (this.CentralNode.endRepTime == null || new TimeSpan(0, 0, (int)this.CentralNode._RealParams.SecondsBetweenRepetitions) <= temp - this.CentralNode.endRepTime))
		{
            
            if (!beginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
			    this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
			    this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
			    this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
			{
				OnRepetitionReallyStart();
				beginRep = true;
			}
            //            Debug.Log(stayInPoseState + "  " + (Time.time - startRestTime));
            //Debug.Log(stateInfo.normalizedTime);
            //Debug.Log("resta tieme" + Time.time);
            //Debug.Log("resta tieme" + startHoldTime);
            //Debug.Log("secondin pose: " + this.CentralNode._RealParams.SecondsInPose);
            if (stayInPoseState == StayInPoseState.GoingTo &&  Math.Abs(stateInfo.normalizedTime - 1) <= DELTA)
			{
                //DebugLifeware.Log("Manteniendo", DebugLifeware.Developer.Marco_Rojas);
                Debug.Log("pasando a HoldingOn");
				animator.speed = 0;
				startHoldTime = Time.time;
				stayInPoseState = StayInPoseState.HoldingOn;
				//Esperar
			}
			
			//Si ya pasó el tiempo en el ángulo máximo
			else if(stayInPoseState == StayInPoseState.HoldingOn && Time.time - startHoldTime >= this.CentralNode._RealParams.SecondsInPose)
			{
                //DebugLifeware.Log("Para atrás", DebugLifeware.Developer.Marco_Rojas);
                Debug.Log("estoy en holdgingON pose");
				animator.StartRecording(0);
				animator.speed = -1;
				animator.StopRecording();
				stayInPoseState = StayInPoseState.Leaving;
			}
			else if (stayInPoseState == StayInPoseState.Leaving && Math.Abs(stateInfo.normalizedTime - 0) <= DELTA)
			{
                Debug.Log("estoy en leaving pose");
                beginRep = false;
				animator.speed = 1;
				stayInPoseState = StayInPoseState.Resting;
				startRestTime = Time.time;
                animator.SetInteger("Movement", -1);
				
				/*if (!this.IsWeb && _BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS && (!IsInterleaved || (IsInterleaved && limb == Limb.Right)))
					this.PauseAnimation();*/
				if (IsInterleaved && this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
				{
					//DebugLifeware.Log("cambiando limb", DebugLifeware.Developer.Marco_Rojas);
					animator.SetTrigger("ChangeLimb");
				}
                Debug.Log("Leaving");
				
				if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    _BehaviourState = AnimationBehaviourState.STOPPED;

                OnRepetitionEnd();

            }
			
			else if (stayInPoseState == StayInPoseState.Resting && Time.time - startRestTime>= this.CentralNode._RealParams.SecondsBetweenRepetitions)
			{
                Debug.Log("estoy en resting pose");
                //DebugLifeware.Log("descansando", DebugLifeware.Developer.Marco_Rojas);
                animator.speed = 1;
				stayInPoseState = StayInPoseState.GoingTo;
				
			}
			//DebugLifeware.Log("termino", DebugLifeware.Developer.Marco_Rojas);
		}
	}
	
	
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("onStateExit iscentralNOde:" + this.IsCentralNode + " estado " + this._BehaviourState);
        if (!IsCentralNode)
		{
			beginRep = false;
			if (this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
			{
				OnLerpRoundTripEnd();
                Debug.Log("Se va a cambiar de variacion desde " + (int)this.CentralNode.randomAnimations[(int)this.CentralNode.actualRandomAnimationIndex].Movement);
                //SetNextVariation();
                Debug.Log("Se va a cambiar de variacion hacia " + (int)this.CentralNode.randomAnimations[(int)this.CentralNode.actualRandomAnimationIndex].Movement);
                if (!IsInterleaved || (IsInterleaved && limb == Limb.Right))
				{
                    if (!this.IsWeb)
					{
						this.PauseAnimation();
					}
                }
                if (IsInterleaved)
				{
					animator.SetTrigger("ChangeLimb");
				}
				if (!IsInterleaved && this.IsWeb)
				{
					animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
				}
				if (this._BehaviourState == AnimationBehaviourState.STOPPED)
				{
					this.CentralNode.endRepTime = null;
				}
			}
			else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
			{
                Debug.Log("else preparing with params");
                OnRepetitionEnd();
				Stop();
			}
		}
	}
    protected override void OnRepetitionEnd()
	{
        if (!IsCentralNode)
            this.CentralNode.OnRepetitionEnd();
        else
        {
            Debug.Log("Final");
            base.OnRepetitionEnd();
        }
	}
	/// <summary>
	/// Detiene la interpolación que actualmente se está ejecutando
	/// </summary>
	override public void Stop()
	{/*
        this._BehaviourState = AnimationBehaviourState.STOPPED;
        if ((_Opposite as FiniteBehaviour)._BehaviourState != AnimationBehaviourState.STOPPED)
            _Opposite.Stop();

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;*/
		//this._BehaviourState = AnimationBehaviourState.STOPPED;
		animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
		
		this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
		//Debug.Log("2Cambiando a " + this._BehaviourState + " "  + this.IsCentralNode  + " "  + this.GetInstanceID());
		//animator.speed = 1;
		
	}
	
	void OnDestroy()
	{
		if (this.IsInterleaved)
			this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;
		
		this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
		
		base.OnDestroy();
	}
}
