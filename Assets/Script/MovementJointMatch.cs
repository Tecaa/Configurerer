using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class MovementJointMatch
{
    public static Dictionary<MovementLimbKey, JointTypePlanoResult> movementJointMatch;
    static MovementJointMatch()
    {
        //AnimatorScript.OnRepetitionEnd+=AnimatorScript_OnRepetitionEnd;
        movementJointMatch = new Dictionary<MovementLimbKey, JointTypePlanoResult>();
        movementJointMatch.Add(new MovementLimbKey(Movement.Stride, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.Stride, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.Stride, Laterality.Single, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        //TODO: Añadir caso Execution.Double
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Laterality.Single, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));


        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Laterality.Single, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));


        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Laterality.Single, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoHorizontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Laterality.Single, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontal));


        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral, Laterality.Single, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoSagital));

        //TODO: Comprobar si este movimiento requiere ser agregado al diccionario
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso025, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso050, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso075, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso100, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
    }
}
