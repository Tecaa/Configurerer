using UnityEngine;
using System.Collections;
using System;

public abstract class AnimationBehaviour : StateMachineBehaviour {

    //protected ExerciseDataGenerator exerciseDataGenerator;
    public event EventHandler RepetitionEnd;
    public Movement movement;
    public Limb limb;
    [HideInInspector]
    public Laterality execution;
    [HideInInspector]
    public Animator animator;
    private bool _isInterleaved;
    [HideInInspector]

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
    public bool isWeb = false;
    
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
    protected BehaviourParams _actualLerpParams;
    protected BehaviourParams _realLerpParams;
    protected BehaviourParams _RealLerpParams
    {
        get
        {
            return _realLerpParams;
        }
        set
        {
            _realLerpParams = value;

            if (this.IsInterleaved)
            {
                Debug.Log("preparing interleaved");
                this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
                //this._Opposite.IsInterleaved = IsInterleaved;
                if (this._Opposite._RealLerpParams != value)
                    this._Opposite._RealLerpParams = value;
            }
        }
    }

    public void SetBehaviourState(AnimationBehaviourState lbs)
    {
        Debug.LogWarning("SetLerpBehaviour : " + lbs);
        this._behaviourState = lbs;
    }
    protected void OnRepetitionEnd()
    {
        //Cambios detector Diego
//        if(this._behaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS && Controller.KsStateMachine.CurrentState == Kinectsiology.Communication.Command.DataTypes.GameInfo.StateType.Playing)
//            ActionDetectorTemp.Instance.Evaluate();
        
        EventHandler eh = RepetitionEnd;
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }
    protected AnimationBehaviourState originalABS = AnimationBehaviourState.STOPPED;
    protected bool IsWaiting = false;
    public virtual void ResumeAnimation(){
        //this.Run();
        //this._behaviourState = originalABS;
        //animator.speed = this._RealLerpParams.ForwardSpeed;
        IsWaiting = false;
    }
    public virtual void PauseAnimation(){
        /*
        originalABS = this._behaviourState;
        this._behaviourState = AnimationBehaviourState.STOPPED;
        animator.speed = 0;
        
        if (IsInterleaved)
        {
            if (this.limb == Limb.Right)
                this._Opposite.PauseAnimation();
        }*/
        IsWaiting = true;
    }
    abstract public void Prepare(BehaviourParams lp);
    abstract public void PrepareWeb();
    abstract public void Run();
    abstract public void RunWeb();
    abstract public void RunWeb(BehaviourParams lerpParams);
    
    public void InitialPose()
    {
        //animator.Play(0, 0, 0);
        //animator.speed = 1;
        this._behaviourState = AnimationBehaviourState.INITIAL_POSE;
        //animator.speed = 0;
    }
   
    public static AnimationBehaviour GetBehaviour(Movement m, Limb l)
    {
        Animator a = GameObject.FindObjectOfType<AnimatorScript>().anim;
        AnimationBehaviour[] behaviours = a.GetBehaviours<AnimationBehaviour>();
        Limb l2 = l;
        if (l == Limb.Interleaved)
        {
            l2 = Limb.Left;
        }
        foreach (AnimationBehaviour lb in behaviours)
        {
            if (lb.movement == m && lb.limb == l2)
            {
                if (lb.animator == null)
                {
                    lb.animator = a;
                    if (l == Limb.Interleaved)
                        lb.IsInterleaved = true;
                }
                return lb;
            }
        }
        return null;
    }

    protected AnimationBehaviourState _behaviourState;

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