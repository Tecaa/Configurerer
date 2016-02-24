using UnityEngine;
using System.Collections;
using System;
using Assets;
using System.Collections.Generic;
using System.Threading;

public class StayInPoseWithMovementBehaviour : AnimationBehaviour {

    protected override bool HasCentralNode { get { return false; } }
    enum StayInPoseState { HoldingOn, Resting }
    private StayInPoseState stayInPoseState;
    [HideInInspector]
    public bool haCambiadoDeEstado = false;
    /*public void SetLerpBehaviourState(AnimationBehaviourState lbs)
    {
        Debug.LogWarning("SetLerpBehaviour : " + lbs);
        this._behaviourState = lbs;
    }*/

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
        //Debug.Log("Tirando Run");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
    }

    override public void RunWeb()
    {
        endRepTime = null;
        //Debug.Log("Tirando RunWeb");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
    }
    override public void RunWeb(BehaviourParams stayInParams)
    {
        endRepTime = null;

        //Debug.Log("Tirando RunWebWithParams");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
	

        this._RealParams = stayInParams;
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
    void LerpBehaviour_LerpRoundTripEnd(object sender, EventArgs e)
    {
        endRepTime = DateTime.Now;
        if (IsInterleaved)
        {
            (this._Opposite as StayInPoseBehaviour).endRepTime = endRepTime;
        }
    }

    //private bool ReadyToLerp = false;
    float startHoldTime;
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        if (this._behaviourState == AnimationBehaviourState.INITIAL_POSE)//Testear si esto funciona en este behaviour.
        {
            animator.speed = 0;
            return;
        }

        DateTime temp = DateTime.Now;

        if (_behaviourState != AnimationBehaviourState.STOPPED && (endRepTime == null || new TimeSpan(0, 0, (int)_RealParams.SecondsBetweenRepetitions) <= temp - endRepTime))
        {

            if (!beginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
            {

                OnRepetitionReallyStart();
                beginRep = true;
                animator.speed = 1;
                startHoldTime = Time.time;
                stayInPoseState = StayInPoseState.HoldingOn;
            }

            //Si ya pasó el tiempo indicado realizando el movimiento
            if (stayInPoseState == StayInPoseState.HoldingOn && (Time.time - startHoldTime) >= this._RealParams.SecondsInPose )
            {
                DebugLifeware.Log("Tiempo en pose maxima = " + (Time.time - startHoldTime).ToString(), DebugLifeware.Developer.Alfredo_Gallardo);
                animator.speed = 0;
                //startHoldTime = 0;
                stayInPoseState = StayInPoseState.Resting;
                beginRep = false;
                if ((!this.IsWeb) && (!this.IsInInstruction) && this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    this.PauseAnimation();
                else if((this.IsWeb) || (this.IsInInstruction) && this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    endRepTime = DateTime.Now;
                }
                if(this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                {
                    this.Stop();
                }
                OnRepetitionEnd();
            }


//            //Si ya pasó el tiempo en el ángulo máximo
//            else if(stayInPoseState == StayInPoseState.HoldingOn && Time.time - startHoldTime >= _realLerpParams.SecondsInPose)
//            {
//                //DebugLifeware.Log("Para atrás", DebugLifeware.Developer.Marco_Rojas);
//                animator.StartRecording(0);
//                animator.speed = -1;
//                animator.StopRecording();
//                stayInPoseState = StayInPoseState.Leaving;
//            }

//            else if (stayInPoseState == StayInPoseState.Leaving && Math.Abs(stateInfo.normalizedTime - 0) <= DELTA && haCambiadoDeEstado)
//            {
//                beginRep = false;
//                animator.speed = 0;
//                stayInPoseState = StayInPoseState.Resting;
//                startRestTime = Time.time;

//                if (!this.isWeb && _behaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS && (!IsInterleaved || (IsInterleaved && limb == Limb.Right)))
//                    this.PauseAnimation();
//                if (IsInterleaved && this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
//                {
//                    //DebugLifeware.Log("cambiando limb", DebugLifeware.Developer.Marco_Rojas);
//                    haCambiadoDeEstado = false;
//                    animator.SetTrigger("ChangeLimb");
//                }
//                OnRepetitionEnd();

//                if (_behaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
//                    _behaviourState = AnimationBehaviourState.STOPPED;
                
//            }

//            else if (stayInPoseState == StayInPoseState.Resting && Time.time - startRestTime>= _realLerpParams.SecondsBetweenRepetitions)
//            {
//                //DebugLifeware.Log("descansando", DebugLifeware.Developer.Marco_Rojas);
//                animator.speed = 1;
//                stayInPoseState = StayInPoseState.GoingTo;

//            }
//            //DebugLifeware.Log("termino", DebugLifeware.Developer.Marco_Rojas);
        }
        
    }


	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.speed = 1.0f;
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

        _behaviourState = AnimationBehaviourState.STOPPED;
        if (IsInterleaved)
        {
            if ((_Opposite as StayInPoseWithMovementBehaviour)._behaviourState != AnimationBehaviourState.STOPPED)
                _Opposite.Stop();
        }
        //this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;

        animator.speed = 1;

        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
    }
    void OnDestroy()
    {
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;
        base.OnDestroy();
    }
}