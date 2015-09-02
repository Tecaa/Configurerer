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
    override public void PrepareWeb()
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
        animator.speed = 0;
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
        float DELTA = 0.05f;
        Debug.Log(_behaviourState);
        if (_behaviourState != AnimationBehaviourState.STOPPED)
        {

//            Debug.Log(stayInPoseState + "  " + (Time.time - startRestTime));
            if (stayInPoseState == StayInPoseState.GoingTo &&  Math.Abs(stateInfo.normalizedTime - 1) <= DELTA)
            {
                Debug.Log("Manteniendo");
             
                animator.speed = 0;
                startHoldTime = Time.time;
                stayInPoseState = StayInPoseState.HoldingOn;
                //Esperar
            }


            //Si ya pasó el tiempo en el ángulo máximo
            else if(stayInPoseState == StayInPoseState.HoldingOn && Time.time - startHoldTime >= _realLerpParams.SecondsInPose)
            {
                Debug.Log("Para atrás");
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
                    Debug.Log("va a cambiar el limb");
                if (IsInterleaved)
                {
                    Debug.Log("cambiando limb");
                    animator.SetTrigger("ChangeLimb");
                }
                    OnRepetitionEnd();
                if (_behaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    _behaviourState = AnimationBehaviourState.STOPPED;
            
            }
            else if (stayInPoseState == StayInPoseState.Resting && Time.time - startRestTime >= _realLerpParams.SecondsBetweenRepetitions)
            {
                Debug.Log("descansando");
                animator.speed = 1;
                stayInPoseState = StayInPoseState.GoingTo;
            }

            Debug.Log("termino la amada");
        }
        /*
        #region Interpolate
        //Si no estamos en estado Stopped 
        //Y estamos preparados para hacer Lerp
        //Y el tiempo que ha transcurrido de la ultima rep es mayor al tiempo de pause entre repeticiones. O no ha habido última rep.
        Debug.Log("state: " +_LerpBehaviourState + GetInstanceID());
        if (_LerpBehaviourState != LerpBehaviourState.STOPPED && ReadyToLerp
            && (endRepTime == null || new TimeSpan(0, 0, (int)_actualLerpParams.SecondsBetweenRepetitions) <= DateTime.Now - endRepTime))
        {
            //if(_LerpBehaviourState == LerpBehaviourState.STOPPED)
                Debug.LogError("ENtRO");
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = 0f;
            switch (_currentLerpState)
            {
                case LerpState.Forward:
                    percentageComplete = timeSinceStarted / timeTakenDuringForwardLerp;
                    break;

                case LerpState.Stopped:
                    if (_lastLerpState == LerpState.Forward)
                    {
                        percentageComplete = timeSinceStarted / timeTakenDuringForwardLerp;
                    }

                    //De ser true, indica que termino una repeticion
                    else if (_lastLerpState == LerpState.Backward)
                    {
                        percentageComplete = timeSinceStarted / timeTakenDuringBackwardLerp;
                    }
                    break;

                case LerpState.Backward:
                    percentageComplete = timeSinceStarted / timeTakenDuringBackwardLerp;
                    break;
            }

            //Aplico el suavizado "Smotherstep"
            //percentageComplete = percentageComplete * percentageComplete * (3f - 2f * percentageComplete);
            percentageComplete = percentageComplete * percentageComplete * percentageComplete * (percentageComplete * (6f * percentageComplete - 15f) + 10f);
            animator.speed = Mathf.Lerp(startPosition, endPosition, percentageComplete);
            if (percentageComplete >= 1.0f)
            {
                InterpolationEnd();
            }
        }
        
        #endregion
        if (_LerpBehaviourState == LerpBehaviourState.PREPARING_DEFAULT || _LerpBehaviourState == LerpBehaviourState.PREPARING_WEB)
            this.SaveTimesAngle(stateInfo);
         * */
    }


	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.speed = 1.0f;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    /*
    /// <summary>
    /// Comienza la interpolación entre 0 y el tiempo donde está el ángulo requerido (LerpTime), aplicando las velocidades concéntras y excéntricas indicadas
    /// </summary>
    /// <param name="_lerpTime">Tiempo donde se encuentra el ángulo al que se quiere llegar</param>
    /// <param name="_forwardSpeed">Velocidad concéntrica</param>
    /// <param name="_backwardSpeed">Velocidad excéntrica</param>
    public void StartLerp()
    {
        _currentLerpState = LerpState.Forward;

        if (_LerpBehaviourState == LerpBehaviourState.PREPARING_DEFAULT || _LerpBehaviourState == LerpBehaviourState.PREPARING_WEB || _LerpBehaviourState == LerpBehaviourState.RUNNING_DEFAULT)
        {
            _actualLerpParams = new LerpParams(defaultAnimationLength, 1, 1);
            timeTakenDuringLerp = (float)Math.Floor(defaultAnimationLength * 10) / 10;
            //timeTakenDuringLerp = defaultAnimationLength;
        }
        else
        {
            _actualLerpParams = _RealLerpParams;
            timeTakenDuringLerp = (float)Math.Floor(GetAnimationInfo(_actualLerpParams.Angle, _timeAndAngles).time * 10) / 10;
            //timeTakenDuringLerp = GetAnimationInfo(_actualLerpParams.Angle, _timeAndAngles).time;
        }
        //Normalizo el tiempo que tardo en parar
        timeTakenDuringForwardLerp = timeTakenDuringLerp / _actualLerpParams.ForwardSpeed;
        timeTakenDuringBackwardLerp = timeTakenDuringLerp / _actualLerpParams.BackwardSpeed;
        forwardSpeed = _actualLerpParams.ForwardSpeed;
        backwardSpeed = _actualLerpParams.BackwardSpeed;
        BeginLerp(0, forwardSpeed, true);
    }
     * */

    /*
    /// <summary>
    /// Inicia una interpolación y ajusta los valores iniciales
    /// </summary>
    private void BeginLerp(float startPos, float endPos, bool pauseBeforeLerping = false)
    {
        timeStartedLerping = Time.time;
        // En caso de que se ejecute un Lerp que requiera pausa entre lerps, se agrega el tiempo de pausa a el tiempo de inicio del Lerp.
        if (pauseBeforeLerping && endRepTime != null)
            timeStartedLerping += _actualLerpParams.SecondsBetweenRepetitions;
        startPosition = startPos;
        endPosition = endPos;
        ReadyToLerp = true;
    }
    
    /// <summary>
    /// Cuando termina una interpolacion se comprueba el estado de la animacion para continuar con el ciclo de aceleración y desaceleracion
    /// </summary>
    private void InterpolationEnd()
    {
        Debug.LogWarning("INTERPOLATION END Y QUE WEA" + GetInstanceID());
        switch (_currentLerpState)
        {
            case LerpState.Forward:
                _currentLerpState = LerpState.Stopped;
                _lastLerpState = LerpState.Forward;
                BeginLerp(forwardSpeed, 0);
                break;

            case LerpState.Stopped:
                if (_lastLerpState == LerpState.Forward)
                {
                    _currentLerpState = LerpState.Backward;
                    _lastLerpState = LerpState.Stopped;
                    BeginLerp(0, -backwardSpeed);
                }

                //De ser true, indica que termino una repeticion
                else if (_lastLerpState == LerpState.Backward)
                {
                    Debug.LogWarning("Retrocedí hasta: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
                    _currentLerpState = LerpState.Forward;
                    _lastLerpState = LerpState.Forward;

                    if (this._LerpBehaviourState == LerpBehaviourState.PREPARING_DEFAULT || this._LerpBehaviourState == LerpBehaviourState.PREPARING_WEB)
                    {
                        try
                        {
                            PreparedExercises.InsertPreparedExercise(new Exercise(movement, execution, limb), _timeAndAngles);
                        }
                        catch
                        {
                            ; // do nothing
                        }

                        if (this._LerpBehaviourState == LerpBehaviourState.PREPARING_DEFAULT)
                        {
                            this._LerpBehaviourState = LerpBehaviourState.PREPARING_WITH_PARAMS;
                        }
                        else
                        {
                            this._LerpBehaviourState = LerpBehaviourState.STOPPED;
                            OnRepetitionEnd();
                        }
                    }
                    else if (this._LerpBehaviourState == LerpBehaviourState.PREPARING_WITH_PARAMS)
                    {
                        _LerpBehaviourState = LerpBehaviourState.STOPPED;
                        OnRepetitionEnd();
                        //TODO: Recolectar datos y entregarlos a jorge
                    }
                    //Hace repeticiones hasta el infinito
                    else if (this._LerpBehaviourState == LerpBehaviourState.RUNNING_DEFAULT || this._LerpBehaviourState == LerpBehaviourState.RUNNING_WITH_PARAMS)
                    {

                        OnLerpRoundTripEnd();
                        if (!IsInterleaved || IsInterleaved && limb == Limb.Right)
                            OnRepetitionEnd();


                        if (IsInterleaved)
                        {
                            animator.SetTrigger("ChangeLimb");
                        }
                        if (this._LerpBehaviourState == LerpBehaviourState.STOPPED)
                        {
                            endRepTime = null;
                            ReadyToLerp = false;

                        }
                        else
                        //BeginLerp(0, forwardSpeed, true);
                        {
                           
                            animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
                        }
                    }
                }
                break;

            case LerpState.Backward:
                _currentLerpState = LerpState.Stopped;
                _lastLerpState = LerpState.Backward;
                BeginLerp(-backwardSpeed, 0);
                break;
        }
    }
    
    /// <summary>
    /// Detiene la interpolación que actualmente se está ejecutando
    /// </summary>
    public void StopLerps()
    {
        _LerpBehaviourState = LerpBehaviourState.STOPPED;
        if (_Opposite._LerpBehaviourState != LerpBehaviourState.STOPPED)
            _Opposite.StopLerps();

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        if(animator.speed < 0)
            animator.speed = animator.speed * -1;
        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
    }
     * */

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