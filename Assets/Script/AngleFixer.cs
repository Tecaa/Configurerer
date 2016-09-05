using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AngleFixer {

    private static Dictionary<Movement, AngleMatch> matches;
    static AngleFixer()
    {
        matches = new Dictionary<Movement, AngleMatch>();
        FillDictionaries();
    }
    private static void FillDictionaries()
    {
        AddFix(Movement.AbducciónDeCaderaEnDecúbitoLateral, 0, 45, 0, 40);
        AddFix(Movement.AducciónResistidaEnPlanoEscapular, 80, 30, 90, 15);
        AddFix(Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, 0, 180, 0, 180);
        AddFix(Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, 0, 180, 0, 180);
        AddFix(Movement.ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral, 0, 180, 0, 171);
        AddFix(Movement.EstocadaFrontalCorta, 0, 90, 17, 102);
        AddFix(Movement.EstocadaFrontalLarga, 0, 90, 17, 107);
        AddFix(Movement.EstocadaLateral, 0, 90, 20, 91);
        AddFix(Movement.ExtensiónDeCaderaEnProno, 0, 30, 10, 30);
        AddFix(Movement.ExtensiónDeRodillaEnSedente_Unilateral, 90, 0, 95, 6);
        AddFix(Movement.ExtensiónDeRodillasEnSedente_Bilateral, 90, 0, 95, 6);
        AddFix(Movement.ExtensiónHorizontalDeHombrosEnSupino, 0, 60, 81, 37);
        AddFix(Movement.FlexiónDeCaderaEnSupino, 0, 60, 7, 64);
        AddFix(Movement.FlexiónDeCodoEnBípedo_Unilateral_90, 0, 140, 11, 133);
        AddFix(Movement.FlexiónDeCodosEnBípedo_Bilateral_90, 0, 140, 11, 133);
        AddFix(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_45, 0, 90, 0, -91);
        AddFix(Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90, 0, 90, 0, -91);
        AddFix(Movement.FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_45, 0, 90, 0, -91);
        AddFix(Movement.FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_90, 0, 90, 0, -91);
        AddFix(Movement.FlexiónDeHombrosEnBípedoConBastón_Bilateral, 0, 160, 0, -183);
        AddFix(Movement.FlexiónDeRodillaEnProno_Unilateral, 0, 120, 22, 126);
        AddFix(Movement.FlexiónDeRodillaEnSupino_Unilateral, 0, 120, 17, 110);
        AddFix(Movement.FlexiónDeRodillasEnProno_Bilateral, 0, 120, 22, 126);
        AddFix(Movement.FlexiónDeRodillasEnSupino_Bilateral, 0, 120, 17, 110);
        AddFix(Movement.RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, 0, 70, 80, -5);
        AddFix(Movement.RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral, 0, 70, 79, 32);
        AddFix(Movement.RotaciónExternaDeHombrosEnSupino_Bilateral, 0, 70, 84, -2);
        AddFix(Movement.RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, 0, 60, 83, 6);
        AddFix(Movement.RotaciónInternaDeHombrosEnSupino_Bilateral, 0, 60, 84, 28);
        AddFix(Movement.Sentadilla, 0, 90, 9, 61);
        AddFix(Movement.SentadillaConBalónSuizo, 0, 90, 38, 83.9f);
        AddFix(Movement.SentadillaMonopodal, 0, 90, 9, 60);
        AddFix(Movement.ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba, 0, 60, 78, 7.4f);
        AddFix(Movement.ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba, 0, 60, 78, 7.4f);
        AddFix(Movement.FlexiónHorizontalResistidaDeHombros_BípedoBilateral, 0, 90, 1, 81);
        AddFix(Movement.RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino, 0, 70, 81, -1);

        
    }
    public static float FixAngle(float angleToFix, Movement movement)
    {
        if (matches.ContainsKey(movement))
        {
            AngleMatch m = matches[movement];
            return m.RealInitialAngle + (m.RealFinalAngle - m.RealInitialAngle) * (angleToFix - m.DreamedInitialAngle) / (m.DreamedFinalAngle - m.DreamedInitialAngle);
        }
        else
        {
            return angleToFix;
        }
    }
    public static void AddFix(Movement movement, float dreamedInitialAngle, float dreamedFinalAngle, float realInitialAngle, float realFinalAngle)
    {
        matches.Add(movement, new AngleMatch(dreamedInitialAngle, dreamedFinalAngle, realInitialAngle, realFinalAngle));
    }
}

public class AngleMatch
{
    public float RealInitialAngle;
    public float RealFinalAngle;
    public float DreamedInitialAngle;
    public float DreamedFinalAngle;

    public AngleMatch(float dreamedInitialAngle, float dreamedFinalAngle, float realInitialAngle, float realFinalAngle)
    {
        RealFinalAngle = realFinalAngle;
        RealInitialAngle = realInitialAngle;
        DreamedFinalAngle = dreamedFinalAngle;
        DreamedInitialAngle = dreamedInitialAngle;
    }

}