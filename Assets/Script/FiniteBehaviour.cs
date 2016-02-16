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
    public bool haCambiadoDeEstado = false;
    private event EventHandler LerpRoundTripEnd;
    /// <summary>
    /// Se utiliza para enviar datos a ExerciseDataGenerator en intervalos de tiempo determinados.
    /// El tiempo que ha pasado desde que se hizo la última captura de datos.
    /// </summary>
    private float timeSinceCapture = 0;
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
                animator.speed = 0;
            else
            {
                //Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.
                animator.speed = this._RealParams.ForwardSpeed;
            }
        }
        if(!haCambiadoDeEstado)
        {
            haCambiadoDeEstado = true;
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceCapture += Time.deltaTime;
        if (this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            animator.speed = 0;
            return;
        }
        const float INTERVAL = 0.1f;
        if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS && timeSinceCapture > INTERVAL)
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

        DateTime temp = DateTime.Now;

        if ((_BehaviourState != AnimationBehaviourState.STOPPED && _BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
    && (endRepTime == null || new TimeSpan(0, 0, (int)_RealParams.SecondsBetweenRepetitions) <= temp - endRepTime))
        {

            if (!beginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
            {
                OnRepetitionReallyStart();
                beginRep = true;
            }
            if (stateInfo.normalizedTime >= 1.0f && haCambiadoDeEstado)
            {
                beginRep = false;
                if (this._behaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    OnLerpRoundTripEnd();
                    if (!IsInterleaved || (IsInterleaved && limb == Limb.Right))
                    {
                        OnRepetitionEnd();

                        if ( (!this.IsWeb) && (!this.IsInInstruction))
                        {
                            Debug.Log("isWeb [" + this.IsWeb + "]esInsutrccion [" + this.IsInInstruction + "]");
                            Debug.Log("pausando animacion");
                            this.PauseAnimation();
                        }
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
                        animator.speed = this._RealParams.ForwardSpeed;
                    }
                    else
                    {
                        animator.speed = this._RealParams.BackwardSpeed;
                    }
                }
            }
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // animator.speed = 1.0f;
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
        Debug.Log("CALLED: stop");
        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
        _BehaviourState = AnimationBehaviourState.STOPPED;
        animator.speed = 1;
        
    }

    void OnDestroy()
    {
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;

        base.OnDestroy();
    }

}