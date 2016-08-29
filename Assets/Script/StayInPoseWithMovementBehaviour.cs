using UnityEngine;
using System.Collections;
using System;
using Assets;
using System.Collections.Generic;
using System.Threading;

public class StayInPoseWithMovementBehaviour : AnimationBehaviour {

    protected override bool HasCentralNode { get { return false; } }
    private StayInPoseState stayInPoseState;
    [HideInInspector]
    public bool haCambiadoDeEstado = false;


    public BehaviourParams GetParams()
    {
        return this._currentParams;
    }

    override public void Prepare(BehaviourParams sp)
    {
        this._RealParams = sp;
        this._behaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;
    }

    private void _Opposite_RepetitionEnd(object sender, EventArgs e)
    {
        OnRepetitionEnd();
    }
    override protected void PrepareWebInternal()
    {

        this._behaviourState = AnimationBehaviourState.PREPARING_WEB;
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;
    }
    override public void Run()
    {
        endRepTime = null;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }
        BeginRep = false;

        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.animator.speed = this._RealParams.ForwardSpeed;
    }

    override public void RunWeb()
    {
        endRepTime = null;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
        }

        this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
        this.animator.speed = this._RealParams.ForwardSpeed;
    }
    override public void RunWeb(BehaviourParams stayInParams)
    {
        endRepTime = null;
        
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
	

        this._RealParams = stayInParams;
        this.animator.speed = this._RealParams.ForwardSpeed;
    }
    
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (this._currentParams == null)
            this._currentParams = new BehaviourParams();
        if(this._realParams == null)
        {
            this._realParams = new BehaviourParams();
        }
        if (_behaviourState == AnimationBehaviourState.PREPARING_WEB)
        {
            OnRepetitionEnd();
            this.Stop();
        }
    
        if (!haCambiadoDeEstado)
        {
            haCambiadoDeEstado = true;
        }
	}

    float startHoldTime;

    private bool repetitionStartFlag = false;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (!animator.IsInTransition(0))
                animator.speed = 0;
            return;
        }

        DateTime temp = DateTime.Now;

        if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
        {
            animator.speed = 0;
            stayInPoseState = StayInPoseState.Resting;
            BeginRep = false;
            this.Stop();
            OnRepetitionEnd();
        }

        else if (_BehaviourState != AnimationBehaviourState.STOPPED && (endRepTime == null || new TimeSpan(0, 0, (int)_RealParams.SecondsBetweenRepetitions) <= temp - endRepTime))
        {

            if (!BeginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
            {

                OnRepetitionReallyStart();
                BeginRep = true;
                animator.speed = this._RealParams.ForwardSpeed;
                startHoldTime = Time.time;
                stayInPoseState = StayInPoseState.HoldingOn;
                repetitionStartFlag = true;
            }

            //Si ya pasó el tiempo indicado realizando el movimiento
            if (stayInPoseState == StayInPoseState.HoldingOn && (Time.time - startHoldTime) >= this._RealParams.SecondsInPose )
            {
                DebugLifeware.Log("Tiempo en pose maxima = " + (Time.time - startHoldTime).ToString(), DebugLifeware.Developer.Alfredo_Gallardo);
                animator.speed = 0;
                stayInPoseState = StayInPoseState.Resting;
                BeginRep = false;
                if ((!this.IsWeb) && (!this.IsInInstruction) && this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    this.PauseAnimation();
                else if((this.IsWeb) || (this.IsInInstruction) && this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    endRepTime = DateTime.Now;
                }
                OnRepetitionEnd();
            }

            
        }
        else if (endRepTime != null && _BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
        {
            if (repetitionStartFlag)
            {
                OnRepetitionStart();
                repetitionStartFlag = false;
            }
        }


    }


	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.speed = this._RealParams.ForwardSpeed;
	}

	


    public override void Stop()
    {

        _behaviourState = AnimationBehaviourState.STOPPED;
        if (IsInterleaved)
        {
            if ((_Opposite as StayInPoseWithMovementBehaviour)._behaviourState != AnimationBehaviourState.STOPPED)
                _Opposite.Stop();
        }

        animator.speed = this._RealParams.ForwardSpeed;

        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
    }
    void OnDestroy()
    {
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;
        base.OnDestroy();
    }
}