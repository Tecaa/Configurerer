using UnityEngine;
using System.Collections;
using System;
using Assets;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Discreto con variación state machine
/// </summary>
public class FiniteVariationBehaviour : AnimationBehaviour {

    enum StayInPoseState { GoingTo, HoldingOn, Leaving, Resting }
    private StayInPoseState stayInPoseState;
    [HideInInspector]
    public List<Exercise> randomAnimations;
    [HideInInspector]
    public uint actualRandomAnimationIndex;
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

    override public void Prepare(BehaviourParams sp)
    {
        this._RealLerpParams = sp;
        this._behaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;

        this.setRandomAnimations(this.GetRandomAnimations(), 0);
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;
    }

    private void setRandomAnimations(List<Exercise> animations, uint index)
    {
        List<AnimationBehaviour> abs = AnimationBehaviour.GetBehaviours(this.movement);
        foreach(FiniteVariationBehaviour ab in abs)
        {
            ab.randomAnimations = animations;
            ab.actualRandomAnimationIndex = index;
        }
    }

    private List<Exercise> GetRandomAnimations()
    {
        List<Exercise> random = new List<Exercise>();
        random.Add(new Exercise(Movement.PruebaA, Laterality.Single, Limb.Left));
        random.Add(new Exercise(Movement.PruebaB, Laterality.Single, Limb.Left));
        random.Add(new Exercise(Movement.PruebaC, Laterality.Single, Limb.Left));
        return random;
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
        Debug.Log("Tirando Run");
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }

        this._behaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;

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
    }
    

    
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    private float defaultAnimationLength;
    private float startAnimationTime;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (this._actualLerpParams == null)
            this._actualLerpParams = new BehaviourParams();
        if(this._realLerpParams == null)
        {
            this._realLerpParams = new BehaviourParams();
        }
        if (_behaviourState == AnimationBehaviourState.PREPARING_WEB)
        {
            Debug.Log("onrep end");
            OnRepetitionEnd();
            this.Stop();

        }

        defaultAnimationLength = stateInfo.length;
        startAnimationTime = Time.time;
    
        //Está la animación en caché
        if (PreparedExercises.tryGetPreparedExercise(new Exercise(movement, execution, limb), out this._timeAndAngles, stateInfo.length))
        {
            //Repetición de preparación
           
        }
        //No está la animación en caché
        else
        {
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

        if (this._behaviourState == AnimationBehaviourState.INITIAL_POSE)//Testear si esto funciona en este behaviour.
        {
            animator.speed = 0;
            return;
        }

        float DELTA = 0.05f;
        if (_behaviourState != AnimationBehaviourState.STOPPED)
        {
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
                if (!this.isWeb && _behaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS)
                    this.PauseAnimation();
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
        if ((_Opposite as FiniteVariationBehaviour)._behaviourState != AnimationBehaviourState.STOPPED)
            _Opposite.Stop();

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