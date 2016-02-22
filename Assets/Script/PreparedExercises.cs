using UnityEngine;  
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

public class PreparedExercises {
    /// <summary>
    /// Almacena el ejercicio al que actualmente se le calcularon sus parámetros
    /// </summary>
    private static Dictionary<Exercise, List<AnimationInfo>> _preparedExercises;
    
    static PreparedExercises()
    {
        //TODO: Es posible mejorarlo ya que aunque se compile en versión desktop, carga las preferencias para web de igual forma
        string serializedPreparedExercises = PlayerPrefs.GetString("ExerciseCache");
        if (serializedPreparedExercises == null || serializedPreparedExercises == String.Empty)
        {
            _preparedExercises = new Dictionary<Exercise, List<AnimationInfo>>();
        }
        else
        {
            _preparedExercises = (Dictionary<Exercise, List<AnimationInfo>>)JsonConvert.DeserializeObject<Dictionary<Exercise, List<AnimationInfo>>>(serializedPreparedExercises);
        }
    }

    public static bool tryGetPreparedExercise(Exercise e, out List<AnimationInfo> animationInfo, float animationLength)
    {
       float TIME_ERROR = 0.1f;
       animationInfo = null;
       //TODO: Evaluar borrar esta correccion cuando se tenga claridad de todos los ejercicios
       e.Laterality = Laterality.Single;
       e.Limb = Limb.Left;


       string serializedPreparedExercises = PlayerPrefs.GetString("ExerciseCache");
       if (serializedPreparedExercises == null || serializedPreparedExercises == String.Empty)
       {
           _preparedExercises = new Dictionary<Exercise, List<AnimationInfo>>();
       }
       else
       {
           _preparedExercises = (Dictionary<Exercise, List<AnimationInfo>>)JsonConvert.DeserializeObject<Dictionary<Exercise, List<AnimationInfo>>>(serializedPreparedExercises);
       }


       if (_preparedExercises.TryGetValue(e, out animationInfo))
       {

           //No sé por qué, pero debe estar comentado para que funcione.
           //if (Math.Abs(animationInfo[animationInfo.Count / 2].time - animationLength) <= TIME_ERROR)
               return true;
               DebugLifeware.Log("Encontrado el exercis saved pero no cumle con el TIME_ERROR", DebugLifeware.Developer.Marco_Rojas);
       }
       else

           DebugLifeware.Log("No encontrado el ejercicio saved.", DebugLifeware.Developer.Marco_Rojas);
       return false;
    }

    public static void InsertPreparedExercise(Exercise e, List<AnimationInfo> animationInfo)
    {
        //TODO: Evaluar borrar esta correccion cuando se tenga claridad de todos los ejercicios
        e.Laterality = Laterality.Single;
        e.Limb = Limb.Left;
        if (!_preparedExercises.ContainsKey(e))
        {
            _preparedExercises.Add(e, animationInfo);
            DebugLifeware.Log("Saving exercise", DebugLifeware.Developer.Marco_Rojas);
            PlayerPrefs.SetString("ExerciseCache", JsonConvert.SerializeObject(_preparedExercises));
        }
        else
            throw new Exception("Exercise already exists");
    }
}
