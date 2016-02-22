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
{/*
    PruebaA = 640000,
    PruebaB = 640001,
    PruebaC = 640002,
    PruebaMantenerPose = 8000000,*/
    Milton_A = 2000000,
    Milton_B = 2000001,
    Milton_C = 2000002,
    Milton_D = 2000003,
    Pablo_A = 900000,
    Pablo_B = 900001,
    Pablo_C = 900002,
    Pablo_D = 900003,

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
    ElevaciónEnPuntaDePies_Nada = 60000,
    ElevaciónEnPuntaDePies_Step = 60001,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral = 70000,
    ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral = 80000,
    ElongaciónBandaIliotibial = 90000,
    ElongaciónCuádriceps = 100000,
    ElongaciónIsquiotibiales_TrícepsSural = 110000,
    EquilibrioBípedo = 120000,
    EquilibrioMonopodal = 130000,
    EquilibrioSedenteEnBalónSuizo = 140000,
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
    ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo45ºF_45_BIPEDO = 220000,
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
    MantenerPosiciónExtrema_EtapaAvanzada_Ninja = 420000,
    MantenerPosiciónExtrema_EtapaAvanzada_Encestar = 420001,
    MantenerPosiciónExtrema_EtapaAvanzada_MusloArribaBrazosAdelanteYAtrás = 420002,
    MantenerPosiciónExtrema_EtapaAvanzada_PosturaDelÁrbol = 420003,
    MantenerPosiciónExtrema_EtapaAvanzada_Reloj = 420004,
    MantenerPosiciónExtrema_EtapaAvanzada_BrazosDiagonal = 420005,
    MantenerPosiciónExtrema_EtapaInicial_CuerdaFloja = 430000,
    MantenerPosiciónExtrema_EtapaInicial_CuerdaFlojaBrazoDerecho = 430001,
    MantenerPosiciónExtrema_EtapaInicial_CuerdaFlojaBrazoIzquierdo = 430002,
    MantenerPosiciónExtrema_EtapaInicial_FlexiónDeRodillasBrazosHorizontal = 430003,
    MantenerPosiciónExtrema_EtapaInicial_FlexiónDeRodillasBrazoDelanteYAtrás = 430004,
    MantenerPosiciónExtrema_EtapaIntermedia_RodillaYBrazosArriba = 440000,
    MantenerPosiciónExtrema_EtapaIntermedia_DominarBalón = 440001,
    MantenerPosiciónExtrema_EtapaIntermedia_MusloYBrazosHorizontal = 440002,
    MantenerPosiciónExtrema_EtapaIntermedia_MusloHorizontalBrazosAlFrente = 440003,
    PénduloEnBípedoCon45ºDeFlexiónDeTronco = 450000,
    PénduloEnBípedoConFlexiónDe90ºDeTronco = 460001,
    PénduloEnProno = 470002,
    PrensaDePiernas_120 = 480000,
    PrensaDePiernas_90 = 480001,
    PrensaDePiernas_45 = 480002,
    RetracciónEscapularEnBípedoCon45ºFlexiónDeTronco_45_BIPEDO = 490000,
    RotaciónExternaDeHombroEnBípedo = 500000,
    RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral = 510000,
    RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral = 520000,
    RotaciónExternaDeHombrosEnSupino_Bilateral = 530000,
    RotaciónInternaDeHombroEnBípedo = 540000,
    RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral = 550000,
    RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral = 560000,
    RotaciónInternaDeHombrosEnSupino_Bilateral = 570000,
    SaltoBipodal_EXT_ROD = 580000,
    SaltoMonopodal = 590000,
    Sentadilla = 600000,
    SentadillaAsistidaConCamilla = 610000,
    SentadillaConBalónSuizo = 620000,
    SentadillaMonopodal = 630000,
    SubirEscalon_Frontal_SubeDerechaBajaDerecha = 640000,
    SubirEscalon_Frontal_SubeDerechaBajaIzquierda = 640001,
    SubirEscalon_Frontal_SubeIzquierdaBajaIzquierda = 640002,
    SubirEscalon_Frontal_SubeIzquierdaBajaDerecha = 640003,
    PlantiflexiónDeTobilloSedenteEnCamilla = 650000,
    ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba = 660000,
    ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba = 670000,
    FlexiónHorizontalResistidaDeHombros_BípedoBilateral = 680000,
    RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino = 690000,
    RecogiendoYGuardandoConAmbasManos_BrazosArribaIzquierda = 700000,
    RecogiendoYGuardandoConAmbasManos_BrazosArribaDerecha = 700001,
    RecogiendoYGuardandoConAmbasManos_BrazosAbajoIzquierda = 700002,
    RecogiendoYGuardandoConAmbasManos_BrazosAbajoDerecha = 700003,
    SubirEscalón_Lateral = 710000,
    FlexiónDeRodillaAutoAsistidaEnSupino_Unilateral = 720000,
    EquilibrioBipedoConMovimientoDeMMSS_ArribaIzquierda = 730000,
    EquilibrioBipedoConMovimientoDeMMSS_ArribaDerecha = 730001,
    EquilibrioBipedoConMovimientoDeMMSS_AbajoIzquierda = 730002,
    EquilibrioBipedoConMovimientoDeMMSS_AbajoDerecha = 730003,
    EquilibrioMonopodalConMovimientoDeMMSS_ArribaIzquierda = 740000,
    EquilibrioMonopodalConMovimientoDeMMSS_ArribaDerecha = 740001,
    EquilibrioMonopodalConMovimientoDeMMSS_AbajoIzquierda = 740002,
    EquilibrioMonopodalConMovimientoDeMMSS_AbajoDerecha = 740003,
    EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAlFrente = 750000,
    EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAlLado = 750001,
    EquilibrioMonopodalConMovimientoDeMiembroContralateral_PiernaAtrás = 750002,
    EquilibrioSedenteEnBalónSuizoConPiernaDeApoyoExtendida = 760000,
    ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalProno_90_PRONO = 770000,
    ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo90ºF_90_BIPEDO = 780000,
    RetracciónEscapularEnBípedoCon90ºFlexiónDeTronco_90_BIPEDO = 790000,
    RetracciónEscapularEnProno_90_PRONO = 800000,
    SaltoBipodalLlevandoRodillasAlPecho_FLEX_ROD = 810000,
    EquilibrioSedenteEnBalónSuizoConPlatilloDeFreeman = 820000,
    ElevaciónEnPuntaDePiesEnStep_Step = 830000,
    RecogiendoYGuardandoConUnaMano_BrazoArribaIzquierda = 840000,
    RecogiendoYGuardandoConUnaMano_BrazoArribaDerecha = 840001,
    RecogiendoYGuardandoConUnaMano_BrazoAbajoIzquierda = 840002,
    RecogiendoYGuardandoConUnaMano_BrazoAbajoDerecha = 840003,


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