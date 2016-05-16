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
        movementJointMatch.Add(new MovementLimbKey(Movement.Stride, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.Stride, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.Stride, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        //TODO: Añadir caso Execution.Double
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.BipedElbowFlexionAndExtension, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));


        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));


        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoHorizontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontal));


        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral, Limb.Interleaved), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoSagital));

        //TODO: Comprobar si este movimiento requiere ser agregado al diccionario
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso025, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso050, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso075, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso100, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
         * */


        
        movementJointMatch.Add(new MovementLimbKey(Movement.AbducciónDeCaderaEnDecúbitoLateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.MusloIzquierda, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.AbducciónDeCaderaEnDecúbitoLateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.MusloDerecha, Plano.planos.planoHorizontalAcostado));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.AducciónResistidaEnPlanoEscapular, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.AducciónResistidaEnPlanoEscapular, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoFrontal));
        
        
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_100, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_75, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_50, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConPaso_25, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_100, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_75, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_50, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.DesplazamientoLateralConSalto_25, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));


        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoFrontal));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónEnPuntaDePies_Nada, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónEnPuntaDePiesEnStep_Step, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        //movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónEnPuntaDePies, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoFrontal));

        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoFrontal));
        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoFrontal));

        movementJointMatch.Add(new MovementLimbKey(Movement.ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoFrontal));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCorta, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_45, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_45, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_60, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_60, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_90, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalCortaConTorsiónDeTronco_90, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLarga, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLarga, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_45, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_45, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_60, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_60, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_90, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaFrontalLargaConTorsiónDeTronco_90, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaLateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.EstocadaLateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeCaderaEnProno, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeCaderaEnProno, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoHorizontalAcostado));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_45, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_45, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_60, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_60, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_90, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConEstabilizaciónEscapular_Unilateral_90, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo45ºF_45_BIPEDO, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo45ºF_45_BIPEDO, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo90ºF_90_BIPEDO, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalBípedo90ºF_90_BIPEDO, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalProno_90_PRONO, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodo_RemoHorizontalProno_90_PRONO, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_45, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_45, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_90, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombroConFlexiónDeCodoEnBípedo_Unilateral_90, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_45, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_60, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConEstabilizaciónEscapular_Bilateral_90, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_45, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_90, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaConRodillo_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaConRodillo_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaEnSedente_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillaEnSedente_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillasConRodillo_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDeRodillasEnSedente_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónHorizontalDeHombrosEnSupino, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoHorizontalAcostado));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCaderaEnSupino, Limb.Left), new JointTypePlanoResult(ArticulacionType.MusloIzquierda, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCaderaEnSupino, Limb.Right), new JointTypePlanoResult(ArticulacionType.MusloDerecha, Plano.planos.planoHorizontalAcostado));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_0, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_0, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_90, Limb.Left), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodoEnBípedo_Unilateral_90, Limb.Right), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodosEnBípedo_Bilateral_0, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeCodosEnBípedo_Bilateral_90, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_CIMA, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_FRENTE, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoDerecho, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_NUCA, Limb.None), new JointTypePlanoResult(ArticulacionType.CodoIzquierdo, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_45, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_90, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));


        
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeHombrosEnBípedoConBastón_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnProno_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnProno_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnSupino_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillaEnSupino_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.RodillaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillasEnProno_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónDeRodillasEnSupino_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.RodillaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_120, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_120, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_45, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_45, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_90, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PrensaDePiernas_90, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoCon45ºFlexiónDeTronco_45_BIPEDO, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoCon45ºFlexiónDeTronco_45_BIPEDO, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoCon90ºFlexiónDeTronco_90_BIPEDO, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnBípedoCon90ºFlexiónDeTronco_90_BIPEDO, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnProno_90_PRONO, Limb.Left), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RetracciónEscapularEnProno_90_PRONO, Limb.Right), new JointTypePlanoResult(ArticulacionType.BrazoDerecho, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnBípedo, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));


        
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoHorizontalAcostado));
        

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoHorizontalAcostado));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónExternaDeHombrosEnSupino_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontalAcostado));
        //

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnBípedo, Limb.Left), new JointTypePlanoResult(ArticulacionType.HombroIzquierdo, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnBípedo, Limb.Right), new JointTypePlanoResult(ArticulacionType.HombroDerecho, Plano.planos.planoSagital));


        
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoHorizontalAcostado));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral, Limb.Left), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral, Limb.Right), new JointTypePlanoResult(ArticulacionType.AnteBrazoDerecho, Plano.planos.planoHorizontalAcostado));
        
        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónInternaDeHombrosEnSupino_Bilateral, Limb.None), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoHorizontalAcostado));
        //

        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoBipodal_EXT_ROD, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoBipodalLlevandoRodillasAlPecho_FLEX_ROD, Limb.None), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoMonopodal, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SaltoMonopodal, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.Sentadilla, Limb.None), new JointTypePlanoResult(ArticulacionType.MusloIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaAsistidaConCamilla,  Limb.None), new JointTypePlanoResult(ArticulacionType.MusloIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaConBalónSuizo, Limb.None), new JointTypePlanoResult(ArticulacionType.MusloIzquierda, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaMonopodal, Limb.Left), new JointTypePlanoResult(ArticulacionType.MusloIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.SentadillaMonopodal, Limb.Right), new JointTypePlanoResult(ArticulacionType.MusloDerecha, Plano.planos.planoSagital));

        movementJointMatch.Add(new MovementLimbKey(Movement.PlantiflexiónDeTobilloSedenteEnCamilla, Limb.Left), new JointTypePlanoResult(ArticulacionType.PiernaIzquierda, Plano.planos.planoSagital));
        movementJointMatch.Add(new MovementLimbKey(Movement.PlantiflexiónDeTobilloSedenteEnCamilla, Limb.Right), new JointTypePlanoResult(ArticulacionType.PiernaDerecha, Plano.planos.planoSagital));


        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoHorizontalAcostado));
        movementJointMatch.Add(new MovementLimbKey(Movement.ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba, Limb.None), new JointTypePlanoResult(ArticulacionType.BrazoIzquierdo, Plano.planos.planoHorizontalAcostado));

        movementJointMatch.Add(new MovementLimbKey(Movement.FlexiónHorizontalResistidaDeHombros_BípedoBilateral,  Limb.None), new JointTypePlanoResult(ArticulacionType.HombroIzquierdo, Plano.planos.planoFrontal));

        movementJointMatch.Add(new MovementLimbKey(Movement.RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino, Limb.None), new JointTypePlanoResult(ArticulacionType.AnteBrazoIzquierdo, Plano.planos.planoSagital)); // OJO CON EL ÁNGULO ESTÁ MAL
    }
}
