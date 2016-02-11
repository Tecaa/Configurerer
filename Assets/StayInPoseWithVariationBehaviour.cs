using Assets;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StayInPoseWithVariationBehaviour : AnimationBehaviour {

    
    private uint storedNextExercice;

	private event EventHandler StayInPoseWithVariationRoundTripEnd;
	public new StayInPoseWithVariationBehaviour CentralNode
	{
		get
		{
			if (_centralNode == null)
				_centralNode = AnimationBehaviour.GetCentralBehaviour(this.movement);
			return (StayInPoseWithVariationBehaviour)_centralNode;
		}
	}


    //
    //==== Controlar tiempos de repeticiones
    //

    const float INTERVAL = 0.1f;
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

    private float TimeSinceStart = 0 ;
    public float timeSinceStart
    {
        get
        {
            if (this.IsCentralNode)
            {
                return this.TimeSinceStart;
            }
            else
            {
                return CentralNode.TimeSinceStart;
            }

        }
        set
        {
            if(this.IsCentralNode)
            {
                TimeSinceStart = value;
            }
            else
            {
                CentralNode.timeSinceStart = value;
            }
        }
    }

    private float DeltaTime = 0;
    public float deltaTime
    {
        get
        {

            if (this.IsCentralNode)
            {
                return this.DeltaTime;
            }
            else
            {
                return CentralNode.DeltaTime;
            }
            
        }
        set
        {
            if (this.IsCentralNode)
            {
                DeltaTime = value;
            }
            else
            {
                CentralNode.DeltaTime = value;
            }
        }
    }

    //
    //==== FIN Controlar tiempos de repeticiones
    //


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
		throw new System.NotImplementedException ();
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
		throw new System.NotImplementedException ();
	}

	public override void RunWeb (BehaviourParams lerpParams)
	{
		throw new System.NotImplementedException ();
	}

	/// <summary>
	/// Detiene la interpolación que actualmente se está ejecutando
	/// </summary>
	override public void Stop()
	{
	
		animator.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
		this.CentralNode._BehaviourState = AnimationBehaviourState.STOPPED;
		animator.speed = 1;
	}
	#endregion


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

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        if (this.IsCentralNode)
            timeSinceStart = 0;

        if (_isAnimationPreparing)
		{
			Debug.Log ("esta preparando");
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

                if(timeSinceStart < secondsInPose)
                {
                    Debug.Log("next variation " + timeSinceStart + "  <  " + secondsInPose);
                    SetNextVariation();
                }
                

				if (!this.IsWeb)
				{
                    //Debug.Log("no es web");
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

		}

	}




    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
        if (_BehaviourState == AnimationBehaviourState.INITIAL_POSE)
        {
            animator.speed = 0;
            return;
        }

        if (_isAnimationPreparing || _isAnimationStopped)
            return;

        float delta = Time.deltaTime;
        timeSinceStart += delta;
        deltaTime += delta;


        if(deltaTime >= INTERVAL) //ha transcurrido un intervalo
        {
            deltaTime = deltaTime - INTERVAL;
            
            if(timeSinceStart >= secondsInPose && _isAnimationRunning)
            {
                finishRepetition();
            }

        }


    }


    protected override void OnRepetitionEnd()
	{
        
		if (!IsCentralNode)
        {
            this.CentralNode.OnRepetitionEnd();
        }	
		else
        {
            Debug.Log("repeticion terminada");
            base.OnRepetitionEnd();
        }
			
	}

	public BehaviourParams GetParams()
	{
		return this._actualParams;
	}

    private void finishRepetition()
    {
        Debug.LogWarning("se ha acabado una repeticion");

        
        animator.SetInteger(AnimatorParams.Movement, -1);
        
        OnRepetitionEnd();
        this.PauseAnimation();

    }


}






