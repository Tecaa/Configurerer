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
    Iddle = 0,
    AbducciónDeCaderaEnDecúbitoLateral = 10000,
    AbducciónDeCaderaEnDecúbitoLateral_20 = 10001,
    AbducciónDeCaderaEnDecúbitoLateral_45 = 10002,
    AducciónResistidaEnPlanoEscapular = 20000,
    AducciónResistidaEnPlanoEscapular_30 = 20001,
    AducciónResistidaEnPlanoEscapular_45 = 20002,
    DesplazamientoLateralConPaso_100 = 30000,
    DesplazamientoLateralConPaso_75 = 30001,
    DesplazamientoLateralConPaso_50 = 30002,
    DesplazamientoLateralConPaso_25 = 30003,
    DesplazamientoLateralConSalto_100 = 40000,
    DesplazamientoLateralConSalto_75 = 40001,
    DesplazamientoLateralConSalto_50 = 40002,
    DesplazamientoLateralConSalto_25 = 40003,
    ElevaciónDeHombroEnPlanoEscapularConBastón = 50000,
    ElevaciónDeHombroEnPlanoEscapularConBastón_45 = 50001,
    ElevaciónDeHombroEnPlanoEscapularConBastón_60 = 50002,
    ElevaciónDeHombroEnPlanoEscapularConBastón_90 = 50003,
    ElevaciónDeHombroEnPlanoEscapularConBastón_120 = 50004,
    ElevaciónEnPuntaDePies_Nada = 60000,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral = 70000,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral_45 = 70001,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral_60 = 70002,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral_90 = 70003,
    ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral_120 = 70004,
    ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral = 80000,
    ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral_45 = 80001,
    ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral_60 = 80002,
    ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral_90 = 80003,
    ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral_120 = 80004,
    ElongaciónBandaIliotibial = 90000,
    ElongaciónCuádriceps = 100000,
    ElongaciónIsquiotibiales_TrícepsSural = 110000,
    EquilibrioBípedo = 120000,
    EquilibrioMonopodal = 130000,
    EquilibrioSedenteEnBalónSuizo = 140000,
    EstocadaFrontalCorta = 150000,
    EstocadaFrontalCorta_45 = 150001,
    EstocadaFrontalCorta_60 = 150002,
    EstocadaFrontalCorta_90 = 150003,
    EstocadaFrontalCortaConTorsiónDeTronco_90 = 160000,
    EstocadaFrontalCortaConTorsiónDeTronco_60 = 160001,
    EstocadaFrontalCortaConTorsiónDeTronco_45 = 160002,
    EstocadaFrontalLarga = 170000,
    EstocadaFrontalLarga_45 = 170001,
    EstocadaFrontalLarga_60 = 170002,
    EstocadaFrontalLarga_90 = 170003,
    EstocadaFrontalLargaConTorsiónDeTronco_90 = 180000,
    EstocadaFrontalLargaConTorsiónDeTronco_60 = 180001,
    EstocadaFrontalLargaConTorsiónDeTronco_45 = 180002,
    EstocadaLateral = 190000,
    EstocadaLateral_45 = 190001,
    EstocadaLateral_60 = 190002,
    EstocadaLateral_90 = 190003,
    EstocadaLateral_120 = 190004,
    ExtensiónDeCaderaEnProno = 200000,
    ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_90 = 210000,
    ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_60 = 210001,
    ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_45 = 210002,
    ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo45ºF_45_BIPEDO = 220000,
    ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_45 = 230000,
    ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_90 = 230001,
    ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_60 = 230002,
    ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_90 = 240000,
    ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_60 = 240001,
    ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_45 = 240002,
    ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_45 = 250000,
    ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_60 = 250002,
    ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_90 = 250001,
    ExtensiónDeRodillaConRodillo_Unilateral = 260000,
    ExtensiónDeRodillaEnSedente_Unilateral = 270000,
    ExtensiónDeRodillaEnSedente_Unilateral_30 = 270001,
    ExtensiónDeRodillaEnSedente_Unilateral_45 = 270002,
    ExtensiónDeRodillaEnSedente_Unilateral_60 = 270003,
    ExtensiónDeRodillaEnSedente_Unilateral_90 = 270004,
    ExtensiónDeRodillasConRodillo_Bilateral = 280000,
    ExtensiónDeRodillasEnSedente_Bilateral = 290000,
    ExtensiónDeRodillasEnSedente_Bilateral_30 = 290001,
    ExtensiónDeRodillasEnSedente_Bilateral_45 = 290002,
    ExtensiónDeRodillasEnSedente_Bilateral_60 = 290003,
    ExtensiónDeRodillasEnSedente_Bilateral_90 = 290004,
    ExtensiónHorizontalDeHombrosEnSupino = 300000,
    ExtensiónHorizontalDeHombrosEnSupino_30 = 300001,
    ExtensiónHorizontalDeHombrosEnSupino_45 = 300002,
    ExtensiónHorizontalDeHombrosEnSupino_60 = 300003,
    FlexiónDeCaderaEnSupino = 310000,
    FlexiónDeCaderaEnSupino_20 = 310001,
    FlexiónDeCaderaEnSupino_45 = 310002,
    FlexiónDeCaderaEnSupino_60 = 310003,
    FlexiónDeCodoEnBípedo_Unilateral_90 = 320000,
    FlexiónDeCodoEnBípedo_Unilateral_45 = 320001,
    FlexiónDeCodoEnBípedo_Unilateral_130 = 320002,
    FlexiónDeCodoEnBípedo_Unilateral_0 = 320001, // Ya no se usa
    FlexiónDeCodosEnBípedo_Bilateral_90 = 330000,
    FlexiónDeCodosEnBípedo_Bilateral_45 = 330001,
    FlexiónDeCodosEnBípedo_Bilateral_130 = 330002,
    FlexiónDeCodosEnBípedo_Bilateral_0 = 330001, // Ya no se usa
    FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_CIMA = 340000,
    FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_FRENTE = 340001,
    FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_NUCA = 340002,
    FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90 = 350000,
    FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45 = 350001,
    FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_90 = 360000,
    FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_45 = 360001,
    FlexiónDeHombrosEnBípedoConBastón_Bilateral = 370000,
    FlexiónDeHombrosEnBípedoConBastón_Bilateral_45 = 370001,
    FlexiónDeHombrosEnBípedoConBastón_Bilateral_60 = 370002,
    FlexiónDeHombrosEnBípedoConBastón_Bilateral_90 = 370003,
    FlexiónDeHombrosEnBípedoConBastón_Bilateral_120 = 370004,
    FlexiónDeRodillaEnProno_Unilateral = 380000,
    FlexiónDeRodillaEnProno_Unilateral_45 = 380001,
    FlexiónDeRodillaEnProno_Unilateral_60 = 380002,
    FlexiónDeRodillaEnProno_Unilateral_90 = 380003,
    FlexiónDeRodillaEnProno_Unilateral_120 = 380004,
    FlexiónDeRodillaEnSupino_Unilateral = 390000,
    FlexiónDeRodillaEnSupino_Unilateral_60 = 390001,
    FlexiónDeRodillaEnSupino_Unilateral_90 = 390002,
    FlexiónDeRodillaEnSupino_Unilateral_120 = 390003,
    FlexiónDeRodillasEnProno_Bilateral = 400000,
    FlexiónDeRodillasEnProno_Bilateral_45 = 400001,
    FlexiónDeRodillasEnProno_Bilateral_60 = 400002,
    FlexiónDeRodillasEnProno_Bilateral_90 = 400003,
    FlexiónDeRodillasEnProno_Bilateral_120 = 400004,
    FlexiónDeRodillasEnSupino_Bilateral = 410000,
    FlexiónDeRodillasEnSupino_Bilateral_60 = 410001,
    FlexiónDeRodillasEnSupino_Bilateral_90 = 410002,
    FlexiónDeRodillasEnSupino_Bilateral_120 = 410003,
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
    RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral_45 = 510001,
    RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral_70 = 510002,
    RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral = 520000,
    RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral_45 = 520001,
    RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral_70 = 520002,
    RotaciónExternaDeHombrosEnSupino_Bilateral = 530000,
    RotaciónExternaDeHombrosEnSupino_Bilateral_45 = 530001,
    RotaciónExternaDeHombrosEnSupino_Bilateral_70 = 530002,
    RotaciónInternaDeHombroEnBípedo = 540000,
    RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral = 550000,
    RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral = 560000,
    RotaciónInternaDeHombrosEnSupino_Bilateral = 570000,
    SaltoBipodal_EXT_ROD = 580000,
    SaltoMonopodal = 590000,
    Sentadilla = 600000,
    Sentadilla_45 = 600001,
    Sentadilla_60 = 600002,
    Sentadilla_90 = 600003,
    SentadillaAsistidaConCamilla = 610000,
    SentadillaConBalónSuizo = 620000,
    SentadillaConBalónSuizo_45 = 620001,
    SentadillaConBalónSuizo_60 = 620002,
    SentadillaConBalónSuizo_90 = 620003,
    SentadillaMonopodal = 630000,
    SentadillaMonopodal_30 = 630001,
    SentadillaMonopodal_45 = 630002,
    SentadillaMonopodal_60 = 630003,
    SubirEscalon_Frontal_SubeDerechaBajaDerecha = 640000,
    SubirEscalon_Frontal_SubeDerechaBajaIzquierda = 640001,
    SubirEscalon_Frontal_SubeIzquierdaBajaIzquierda = 640002,
    SubirEscalon_Frontal_SubeIzquierdaBajaDerecha = 640003,
    PlantiflexiónDeTobilloSedenteEnCamilla = 650000,
    ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba = 660000,
    ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba_30 = 660001,
    ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba_45 = 660002,
    ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba_60 = 660003,
    ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba = 670000,
    ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba_30 = 670001,
    ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba_45 = 670002,
    ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba_60 = 670003,
    FlexiónHorizontalResistidaDeHombros_BípedoBilateral = 680000,
    FlexiónHorizontalResistidaDeHombros_BípedoBilateral_45 = 680001,
    FlexiónHorizontalResistidaDeHombros_BípedoBilateral_60 = 680002,
    FlexiónHorizontalResistidaDeHombros_BípedoBilateral_90 = 680003,
    RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino = 690000,
    RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino_30 = 690001,
    RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino_70 = 690002,
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
    Oruga = 850000,
    SupermanContralateral = 860000,
    SupermanHomolateral = 870000,
    SupermanSinApoyo = 880000,
    AbdominalesHipopresivosEnBípedo = 890000,
    GatoEngrifado = 900000,
    Mahometano = 910000,
    HiperextensiónDeHombrosConCodosExtendidosEnSedente = 920000,
    ExtensiónHorizontalResistidaDeHombrosEnSedente_30 = 930000,
    ExtensiónHorizontalResistidaDeHombrosEnSedente_45 = 930001,
    ExtensiónHorizontalResistidaDeHombrosEnSedente_60 = 930002,
    FlexiónDeHombrosConCodosExtendidosEnSedente_60 = 940000,
    FlexiónDeHombrosConCodosExtendidosEnSedente_90 = 940001,
    FlexiónDeHombrosConCodosExtendidosEnSedente_120 = 940002,
}

public enum     Limb
{
    Left = 0,
    Right = 1,
    Interleaved = 2,
    None = 3
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
    Limb limb;
    public MovementLimbKey(Movement m, Limb l)
    {
        movement = m;
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

public enum GameStationType
{
    Módulo1,
    Módulo2,
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
