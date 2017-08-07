﻿using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StayInPoseWithVariationBehaviour : AnimationBehaviour
{

    private uint storedNextExercice;
    private bool hasEnteredBefore = false;
    private event EventHandler StayInPoseWithVariationRoundTripEnd;
    protected override bool HasCentralNode { get { return true; } }
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

            if (this.IsCentralNode)
            {
                return this;
            }
            else
            {
                if (_centralNode == null)
                    _centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement, this.limb);
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


    #region implemented abstract members of AnimationBehaviour

    public override void Prepare(BehaviourParams bp)
    {
        BehaviourParams lp = (BehaviourParams)bp;
        this.CentralNode._RealParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
        this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));

        /*
        int i = 0;
        foreach (Movement x in this.CentralNode.randomAnimations)
            Debug.Log(i++ + " " + x);*/
    }

    protected override void PrepareWebInternal()
    {
        this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
    }

    public override void Run()
    {
        Debug.Log("Run : " + (int)this.CentralNode.currentRandomAnimationIndex % this.CentralNode.randomAnimations.Count + " " + this.CentralNode.randomAnimations[(int)this.CentralNode.currentRandomAnimationIndex % this.CentralNode.randomAnimations.Count]);
        AnimatorScript.instance.CurrentExercise.Movement = this.CentralNode.randomAnimations[(int)this.CentralNode.currentRandomAnimationIndex % this.CentralNode.randomAnimations.Count];

        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed;
        clockBehaviour.stopExecutionTimer();
        clockBehaviour.stopTimeBetweenRepetitionsTimer();
    }

    public override void RunWeb()
    {

        this.CentralNode._RealParams = new BehaviourParams();
        this.initializeRandomAnimations();
        this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
        this.CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed;
    }

    public override void RunWeb(BehaviourParams lerpParams)
    {


        if (_BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
        {
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
            clockBehaviour.stopExecutionTimer();
            clockBehaviour.stopTimeBetweenRepetitionsTimer();
            isRewinding = true;
            this.CentralNode.currentRandomAnimationIndex = 0;
            startNewExecution();
        }
        else
        {
            this.CentralNode._RealParams = lerpParams;

            this.initializeRandomAnimations();
            this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
        }

        this.CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed;
    }

    /// <summary>
    /// Detiene la interpolación que actualmente se está ejecutando
    /// </summary>
    override public void Stop()
    {
        exerciceMovement = (int)Movement.Iddle;
        this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
        CurrentSpeed = 1;
    }
    #endregion


    private void executionTimerStart()
    {
        isExecutingMovement = true;
        OnRepetitionReallyStart();
        this.OnRepetitionHoldOn();
    }

    private void executionTimerFinish()
    {
        isExecutingMovement = false;
        if (_isAnimationRunning)
        {
            finishRepetitionExecution();
        }

        if (IsWeb == true || IsInInstruction == true)
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

        SetNextVariation();
        this.CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed;

    }

    private void pauseBetweenRepetitionsStart()
    {
        OnRepetitionStart();
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

        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (this.IsCentralNode)
                CurrentSpeed = 0;
            return;
        }
        IsRewinding = false;
        if (this.IsCentralNode && this.exerciceMovement == -1)
            OnRepetitionEnd();

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

            CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!IsCentralNode)
        {
            BeginRep = false;

            OnSubrepetitionEnd();

            if (_isAnimationRunning == true)
            {

                if (isExecutingMovement == true && exerciceMovement != -1 && !IsRewinding)
                {
                    SetNextVariation();
                }

            }

        }
        else
        {
            if (exerciceMovement > 0)
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
            if (!animator.IsInTransition(0))
                CurrentSpeed = 0;
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

    DateTime lastSubRepTime = DateTime.Now;
    DateTime LastSubRepTime
    {
        get
        {

            if (this.IsCentralNode)
                return lastSubRepTime;
            else
                return CentralNode.lastSubRepTime;
        }
        set
        {

            if (this.IsCentralNode)
                lastSubRepTime = value;
            else
                CentralNode.LastSubRepTime = value;

        }
    }
    protected override void OnSubrepetitionEnd()
    {
        // Parche que evita que se dispare dos veces consecutivas el evento OnSubRepetitionEnd
        TimeSpan MILISECONDS_ERROR = new TimeSpan(0, 0, 0, 0, 500);

        if (DateTime.Now - CentralNode.LastSubRepTime <= MILISECONDS_ERROR)
            return;
        if (!IsCentralNode)
        {
            this.CentralNode.OnSubrepetitionEnd();
        }
        else
        {


            CentralNode.LastSubRepTime = DateTime.Now;
            base.OnSubrepetitionEnd();

        }

    }

    public BehaviourParams GetParams()
    {
        return this._currentParams;
    }

    private void finishRepetitionExecution()
    {
        exerciceMovement = -1;
        this.PauseAnimation();
        clockBehaviour.stopExecutionTimer();
    }



    public override void ResumeAnimation()
    {
        IsResumen = true;
        if (IsRepetitionEnd)
        {
            base.ResumeAnimation();
            clockBehaviour.executeTimeBetweenRepetitions(secondsBetweenRepetitions);
            IsResumen = false;
        }
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

        base.OnDestroy();
        try
        {
            clockBehaviour.pauseBetweenRepetitionsStart -= pauseBetweenRepetitionsStart;
            clockBehaviour.pauseBetweenRepetitionsFinish -= pauseBetweenRepetitionsFinish;
        }
        catch
        {

        }

    }
}






