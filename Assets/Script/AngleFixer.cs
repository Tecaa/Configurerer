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
        AddFix(Movement.FlexiónDeCaderaEnSupino, 0, 60, -4, -61);
        AddFix(Movement.ExtensiónDeRodillaEnSedente_Unilateral, 90, 0, 90, 6.3f);
        AddFix(Movement.ExtensiónDeRodillasEnSedente_Bilateral, 90, 0, 90, 8.8f);
        AddFix(Movement.ExtensiónHorizontalDeHombrosEnSupino, 0, 60, 81, 37);
        AddFix(Movement.FlexiónDeRodillaEnSupino_Unilateral, 0, 120, 18, 111);
        AddFix(Movement.FlexiónDeRodillasEnSupino_Bilateral, 0, 120, 17, 124);
        AddFix(Movement.RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, 0, 70, 80, 0);
        AddFix(Movement.RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral, 0, 70, 79, 32);
        AddFix(Movement.RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, 0, 60, 82, 6);
        AddFix(Movement.RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral, 0, 60, 79, 32);
        AddFix(Movement.RotaciónInternaDeHombrosEnSupino_Bilateral, 0, 60, 80, 30);
        AddFix(Movement.SentadillaConBalónSuizo, 0, 90, 36, 88);
        AddFix(Movement.ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba, 0, 60, 78, 7.4f);
        AddFix(Movement.ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba, 0, 60, 78, 7.4f);
        AddFix(Movement.RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino, -45, 70, -57, -185.5f);
        AddFix(Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, 33, 180, 33, 177);
    }
    public static float FixAngle(float angleToFix, Movement movement)
    {
        if (matches.ContainsKey(movement))
        {
            AngleMatch m = matches[movement];
            Debug.Log(m.RealInitialAngle + (m.RealFinalAngle - m.RealInitialAngle) * (angleToFix - m.DreamedInitialAngle) / (m.DreamedFinalAngle - m.DreamedInitialAngle));
            return m.RealInitialAngle + (m.RealFinalAngle - m.RealInitialAngle) * (angleToFix - m.DreamedInitialAngle) / (m.DreamedFinalAngle - m.DreamedInitialAngle);
        }
        else
        {
            Debug.Log(angleToFix);
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