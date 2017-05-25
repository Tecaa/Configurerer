﻿using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FiniteVariationBehaviour : AnimationBehaviour
{
    protected override bool HasCentralNode { get { return true; } }

    /// <summary>
    /// Esta variable se utiliza para comprobar si es primera vez que se entra al este behaviour
    /// </summary>
    [HideInInspector]
    private event EventHandler LerpRoundTripEnd;
    public FiniteVariationBehaviour CentralNode
    {
        get
        {
            if (_centralNode == null)
                _centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement, this.limb);
			return (FiniteVariationBehaviour)_centralNode;
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
        this._RealParams = bp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
        timeSinceCapture = 0;
        this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;

        int i = 0;
        foreach (Movement x in this.CentralNode.randomAnimations)
            Debug.Log(i++ + " " + x);
    }

    
    private void _Opposite_RepetitionEnd(object sender, EventArgs e)
    {
        OnRepetitionEnd();
    }
    override protected void PrepareWebInternal()
    {
        this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
    }
    override public void Run()
    {
        Debug.Log("Run : " + (int)this.CentralNode.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count + " " + this.CentralNode.randomAnimations[(int)this.CentralNode.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count]);
        AnimatorScript.instance.CurrentExercise.Movement = this.CentralNode.randomAnimations[(int)this.CentralNode.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count];

        this.CentralNode.endRepTime = null;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed; //this.animator.speed = this.CentralNode._RealParams.ForwardSpeed;
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
        this.initializeRandomAnimations();

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
        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (this.IsCentralNode)
                CurrentSpeed = 0; //animator.speed = 0;
            return;
        }

        if (this._BehaviourState == AnimationBehaviourState.PREPARING_WEB || this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
        {
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

            CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed; //animator.speed = this.CentralNode._RealParams.ForwardSpeed;

        }

    }


    private bool repetitionStartFlag = false;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (!animator.IsInTransition(0) && this.IsCentralNode)
            {
                CurrentSpeed = 0; //animator.speed = 0;
            }
                return;
        }

        const float INTERVAL = 0.1f;
        if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
        {
            timeSinceCapture += Time.deltaTime;
            if (timeSinceCapture > INTERVAL)
            {
                timeSinceCapture = timeSinceCapture - INTERVAL;
                //if (exerciseDataGenerator == null)
                //    exerciseDataGenerator = GameObject.FindObjectOfType<ExerciseDataGenerator>();
                //TODO: rescatar de base de datos o diccionario
                //TODO: rescatar captureData
                //DebugLifeware.Log("grabando frame ", DebugLifeware.Developer.Marco_Rojas);
                //if (this.exerciseDataGenerator != null)
                //    this.exerciseDataGenerator.captureData(ActionDetector.ActionDetector.DetectionMode.BoundingBoxBased);
            }
        }

        DateTime now = DateTime.Now;
        if (this.CentralNode.endRepTime != null && _BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT &&
            new TimeSpan(0, 0, (int)this.CentralNode._RealParams.SecondsBetweenRepetitions) > now - this.CentralNode.endRepTime)
        {
            CurrentSpeed = 0; //animator.speed = 0;
        }

        if ((_BehaviourState != AnimationBehaviourState.STOPPED && _BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
            && (this.CentralNode.endRepTime == null || new TimeSpan(0, 0, (int)this.CentralNode._RealParams.SecondsBetweenRepetitions) <= now - this.CentralNode.endRepTime))
        {

            if (!BeginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
            {
                Debug.Log("supuesto really start" + "  " + stateInfo.normalizedTime + " " + CurrentSpeed); 
                this.CentralNode.OnRepetitionReallyStart();
                BeginRep = true;
                repetitionStartFlag = true;
            }

            //Debug.Log("ha cambiado de estado " + haCambiadoDeEstado);
            
            if (stateInfo.normalizedTime < 1.0f)
            {
                if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    if (CurrentSpeed != this.CentralNode._RealParams.ForwardSpeed && stateInfo.normalizedTime <= 0.5f)
                    {
                        CurrentSpeed = this.CentralNode._RealParams.ForwardSpeed;
                    }
                    else if (CurrentSpeed != this.CentralNode._RealParams.BackwardSpeed && stateInfo.normalizedTime > 0.5f)
                    {
                        CurrentSpeed = this.CentralNode._RealParams.BackwardSpeed;
                    }
                }
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
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!IsCentralNode)
        {
            BeginRep = false;
            if (this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
            {
                OnLerpRoundTripEnd();
                if (!IsInterleaved || (IsInterleaved && limb == Limb.Right))
                {

                    if ((!this.IsWeb) && (!this.IsInInstruction))
                    {
                        Debug.Log("isWeb [" + this.IsWeb + "]esInsutrccion [" + this.IsInInstruction + "]");
                        this.PauseAnimation();
                    }

                    Debug.Log("repetition endddddddd");
                    OnRepetitionEnd();
                    if (IsInInstruction)
                        SetNextVariation();
                }
                if (IsInterleaved)
                {
                    animator.SetTrigger("ChangeLimb");
                }
                if (!IsInterleaved && (this.IsWeb) || this.IsInInstruction)
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
                OnRepetitionEnd();
                Stop();
            }
        }
    }
    protected override void OnRepetitionEnd()
    {

        animator.SetInteger(AnimatorParams.Movement, -1);
        if (!IsCentralNode)
        {
            this.CentralNode.OnRepetitionEnd();
        }
        else
        {
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
        this.CentralNode.endRepTime = null;
        IsResumen = false;
        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
        
        this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
        //Debug.Log("2Cambiando a " + this._BehaviourState + " "  + this.IsCentralNode  + " "  + this.GetInstanceID());
        CurrentSpeed = 1;
    }
    public override void ResumeAnimation()
    {
        // ResumeAnimation en esta maquina de estado solo se llama cuando !IsInInstruction
        if (IsRepetitionEnd)
            SetNextVariation();
        
        base.ResumeAnimation();
    }
    void OnDestroy()
    {
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;

        base.OnDestroy();
    }

}