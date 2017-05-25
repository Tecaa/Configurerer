using UnityEngine;
using System.Collections;
using System;
using Assets;
using System.Collections.Generic;
using System.Threading;

public class StayInPoseBehaviour : AnimationBehaviour {
    private StayInPoseState stayInPoseState;
    [HideInInspector]
    public bool haCambiadoDeEstado = false;
    private float timeSinceCapture = 0;
    protected override bool HasCentralNode { get { return false; } }
    public void SetLerpBehaviourState(AnimationBehaviourState lbs)
    {
        this._BehaviourState = lbs;
    }
    //public DateTime? endRepTime = null;
    private List<AnimationInfo> _timeAndAngles;

    /// <summary>
    /// Tiempo que demora acelerar o desacelerar el movimiento concéntrico
    /// </summary>
    private float timeTakenDuringForwardLerp = 1f;

    /// <summary>
    /// Tiempo que demora acelerar o desacelerar el movimiento excéntrico
    /// </summary>
    private float timeTakenDuringBackwardLerp = 1f;

    public BehaviourParams GetParams()
    {
        return this._currentParams;
    }

    override public void Prepare(BehaviourParams sp)
    {
        this._RealParams = sp;
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
        Debug.Log("prepareWebInternal");
        this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;
    }
    override public void Run()
    {
        endRepTime = null;
        Debug.Log("Tirando Run");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
    }

    override public void RunWeb()
    {
        endRepTime = null;
        Debug.Log("Tirando RunWeb");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
        }

        this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
    }
    override public void RunWeb(BehaviourParams stayInParams)
    {
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
        endRepTime = null;

        Debug.Log("Tirando RunWebWithParams");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
	

        this._RealParams = stayInParams;
        stayInPoseState = StayInPoseState.GoingTo;
        this.CurrentSpeed = this._RealParams.ForwardSpeed;
    }
    
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private float defaultAnimationLength;
    private float startAnimationTime;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {        
        if (this._currentParams == null)
            this._currentParams = new BehaviourParams();
        if(this._realParams == null)
        {
            this._realParams = new BehaviourParams();
        }
        if (_BehaviourState == AnimationBehaviourState.PREPARING_WEB)
        {
            OnRepetitionEnd();
            this.Stop();

        }
        this.CurrentSpeed = this._realParams.ForwardSpeed;
        defaultAnimationLength = stateInfo.length;
        startAnimationTime = Time.time;
    
        if (!haCambiadoDeEstado)
        {
            haCambiadoDeEstado = true;
        }
	}
    void LerpBehaviour_LerpRoundTripEnd(object sender, EventArgs e)
    {
        endRepTime = DateTime.Now;
        if (IsInterleaved)
        {
            (this._Opposite as StayInPoseBehaviour).endRepTime = endRepTime;
        }
    }
    const float INTERVAL = 0.1f;
    float startHoldTime;
    float startRestTime;
    private bool repetitionStartFlag = false;
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceCapture += Time.deltaTime;
        if (this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)//Testear si esto funciona en este behaviour.
        {
            if (!animator.IsInTransition(0))
                CurrentSpeed = 0;
            return;
        }

        
        if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS && timeSinceCapture > INTERVAL)
        {
            timeSinceCapture = timeSinceCapture - INTERVAL;
            /*
            if (exerciseDataGenerator == null)
                exerciseDataGenerator = GameObject.FindObjectOfType<Detector.ExerciseDataGenerator>();
            if (this.exerciseDataGenerator != null)
                this.exerciseDataGenerator.CaptureData();*/
        }
        float DELTA = 0.05f;
        DateTime temp = DateTime.Now;
        if (_BehaviourState != AnimationBehaviourState.STOPPED && 
            (endRepTime == null || new TimeSpan(0, 0, (int)_RealParams.SecondsBetweenRepetitions) <= temp - endRepTime))
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

            if (stayInPoseState == StayInPoseState.GoingTo &&  stateInfo.normalizedTime + DELTA >= 1)
            {
                CurrentSpeed = 0;
                startHoldTime = Time.time;
                stayInPoseState = StayInPoseState.HoldingOn;
                //Esperar
            }

            //Si ya pasó el tiempo en el ángulo máximo
            else if(stayInPoseState == StayInPoseState.HoldingOn && Time.time - startHoldTime >= _RealParams.SecondsInPose)
            {
                CurrentSpeed = -this._RealParams.BackwardSpeed;
                stayInPoseState = StayInPoseState.Leaving;
            }

            else if (stayInPoseState == StayInPoseState.Leaving && stateInfo.normalizedTime - DELTA <= 0 && haCambiadoDeEstado)
            {
                BeginRep = false;
                CurrentSpeed = 0;
                stayInPoseState = StayInPoseState.Resting;
                startRestTime = Time.time;

                if (((!this.IsWeb) && (!this.IsInInstruction)) && _BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS && (!IsInterleaved || (IsInterleaved && limb == Limb.Right)))
                    this.PauseAnimation();
                if (IsInterleaved && this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    haCambiadoDeEstado = false;
                    animator.SetTrigger("ChangeLimb");
                }
                OnRepetitionEnd();
                if (_BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    this.Stop();

            }

            else if (stayInPoseState == StayInPoseState.Resting && Time.time - startRestTime>= _realParams.SecondsBetweenRepetitions)
            {
                this.CurrentSpeed = this._realParams.ForwardSpeed;
                stayInPoseState = StayInPoseState.GoingTo;

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
        CurrentSpeed = 1.0f;
    }

	

    /// <summary>
    /// Obtiene el intervalo de tiempo en segundos y el ángulo más cercano al entregado por parámetro
    /// </summary>
    /// <param name="angle">Ángulo que se quiere buscar en la lista de frames</param>
    /// <param name="aiList">Lista de AnimationInfo que representan el tiempo en segundos y el ángulo en cada frame</param>
    /// <returns>Retorna una AnimationInfo que contiene el tiempo y el ángulo encontrado más cercano al solicitado</returns>
    AnimationInfo GetAnimationInfo(float angle, List<AnimationInfo> aiList)
    {
        //TODO: Quizás se pueda mejorar
        List<AnimationInfo> aiListTemp = aiList.GetRange(1, (int)aiList.Count / 2);
        aiListTemp.Reverse();
        AnimationInfo aiTemp = new AnimationInfo(float.MaxValue, float.MaxValue);
        bool wasNear = false;
        foreach (AnimationInfo ai in aiListTemp)
        {
            float dif = Math.Abs(ai.angle - angle);
            if ((dif <= 3) && !wasNear)
            {
                wasNear = true;
            }
            else if ((dif > 3) && wasNear)
            {
                return aiTemp;
            }

            if (dif < Math.Abs(aiTemp.angle - angle))
                aiTemp = ai;
            else
                continue;
        }

        return aiTemp;
    }



    public override void Stop()
    {
        _BehaviourState = AnimationBehaviourState.STOPPED;
        if (this.IsInterleaved)
        {
            if ((_Opposite as StayInPoseBehaviour)._BehaviourState != AnimationBehaviourState.STOPPED)
                _Opposite.Stop();
        }
        CurrentSpeed = 1;
        stayInPoseState = StayInPoseState.Resting;
        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
    }
    void OnDestroy()
    {
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;
        base.OnDestroy();
    }
}