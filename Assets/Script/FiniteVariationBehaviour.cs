using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FiniteVariationBehaviour : AnimationBehaviour
{
    /// <summary>
    /// Esta variable se utiliza para comprobar si es primera vez que se entra al este behaviour
    /// </summary>
    [HideInInspector]
    public uint actualRandomAnimationIndex;
    private event EventHandler LerpRoundTripEnd;
    private FiniteVariationBehaviour _centralNode;
    public FiniteVariationBehaviour CentralNode
    {
        get
        {
            if (_centralNode == null)
                _centralNode = (FiniteVariationBehaviour)AnimationBehaviour.GetCentralBehaviour(this.movement);
            return _centralNode;
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
        return this._actualLerpParams;
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
        BehaviourParams lp = (BehaviourParams)bp;
        this.CentralNode._RealLerpParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
        this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;

    }

    private void initializeRandomAnimations(List<Exercise> animations)
    {
        FiniteVariationBehaviour ab = (FiniteVariationBehaviour)this.CentralNode;
        
        ab.randomAnimations = animations;
        ab.actualRandomAnimationIndex = 0;
        
        //ab.friendsBehaviours = this.friendsBehaviours;
    }

    private void SetNextVariation()
    {
        FiniteVariationBehaviour ab = (FiniteVariationBehaviour)this.CentralNode;
        ++ab.actualRandomAnimationIndex;
        int index = (int)ab.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count;
        AnimatorScript.instance.CurrentExercise = this.CentralNode.randomAnimations[index];
    }
    
    private List<Exercise> GetRandomAnimations(List<Exercise> exs)
    {
        List<Exercise> random = new List<Exercise>();

        exs.AddRange(exs);
        exs.AddRange(exs);
        exs.AddRange(exs);
        exs.AddRange(exs);

        System.Random r = new System.Random();
        int rval;
        int actualCount = exs.Count;
        while (exs.Count > 0)
        {
            rval = r.Next() % actualCount;
            --actualCount;
            random.Add(exs[rval]);
            exs.RemoveAt(rval);
        }

        return random;
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
        this.CentralNode.endRepTime = null;
        if (this.IsInterleaved)
        {
            this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
        }
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;

        //Debug.Log("cambiando a " + _BehaviourState + " " + this.IsCentralNode + "  " + this.GetInstanceID());
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
        if (this.randomAnimations == null)
        {
            this.randomAnimations = new List<Exercise>();
            List<AnimationBehaviour> friends = AnimationBehaviour.getFriendBehaviours(this.movement);
            foreach(AnimationBehaviour a in friends)
            {
                randomAnimations.Add(new Exercise(a.movement, a.laterality, a.limb));
            }
        }

        this.CentralNode.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.CentralNode.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }
    override public void RunWeb(BehaviourParams bp)
    {

        BehaviourParams p = (BehaviourParams)bp;
        this.CentralNode.endRepTime = null;
        this.CentralNode._RealLerpParams = p;
        this.initializeRandomAnimations(bp.Variations);
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.CentralNode.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.CentralNode.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

            animator.speed = this.CentralNode._RealLerpParams.ForwardSpeed;

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

        DateTime now = DateTime.Now;
        if (this.CentralNode != null && this.CentralNode._realLerpParams != null)
            DebugLifeware.Log((int)this.CentralNode._RealLerpParams.SecondsBetweenRepetitions + "   " + (now - this.CentralNode.endRepTime), DebugLifeware.Developer.Marco_Rojas);
        if (this.CentralNode.endRepTime != null && _BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT &&
            new TimeSpan(0, 0, (int)this.CentralNode._RealLerpParams.SecondsBetweenRepetitions) > now - this.CentralNode.endRepTime)
            animator.speed = 0;

        if ((_BehaviourState != AnimationBehaviourState.STOPPED && _BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
            && (this.CentralNode.endRepTime == null || new TimeSpan(0, 0, (int)this.CentralNode._RealLerpParams.SecondsBetweenRepetitions) <= now - this.CentralNode.endRepTime))
        {

            if (!beginRep && (!IsInterleaved || (IsInterleaved && limb == Limb.Left)) &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WEB &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_WITH_PARAMS &&
                this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT)
            {
                OnRepetitionReallyStart();
                beginRep = true;
            }

            //Debug.Log("ha cambiado de estado " + haCambiadoDeEstado);
            
            if (stateInfo.normalizedTime < 1.0f)
            {
                if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    if (animator.speed != this.CentralNode._RealLerpParams.ForwardSpeed  && stateInfo.normalizedTime <= 0.5f)
                    {
                        animator.speed = this.CentralNode._RealLerpParams.ForwardSpeed;
                    }
                    else if (animator.speed != this.CentralNode._RealLerpParams.BackwardSpeed && stateInfo.normalizedTime > 0.5f)
                    {
                        animator.speed = this.CentralNode._RealLerpParams.BackwardSpeed;
                    }
                }
            }
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!IsCentralNode)
        {
            beginRep = false;
            if (this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
            {
                OnLerpRoundTripEnd();
                if (!IsInterleaved || (IsInterleaved && limb == Limb.Right))
                {
                    SetNextVariation();
                    OnRepetitionEnd();
                    
                    if (!this.IsWeb)
                    {
                        this.PauseAnimation();
                    }
                }
                if (IsInterleaved)
                {
                    animator.SetTrigger("ChangeLimb");
                }
                if (!IsInterleaved && this.IsWeb)
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
        if (!IsCentralNode)
            this.CentralNode.OnRepetitionEnd();
        else
            base.OnRepetitionEnd();
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
        
        this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
        //Debug.Log("2Cambiando a " + this._BehaviourState + " "  + this.IsCentralNode  + " "  + this.GetInstanceID());
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