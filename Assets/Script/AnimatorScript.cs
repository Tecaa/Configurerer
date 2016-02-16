using UnityEngine;
using System.Collections;
using System.Timers;
using System;
using System.ComponentModel;
using Assets;
using System.Collections.Generic;
using Newtonsoft.Json;

// Require these components when using this script
[RequireComponent(typeof(Animator))]

public class AnimatorScript : MonoBehaviour
{
    #region Variables privadas
    /// <summary>
    /// Referencia al Animator
    /// </summary>
    [HideInInspector]
    public Animator anim  {
        get
        {
            return AnimatorScript.instance.GetComponent<Animator>();
        }
    }
    public HumanoidUtils utils
    {
        get
        {

            return AnimatorScript.instance.GetComponent<HumanoidUtils>();
        }
    }
    private Exercise _currentExercise = new Exercise();
    private const float DELAY = 2f;
    public static AnimatorScript instance{
        get{
            return FindObjectOfType<AnimatorScript>();
        }
    }

    private IEnumerator prepareCR;

    public event EventHandler OnRepetitionStart;
    public event EventHandler OnRepetitionEnd;
    public event EventHandler OnRepetitionReallyStart;
    public event EventHandler<PrepareEventArgs> OnPrepareExerciseStart;
    public event EventHandler<PrepareEventArgs> OnPrepareExerciseEnd;


    Caller prepareCaller;
    [HideInInspector]
    public BehaviourParams behaviourParams;

    public Exercise CurrentExercise
    {
        get
        {
            return _currentExercise;
        }
        set
        {
            //value = (Exercise)value;
            _currentExercise = value;
            RewindExercise();
        }
    }


    #endregion

    
    void Start()
    {
        CurrentExercise.PropertyChanged += currentExercise_PropertyChanged;

        /////////   INICIO CODIGO DE TESTEO //////////
        Invoke("testPrepare", 1);

        //Invoke("testRun", 12);

//        AnimatorScript.OnPrepareExerciseEnd += testing;
        /////////   FIN CODIGO DE TESTEO    //////////
    }

