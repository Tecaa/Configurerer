using UnityEngine;
using System.Collections;
using System.Timers;
using System;
using System.ComponentModel;
using Assets;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;

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
    private const float DELAY_TO_FAST_PREPARE = 0.3F;
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
    }

    public void SetIdle(int pose)
    {
        anim.SetInteger("Idle", pose);
    }

    public void RewindExercise()
    {
        anim.SetInteger(AnimatorParams.Limb, (int)_currentExercise.Limb);
        anim.SetInteger(AnimatorParams.Movement, (int)_currentExercise.Movement);
    }
    public void testPrepare()
    {
        float forwardSpeed = (float)Convert.ToDouble(GameObject.Find("VEL IDA/Text").GetComponent<Text>().text);
        float backwardSpeed = (float)Convert.ToDouble(GameObject.Find("VEL VUELTA/Text").GetComponent<Text>().text);
        int secondsBE = Convert.ToInt32(GameObject.Find("PAUSA EXE/Text").GetComponent<Text>().text);
        int secondsBR = Convert.ToInt32(GameObject.Find("PAUSA REPS/Text").GetComponent<Text>().text);
        float rom = (float)Convert.ToDouble(GameObject.Find("ROM/Text").GetComponent<Text>().text);


        //PrepareExercise(new Exercise(Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, Limb.Left), 
        //    new BehaviourParams(rom, forwardSpeed, backwardSpeed, secondsBR, secondsBE));
        //    new BehaviourParams(new List<Movement>(){
        //        Movement.EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAlFrente,
        //        Movement.EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAlLado,
        //        Movement.EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAtrás,
        //    }, 1, 1, 2));
        /*
        PrepareExercise(new Exercise(Movement.SubirEscalon_Frontal_SubeDerechaBajaDerecha, Laterality.Double, Limb.None),
            new BehaviourParams(new List<Movement>(){
                Movement.SubirEscalon_Frontal_SubeDerechaBajaDerecha,
                Movement.SubirEscalon_Frontal_SubeDerechaBajaIzquierda,
                Movement.SubirEscalon_Frontal_SubeIzquierdaBajaDerecha,
                Movement.SubirEscalon_Frontal_SubeIzquierdaBajaIzquierda,
            }, 3, 1, 1));*/
        /*
    PrepareExercise(new Exercise(Movement.MantenerPosiciónExtrema_EtapaAvanzada_BrazosDiagonal, Laterality.Single, Limb.Right),
   new BehaviourParams(new List<Movement>(){
            Movement.MantenerPosiciónExtrema_EtapaAvanzada_BrazosDiagonal,
            Movement.MantenerPosiciónExtrema_EtapaAvanzada_Encestar,
            Movement.MantenerPosiciónExtrema_EtapaAvanzada_MusloArribaBrazosAdelanteYAtrás,
            Movement.MantenerPosiciónExtrema_EtapaAvanzada_PosturaDelÁrbol,
   }, 3, 1, 5));
    */
        //PrepareExercise(new Exercise(Movement.EstocadaFrontalLarga, Limb.Left), new BehaviourParams(rom, forwardSpeed, backwardSpeed, secondsBR, secondsInP));
        //PrepareExercise(new Exercise(Movement.DesplazamientoLateralConSalto_100, Laterality.Double, Limb.None), new BehaviourParams(2, 1f, 1f));
        //PrepareExercise(new Exercise(Movement.EquilibrioSedenteEnBalónSuizoConPlatilloDeFreeman, Laterality.Single, Limb.Right), new BehaviourParams(3,2,1 ,1));
        //PrepareExercise(new Exercise(Movement.PénduloEnProno, Laterality.Single, Limb.Right), new BehaviourParams(5, 2));

        //Para correr Juego mp*****************************************
        //PrepareExercise(new Exercise(Movement.EquilibrioBipedoConMovimientoDeMMSS_AbajoDerecha, Laterality.Double, Limb.None), new BehaviourParams(3, 2));
        //***********************************************************
        //Para correr Juego mp con variacion*****************************************
        //PrepareExercise(new Exercise(Movement.EquilibrioBipedoConMovimientoDeMMSS_AbajoDerecha, Laterality.Double, Limb.None), new BehaviourParams(new List<Movement>() {Movement.EquilibrioBipedoConMovimientoDeMMSS_ArribaIzquierda }, 1, 3, 2));
        //***********************************************************
        //Para correr web mp*******************************************
        PrepareExerciseWebParams p = new PrepareExerciseWebParams(new Exercise(Movement.DesplazamientoLateralConPaso_75, Limb.None), Caller.Config);
        PrepareExerciseWeb(Newtonsoft.Json.JsonConvert.SerializeObject(p));
        //***********************************************************
        //Para correr web mp con variacion*******************************************
        //PrepareExerciseWebParams p = new PrepareExerciseWebParams(new Exercise(Movement.EquilibrioBipedoConMovimientoDeMMSS_ArribaIzquierda, Laterality.Double, Limb.None), Caller.Config);
        //PrepareExerciseWeb(Newtonsoft.Json.JsonConvert.SerializeObject(p));
        //***********************************************************
        //Para correr Juego IV*****************************************
        //PrepareExercise(new Exercise(Movement.ExtensiónHorizontalDeHombrosEnSupino, Laterality.Double, Limb.None), new BehaviourParams(80, 0.8f, 0.8f, 1, 6));
        //***********************************************************

        //Para correr Juego mpx*****************************************
        //PrepareExercise(new Exercise(Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoIzquierda, Laterality.Single, Limb.Left), new BehaviourParams(new List<Movement>() {
        //    { Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoDerecha},
        //}, 3, 2));
        //***********************************************************
        //Para correr web mpx *******************************************
        //PrepareExerciseWebParams p = new PrepareExerciseWebParams(new Exercise(Movement.RecogiendoYGuardandoConAmbasManos_BrazosArribaIzquierda, Laterality.Double, Limb.None), Caller.Config);
        //PrepareExerciseWeb(Newtonsoft.Json.JsonConvert.SerializeObject(p));
        //***********************************************************

        //jExercise ex = new Exercise(Movement.PenduloEnBipedoCon45DeFlexiónDeTronco, Laterality.Single, Limb.Left);
        //BehaviourParams bp = new BehaviourParams(5, 2);
        //PrepareExercise(ex, bp);
        /*
        PrepareExerciseWebParams webParam = new PrepareExerciseWebParams(new Exercise(Movement.FlexiónDeCaderaEnSupino, Limb.Left),
            Caller.Preview);
        PrepareExerciseWeb(JsonConvert.SerializeObject(webParam));*/



        /*PrepareExercise(new Exercise(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Limb.Left),
          new BehaviourParams(70f, 1f, 1f, 2, 2));*/
        /*
		PrepareExercise(new Exercise(Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoDerecha, Limb.Right), 
            new BehaviourParams(new List<Movement>() {
			{ Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoDerecha},
			{ Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoIzquierda},
			{ Movement.RecogiendoYGuardandoConUnaMano_BrazoArribaDerecha},
			{ Movement.RecogiendoYGuardandoConUnaMano_BrazoArribaIzquierda},
        }, 2, 0));
        */

        /*
		PrepareExercise(new Exercise(Movement.EquilibrioBipedoConMovimientoDeMMSS_AbajoDerecha, Laterality.Double, Limb.None), 
            new BehaviourParams(new List<Movement>() {
			{ Movement.EquilibrioBipedoConMovimientoDeMMSS_AbajoIzquierda },
            { Movement.EquilibrioBipedoConMovimientoDeMMSS_ArribaDerecha },
            { Movement.EquilibrioBipedoConMovimientoDeMMSS_ArribaIzquierda },
            { Movement.EquilibrioBipedoConMovimientoDeMMSS_AbajoDerecha},
        }, 2, 1.5f, 4));*/

        /**
        PrepareExerciseWebParams webParam = new PrepareExerciseWebParams(new Exercise(Movement.Pablo_A, Laterality.Single, Limb.Left), Caller.Preview);
        PrepareExerciseWeb(JsonConvert.SerializeObject(webParam));**/
        /*
        PrepareExerciseWeb("{\"Exercise\":{\"Movement\":" + (int)Movement.PruebaA + ",\"Laterality\":" + (int)Laterality.Single + ",\"Limb\":"
           + (int)Limb.Right + "}, \"Caller\": 1}");
        */
        //PrepareExerciseWeb("{\"Exercise\":{\"Movement\":" + (int)Movement.PruebaMantenerPose + ",\"Laterality\":" + (int)Laterality.Single + ",\"Limb\":"
        //  + (int)Limb.Right + "}, \"Caller\": 1}");
    }
    public void testRun()
    {
        float forwardSpeed = (float)Convert.ToDouble(GameObject.Find("VEL IDA/Text").GetComponent<Text>().text);
        float backwardSpeed = (float)Convert.ToDouble(GameObject.Find("VEL VUELTA/Text").GetComponent<Text>().text);
        int secondsBE = Convert.ToInt32(GameObject.Find("PAUSA EXE/Text").GetComponent<Text>().text);
        int secondsBR = Convert.ToInt32(GameObject.Find("PAUSA REPS/Text").GetComponent<Text>().text);
        float rom = (float)Convert.ToDouble(GameObject.Find("ROM/Text").GetComponent<Text>().text);
        
        BehaviourParams param = new BehaviourParams(rom, forwardSpeed, backwardSpeed, secondsBE, secondsBR);

        //RunExerciseWeb(Newtonsoft.Json.JsonConvert.SerializeObject(param));

        //Para correr en juego (True con instruccion - false sin instruccion)***
        //RunExercise(true);
        //**********************************************************************

        //Para correr web con parametros****************************************
        RunExerciseWeb(Newtonsoft.Json.JsonConvert.SerializeObject(new BehaviourParams(rom, forwardSpeed, backwardSpeed, secondsBE, secondsBR)));
        //**********************************************************************

        //Para correr web con parametros mpx************************************
        //BehaviourParams p = new BehaviourParams(new List<Movement>() {
        //    { Movement.MantenerPosiciónExtrema_EtapaAvanzada_BrazosDiagonal},
        //    { Movement.MantenerPosiciónExtrema_EtapaAvanzada_Encestar},
        //    { Movement.MantenerPosiciónExtrema_EtapaAvanzada_MusloArribaBrazosAdelanteYAtrás},
        //}, 1, 1f, 0);
        //string s = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        //RunExerciseWeb(s);
        //**********************************************************************

        //Para correr web con parametros mp con variacion***********************
        /*BehaviourParams p = new BehaviourParams(new List<Movement>() {
            {Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoDerecha},
        }, 1, 3, 2);
        string s = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        RunExerciseWeb(s);*/
        //**********************************************************************
        //Para correr web sin parametros ***************************************
        //RunExerciseWebWithoutParams();
        //**********************************************************************

        //Para correr en juego (True con instruccion - false sin instruccion)***
        //RunExercise(true);
        //**********************************************************************


        /*BehaviourParams p = new BehaviourParams(2, 2);
        RunExerciseWeb(Newtonsoft.Json.JsonConvert.SerializeObject(p));*/

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

            case AnimatorParams.Limb:
                anim.SetInteger(AnimatorParams.Limb, (int)exercise.Limb);
                break;
        }
    }

    public AnimationBehaviour behaviour;

   
    /// <summary>
    /// Nota: Si hay errores de Quaternion to Matrix comprobar que los parametros enviados son validos para el ejercicio indicado
    /// </summary>
    /// <param name="e"></param>
    /// <param name="param"></param>
    public void PrepareExercise(Exercise e, BehaviourParams param)
    {

        param.Angle = AngleFixer.FixAngle(param.Angle, e.Movement);
        param.BackwardSpeed *= SpeedFixer.FixSpeed(e.Movement);
        param.ForwardSpeed *= SpeedFixer.FixSpeed(e.Movement);

        if (param.Variations == null || param.Variations.Count == 0)
            behaviour = AnimationBehaviour.GetBehaviour(e.Movement, e.Limb);
        else
            behaviour = AnimationBehaviour.GetCentralBehaviour(e.Movement, e.Limb);

        if (behaviour == null)
        {
            Debug.LogError("No se encontró la máquina de estado. (Ejercicio = " + e.Movement + " "
                + (int)e.Movement + ") (Limb = " + e.Limb + ")" +
                "). Posiblemente se deba a una mala combinación de esos parámetros o el MonitoAnimatorController se bugeó");
            return;
        }

        behaviour.Prepare(param);
        behaviour.RepetitionEnd += behaviour_PrepareEnd;

        if (param.Variations == null || param.Variations.Count == 0)
            CurrentExercise = e;
        else
            CurrentExercise = new Exercise(behaviour.randomAnimations[0], e.Limb);
    }

    private IEnumerator InitialPoseDelayed()
    {

        //yield return new WaitForSeconds(DELAY);
        RewindExercise();
        yield return new WaitForSeconds(0);//0.1f);
        behaviour.InitialPose();
    }


    public void InitialPose()
    {
        StartCoroutine(InitialPoseDelayed());
    }

    public void ResetVariations()
    {
        if (this.behaviour != null && this.behaviour.CentralNode != null)
            this.behaviour.CentralNode.actualRandomAnimationIndex = 0;
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
    float timeSinceStartPrepareWeb;
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
        timeSinceStartPrepareWeb = Time.time;  
        behaviour.PrepareWeb();
        CurrentExercise = e;    
    }

    void behaviour_PrepareEndWeb(object sender, EventArgs e)
    {
        //behaviour = sender as LerpBehaviour;
        behaviour = sender as AnimationBehaviour;
        behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
        //Debug.Log(timeSinceStartPrepareWeb + "  < " + Time.time + " - " + DELAY_TO_FAST_PREPARE + " =" + (Time.time - DELAY_TO_FAST_PREPARE));
        if (timeSinceStartPrepareWeb > Time.time - DELAY_TO_FAST_PREPARE)
            StartCoroutine(RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared, DELAY_TO_FAST_PREPARE));
        else 
            RaiseEvent(OnPrepareExerciseEnd, PrepareStatus.Prepared);
    }

    public void RunExercise(bool isInInstruction)
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.Run(isInInstruction);
        RewindExercise();
        behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
        behaviour.RepetitionEnd += behaviour_RepetitionEnd;
        behaviour.RepetitionReallyStart -= behaviour_RepetitionReallyStart;
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
        BehaviourParams p = JsonConvert.DeserializeObject<BehaviourParams>(s);
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);

        behaviour.Stop();
        p.Angle = AngleFixer.FixAngle(p.Angle, CurrentExercise.Movement);
        p.BackwardSpeed *= SpeedFixer.FixSpeed(CurrentExercise.Movement);
        p.ForwardSpeed *= SpeedFixer.FixSpeed(CurrentExercise.Movement);
        StartCoroutine(RunWebInSeconds(0.4f, p));

    }
    private IEnumerator RunWebInSeconds(float time, BehaviourParams p)
    {
        yield return new WaitForSeconds(time);
        behaviour.RunWeb(p);

        if (p.Variations == null)
            RewindExercise();
        else
            CurrentExercise = new Exercise(behaviour.randomAnimations[0], CurrentExercise.Limb);
    }
    private IEnumerator RunWebInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        behaviour.RunWeb();
        RewindExercise();
    }

    public void RunExerciseWebWithoutParams()
    {
        behaviour = AnimationBehaviour.GetBehaviour(CurrentExercise.Movement, CurrentExercise.Limb);
        behaviour.Stop();
        StartCoroutine(RunWebInSeconds(0.4f));
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
        if (/*(CurrentExercise.Laterality == Laterality.Single) &&*/ (CurrentExercise.Limb == Limb.Interleaved))
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
    IEnumerator RaiseEvent(EventHandler<PrepareEventArgs> eh, PrepareStatus status, float delay)
    {
        yield return new WaitForSeconds(delay);
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
        if (behaviour != null)
        {
            behaviour.RepetitionEnd -= behaviour_PrepareEndWeb;
            behaviour.RepetitionEnd -= behaviour_PrepareEnd;
            behaviour.RepetitionEnd -= behaviour_RepetitionEnd;
            behaviour.RepetitionReallyStart -= behaviour_RepetitionReallyStart;
        }
        Destroy(instance);
        Destroy(this);
    }
}
