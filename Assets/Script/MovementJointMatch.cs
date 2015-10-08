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
        /*
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
         * */

        //TODO: Falta artculacion cadera
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.AbducciónDeCaderaEnDecúbitoLateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType., Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.AbducciónDeCaderaEnDecúbitoLateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoFrontal));
        
         //TODO: Falta artculacion hombre       
        movementJointMatch.Add(new MovementLimbKey(Movement.AducciónResistidaEnPlanoEscapular, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType., Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.AducciónResistidaEnPlanoEscapular, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoFrontal));
        */
        
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_100, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_75, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_50, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_25, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_100, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_75, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_50, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_25, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        //TODO: Falta artculacion hombro
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType., Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoFrontal));
        */

        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónEnPuntaDePies, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónEnPuntaDePies_Step, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónEnPuntaDePies, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        //TODO: Falta articulación hombro
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        */

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_45, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_45, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_60, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_60, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_90, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_90, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLarga, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLarga, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_45, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_45, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_60, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_60, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_90, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_90, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaLateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaLateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        //TODO: Falta joint cadera
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeCaderaEnProno, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeCaderaEnProno, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));
        */

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_45, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_45, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_60, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_60, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_90, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_90, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_45_BIPEDO, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_45_BIPEDO, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_90_BIPEDO, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_90_BIPEDO, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_90_PRONO, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontal_90_PRONO, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_45, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_45, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_90, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_90, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_45, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_60, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_90, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_45, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_90, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaConRodillo_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaConRodillo_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaEnSedente_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaEnSedente_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillasConRodillo_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillasEnSedente_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        //TODO: Falta joint cadera
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCaderaEnSupino, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCaderaEnSupino, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));
        */

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_0, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_0, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_90, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_90, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodosEnBípedo_Bilateral_0, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodosEnBípedo_Bilateral_90, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_CIMA, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_FRENTE, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_NUCA, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_45, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_90, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));

        //TODO: Falta joint hombro
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombrosEnBípedoConBastón_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        */

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnProno_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnProno_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnSupino_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnSupino_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillasEnProno_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillasEnSupino_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_120, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_120, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_45, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_45, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_90, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_90, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoConFlexiónDeTronco_45_BIPEDO, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoConFlexiónDeTronco_45_BIPEDO, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoConFlexiónDeTronco_90_BIPEDO, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoConFlexiónDeTronco_90_BIPEDO, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoConFlexiónDeTronco_90_PRONO, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoConFlexiónDeTronco_90_PRONO, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        //TODO: Falta joint de hombro
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));
        
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombrosEnSupino_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        */

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnBípedo, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnBípedo, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        //TODO: Falta joint hombro
        /*
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));
        
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombrosEnSupino_Bilateral, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        */

        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoBipodal_EXT_ROD, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoBipodal_FLEX_ROD, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoMonopodal, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoMonopodal, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.Sentadilla, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaAsistidaConCamilla, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaConBalónSuizo, Laterality.Double, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaMonopodal, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaMonopodal, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PlantiflexiónDeTobilloSedenteEnCamilla, Laterality.Single, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PlantiflexiónDeTobilloSedenteEnCamilla, Laterality.Single, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

    }
}