    public void RewindExercise()
    {
        anim.SetInteger(AnimatorParams.Limb, (int)_currentExercise.Limb);
        anim.SetInteger(AnimatorParams.Laterality, (int)((Laterality)_currentExercise.Laterality));
        anim.SetInteger(AnimatorParams.Movement, (int)_currentExercise.Movement);
    
    }
    private void testing(object sender, EventArgs e)
    {
        BehaviourParams p = new BehaviourParams(40, 1, 1, 0, 0);
        this.RunExerciseWeb(JsonConvert.SerializeObject(p));
    }
    public void testPrepare()
    {
                
        //PrepareExercise(new Exercise(Movement.EstocadaFrontalCortaConTorsiónDeTronco_60, Laterality.Single, Limb.Interleaved), new BehaviourParams(70, 1.5f, 0.4f, 3));
        //PrepareExercise(new Exercise(Movement.DesplazamientoLateralConSalto_100, Laterality.Double, Limb.None), new BehaviourParams(60, 1.5f, 0.4f, 3));
        //PrepareExercise(new Exercise(Movement.PruebaMantenerPose, Laterality.Single, Limb.Right), new BehaviourParams(60, 1.5f,1.5f, 3, 2));
		//PrepareExercise(new Exercise(Movement.PenduloEnBipedoCon45DeFlexiónDeTronco, Laterality.Single, Limb.Left), new BehaviourParams(60, 1.5f,1.5f, 6, 3));
        /*PrepareExercise(new Exercise(Movement.PruebaA, Laterality.Single, Limb.Left), new BehaviourParams(new List<Exercise>() {
            { new Exercise(Movement.PruebaC, Laterality.Single, Limb.Left) },
            { new Exercise(Movement.PruebaA, Laterality.Single, Limb.Left) },
            { new Exercise(Movement.PruebaB, Laterality.Single, Limb.Left) },
            { new Exercise(Movement.PruebaC, Laterality.Single, Limb.Right) },
            { new Exercise(Movement.PruebaA, Laterality.Single, Limb.Right) },
            { new Exercise(Movement.PruebaB, Laterality.Single, Limb.Right) },
        }, 2, 2, 0.5f));*/
		
		PrepareExercise(new Exercise(Movement.Milton_A, Laterality.Single, Limb.Left), new BehaviourParams(new List<Exercise>() {
			{ new Exercise(Movement.Milton_A, Laterality.Single, Limb.Left) },
			{ new Exercise(Movement.Milton_B, Laterality.Single, Limb.Left) },
			{ new Exercise(Movement.Milton_C, Laterality.Single, Limb.Left) },
			{ new Exercise(Movement.Milton_D, Laterality.Single, Limb.Left) },
		}, 2, 1));
        /*
		PrepareExercise(new Exercise(Movement.Pablo_A, Laterality.Single, Limb.Left), new BehaviourParams(new List<Exercise>() {
			{ new Exercise(Movement.Pablo_A, Laterality.Single, Limb.Left) },
			{ new Exercise(Movement.Pablo_B, Laterality.Single, Limb.Left) },
			{ new Exercise(Movement.Pablo_C, Laterality.Single, Limb.Left) },
			{ new Exercise(Movement.Pablo_D, Laterality.Single, Limb.Left) },
		}, 2, 1.5f, 8));
        */

        PrepareExerciseWebParams webParam = new PrepareExerciseWebParams(new Exercise(Movement.Pablo_A, Laterality.Single, Limb.Left), Caller.Preview);
        PrepareExerciseWeb(JsonConvert.SerializeObject(webParam));
        /*
        PrepareExerciseWeb("{\"Exercise\":{\"Movement\":" + (int)Movement.PruebaA + ",\"Laterality\":" + (int)Laterality.Single + ",\"Limb\":"
           + (int)Limb.Right + "}, \"Caller\": 1}");
        */
        //PrepareExerciseWeb("{\"Exercise\":{\"Movement\":" + (int)Movement.PruebaMantenerPose + ",\"Laterality\":" + (int)Laterality.Single + ",\"Limb\":"
        //  + (int)Limb.Right + "}, \"Caller\": 1}");
    }
    public void testRun()
    {
        //string s = "{\"Angle\":45,\"ForwardSpeed\":2,\"BackwardSpeed\":2,\"SecondsInPose\":3,\"SecondsBetweenRepetitions\":2}";
        //RunExerciseWeb(s);
        /*BehaviourParams p = new BehaviourParams(new List<Exercise>() {
            { new Exercise(Movement.PruebaC, Laterality.Single, Limb.Left) },
            { new Exercise(Movement.PruebaA, Laterality.Single, Limb.Left) },
        }, 3, 0.5f, 2);
        string s = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        RunExerciseWeb(s);
    */    
        //RunExercise();
       // RunExerciseWebWithoutParams();
     
        BehaviourParams p = new BehaviourParams(new List<Exercise>() {
            { new Exercise(Movement.Pablo_A, Laterality.Single, Limb.Left) },
            { new Exercise(Movement.Pablo_B, Laterality.Single, Limb.Left) },
            { new Exercise(Movement.Pablo_C, Laterality.Single, Limb.Left) },
            { new Exercise(Movement.Pablo_D, Laterality.Single, Limb.Left) },
        }, 2, 1.5f, 8);
        RunExerciseWeb(Newtonsoft.Json.JsonConvert.SerializeObject(p));
    }
    public void testResume()
    {
        ResumeExercise();
    }
    void pruebaRun3()
    {
        //PrepareExerciseWeb("{\"Exercise\":{\"Movement\":30000,\"Laterality\":1,\"Limb\":3}, \"Caller\": 1}");
        //PrepareExerciseWeb("{\"Exercise\":{\"Movement\": 160002,\"Laterality\":0,\"Limb\":0}, \"Caller\": 1}");
        PrepareExerciseWeb("{\"Exercise\":{\"Movement\": 220000,\"Laterality\":0,\"Limb\":0}, \"Caller\": 1}");

    }

    void Awake()
    {
        //TODO: No olvidar borrar, elimina el caché de los ejercicios ya preparados
        PlayerPrefs.SetString("ExerciseCache", null);
    }
    // Update is called once per frame


