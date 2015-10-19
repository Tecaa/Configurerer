using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FiniteBehaviour : AnimationBehaviour
{
    private bool isLerping = false;
    private bool ReadyToLerp = false;
    private event EventHandler LerpRoundTripEnd;
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
        if (IsInterleaved)
        {
            (this._Opposite as LerpBehaviour).endRepTime = endRepTime;
        }
    }

    public DateTime? endRepTime = null;

    public BehaviourParams GetParams()
    {
        return this._actualLerpParams;
    }
    private AnimationBehaviourState _BehaviourState
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
        this._RealLerpParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
        if (IsInterleaved)
            this._Opposite.RepetitionEnd += _Opposite_RepetitionEnd;
    }

    private void _Opposite_RepetitionEnd(object sender, EventArgs e)
    {
        OnRepetitionEnd();
    }
    override public void PrepareWeb()
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
        Application.ExternalCall("Write", "Unity: RunWeb(BehaviourParams bp) " + Newtonsoft.Json.JsonConvert.SerializeObject(bp) + " " + movement + " " + this.GetInstanceID());
        BehaviourParams lerpParams = (BehaviourParams)bp;
        endRepTime = null;
        isLerping = false;
        this._RealLerpParams = lerpParams;
        this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
        this.LerpRoundTripEnd += LerpBehaviour_LerpRoundTripEnd;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Application.ExternalCall("Write", "Estado: " + "  debug: OnStateEnter 1" + this._BehaviourState.ToString() + "  " + movement);
        if(this._BehaviourState == AnimationBehaviourState.PREPARING_WEB)
        {
            OnRepetitionEnd();
            Stop();
        }
        else if(this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS
            || this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            //Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.
            animator.speed = this._RealLerpParams.ForwardSpeed;
            Application.ExternalCall("Write", "debug: OnStateEnter 2" + this._behaviourState.ToString() + " " + movement + " " + this.GetInstanceID());

        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            animator.speed = 0;
            return;
        }

        if ((_BehaviourState != AnimationBehaviourState.STOPPED && _BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
    && (endRepTime == null || new TimeSpan(0, 0, (int)_RealLerpParams.SecondsBetweenRepetitions) <= DateTime.Now - endRepTime))
        {
            if(stateInfo.normalizedTime >= 1.0f)
            {
                if(this._behaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                {
                    OnLerpRoundTripEnd();
                    if (!IsInterleaved || IsInterleaved && limb == Limb.Right)
                    {
                        OnRepetitionEnd();
                        this._BehaviourState = AnimationBehaviourState.STOPPED;
                        animator.speed = 0;
                    }
                    else if (IsInterleaved)
                    {
                        animator.SetTrigger("ChangeLimb");
                    }/*
                    if (this._BehaviourState == AnimationBehaviourState.STOPPED)
                    {
                        endRepTime = null;
                        ReadyToLerp = false;
                    }*/
                }
                else if(this._behaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS)
                {
                    OnRepetitionEnd();
                    Stop();
                }

            }
            else
            {
                if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._behaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS)
                    if(stateInfo.normalizedTime <= 0.5f)
                    {

                        animator.speed = this._RealLerpParams.ForwardSpeed;
                        Debug.Log("Time: " + stateInfo.normalizedTime +  "     Velpalante: " + this._RealLerpParams.ForwardSpeed);
                    }
                    else
                    {
                        animator.speed = this._RealLerpParams.BackwardSpeed;
                        Debug.Log("Time: " + stateInfo.normalizedTime + "     Velpatras: " + this._RealLerpParams.BackwardSpeed);
                    }
            }
        }
        else if (this._BehaviourState == AnimationBehaviourState.STOPPED &&
            (endRepTime != null && new TimeSpan(0, 0, (int)_RealLerpParams.SecondsBetweenRepetitions) <= DateTime.Now - endRepTime))
        {
            //Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.
            endRepTime = null;
            this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
            animator.speed = this._RealLerpParams.ForwardSpeed;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1.0f;
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
        Application.ExternalCall("Write", "stop  " + movement + " " + this.GetInstanceID());
        //this._BehaviourState = AnimationBehaviourState.STOPPED;
        animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
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