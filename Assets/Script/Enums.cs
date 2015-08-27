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
}

public enum Laterality
{
    Single,
    Double
}

public enum Limb
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