    void currentExercise_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Exercise exercise = (Exercise)sender;
        switch (e.PropertyName)
        {
            case AnimatorParams.Movement:
                anim.SetInteger(AnimatorParams.Movement, (int)exercise.Movement);
                break;

            case AnimatorParams.Laterality:
                anim.SetInteger(AnimatorParams.Laterality, (int)exercise.Laterality);
                break;

            case AnimatorParams.Limb:
                anim.SetInteger(AnimatorParams.Limb, (int)exercise.Limb);
                break;
        }
    }

    AnimationBehaviour behaviour;

   
    /// <summary>
    /// Nota: Si hay errores de Quaternion to Matrix comprobar que los parametros enviados son validos para el ejercicio indicado
    /// </summary>
    /// <param name="e"></param>
    /// <param name="param"></param>
    public void PrepareExercise(Exercise e, BehaviourParams param)
    {
        if (param.Variations == null)
            behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        else
            behaviour = AnimationBehaviour.GetCentralBehaviour(e.Movement);
        DebugLifeware.Log(e.Movement + " " + e.Limb + "  " + e.Laterality + " " + behaviour.GetType(), DebugLifeware.Developer.Marco_Rojas);
		DebugLifeware.Log (Newtonsoft.Json.JsonConvert.SerializeObject(param), DebugLifeware.Developer.Marco_Rojas);

        behaviour.Prepare(param);
        behaviour.RepetitionEnd += behaviour_PrepareEnd;

        if (param.Variations == null)
            CurrentExercise = e;
        else
            CurrentExercise = behaviour.randomAnimations[0];
    }

    private IEnumerator InitialPoseDelayed()
    {

        //yield return new WaitForSeconds(DELAY);
        RewindExercise();
        yield return new WaitForSeconds(0.1f);
        behaviour.InitialPose();
    }


    public void InitialPose()
    {
        StartCoroutine(InitialPoseDelayed());
    }

    public class PrepareExerciseWebParams
    {
        public Exercise Exercise { get; set; }
        public Caller Caller { get; set; }
        public PrepareExerciseWebParams(Exercise e, Caller c)
        {
            this.Exercise = e;
            this.Caller = c;
        }
    }
    public void PrepareExerciseWeb(string s)
    {
        
        RaiseEvent(OnPrepareExerciseStart, PrepareStatus.Preparing);
        PrepareExerciseWebParams pwp = JsonConvert.DeserializeObject<PrepareExerciseWebParams>(s);
        Exercise e = (pwp.Exercise) as Exercise;
        this.prepareCaller = (Caller)(pwp.Caller);
        //Exercise e = JsonConvert.DeserializeObject<Exercise>(s);

        behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        if (behaviour == null)
        {
            DebugLifeware.LogAllDevelopers("Importante: Behaviour no encontrado");
            RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.NotFound);
            return;
        }
        behaviour.RepetitionEnd += behaviour_PrepareEndWeb;
        behaviour.PrepareWeb();
        CurrentExercise = e;    
    }

    void behaviour_PrepareEndWeb(object sender, EventArgs e)
    {
        //behaviour = sender as LerpBehaviour;
        behaviour = sender as AnimationBehaviour;
        behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
        RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared);
    }

    public void RunExercise()
    {
        //DebugLifeware.Log("Comienza el run", DebugLifeware.Developer.Marco_Rojas);
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.Run();
        RewindExercise();
        behaviour.RepetitionEnd += behaviour_RepetitionEnd;
        behaviour.RepetitionReallyStart += behaviour_RepetitionReallyStart;
    }

    void behaviour_RepetitionReallyStart(object sender, EventArgs e)
    {
        RaiseEvent(OnRepetitionReallyStart);
    }

    public void ResumeExercise()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.ResumeAnimation();
    }
    void behaviour_RepetitionEnd(object sender, EventArgs e)
    {
        //Debug.Log("Rep Realizada por instructor");
        //Terminó una repetición
        RaiseEvent(OnRepetitionEnd);
    }

    void behaviour_PrepareEnd(object sender, EventArgs e)
    {
//        GoToIddle();
        //DebugLifeware.Log("Preparación terminada", DebugLifeware.Developer.Marco_Rojas);
        behaviour = sender as AnimationBehaviour;
        behaviour.RepetitionEnd -= behaviour_PrepareEnd;
        RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared);
    }

    public void testWeb()
    {
        //string s = "{\"Angle\":45,\"ForwardSpeed\":1.0,\"BackwardSpeed\":1.0,\"SecondsInPose\":0,\"SecondsBetweenRepetitions\":3}";
        //RunExerciseWeb(s);
        RunExerciseWebWithoutParams();
    }
    public void testWeb2()
    {
        StopExercise();
    }
    public void RunExerciseWeb(string s)
    {
        BehaviourParams rep = JsonConvert.DeserializeObject<BehaviourParams>(s);
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.RunWeb(rep);


        if (rep.Variations == null)
            RewindExercise();
        else //if (behaviour.GetType() != typeof(StayInPoseWithVariationBehaviour))
            CurrentExercise = behaviour.randomAnimations[0];




    }
    public void RunExerciseWebWithoutParams()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.RunWeb();
        RewindExercise();
    }
    
    public void StopExercise()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);

        if (behaviour == null)
            return;

        behaviour.Stop();

        behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
        behaviour.RepetitionEnd -= behaviour_PrepareEnd;
        behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
        behaviour.RepetitionReallyStart -= behaviour_RepetitionReallyStart;
    }


    /// <summary>
    /// Se ejecuta cuando termina una repetición
    /// </summary>
    private void AnimatorScript_OnRepetitionEnd(object sender, System.EventArgs e)
    {
        if ((CurrentExercise.Laterality == Laterality.Single) && (CurrentExercise.Limb == Limb.Interleaved))
        {
           // ChangeLimb();
        }
    }

    void RaiseEvent(EventHandler eh)
    {
        if (eh != null)
        {
            eh(this, new EventArgs());
        }
    }
    
    void RaiseEvent(EventHandler<PrepareEventArgs> eh, PrepareStatus status)
    {
        if (eh != null)
        {
            eh(this, new PrepareEventArgs(status, this.prepareCaller));
        }
    }
    void OnDestroy()
    {

        //StopExercise();
        if (instance != null)
            instance.StopAllCoroutines();
        if (AnimatorScript.instance != null)
            AnimatorScript.instance.OnRepetitionEnd -= AnimatorScript_OnRepetitionEnd;
        StopAllCoroutines();
        CurrentExercise.PropertyChanged -= currentExercise_PropertyChanged;

        behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
        behaviour.RepetitionEnd -= behaviour_PrepareEnd;
        behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
        behaviour.RepetitionReallyStart -= behaviour_RepetitionReallyStart;
        
        Destroy(instance);
        Destroy(this);
    }
}
