using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FiniteBehaviour : AnimationBehaviour
{
    /// <summary>
    /// Esta variable se utiliza para comprobar si es primera vez que se entra al este behaviour
    /// </summary>
    [HideInInspector]
    public bool haCambiadoDeEstado = false;
    private event EventHandler LerpRoundTripEnd;
    /// <summary>
    /// Se utiliza para enviar datos a ExerciseDataGenerator en intervalos de tiempo determinados.
    /// El tiempo que ha pasado desde que se hizo la última captura de datos.
    /// </summary>
    private float timeSinceCapture = 0;
    protected override bool HasCentralNode { get { return false; } }

    protected void OnLerpRoundTripEnd()
    {
        EventHandler eh = LerpRoundTripEnd;
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }

    void LerpBehaviour_LerpRoundTripEnd(object sender, EventArgs e)
    {
        endRepTime = DateTime.Now;
        if (IsInterleaved && this.limb == Limb.Right)
        {
            (this._Opposite as FiniteBehaviour).endRepTime = endRepTime;
        }
    }

    public BehaviourParams GetParams()
    {
        return this._currentParams;
    }
    protected override  AnimationBehaviourState _BehaviourState
    {
        get { return this._behaviourState; }
        set
        {
            this._behaviourState = value;
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
        this._RealParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
        timeSinceCapture = 0;
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;

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
        endRepTime = null;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }


    override public void RunWeb()
    {
        endRepTime = null;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
        }
        this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }
    override public void RunWeb(BehaviourParams bp)
    {

        BehaviourParams lerpParams = (BehaviourParams)bp;
        endRepTime = null;
        this._RealParams = lerpParams;
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(this._BehaviourState == AnimationBehaviourState.PREPARING_WEB)
        {
            OnRepetitionEnd();
            Stop();
        }
        else if(this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS
            || this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            //Como en este behaviour se utiliza animation.Play para cada repetición, se entra más de una vez al metodo OnStateEnter, 
            //por lo que si ya se ha entrado alguna vez, la velocidad se asigna como 0 para que se respete el tiempo entre ejecución 
            //antes de comenzar la siguiente repetición.
            if (haCambiadoDeEstado)
                CurrentSpeed = 0;
            else
            {
                //Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.
                CurrentSpeed = this._RealParams.ForwardSpeed;
            }
        }
        if(!haCambiadoDeEstado)
        {
            haCambiadoDeEstado = true;
        }
    }

    private bool repetitionStartFlag = false;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            if (!animator.IsInTransition(0))
                CurrentSpeed = 0;
            return;
        }
        const float INTERVAL = 0.1f;
        if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
        {
            timeSinceCapture += Time.deltaTime;
            /*
            if (timeSinceCapture > INTERVAL)
            {
                timeSinceCapture = timeSinceCapture - INTERVAL;
                if (exerciseDataGenerator == null)
                    exerciseDataGenerator = GameObject.FindObjectOfType<Detector.ExerciseDataGenerator>();
                if (this.exerciseDataGenerator != null)
                    this.exerciseDataGenerator.CaptureData();
            }*/
        }

        DateTime temp = DateTime.Now;

        if ((_BehaviourState != AnimationBehaviourState.STOPPED && _BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
    && (endRepTime == null || new TimeSpan(0, 0, (int)_RealParams.SecondsBetweenRepetitions) <= temp - endRepTime))
        {

            if (!BeginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
            {
                OnRepetitionReallyStart();
                BeginRep = true;
                repetitionStartFlag = true;
            }
            if (stateInfo.normalizedTime >= 1.0f && haCambiadoDeEstado)
            {
                BeginRep = false;
                if (this._behaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    OnLerpRoundTripEnd();
                    if (!IsInterleaved || (IsInterleaved && limb == Limb.Right))
                    {

                        if ((!this.IsWeb) && (!this.IsInInstruction) && (!this.IsInInstruction))
                        {

                            this.PauseAnimation();
                        }
                        // importante: el orden de llamadas es esencial para el correcto funcionamiento
                        OnRepetitionEnd();
                    }
                    if (IsInterleaved)
                    {
                        haCambiadoDeEstado = false;
                        animator.SetTrigger("ChangeLimb");
                    }
                    if(!IsInterleaved && (this.IsWeb || this.IsInInstruction) )
                    {
                        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
                    }
                    if (this._BehaviourState == AnimationBehaviourState.STOPPED)
                    {
                        endRepTime = null;
                    }
                }
                else if (this._behaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                {
                    OnRepetitionEnd();
                    Stop();
                }

            }
            else
            {
                if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._behaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    if (stateInfo.normalizedTime <= 0.5f)
                    {
                        CurrentSpeed = this._RealParams.ForwardSpeed;
                    }
                    else
                    {
                        CurrentSpeed = this._RealParams.BackwardSpeed;
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
        _BehaviourState = AnimationBehaviourState.STOPPED;
        CurrentSpeed = 1;
        
    }

    void OnDestroy()
    {
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;

        base.OnDestroy();
    }

}