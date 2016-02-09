using Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public enum PrepareStatus
{
    Preparing,
    Prepared,
    NotFound
}
public enum Caller
{
    Preview,
    Config
}
public enum Movement
{
    /*
    Iddle,
    Stride,
    BipedElbowFlexionAndExtension,
    PruebaEstado,
    Step,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral = 100000,
    EstocadaFrontalCorta = 280000,
    FlexiónDeCodoEnBípedo_Unilateral = 610000,
    RotaciónExternaDeHombroEnBípedo = 950000,
    DesplazamientoLateralConPaso025 = 50000,
    DesplazamientoLateralConPaso050 = 50001,
    DesplazamientoLateralConPaso075 = 50002,
    DesplazamientoLateralConPaso100 = 50003
     * */

	milton = 100,
    Iddle = 0,
    AbducciónDeCaderaEnDecúbitoLateral = 10000,
    AducciónResistidaEnPlanoEscapular = 20000,
    DesplazamientoLateralConPaso_100 = 30000,
    DesplazamientoLateralConPaso_75 = 30001,
    DesplazamientoLateralConPaso_50 = 30002,
    DesplazamientoLateralConPaso_25 = 30003,
    DesplazamientoLateralConSalto_100 = 40000,
    DesplazamientoLateralConSalto_75 = 40001,
    DesplazamientoLateralConSalto_50 = 40002,
    DesplazamientoLateralConSalto_25 = 40003,
    ElevaciónDeHombroEnPlanoEscapularConBastón = 50000,
    ElevaciónEnPuntaDePies = 60000,
    ElevaciónEnPuntaDePies_Step = 60001,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral = 70000,
    ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral = 80000,
    EstocadaFrontalCorta = 150000,
    EstocadaFrontalCortaConTorsiónDeTronco_90 = 160000,
    EstocadaFrontalCortaConTorsiónDeTronco_60 = 160001,
    EstocadaFrontalCortaConTorsiónDeTronco_45 = 160002,
    EstocadaFrontalLarga = 170000,
    EstocadaFrontalLargaConTorsiónDeTronco_90 = 180000,
    EstocadaFrontalLargaConTorsiónDeTronco_60 = 180001,
    EstocadaFrontalLargaConTorsiónDeTronco_45 = 180002,
    EstocadaLateral = 190000,
    ExtensiónDeCaderaEnProno = 200000,
    ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_90 = 210000,
    ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_60 = 210001,
    ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_45 = 210002,
    ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_45_BIPEDO = 220000,
    ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_90_PRONO = 770000,
    ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_90_BIPEDO = 780000,
    ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_45 = 230000,
    ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_90 = 230001,
    ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_90 = 240000,
    ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_60 = 240001,
    ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_45 = 240002,
    ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_45 = 250000,
    ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_90 = 250001,
    ExtensiónDeRodillaConRodillo_Unilateral = 260000,
    ExtensiónDeRodillaEnSedente_Unilateral = 270000,
    ExtensiónDeRodillasConRodillo_Bilateral = 280000,
    ExtensiónDeRodillasEnSedente_Bilateral = 290000,
    ExtensiónHorizontalDeHombrosEnSupino = 300000,
    FlexiónDeCaderaEnSupino = 310000,
    FlexiónDeCodoEnBípedo_Unilateral_90 = 320000,
    FlexiónDeCodoEnBípedo_Unilateral_0 = 320001,
    FlexiónDeCodosEnBípedo_Bilateral_90 = 330000,
    FlexiónDeCodosEnBípedo_Bilateral_0 = 330001,
    FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_CIMA = 340000,
    FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_FRENTE = 340001,
    FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_NUCA = 340002,
    FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90 = 350000,
    FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45 = 350001,
    FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_90 = 360000,
    FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_45 = 360001,
    FlexiónDeHombrosEnBípedoConBastón_Bilateral = 370000,
    FlexiónDeRodillaEnProno_Unilateral = 380000,
    FlexiónDeRodillaEnSupino_Unilateral = 390000,
    FlexiónDeRodillasEnProno_Bilateral = 400000,
    FlexiónDeRodillasEnSupino_Bilateral = 410000,
    PrensaDePiernas_120 = 480000,
    PrensaDePiernas_90 = 480001,
    PrensaDePiernas_45 = 480002,
    RetracciónEscapularEnBípedoConFlexiónDeTronco_45_BIPEDO = 490000,
    RetracciónEscapularEnBípedoConFlexiónDeTronco_90_BIPEDO = 790000,
    RetracciónEscapularEnBípedoConFlexiónDeTronco_90_PRONO = 800000,
    RotaciónExternaDeHombroEnBípedo = 500000,
    RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral = 510000,
    RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral = 520000,
    RotaciónExternaDeHombrosEnSupino_Bilateral = 530000,
    RotaciónInternaDeHombroEnBípedo = 540000,
    RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral = 550000,
    RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral = 560000,
    RotaciónInternaDeHombrosEnSupino_Bilateral = 570000,
    SaltoBipodal_EXT_ROD = 580000,
    SaltoBipodal_FLEX_ROD = 580001,
    SaltoMonopodal = 590000,
    Sentadilla = 600000,
    SentadillaAsistidaConCamilla = 610000,
    SentadillaConBalónSuizo = 620000,
    SentadillaMonopodal = 630000,
    PruebaA = 640000,
    PruebaB = 640001,
    PruebaC = 640002,
    PlantiflexiónDeTobilloSedenteEnCamilla = 650000,

