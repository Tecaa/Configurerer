using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StayInPoseWithVariationBehaviour : AnimationBehaviour {

    private uint storedNextExercice;
    private bool hasEnteredBefore = false;
    private event EventHandler StayInPoseWithVariationRoundTripEnd;
	public new StayInPoseWithVariationBehaviour CentralNode
	{
		get
		{
            
            if( this.IsCentralNode)
            {
                return this;
            }
            else
            {
                if (_centralNode == null)
                    _centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement);
                return (StayInPoseWithVariationBehaviour)_centralNode;
            }
            
            
		}
	}

    private float ExerciceVelocity;
    public float exerciceVelocity
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._realParams.ForwardSpeed;
            }
            else
            {
                return CentralNode._realParams.ForwardSpeed;
            }

        }
    }



    private float SecondsBetweenRepetitions;
    public float secondsBetweenRepetitions
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._realParams.SecondsBetweenRepetitions;
            }
            else
            {
                return CentralNode._realParams.SecondsBetweenRepetitions;
            }

        }
    }

    private bool IsExecutingMovement = false;
    public bool isExecutingMovement
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this.IsExecutingMovement;
            }
            else
            {
                return CentralNode.IsExecutingMovement;
            }

        }
        set
        {
            if (this.IsCentralNode)
            {
                this.IsExecutingMovement = value;
            }
            else
            {
                CentralNode.IsExecutingMovement = value;
            }

        }
    }


    //
    //==== Controlar tiempos de repeticiones
    //


    public ClockBehaviour _ClockBehaviour;
    public ClockBehaviour clockBehaviour
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._ClockBehaviour;
            }
            else
            {
                return CentralNode._ClockBehaviour;
            }

        }
        set
        {
            if (this.IsCentralNode)
            {
                this._ClockBehaviour = value;
            }
            else
            {
                CentralNode._ClockBehaviour = value;
            }
        }
    }

    public float secondsInPose
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this._realParams.SecondsInPose; 
            }
            else
            {
                return CentralNode._realParams.SecondsInPose;
            }

        }
    }

   

    //
    //==== FIN Controlar tiempos de repeticiones
    //


    //===== FIN RELOJ


    #region implemented abstract members of AnimationBehaviour

    public override void Prepare (BehaviourParams bp)
	{
		BehaviourParams lp = (BehaviourParams)bp;
		this.CentralNode._RealParams = lp;
        this._BehaviourState = AnimationBehaviourState.PREPARING_WITH_PARAMS;
		this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
	}

	protected override void PrepareWebInternal ()
	{
        //BehaviourParams lp = (BehaviourParams)bp;

        Debug.Log("haciendo prepare web");
        this._BehaviourState = AnimationBehaviourState.PREPARING_WEB;
        //this.initializeRandomAnimations(this.GetRandomAnimations(bp.Variations));
    }

	public override void Run ()
	{

		this.CentralNode.endRepTime = null;
		this._BehaviourState = AnimationBehaviourState.RUNNING_WITH_PARAMS;
		this.StayInPoseWithVariationRoundTripEnd -= StayInPoseWithVariationBehaviour_VariationTripEnd;
		this.StayInPoseWithVariationRoundTripEnd += StayInPoseWithVariationBehaviour_VariationTripEnd; //¿por que subscribirse y desuscribirse?


	}

	public override void RunWeb ()
    {
        this.CentralNode.endRepTime = null;
        this.CentralNode._RealParams = new BehaviourParams();
        Debug.Log("cayendo en el run web sin parametros");
        this.initializeRandomAnimations();
        this._BehaviourState = AnimationBehaviourState.RUNNING_DEFAULT;
    }

	public override void RunWeb (BehaviourParams lerpParams)
	{
		
	}

	/// <summary>
	/// Detiene la interpolación que actualmente se está ejecutando
	/// </summary>
	override public void Stop()
	{
        exerciceMovement = (int)Movement.Iddle;
		this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
		animator.speed = 1;
	}
    #endregion


    private void executionTimerStart()
    {
        Debug.Log("comienza ejecucion| HORA: " + DateTime.Now.ToString());
        isExecutingMovement = true;
    }

    private void executionTimerFinish()
    {
        Debug.Log("termina ejecucion| HORA: " + DateTime.Now.ToString());
        isExecutingMovement = false;

        if (_isAnimationRunning)
        {
            finishRepetitionExecution();
        }

    }


    /**
    Cuando se acaba el tiempo de pausa entre repeticiones
    **/
    private void pauseBetweenRepetitionsFinish()
    {
        clockBehaviour.stopTimeBetweenRepetitionsTimer();
        Debug.Log("termina pausa entre repeticiones| HORA: " + DateTime.Now.ToString());
        SetNextVariation();
        animator.speed = exerciceVelocity;

    }

    private void pauseBetweenRepetitionsStart()
    {
        Debug.Log("comienza pausa entre repeticiones| HORA: " + DateTime.Now.ToString());
    }

    protected void OnStayInPoseWithVariationRoundTripEnd()
	{
		
		if (!IsCentralNode)
		{
			this.CentralNode.OnStayInPoseWithVariationRoundTripEnd();
		}
		else
		{
			EventHandler eh = StayInPoseWithVariationRoundTripEnd;
			if (eh != null)
			{
				eh(this, new EventArgs());
			}
		}
	}
	
	void StayInPoseWithVariationBehaviour_VariationTripEnd(object sender, EventArgs e)
	{
		if (_BehaviourState != AnimationBehaviourState.RUNNING_DEFAULT)
		{
			this.CentralNode.endRepTime = DateTime.Now;
		}
	}


    private void startNewExecution()
    {

        clockBehaviour.executeRepetitionTime(secondsInPose);
    }


	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{

        if (this.IsCentralNode && hasEnteredBefore == false)
        {
            hasEnteredBefore = true;

            clockBehaviour = new ClockBehaviour();
            clockBehaviour.executionTimerFinish += executionTimerFinish;
            clockBehaviour.executionTimerStart += executionTimerStart;
            clockBehaviour.pauseBetweenRepetitionsStart += pauseBetweenRepetitionsStart;
            clockBehaviour.pauseBetweenRepetitionsFinish += pauseBetweenRepetitionsFinish;
            
        }


            

        if (_isAnimationPreparing)
		{
			Debug.Log ("esta preparando");
			OnRepetitionEnd();
			Stop();
            clockBehaviour.stopExecutionTimer();
        }
		else if (this._BehaviourState == AnimationBehaviourState.PREPARING_WITH_PARAMS || this._BehaviourState == AnimationBehaviourState.RUNNING_WITH_PARAMS
		         || this._BehaviourState == AnimationBehaviourState.INITIAL_POSE)
		{
			//Como en este behaviour se utiliza animation.Play para cada repetición, se entra más de una vez al metodo OnStateEnter, 
			//por lo que si ya se ha entrado alguna vez, la velocidad se asigna como 0 para que se respete el tiempo entre ejecución 
			//antes de comenzar la siguiente repetición.
			
			//Se asume que si el ejercicio utiliza solo un tipo de velocidad, el forwardspeed y backwardspeed serán iguales.
			
			animator.speed = this.CentralNode._RealParams.ForwardSpeed;
			
		}
		
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!IsCentralNode)
		{
			beginRep = false;

			if ( _isAnimationRunning == true )
			{


                if(isExecutingMovement == true)
                {
                    SetNextVariation();
                }
                

				if (!this.IsWeb)
				{
					//this.PauseAnimation();
				}

				if (_isAnimationStopped == true)
				{
					this.CentralNode.endRepTime = null;
				}
			}
			else
			{

			}


		}
		else
		{
            if(animator.GetInteger("Movement") != -1)
            {
                    startNewExecution();
            }
                

        }

	}




    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        clockBehaviour.Update();

        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            animator.speed = 0;
            return;
        }

        if (_isAnimationPreparing || _isAnimationStopped)
            return;

    }


    protected override void OnRepetitionEnd()
	{
        
		if (!IsCentralNode)
        {
            this.CentralNode.OnRepetitionEnd();
        }	
		else
        {
            base.OnRepetitionEnd();
            
        }
			
	}

	public BehaviourParams GetParams()
	{
		return this._currentParams;
	}

    private void finishRepetitionExecution()
    {
        exerciceMovement = -1;
        OnRepetitionEnd();
        this.PauseAnimation();
        clockBehaviour.stopExecutionTimer();
    }



    public override void ResumeAnimation()
    {
        base.ResumeAnimation();
        clockBehaviour.executeTimeBetweenRepetitions(secondsBetweenRepetitions);
    }


    public override void PauseAnimation()
    {
        originalABS = this._BehaviourState;


        if (this.CentralNode != null)
        {
            this.CentralNode.originalABS = this._BehaviourState;
        }

        this._BehaviourState = AnimationBehaviourState.STOPPED;
        
        if (IsInterleaved)
        {
            if (this.limb == Limb.Right)
                this._Opposite.PauseAnimation();
        }
    }

    new void OnDestroy()
    {
        /**
        if (this.IsInterleaved)
            this._Opposite.RepetitionEnd -= _Opposite_RepetitionEnd;

        this.LerpRoundTripEnd -= LerpBehaviour_LerpRoundTripEnd;
         **/

        //animatorClock.pauseBetweenRepetitionsStart -= pauseBetweenRepetitionsStart;
        //animatorClock.pauseBetweenRepetitionsFinish -= pauseBetweenRepetitionsFinish;

        base.OnDestroy();
    }

    

}






