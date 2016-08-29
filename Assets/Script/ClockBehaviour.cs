using UnityEngine;
using System.Collections;
using System;

public class ClockBehaviour {
    DateTime? endExecutionTime, endRepetitionTime;



    /// <summary>
    /// Correr este metodo al inicio del Update del AnimationBehaviour
    /// </summary>
    public void Update()
    {
        if (endRepetitionTime.HasValue &&  endRepetitionTime < DateTime.Now)
        {
            pauseBetweenRepetitionsFinish();
            stopTimeBetweenRepetitionsTimer();
        }
        if (endExecutionTime.HasValue && endExecutionTime < DateTime.Now)
        {
            executionTimerFinish();
            stopExecutionTimer();
        }

    }

    /// <summary>
    /// Inicia el contador de duracion de una repeticion hasta cumplir con el "time". 
    /// </summary>
    /// <param name="time"></param>
    public void executeRepetitionTime(float time)
    {
        endExecutionTime = DateTime.Now + new TimeSpan(0, 0,(int)time);
        executionTimerStart();
    }

    /// <summary>
    /// Detiene el contador de la duracion de un repeticion.
    /// </summary>
    public void stopExecutionTimer()
    {
        endExecutionTime = null;
    }


    /// <summary>
    /// Inicia el contador de duracion del tiempo entre repeticiones hasta cumplir con el "time". 
    /// </summary>
    /// <param name="time"></param>
    public void executeTimeBetweenRepetitions(float time)
    {

        endRepetitionTime = DateTime.Now + new TimeSpan(0, 0, (int)time);
        pauseBetweenRepetitionsStart();
    }

    /// <summary>
    /// Inicia el contador de duracion del tiempo entre repeticiones hasta cumplir con el "time". 
    /// </summary>
    public void stopTimeBetweenRepetitionsTimer()
    {
        endRepetitionTime = null;
    }

    public delegate void ExecutionTimerStart();
    public ExecutionTimerStart executionTimerStart;

    public delegate void ExecutionTimerFinish();
    public ExecutionTimerFinish executionTimerFinish;

    public delegate void PauseBetweenRepetitionsStart();
    public PauseBetweenRepetitionsStart pauseBetweenRepetitionsStart;

    public delegate void PauseBetweenRepetitionsFinish();
    public PauseBetweenRepetitionsFinish pauseBetweenRepetitionsFinish;
}