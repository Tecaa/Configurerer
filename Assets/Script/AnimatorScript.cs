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

    public event EventHandler<PrepareEventArgs> OnPrepareExerciseStart;
    public event EventHandler<PrepareEventArgs> OnPrepareExerciseEnd;


    Caller prepareCaller;
    [HideInInspector]
    public BehaviourParams lerpParams;

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
        
        //anim = GetComponent<Animator>();
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
        //PrepareExerciseWeb("{\"Movement\":100000,\"Laterality\":0,\"Limb\":0}");
        //PrepareExerciseWeb("{\"Exercise\":{\"Movement\":280000,\"Laterality\":0,\"Limb\":0}, \"Caller\": 1}");
        PrepareExercise(new Exercise(Movement.EstocadaFrontalCorta, Laterality.Single, Limb.Interleaved), new BehaviourParams(60f, 1.0f, 1.0f, 0, 0));
        //PrepareExercise(new Exercise(Movement.DesplazamientoLateralConPaso_25, Laterality.Double, Limb.None), new BehaviourParams(40, 1.0f, 1.0f, 0, 0));
        //Invoke("pruebaRun2", 20);
        //Invoke("pruebaRun", 10);
        //RunExerciseWebWithoutParams("{\"Movement\":280000,\"Laterality\":0,\"Limb\":0}");
        //PrepareExercise(new Exercise(Movement.DesplazamientoLateralConPaso100, Laterality.Single, Limb.Left), new BehaviourParams(2, 0.5f, 0.5f));
        //PrepareExercise(new Exercise(Movement.Stride, Execution.Single, Limb.Interleaved), new BehaviourParams(0.5f, 1f, 120, 0, 3));
        //PrepareExercise(new Exercise(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Laterality.Single, Limb.Left), new BehaviourParams(-120,0.8f, 1, 2));
    }
    public void testRun()
    {
        //string s = "{\"Angle\":45,\"ForwardSpeed\":1.5,\"BackwardSpeed\":0.8,\"SecondsInPose\":0,\"SecondsBetweenRepetitions\":2}";
        //RunExerciseWeb(s);
        RunExercise();
        //RunExerciseWebWithoutParams();
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

        behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        DebugLifeware.Log(e.Movement + " " + e.Limb, DebugLifeware.Developer.Marco_Rojas);

        behaviour.Prepare(param);
        behaviour.RepetitionEnd += behaviour_PrepareEnd;
        CurrentExercise = e;
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
        DebugLifeware.Log("Comienza el run", DebugLifeware.Developer.Marco_Rojas);
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.Run();
        RewindExercise();
        behaviour.RepetitionEnd += behaviour_RepetitionEnd;
    }
    public void ResumeExercise()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.ResumeAnimation();
    }
    void behaviour_RepetitionEnd(object sender, EventArgs e)
    {
//        Debug.Log("Rep Realizada por instructor");
        //Terminó una repetición
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
        string s = "{\"Angle\":45,\"ForwardSpeed\":1.0,\"BackwardSpeed\":1.0,\"SecondsInPose\":0,\"SecondsBetweenRepetitions\":3}";
        RunExerciseWeb(s);
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
        RewindExercise();
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

        
        Destroy(instance);
        Destroy(this);
    }
}
