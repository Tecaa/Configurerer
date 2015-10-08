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
    public Animator anim;
    public static HumanoidUtils utils;
    private Exercise _currentExercise = new Exercise();

    public static AnimatorScript instance;

    private IEnumerator prepareCR;

    public static event EventHandler OnRepetitionStart;
    public static event EventHandler OnRepetitionEnd;

    public static event EventHandler<PrepareEventArgs> OnPrepareExerciseStart;
    public static event EventHandler<PrepareEventArgs> OnPrepareExerciseEnd;


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

        anim = GetComponent<Animator>();
        utils = GetComponent<HumanoidUtils>();
        CurrentExercise.PropertyChanged += currentExercise_PropertyChanged;

        /////////   INICIO CODIGO DE TESTEO //////////
        //Invoke("pruebaRun", 2);
        Invoke("pruebaRun3", 3);

        Invoke("pruebaRun2", 18);
        
//        AnimatorScript.OnPrepareExerciseEnd += testing;
        /////////   FIN CODIGO DE TESTEO    //////////
    }
    public void RewindExercise()
    {
        anim.SetInteger(AnimatorParams.Limb, (int)_currentExercise.Limb);
        anim.SetInteger(AnimatorParams.Laterality, (int)((Laterality)_currentExercise.Laterality));
        anim.SetInteger(AnimatorParams.Movement, (int)_currentExercise.Movement);

        //   anim.Play(anim.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, 0);
    }
    private void testing(object sender, EventArgs e)
    {
        BehaviourParams p = new BehaviourParams(40, 1, 1, 0, 0);
        this.RunExerciseWeb(JsonConvert.SerializeObject(p));
    }
    /*
    void AnimatorScript_OnPrepareExerciseEnd(object sender, EventArgs e)
    {
        //RunExercise();
        Debug.Log("Preparado, comienza Run");
        //RunExercise();
        RunExerciseWebWithoutParams();
        //RunExerciseWeb("{\"Angle\": 40, \"ForwardSpeed\": 1, \"BackwardSpeed\": 1, \"PauseBetweenRepetitions\": 0, \"RepetitionMax\": 0}");
    }
    */
    public void pruebaRun()
    {
        //PrepareExerciseWeb("{\"Movement\":100000,\"Laterality\":0,\"Limb\":0}");
        //PrepareExerciseWeb("{\"Exercise\":{\"Movement\":280000,\"Laterality\":0,\"Limb\":0}, \"Caller\": 1}");
        PrepareExerciseWeb("{\"Exercise\":{\"Movement\":3,\"Laterality\":0,\"Limb\":0}, \"Caller\": 1}");
        //Invoke("pruebaRun2", 20);
        //Invoke("pruebaRun", 10);
        //RunExerciseWebWithoutParams("{\"Movement\":280000,\"Laterality\":0,\"Limb\":0}");
        //PrepareExercise(new Exercise(Movement.DesplazamientoLateralConPaso100, Laterality.Single, Limb.Left), new BehaviourParams(2, 0.5f, 0.5f));
        //PrepareExercise(new Exercise(Movement.Stride, Execution.Single, Limb.Interleaved), new BehaviourParams(0.5f, 1f, 120, 0, 3));
        //PrepareExercise(new Exercise(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Laterality.Single, Limb.Left), new BehaviourParams(-120,0.8f, 1, 2));
    }
    void pruebaRun2()
    {
        string s = "{\"Angle\":30,\"ForwardSpeed\":1,\"BackwardSpeed\":1,\"SecondsInPose\":0,\"SecondsBetweenRepetitions\":1}";
        RunExerciseWeb(s);
        //RunExerciseWebWithoutParams();
    }
    void pruebaRun3()
    {
        PrepareExerciseWeb("{\"Movement\":10000,\"Laterality\":0,\"Limb\":0}");
    }

    void Awake()
    {
        instance = this;

        //anim.SetInteger(AnimatorParams.Movement, (int)Movement.Iddle);
        //anim.speed = 1;
        //TODO: No olvidar borrar, elimina el caché de los ejercicios ya preparados
        PlayerPrefs.SetString("ExerciseCache", null);
    }
    // Update is called once per frame
    void Update()
    {

       // Debug.Log("animator state: " + anim.GetInteger(AnimatorParams.Movement) + " " + anim.GetInteger(AnimatorParams.Laterality) + " " + anim.GetInteger(AnimatorParams.Limb) + " " + anim.speed);    
        
    }

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
        Debug.Log("Comienza preparacion");
        behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        Debug.Log("qq" + e.Movement + e.Limb);

        behaviour.Prepare(param);
        behaviour.RepetitionEnd += behaviour_PrepareEnd;
        CurrentExercise = e;
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
        //Application.ExternalCall("Write", "Prepare 1  " + CurrentExercise.Movement + e.Movement + " " + this.GetInstanceID());
        behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        if(behaviour == null)
        {
            Application.ExternalCall("Write", "Prepare 2  " + CurrentExercise.Movement + e.Movement + " " + this.GetInstanceID());

            Debug.Log("Behaviour no encontrado");
            RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.NotFound);
            return;
        }
        behaviour.RepetitionEnd += behaviour_PrepareEndWeb;
        Application.ExternalCall("Write", "Prepare 3  " + CurrentExercise.Movement + e.Movement + " " + this.GetInstanceID());

        behaviour.PrepareWeb();
        Application.ExternalCall("Write", "Prepare 4  " + CurrentExercise.Movement + e.Movement + " " + this.GetInstanceID());

        CurrentExercise = e;
        Application.ExternalCall("Write", "Prepare 5 " + CurrentExercise.Movement + e.Movement + " " + this.GetInstanceID());

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
        Debug.Log("Comienza el run");
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.Run();
        RewindExercise();
        behaviour.RepetitionEnd += behaviour_RepetitionEnd;
    }

    void behaviour_RepetitionEnd(object sender, EventArgs e)
    {
        Debug.Log("Rep Realizada");
        //Terminó una repetición
    }

    void behaviour_PrepareEnd(object sender, EventArgs e)
    {
//        GoToIddle();
        Debug.Log("prep end");
        behaviour = sender as AnimationBehaviour;
        behaviour.RepetitionEnd -= behaviour_PrepareEnd;
        RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared);
    }

    public void testWeb()
    {
       /* Debug.Log("test run  web");
        RunExerciseWeb("{\"Angle\": "+ GameObject.Find("angle").GetComponent<UnityEngine.UI.Text>().text + ", \"ForwardSpeed\": "
            + GameObject.Find("forward").GetComponent<UnityEngine.UI.Text>().text + ", \"BackwardSpeed\": "
            + GameObject.Find("backward").GetComponent<UnityEngine.UI.Text>().text + ", \"PauseBetweenRepetitions\": 0, \"RepetitionMax\": 0}" + " " + this.GetInstanceID());
        //RunExercise();
        */
        string s = "{\"Angle\":45,\"ForwardSpeed\":1.0,\"BackwardSpeed\":1.0,\"SecondsInPose\":5,\"SecondsBetweenRepetitions\":21}";
        RunExerciseWeb(s);
    }
    public void testWeb2()
    {
        StopExercise();
    }
    public void RunExerciseWeb(string s/*Exercise exercise, float maxAngle, float forwardSpeed, float backwardSpeed*/)
    {
        /*exerciseRunner.StopExercise();
        anim.Play(StatesNames.Iddle, 0, 0);
        RunExerciseParams rep = JsonConvert.DeserializeObject<RunExerciseParams>(s);
        if (!exerciseRunner.IsPreparedWeb(rep.exercise))
        {
            return;
        }
        exerciseRunner.RunExerciseWeb(rep.exercise, rep.maxAngle, rep.forwardSpeed, rep.backwardSpeed);*/
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

        Application.ExternalCall("Write", "stop 1 " + CurrentExercise.Movement + " " + this.GetInstanceID());
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);

        Application.ExternalCall("Write", "stop 2 " + CurrentExercise.Movement + " " + this.GetInstanceID());
        if (behaviour == null)
            return;

        Application.ExternalCall("Write", "stop 3 " + CurrentExercise.Movement + " " + this.GetInstanceID());
        behaviour.Stop();

        Application.ExternalCall("Write", "stop 4 " + CurrentExercise.Movement + " " + this.GetInstanceID());
        behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
        behaviour.RepetitionEnd -= behaviour_PrepareEnd;
        behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
        /*
        StopAllCoroutines();
        exerciseRunner.StopExercise();
        GoToIddle();*/

        Application.ExternalCall("Write", "stop 5 " + CurrentExercise.Movement + " " + this.GetInstanceID());
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
        StopAllCoroutines();
        instance.StopAllCoroutines();
        CurrentExercise.PropertyChanged -= currentExercise_PropertyChanged;
        AnimatorScript.OnRepetitionEnd -= AnimatorScript_OnRepetitionEnd;

        Destroy(instance);
        instance = null;
        Destroy(this);
    }
}
