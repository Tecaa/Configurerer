using UnityEngine;
using System.Collections;
using System.Timers;
using System;

public class AnimatorClock  {

    public AnimatorClock()
    {

    }


    public class AnimatorTimer : System.Timers.Timer
    {

        private MethodAfterTime method = null;
        private float time;

        public AnimatorTimer(float timeInput, MethodAfterTime methodInput) : base(timeInput)
        {
            method = methodInput;
            time = timeInput;
        }

        public void runMethodAfterTime()
        {
            this.Elapsed += new ElapsedEventHandler(runMethodAfterTimeEvent);
            this.Interval = 2000;
            this.Enabled = true;
            this.Start();
        }

        private void runMethodAfterTimeEvent(object source, ElapsedEventArgs e)
        {
            this.Stop();
            this.Enabled = false;

            this.Elapsed -= new ElapsedEventHandler(runMethodAfterTimeEvent);

            Debug.Log("The Elapsed event was raised at " + e.SignalTime);
            methodTrigger(this);
        }

        public void runMethod()
        {
            
            method();
        }

        public delegate void MethodTrigger(AnimatorTimer self);
        public MethodTrigger methodTrigger;

    }


    public void StartTimerCoroutine(float time, MethodAfterTime method)
    {
        AnimatorTimer aTimer = new AnimatorTimer(time, method);
        aTimer.methodTrigger += methodTrigger;
        aTimer.runMethodAfterTime();
    }

    private void methodTrigger(AnimatorTimer aTimer)
    {
        Debug.Log("iniciando metodo");
        aTimer.runMethod();
        Debug.Log("metodo ejecutado");
        aTimer.methodTrigger -= methodTrigger;
    }


    //IEnumerator startPauseBetweenRepetitionsIenumerator(float duration)
    //{
        //pauseBetweenRepetitionsStart();
       // yield return new WaitForSeconds(duration);
        //pauseBetweenRepetitionsFinish();
    //}


    public void startPauseBetweenRepetitions(float duration)
    {
        //StartCoroutine(startPauseBetweenRepetitionsIenumerator(duration));
    }


    public void startExecutionTimerHelper()
    {
        Debug.Log("se termina ejecucion");
        executionTimerFinish();
    }

    public void startExecutionTimer(float duration)
    {
        executionTimerStart();
        StartTimerCoroutine(2F, startExecutionTimerHelper);
    }


    public delegate void PauseBetweenRepetitionsStart();
    public PauseBetweenRepetitionsStart pauseBetweenRepetitionsStart;

    public delegate void PauseBetweenRepetitionsFinish();
    public PauseBetweenRepetitionsFinish pauseBetweenRepetitionsFinish;


    public delegate void ExecutionTimerStart();
    public ExecutionTimerStart executionTimerStart;

    public delegate void ExecutionTimerFinish();
    public ExecutionTimerFinish executionTimerFinish;

}

public delegate void MethodAfterTime();