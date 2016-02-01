using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public abstract class AnimationBehaviour : StateMachineBehaviour {

    //protected ExerciseDataGenerator exerciseDataGenerator;
    
    public event EventHandler RepetitionEnd;

    /// <summary>
    /// Se dispara despues del tiempo entre ejecciones
    /// </summary>
    public event EventHandler RepetitionReallyStart;
    public Movement movement;
    public Limb limb;
    public Laterality execution;
    [HideInInspector]
    public Animator animator;
    private bool _isInterleaved;
    [HideInInspector]
    public DateTime? endRepTime = null;
    [HideInInspector]
    public bool beginRep = false;
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
                this._Opposite.SetBehaviourState(AnimationBehaviourState.RUNNING_WITH_PARAMS);
                //this._Opposite.IsInterleaved = IsInterleaved;
                if (this._Opposite._RealLerpParams != value)
                    this._Opposite._RealLerpParams = value;
            }
        }
    }

    public void SetBehaviourState(AnimationBehaviourState lbs)
    {
        this._behaviourState = lbs;
    }
    protected void OnRepetitionEnd()
    {
        DebugLifeware.Log("OnRepetitionEnd", DebugLifeware.Developer.Alfredo_Gallardo);        
        EventHandler eh = RepetitionEnd;
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }
    protected void OnRepetitionReallyStart()
    {
        DebugLifeware.Log("OnRepetitionStart after " + (DateTime.Now - endRepTime), DebugLifeware.Developer.Alfredo_Gallardo);
        EventHandler eh = RepetitionReallyStart;
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }
    protected AnimationBehaviourState originalABS = AnimationBehaviourState.STOPPED;
    public void ResumeAnimation()
    {
        if (this.IsInterleaved && this.limb == Limb.Left)
        {
            animator.SetTrigger("ChangeLimb");
            this._Opposite.SetBehaviourState(originalABS);
        }

        this._BehaviourState = originalABS;
        this.endRepTime = DateTime.Now;
        if (this.IsInterleaved)
            this._Opposite.endRepTime = this.endRepTime;
        
    }
    public void PauseAnimation(){
        originalABS = this._behaviourState;
        this._behaviourState = AnimationBehaviourState.STOPPED;
        animator.speed = 0;
        
        
        if (IsInterleaved)
        {
            if (this.limb == Limb.Right)
                this._Opposite.PauseAnimation();
        }
    }
    abstract public void Prepare(BehaviourParams lp);
    abstract protected void PrepareWebInternal();
    abstract public void Run();
    abstract public void RunWeb();
    abstract public void RunWeb(BehaviourParams lerpParams);

    
    public void PrepareWeb() {
        this.isWeb = true;
        if(IsInterleaved)
            this._Opposite.isWeb = true;
        PrepareWebInternal(); 
    }
    
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

    /// <summary>
    /// Obtiene todos los behaviours que calcen con el movement
    /// </summary>
    /// <param name="movement"></param>
    public static List<AnimationBehaviour> GetBehaviours(Movement movement)
    {

        Animator a = GameObject.FindObjectOfType<AnimatorScript>().anim;
        AnimationBehaviour[] behaviours = a.GetBehaviours<AnimationBehaviour>();
        List<AnimationBehaviour> list = new List<AnimationBehaviour>();
        foreach (AnimationBehaviour lb in behaviours)
        {
            if (lb.movement == movement)
            {
                if (lb.animator == null)
                {
                    lb.animator = a;
                }
                list.Add(lb);
            }
        }
        return list;
    }

    protected AnimationBehaviourState _behaviourState;
    protected virtual AnimationBehaviourState _BehaviourState {   get  { return _behaviourState; } set { _behaviourState = value; }}

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