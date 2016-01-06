using UnityEngine;
using System.Collections;
using System;
using Assets;
using System.Collections.Generic;
using System.Threading;

public class StayInPoseBehaviour : AnimationBehaviour {

    enum StayInPoseState { GoingTo, HoldingOn, Leaving, Resting }
    private StayInPoseState stayInPoseState;
    /*
    private LerpBehaviourState _lerpBehaviourState;
    private LerpBehaviourState _LerpBehaviourState
    {
        get { return _lerpBehaviourState; }
        set { 
            _lerpBehaviourState = value;
            Debug.LogWarning(_lerpBehaviourState + GetInstanceID());
            switch (value) 
            { 
                case LerpBehaviourState.RUNNING_DEFAULT:
                case LerpBehaviourState.RUNNING_WITH_PARAMS:
                    animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
                    break;
                case LerpBehaviourState.PREPARING_WITH_PARAMS:
                    StartLerp(); 
                    break;
            } 
        }
    }
    */
 

    public void SetLerpBehaviourState(AnimationBehaviourState lbs)
    {
        Debug.LogWarning("SetLerpBehaviour : " + lbs);
        this._behaviourState = lbs;
    }
    public DateTime? endRepTime = null;
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
        return this._actualLerpParams;
    }
    /*
    public void SetParams(LerpParams lp, LerpBehaviourState lbs)
    {
        this._realLerpParams = lp;
        this._lerpBehaviourState = lbs;
    }
    */
    override public void Prepare(BehaviourParams sp)
    {
        this._RealLerpParams = sp;
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

        //this._LerpBehaviourState = LerpBehaviourState.PREPARING_WEB;
    }
    override public void Run()
    {
        endRepTime = null;
        Debug.Log("Tirando Run");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        //this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;

        //animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
        //this.StartLerp();
    }

   
    override public void RunWeb()
    {
        endRepTime = null;
        Debug.Log("Tirando RunWeb");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_DEFAULT);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
        //this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
        //this._LerpBehaviourState = LerpBehaviourState.RUNNING_DEFAULT;
        //this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }
    override public void RunWeb(BehaviourParams stayInParams)
    {
        endRepTime = null;

        Debug.Log("Tirando RunWebWithParams");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
	

        this._RealLerpParams = stayInParams;
        /*
        this._LerpBehaviourState = LerpBehaviourState.RUNNING_WITH_PARAMS;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
         * */
    }
    

    
    /*
    /// <summary>
    /// Almacena una lista de frames con su correspondiente ángulo, considerando el ejercicio especificado, por ejemplo, para rodilla puede importar el ángulo que se genera entre los segmentos involucrados con el plano sagital
    /// </summary>
    /// <param name="exercise">Ejercicio al cual se le quieren extraer los ángulo de interés</param>
    /// <param name="action">Callback especificado para guardar los frames que se van generando</param>
    /// <returns></returns>
    private void SaveTimesAngle(AnimatorStateInfo animatorState) //ref List<AnimationInfo> aInfo)
    {
        JointTypePlanoResult tempJointTypePlanoResult = MovementJointMatch.movementJointMatch[new MovementLimbKey(movement, execution, limb)];
        ArticulacionClass joint = AnimatorScript.utils.getArticulacion(tempJointTypePlanoResult.jointType);
        AnimationInfo tempAnimationInfo = new AnimationInfo();
        float time = animatorState.normalizedTime * animatorState.length;
        switch (tempJointTypePlanoResult.plain)
        {   
            case Plano.planos.planoFrontal:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleFrontal);
                break;
            case Plano.planos.planoHorizontal:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleHorizontal);
                break;
            case Plano.planos.planoSagital:
                tempAnimationInfo = new AnimationInfo(time, joint.AngleSagital);
                break;
        }
        _timeAndAngles.Add(tempAnimationInfo);
    }
    */
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private float defaultAnimationLength;
    private float startAnimationTime;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {

        if (_behaviourState == AnimationBehaviourState.PREPARING_WEB)
        {
            Debug.Log("onrep end");
            OnRepetitionEnd();
            this.Stop();

        }

        //animator.speed = 0;
        defaultAnimationLength = stateInfo.length;
        startAnimationTime = Time.time;
    
        //Está la animación en caché
        if (PreparedExercises.tryGetPreparedExercise(new Exercise(movement, execution, limb), out this._timeAndAngles, stateInfo.length))
        {
            //Repetición de preparación
            /*
            if (this._LerpBehaviourState != LerpBehaviourState.PREPARING_DEFAULT && this._LerpBehaviourState != LerpBehaviourState.PREPARING_WEB)
            {
                this.StartLerp();
            }
            
            else if (this._LerpBehaviourState == LerpBehaviourState.PREPARING_DEFAULT)
            {
                OnRepetitionEnd();
                _LerpBehaviourState = LerpBehaviourState.PREPARING_WITH_PARAMS;
                this.StartLerp();
            }

            else if (this._LerpBehaviourState == LerpBehaviourState.PREPARING_WEB)
            {
                OnRepetitionEnd();
            }
             * */
        }
        //No está la animación en caché
        else
        {/*
            if (this._LerpBehaviourState == LerpBehaviourState.PREPARING_DEFAULT || this._LerpBehaviourState == LerpBehaviourState.PREPARING_WEB)
            {
                this._timeAndAngles = new List<AnimationInfo>();
                this.StartLerp();
            }
            else
            {
                throw new Exception("Animation not prepared");
            }*/
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
    float startRestTime;
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*if (Time.time - startAnimationTime >= defaultAnimationLength)
            animator.speed = 0;*/
//        Debug.Log(_behaviourState + GetInstanceID());


        if (this._behaviourState == AnimationBehaviourState.INITIAL_POSE)//Testear si esto funciona en este behaviour.
        {
            animator.speed = 0;
            return;
        }

        float DELTA = 0.05f;
        Debug.Log(_behaviourState);
        if (_behaviourState != AnimationBehaviourState.STOPPED)
        {

//            Debug.Log(stayInPoseState + "  " + (Time.time - startRestTime));
            if (stayInPoseState == StayInPoseState.GoingTo &&  Math.Abs(stateInfo.normalizedTime - 1) <= DELTA)
            {
                DebugLifeware.Log("Manteniendo", DebugLifeware.Developer.Marco_Rojas);
             
                animator.speed = 0;
                startHoldTime = Time.time;
                stayInPoseState = StayInPoseState.HoldingOn;
                //Esperar
            }


            //Si ya pasó el tiempo en el ángulo máximo
            else if(stayInPoseState == StayInPoseState.HoldingOn && Time.time - startHoldTime >= _realLerpParams.SecondsInPose)
            {
                DebugLifeware.Log("Para atrás", DebugLifeware.Developer.Marco_Rojas);
                animator.StartRecording(0);
                animator.speed = -1;
                animator.StopRecording();
                stayInPoseState = StayInPoseState.Leaving;
            }

            else if (stayInPoseState == StayInPoseState.Leaving && Math.Abs(stateInfo.normalizedTime - 0) <= DELTA)
            {
                animator.speed = 0;
                stayInPoseState = StayInPoseState.Resting;
                startRestTime = Time.time;
                if (IsInterleaved)
                    DebugLifeware.Log("va a cambiar el limb", DebugLifeware.Developer.Marco_Rojas);
                if (IsInterleaved)
                {
                    DebugLifeware.Log("cambiando limb", DebugLifeware.Developer.Marco_Rojas);
                    animator.SetTrigger("ChangeLimb");
                }
                    OnRepetitionEnd();
                if (_behaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    _behaviourState = AnimationBehaviourState.STOPPED;
            
            }
            else if (stayInPoseState == StayInPoseState.Resting && Time.time - startRestTime >= _realLerpParams.SecondsBetweenRepetitions)
            {
                DebugLifeware.Log("descansando", DebugLifeware.Developer.Marco_Rojas);
                animator.speed = 1;
                stayInPoseState = StayInPoseState.GoingTo;
            }
            DebugLifeware.Log("termino", DebugLifeware.Developer.Marco_Rojas);
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
        if ((_Opposite as StayInPoseBehaviour)._behaviourState != AnimationBehaviourState.STOPPED)
            _Opposite.Stop();

        //this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;

        animator.speed = 1;
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