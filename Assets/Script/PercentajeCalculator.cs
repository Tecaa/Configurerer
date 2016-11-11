using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using System.IO;

public static class PercentajeCalculator {
    private static string Filename = "Angles";
    private static Dictionary<Movement, SortedList<float, int>> angulos;


    static PercentajeCalculator()
    {
        Debug.Log(ReadAnglesFile());
    }
    /*
    static public float GetPercentage(float angle, Movement movement)
    {
        if (percentages.ContainsKey(movement))
        {
            Limits lim = percentages[movement];
            return (angle - lim.InitialAngle) / (lim.FinalAngle - lim.InitialAngle);
        }
        else
            return angle;
    }*/

    static public float GetPercentage2(float angle, Movement movement)
    {
        if (angulos.ContainsKey(movement))
        {
            SortedList<float, int> angs = angulos[movement];
            KeyValuePair<float, int>? lastPair = null;
            foreach (KeyValuePair<float, int> pair in angs)
            {
                if (!lastPair.HasValue)
                {
                    lastPair = pair;
                    continue;
                }
                if (angle <= Math.Max(pair.Value, lastPair.Value.Value) && angle >= Math.Min(pair.Value, lastPair.Value.Value))
                {
                    return lastPair.Value.Key + (angle - lastPair.Value.Value) * (pair.Key - lastPair.Value.Key) / (pair.Value - lastPair.Value.Value);
                }
                lastPair = pair;
            }
            return 1;
        }
        else
            return angle;
    }

    private static bool ReadAnglesFile()
    {
        try
        {
            TextAsset anglesData = Resources.Load(Filename) as TextAsset;
            angulos = (Dictionary<Movement, SortedList<float, int>>)JsonConvert.DeserializeObject(anglesData.text, typeof(Dictionary<Movement, SortedList<float, int>>));

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }

    
    /*
    private static Dictionary<Movement, Limits> percentages = new Dictionary<Movement, Limits>() {
        { Movement.AbducciónDeCaderaEnDecúbitoLateral, new Limits(0, 45) },
        { Movement.AducciónResistidaEnPlanoEscapular, new Limits(75, 30) },
        { Movement.ElevaciónDeHombroEnPlanoEscapularConBastón, new Limits(35, 180) },
        { Movement.ElevaciónResistidaDeHombroEnPlanoEscapular_Unilateral, new Limits(0, 180) },
        { Movement.ElevaciónResistidaDeHombrosEnPlanoEscapular_Bilateral, new Limits(0, 180) },
        { Movement.EstocadaFrontalCorta, new Limits(0, 90) },
        { Movement.EstocadaFrontalLarga, new Limits(-65, 90) },
        { Movement.EstocadaLateral, new Limits(-20, 90) },
        { Movement.ExtensiónDeCaderaEnProno, new Limits(0, 30) },
        { Movement.ExtensiónDeRodillaEnSedente_Unilateral, new Limits(90, 0) },
        { Movement.ExtensiónDeRodillasEnSedente_Bilateral, new Limits(90, 0) },
        { Movement.ExtensiónHorizontalDeHombrosEnSupino, new Limits(0, 60) },
        { Movement.FlexiónDeCaderaEnSupino, new Limits(0, 60) },
        { Movement.FlexiónDeCodoEnBípedo_Unilateral_90, new Limits(45, 140) },
        { Movement.FlexiónDeCodosEnBípedo_Bilateral_90, new Limits(40, 140) },
        { Movement.FlexiónDeHombroConExtensiónDeCodoEnBípedo_Unilateral_90, new Limits(0, 90) },
        { Movement.FlexiónDeHombrosConExtensiónDeCodosEnBípedo_Bilateral_90, new Limits(0, 90) },
        { Movement.FlexiónDeHombrosEnBípedoConBastón_Bilateral, new Limits(25, 160) },
        { Movement.FlexiónDeRodillaEnProno_Unilateral, new Limits(45, 120) },
        { Movement.FlexiónDeRodillaEnSupino_Unilateral, new Limits(0, 120) },
        { Movement.FlexiónDeRodillasEnProno_Bilateral, new Limits(30, 120) },
        { Movement.FlexiónDeRodillasEnSupino_Bilateral, new Limits(45, 120) },


        { Movement.RotaciónExternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, new Limits(0, 70) },
        { Movement.RotaciónExternaDeHombroEnSupinoConBastónT_Unilateral, new Limits(0, 70) },
        { Movement.RotaciónExternaDeHombrosEnSupino_Bilateral, new Limits(0, 70) },
        { Movement.RotaciónInternaDeHombroEnSupinoConAbducciónDeHombro_Unilateral, new Limits(0, 60) },
        { Movement.RotaciónInternaDeHombroEnSupinoConBastónT_Unilateral, new Limits(0, 60) },
        { Movement.RotaciónInternaDeHombrosEnSupino_Bilateral, new Limits(0, 60) },


        { Movement.Sentadilla, new Limits(0, 90) },
        { Movement.SentadillaConBalónSuizo, new Limits(0, 90) },
        { Movement.SentadillaMonopodal, new Limits(0, 90) },

        { Movement.ExtensiónDiagonalDeHombrosEnSupino_DerechaArriba, new Limits(0, 60) },
        { Movement.ExtensiónDiagonalDeHombrosEnSupino_IzquierdaArriba, new Limits(0, 60) },
        { Movement.FlexiónHorizontalResistidaDeHombros_BípedoBilateral, new Limits(0, 90) },
        { Movement.RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino, new Limits(-30, 70) },
    };

    private class Limits
    {
        public float InitialAngle;
        public float FinalAngle;

        public Limits(float start, float end)
        {
            InitialAngle = start;
            FinalAngle = end;
        }
    }*/
}

