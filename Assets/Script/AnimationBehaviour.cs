using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class AnimationBehaviour : StateMachineBehaviour {

    // IMPORATANTE DETECTOR: DESCOMENTAR LÍNEA EN GRAN JUEGO y TAMBIÉN EN LERPBEHAVIOUR.cs EN EXTRACCIpon.
    //protected Detector.ExerciseDataGenerator exerciseDataGenerator;
	
	protected enum StayInPoseState { GoingTo, HoldingOn, Leaving, Resting }
    public event EventHandler RepetitionEnd;
    public bool IsCentralNode;
    /// <summary>
    /// Se dispara despues del tiempo entre ejecciones
    /// </summary>
    public event EventHandler RepetitionReallyStart;
    public Movement movement;
    public Limb limb;
    public Laterality laterality;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public List<Movement> randomAnimations;
    private bool _isInterleaved;
    [HideInInspector]
    public DateTime? endRepTime = null;
    private bool _beginRep;
    [HideInInspector]
    public bool BeginRep
    {
        get
        {
            if (IsCentralNode || !this.HasCentralNode)
                return this._beginRep;
            else
            {
                return this.CentralNode._beginRep;
            }
        }
        set
        {
            if (IsCentralNode || !this.HasCentralNode)
                this._beginRep = value;
            else
            {
                this.CentralNode._beginRep = value;
            }
        }
    }
    protected  AnimationBehaviour _centralNode;
    public AnimationBehaviour CentralNode
    {
        get
        {
            if (_centralNode == null)
                _centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement, this.limb);
            return (AnimationBehaviour)_centralNode;
        }
    }

    const int MAGIC_NUMBER = 10000;
    [HideInInspector]
	public uint actualRandomAnimationIndex;
    [HideInInspector]
    public uint savedRandomAnimationIndex;
    private bool _isRepetitionEnd = false;       //Indica si la repeticion ha finalizado
    public bool IsRepetitionEnd
    {
        get
        {
            if (IsCentralNode || !this.HasCentralNode)
                return this._isRepetitionEnd;
            else
            {
                return this.CentralNode.IsRepetitionEnd;
            }
        }
        set
        {
            if (IsCentralNode || !this.HasCentralNode)
                this._isRepetitionEnd = value;
            else
            {
                this.CentralNode.IsRepetitionEnd = value;
            }
        }
    }
    private bool _isResumen = false;             //Indica si la funcion resumen ha sido llamada
    public bool IsResumen
    {
        get
        {
            if (IsCentralNode || !this.HasCentralNode)
                return this._isResumen;
            else
            {
                return this.CentralNode.IsResumen;
            }
        }
        set
        {
            if (IsCentralNode || !this.HasCentralNode)
                this._isResumen = value;
            else
            {
                this.CentralNode.IsResumen = value;
            }
        }
    }

    //[HideInInspector]
    //protected List<AnimationBehaviour> friendsBehaviours;
    public bool IsInterleaved
    {
        get { return _isInterleaved; }
        set
        {
            _isInterleaved = value;
            if (this.limb == Limb.Left)
                this._Opposite.IsInterleaved = IsInterleaved;
        }
    }
    [HideInInspector]
    private bool isWeb = false;
    public bool IsWeb
    {
        get
        {
            if (IsCentralNode || !this.HasCentralNode)
                return this.isWeb;
            else
            {
                return this.CentralNode.IsWeb;
            }
        }
        set
        {
            if (IsCentralNode || !this.HasCentralNode)
                this.isWeb = value;
            else
            {
                this.CentralNode.IsWeb = value;
            }
        }
    }
    
    private AnimationBehaviour _opposite;
    protected AnimationBehaviour _Opposite
    {
        get
        {
            if (this._opposite != null)
                return this._opposite;
            else
            {
                Limb opposite = Limb.Right;
                if (limb == Limb.Right)
                    opposite = Limb.Left;
                this._opposite = AnimationBehaviour.GetBehaviour(movement, opposite);
                return this._opposite;
            }
        }
    }
    protected BehaviourParams _currentParams;
    protected BehaviourParams _realParams;
    protected BehaviourParams _RealParams
    {
        get
        {
            if (this.IsCentralNode || !this.HasCentralNode)
                return _realParams;
            else
                return this.CentralNode._RealParams;
        }
        set
        {

            if (this.IsInterleaved)
            {
                this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
                //this._Opposite.IsInterleaved = IsInterleaved;
                if (this._Opposite._RealParams != value)
                    this._Opposite._RealParams = value;
            }
            if (this.IsCentralNode || !this.HasCentralNode)
                this._realParams = value;
            else
                this.CentralNode._RealParams = value;
        }
    }
    

    private bool isInInstruction = false;
    public bool IsInInstruction
    {
        get
        {
            if (this.IsCentralNode || !this.HasCentralNode)
            {
                return this.isInInstruction;
            }
            else
            {
                return CentralNode.IsInInstruction;
            }

        }
        set
        {
            if (this.IsCentralNode || !this.HasCentralNode)
            {
                this.isInInstruction = value;
            }
            else
            {
                CentralNode.IsInInstruction = value;
            }
        }
    }

    public void SetBehaviourState(AnimationBehaviourState lbs)
    {
        this._behaviourState = lbs;
    }
    protected virtual void OnRepetitionEnd()
    {
        //TODO: Fix rapido pero que debe arreglarse ya que el evento se lanza aún cuando se está en modo web.
        if(this.isWeb && (this._BehaviourState != AnimationBehaviourState.PREPARING_DEFAULT && this._BehaviourState != AnimationBehaviourState.PREPARING_WEB))
        {
            return;
        }
        //DebugLifeware.Log("OnRepetitionEnd" + " " + this.IsCentralNode, DebugLifeware.Developer.Alfredo_Gallardo | DebugLifeware.Developer.Marco_Rojas);
        EventHandler eh = RepetitionEnd;
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
        IsRepetitionEnd = true;
        //Debug.Log("onRepetitionEnd: " + IsRepetitionEnd + " _isResumen: " + IsResumen);
        if (IsResumen && !this.HasCentralNode)
        {
            Debug.Log(" RESUMIENDO DESDE 1: " + this.IsResumen + " " + !this.HasCentralNode);
            ResumeAnimation();
        }
        if (IsResumen && this.HasCentralNode)
        {
            Debug.Log(" RESUMIENDO DESDE 2: " + this.IsResumen + " " + this.HasCentralNode);
            CentralNode.ResumeAnimation();
        }
    }
    protected void OnRepetitionReallyStart()
    {
        //TODO: Fix rapido pero que debe arreglarse ya que el evento se lanza aún cuando se está en modo web.
        if (this.isWeb)
        {
            return;
        }
        EventHandler eh = RepetitionReallyStart;
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }
    protected AnimationBehaviourState originalABS = AnimationBehaviourState.STOPPED;
    public virtual void ResumeAnimation()
    {
        IsResumen = true;
        if (IsRepetitionEnd == true)
        {
            //Debug.Log("Resume Animation");
            if (this.CentralNode != null)
            {
                this.originalABS = this.CentralNode.originalABS;
                this.CentralNode.endRepTime = DateTime.Now;
            }
            else
                this.endRepTime = DateTime.Now;

            if (this.IsInterleaved && this.limb == Limb.Left)
            {
                animator.SetTrigger("ChangeLimb");
                this._Opposite.SetBehaviourState(originalABS);
            }

            this._BehaviourState = originalABS;
            if (this.IsInterleaved)
                this._Opposite.endRepTime = this.endRepTime;
            IsResumen = false;
        }
        IsRepetitionEnd = false;
        //Debug.Log("ResumeAnimation: " + IsRepetitionEnd + " _isResumen: " + IsResumen);
    }
        
    public virtual void PauseAnimation(){
        originalABS = this._BehaviourState;


        if (this.CentralNode != null)
        {
            this.CentralNode.originalABS = this._BehaviourState;
        }
        //Debug.Log("pasuse " + _BehaviourState);
        this._BehaviourState = AnimationBehaviourState.STOPPED;
        animator.speed = 0;

       
        if (IsInterleaved)
        {
            if (this.limb == Limb.Right)
                this._Opposite.PauseAnimation();
        }
    }
    abstract public void Prepare(BehaviourParams lp);
    abstract protected void PrepareWebInternal();
    abstract public void Run(); // cambiar a protected
    abstract public void RunWeb();
    abstract public void RunWeb(BehaviourParams lerpParams);

    public void Run(bool isInInstructionInput)
    {
        this.IsRepetitionEnd = false;
        bool oldIsInInstruction = this.IsInInstruction;
        this.IsInInstruction = isInInstructionInput;
        this.BeginRep = false;
        this.IsResumen = false;
        Debug.Log("::::::::::::::::::::. " + this.CentralNode.savedRandomAnimationIndex);
        Debug.Log("::::::::::::::::::::. " + this.CentralNode.actualRandomAnimationIndex);
        if (this.HasCentralNode)
        {
            if (!oldIsInInstruction && IsInInstruction)
                this.CentralNode.savedRandomAnimationIndex = this.CentralNode.actualRandomAnimationIndex;
            else if (oldIsInInstruction && !IsInInstruction)
                this.CentralNode.actualRandomAnimationIndex = this.CentralNode.savedRandomAnimationIndex;
        }
        Debug.Log(this.CentralNode.savedRandomAnimationIndex);
        Debug.Log(this.CentralNode.actualRandomAnimationIndex);
        Run();
    }


    public void PrepareWeb() {
        this.IsWeb = true;
        
        if(IsInterleaved)
            this._Opposite.IsWeb = true;
        PrepareWebInternal(); 
    }
    
    public void InitialPose()
    {
        this._behaviourState = AnimationBehaviourState.INITIAL_POSE;
    }

    public static AnimationBehaviour GetBehaviour(Movement m, Limb l)
    {
        int mov_search = (int)m / MAGIC_NUMBER;
        AnimationBehaviour encontrado = null;
        Animator a = GameObject.FindObjectOfType<AnimatorScript>().anim;
        AnimationBehaviour[] behaviours = a.GetBehaviours<AnimationBehaviour>();

        foreach (AnimationBehaviour lb in behaviours)
        {
            int lb_mov = (int)lb.movement / MAGIC_NUMBER;
            if (lb_mov == mov_search)
            {
                if (lb.animator == null)
                    lb.animator = a;

                if (lb.IsCentralNode && lb.limb == l)
                {
                    // Si se encontró un nodo central que calce con el 'Movement' entonces se retorna.
                    encontrado = lb;
                    break;
                }
                else if (!lb.HasCentralNode && lb.movement == m && l == lb.limb)
                    encontrado = lb;
            }
        }
        // Si no es un movimiento con nodo central, entonces se retorna. Se asume sin problemas que es el único encontrado.
        return encontrado;
    }


    protected static List<AnimationBehaviour> getFriendBehaviours(Movement m)
    {
        int mov_search = (int)m / MAGIC_NUMBER;
        List<AnimationBehaviour> encontrados = new List<AnimationBehaviour>();
        Animator a = GameObject.FindObjectOfType<AnimatorScript>().anim;
        AnimationBehaviour[] behaviours = a.GetBehaviours<AnimationBehaviour>();

        foreach (AnimationBehaviour lb in behaviours)
        {
            int lb_mov = (int)lb.movement / MAGIC_NUMBER;
            if (lb_mov == mov_search)
            {
                if (lb.animator == null)
                    lb.animator = a;

                if (!lb.IsCentralNode)
                {
                    //if (l == lb.limb)
                    encontrados.Add(lb);
                }
                
            }
        }
        // Si no es un movimiento con nodo central, entonces se retorna. Se asume sin problemas que es el único encontrado.
        return encontrados;
    }

    /// <summary>
    /// Obtiene todos los behaviours que calcen con el movement
    /// </summary>
    /// <param name="movement"></param>
    public static AnimationBehaviour GetCentralBehaviour(Movement movement, Limb limb)
    {
        int mov = (int)movement / MAGIC_NUMBER;
        Animator a = GameObject.FindObjectOfType<AnimatorScript>().anim;
        AnimationBehaviour[] behaviours = a.GetBehaviours<AnimationBehaviour>();
        List<AnimationBehaviour> centrals = new List<AnimationBehaviour>();
        foreach (AnimationBehaviour lb in behaviours)
        {
            int m = (int)lb.movement / MAGIC_NUMBER;
            if (m == mov)
            {
                if (lb.animator == null)
                {
                    lb.animator = a;
                }
                if (lb.IsCentralNode)
                {
                    centrals.Add(lb);
                }
            }
        }
        foreach (AnimationBehaviour lb in centrals)
        {
            if (lb.limb == limb)
                return lb;
        }
        return null;
        
    }

    protected AnimationBehaviourState _behaviourState;
    protected virtual AnimationBehaviourState _BehaviourState {
        get
        {
            if(IsCentralNode || !this.HasCentralNode)
            {
                return this._behaviourState;
            }
            else
            {
                return CentralNode._BehaviourState;
            }
            
        }
        set
        {
            if (IsCentralNode || !this.HasCentralNode)
            {
                this._behaviourState = value;
            }
            else
            {
                CentralNode._BehaviourState = value;
            }
            
        }
    }

    abstract public void Stop();
    protected void OnDestroy()
    {
        if (this.RepetitionEnd != null)
        {
            Delegate[] clientList = this.RepetitionEnd.GetInvocationList();
            foreach (var d in clientList)
                this.RepetitionEnd -= (d as EventHandler);
        }
    }

	protected void initializeRandomAnimations(List<Movement> animations)
	{
		
		AnimationBehaviour central = AnimationBehaviour.GetCentralBehaviour(this.movement, this.limb);
		AnimationBehaviour ab = (AnimationBehaviour)central;
		
		ab.randomAnimations = animations;
		ab.actualRandomAnimationIndex = 0;

        //ab.friendsBehaviours = this.friendsBehaviours;
    }
    protected void initializeRandomAnimations()
    {
        this.CentralNode.randomAnimations = new List<Movement>();
        List<AnimationBehaviour> friends = AnimationBehaviour.getFriendBehaviours(this.movement);

        foreach (AnimationBehaviour a in friends)
        {
            //randomAnimations.Add(new Movement(a.movement, a.laterality, a.limb));
            this.CentralNode.randomAnimations.Add(a.movement);
        }
    }
   
	
	protected void SetNextVariation()
	{
		++this.CentralNode.actualRandomAnimationIndex;
		int index = (int)this.CentralNode.actualRandomAnimationIndex % this.CentralNode.randomAnimations.Count;
		AnimatorScript.instance.CurrentExercise = 
            new Exercise(this.CentralNode.randomAnimations[index], this.CentralNode.laterality, this.CentralNode.limb);

        Debug.Log("Next Variation " + this.CentralNode.actualRandomAnimationIndex + " " + this.CentralNode.randomAnimations[index]);
    }
	
	protected List<Movement> GetRandomAnimations(List<Movement> exs)
	{
		List<Movement> random = new List<Movement>();
		
		exs.AddRange(exs);
		exs.AddRange(exs);
		//exs.AddRange(exs);
		//exs.AddRange(exs);
		
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


	protected bool _isAnimationRunning
	{
		get
		{
			return animationIsRunning();
		}
	}

	private bool animationIsRunning()
	{
        if (this.CentralNode._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS || this.CentralNode._BehaviourState == AnimationBehaviourState.RUNNING_DEFAULT)
        {
           // Debug.Log("True: " + this.CentralNode._BehaviourState);
            return true;
        }
        else
         {
           // Debug.Log("False: " + this.CentralNode._BehaviourState);
            return false;
        }
	}


	protected bool _isAnimationPreparing
	{
		get
		{
			return animationIsPreparing();
		}
	}
	
	private bool animationIsPreparing()
	{
		if (this.CentralNode._BehaviourState == AnimationBehaviourState.PREPARING_WEB || this.CentralNode._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
			return true;
		else
			return false;	
}

	protected bool _isAnimationStopped
	{
		get
		{
			return animationIsStopped();
		}
	}
	
	private bool animationIsStopped()
	{
		if (this.CentralNode._BehaviourState == AnimationBehaviourState.STOPPED )
			return true;
		else
			return false;	
	}

    protected int exerciceMovement
    {
        get
        {
            return this.animator.GetInteger("Movement");
        }
        set
        {
            this.animator.SetInteger("Movement", value);
        }
    }
    /// <summary>
    /// Sobreescribir en máquinas de estado que tengan nodo central
    /// </summary>
    protected abstract bool HasCentralNode { get; }
}
public enum AnimationBehaviourState
{
    STOPPED,
    PREPARING_DEFAULT,
    PREPARING_WITH_PARAMS,
    PREPARING_WEB,
    RUNNING_DEFAULT,
    RUNNING_WITH_PARAMS,
    INITIAL_POSE
}
public class BehaviourParams //: BehaviourParams
{
    public float Angle, ForwardSpeed = 1, BackwardSpeed = 1;
    public const float DEFAULT_TIME = 1.0f;
    public int SecondsBetweenRepetitions = 1;
    public int SecondsInPose = 1;
    public List<Movement> Variations;

    public BehaviourParams()
    {

    }

    /// <summary>
    /// Constructor usable en StayInPoseWithMovement
    /// </summary>
    /// <param name="_secondsInPose"></param>
    /// <param name="_secondsBetweenReps"></param>
    public BehaviourParams(int _secondsInPose, int _secondsBetweenReps)
    {
        SecondsBetweenRepetitions = _secondsBetweenReps;
        SecondsInPose = _secondsInPose;
    }
    /// <summary>
    /// Constructor usable en StayInPoseBehaviour
    /// </summary>
    /// <param name="_secondsInPose"></param>
    /// <param name="_secondsBetweenReps"></param>
    public BehaviourParams(int _secondsInPose, int _secondsBetweenReps, float _forwardSpeed, float _backwardSpeed)
    {
        SecondsBetweenRepetitions = _secondsBetweenReps;
        SecondsInPose = _secondsInPose;
        ForwardSpeed = _forwardSpeed;
        BackwardSpeed = _backwardSpeed;
    }

    /// <summary>
    /// Constructor para FiniteBehaviour
    /// </summary>
    /// <param name="_secondsBetweenReps"></param>
    /// <param name="_forwardSpeed"></param>
    /// <param name="_backwardSpeed"></param>
    public BehaviourParams(int _secondsBetweenReps, float _forwardSpeed, float _backwardSpeed)
    {
        SecondsBetweenRepetitions = _secondsBetweenReps;
        ForwardSpeed = _forwardSpeed;
        BackwardSpeed = _backwardSpeed;
    }

    /// <summary>
    /// Constructor para FiniteVariationBehaviour
    /// </summary>
    /// <param name="_secondsBetweenReps"></param>
    /// <param name="_forwardSpeed"></param>
    /// <param name="_backwardSpeed"></param>
    public BehaviourParams(List<Movement> _variations, int _secondsBetweenReps, float _forwardSpeed, float _backwardSpeed)
    {
        SecondsBetweenRepetitions = _secondsBetweenReps;
        ForwardSpeed = _forwardSpeed;
        BackwardSpeed = _backwardSpeed;
        Variations = _variations;
    }

    /// <summary>
    /// Constructor para StayInPoseWithVariationBehaviour 
    /// </summary>
    /// <param name="_secondsBetweenReps"></param>
    /// <param name="_forwardSpeed"></param>
    /// <param name="_backwardSpeed"></param>
    public BehaviourParams(List<Movement> _variations, int _secondsBetweenReps, float _speed, int _secondsInPose)
    {
        SecondsBetweenRepetitions = _secondsBetweenReps;
        ForwardSpeed = _speed;
        BackwardSpeed = _speed;
        Variations = _variations;
        SecondsInPose = _secondsInPose;
    }

    /// <summary>
    /// Constructor para StayInPoseXtreme 
    /// </summary>
    /// <param name="_secondsBetweenReps"></param>
    /// <param name="_forwardSpeed"></param>
    /// <param name="_backwardSpeed"></param>
    public BehaviourParams(List<Movement> _variations, int _secondsBetweenReps, int _secondsInPose)
    {
        SecondsBetweenRepetitions = _secondsBetweenReps;
        Variations = _variations;
        SecondsInPose = _secondsInPose;
    }
    /// <summary>
    /// Constructor usable en LerpBehaviour
    /// </summary>
    /// <param name="_angle"></param>
    /// <param name="_forwardSpeed"></param>
    /// <param name="_backwardSpeed"></param>
    /// <param name="_secondsBetweenReps"></param>
    
    public BehaviourParams(float _angle, float _forwardSpeed, float _backwardSpeed, int _secondsBetweenReps, int _secondsInPose)
    {
        Angle = _angle;
        ForwardSpeed = _forwardSpeed;
        BackwardSpeed = _backwardSpeed;
        SecondsBetweenRepetitions = _secondsBetweenReps;
        SecondsInPose = _secondsInPose;
    }

    /// <summary>
    /// Constructor general
    /// </summary>
    /// <param name="_angle"></param>
    /// <param name="_forwardSpeed"></param>
    /// <param name="_backwardSpeed"></param>
    /// <param name="_secondsInPose"></param>
    /// <param name="_secondsBetweenReps"></param>
    public BehaviourParams(float _angle, float _forwardSpeed, float _backwardSpeed, int _secondsInPose, int _secondsBetweenReps, List<Movement> variations)
    {
        Angle = _angle;
        ForwardSpeed = _forwardSpeed;
        BackwardSpeed = _backwardSpeed;
        SecondsBetweenRepetitions = _secondsBetweenReps;
        SecondsInPose = _secondsInPose;
        Variations = variations;
    }





}