    PenduloEnBipedoCon45DeFlexiónDeTronco = 450000,
    PenduloEnBipedoConFlexiónDe90DeTronco = 460000,
    PenduloEnProno = 470000,

    PruebaMantenerPose = 10

}

public enum Laterality
{
    Single,
    Double
}

public enum     Limb
{
    Left = 0,
    Right = 1,
    Interleaved = 2,
    None = 3
}

internal static class AnimatorParams
{
    public const string Movement = "Movement";
    public const string Laterality = "Laterality";
    public const string Limb = "Limb";
}

internal static class StatesNames
{
    public const string Iddle = "Quieto";
}

public enum LerpState
{
    Forward,
    Stopped,
    Backward
}

public struct AnimationInfo
{
    public float time;
    public float angle;
    public AnimationInfo(float t, float a)
    {
        time = t;
        angle = a;
    }
}

public struct MovementLimbKey
{
    Movement movement;
    Laterality execution;
    Limb limb;
    public MovementLimbKey(Movement m, Laterality e, Limb l)
    {
        movement = m;
        execution = e;
        limb = l;
    }
}

public struct JointTypePlanoResult
{
    public ArticulacionType jointType;
    public Plano.planos plain;
    public JointTypePlanoResult(ArticulacionType jt, Plano.planos p)
    {
        jointType = jt;
        plain = p;
    }
}

//TODO: Código Jorge, mejorar si es posible
[SerializableAttribute]
public class SkeletonPoint
{
    public UnityEngine.GameObject joint;
    public Vector3 posicionRelativa;
    public pointNames pointNames;

    public SkeletonPoint(UnityEngine.GameObject joint, pointNames pointNames)
    {
        // TODO: Complete member initialization
        this.joint = joint;
        this.pointNames = pointNames;
    }

    public float radius;
    public float theta;
    public float phi;

    internal void Spherical()
    {
        radius = posicionRelativa.magnitude;
        theta = Mathf.Acos(posicionRelativa.z / radius);
        phi = Mathf.Atan2(posicionRelativa.y, posicionRelativa.x);
    }
}

//TODO: Código Jorge, mejorar si es posible
public enum pointNames
{
    [Description("hips")]
    hips,
    [Description("chest")]
    chest,
    [Description("neck")]
    neck,
    [Description("head")]
    head,
    [Description("spine")]
    spine,
    [Description("leftShoulder")]
    leftShoulder,
    [Description("leftArm")]
    leftArm,
    [Description("leftForeArm")]
    leftForeArm,
    [Description("leftHand")]
    leftHand,
    [Description("leftUpperLeg")]
    leftUpperLeg,
    [Description("leftLeg")]
    leftLeg,
    [Description("leftFoot")]
    leftFoot,
    [Description("rightShoulder")]
    rightShoulder,
    [Description("rightArm")]
    rightArm,
    [Description("rightForeArm")]
    rigthForeArm,
    [Description("rightHand")]
    rightHand,
    [Description("rightUpperLeg")]
    rightUpperLeg,
    [Description("rightLeg")]
    rightLeg,
    [Description("rightFoot")]
    rightFoot,
    [Description("rightToe")]
    rightToe,
    [Description("leftToe")]
    LeftToe
};

public struct ExpertReg
{
    public float time;
    public MarkType markType;
    public List<Quaternion> quaterions;
}

public enum MarkType
{
    Begin,
    MaxAngle,
    End
}
/*
internal static class AnimationClips
{
    public static Dictionary<Exercise, float> AnimationDurations;

    static AnimationClips()
    {
        AnimationDurations = new Dictionary<Exercise, float>();

        AnimationDurations.Add( new Exercise(Movement.Stride, Laterality.Single, Limb.Left), 1.4f);
        AnimationDurations.Add( new Exercise(Movement.Stride, Laterality.Single, Limb.Right), 1.4f);
        AnimationDurations.Add( new Exercise(Movement.Stride, Laterality.Single, Limb.Interleaved), 1.4f);

        AnimationDurations.Add( new Exercise(Movement.BipedElbowFlexionAndExtension, Laterality.Single, Limb.Left), 1.1f);
        AnimationDurations.Add( new Exercise(Movement.BipedElbowFlexionAndExtension, Laterality.Single, Limb.Right), 1.1f);
        AnimationDurations.Add( new Exercise(Movement.BipedElbowFlexionAndExtension, Laterality.Single, Limb.Interleaved), 1.1f);
        AnimationDurations.Add( new Exercise(Movement.BipedElbowFlexionAndExtension, Laterality.Double, Limb.None), 2.1f);


    }
}
*/