using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Lerper
{
    #region Eventos

    public event EventHandler OnLerpStart;

    public event EventHandler OnLerpEnd;


    #endregion

    #region Variables
    /// <summary>
    /// Velocidad durante la fase concéntrica
    /// </summary>
    private float forwardSpeed = 1f;

    /// <summary>
    /// Velocidad durante la fase excentrica
    /// </summary>
    private float backwardSpeed = 1f;

    /// <summary>
    /// Momento en segundos en que la animación está en el ángulo máximo
    /// </summary>
    /// //TODO: Debe ser privado
    private float timeTakenDuringLerp = 1f;

    /// <summary>
    /// Tiempo que demora acelerar o desacelerar el movimiento concéntrico
    /// </summary>
    private float timeTakenDuringForwardLerp = 1f;

    /// <summary>
    /// Tiempo que demora acelerar o desacelerar el movimiento excéntrico
    /// </summary>
    private float timeTakenDuringBackwardLerp = 1f;

    /// <summary>
    /// Indica si actualmente de está interpolando
    /// </summary>
    private bool isLerping = false;

    /// <summary>
    /// Indica desde que punto se hará la interpolación
    /// </summary>
    private float startPosition;

    /// <summary>
    /// Indica hasta qué punto se hará la interpolación
    /// </summary>
    private float endPosition;

    /// <summary>
    /// Marca de tiempo en que se inició la interpolación
    /// </summary>
    private float timeStartedLerping;

    /// <summary>
    /// Representa el estado actual de la animación, este se utiliza para identificar el sentido de la animación y aplicar las curvas de aceleración o desaceleración
    /// </summary>
    private LerpState currentAnimationState;

    /// <summary>
    /// Representa el último estado de la animación, este se utiliza para identificar el sentido de la animación y aplicar las curvas de aceleración o desaceleración
    /// </summary>
    private LerpState lastState;

    /// <summary>
    /// Almacena el IENumerator utilizado para correr las Corutinas que hacen la interpolación
    /// </summary>
    private IEnumerator InterpolateCR;
    #endregion

    /// <summary>
    /// Comienza la interpolación entre 0 y el tiempo donde está el ángulo requerido (LerpTime), aplicando las velocidades concéntras y excéntricas indicadas
    /// </summary>
    /// <param name="_lerpTime">Tiempo donde se encuentra el ángulo al que se quiere llegar</param>
    /// <param name="_forwardSpeed">Velocidad concéntrica</param>
    /// <param name="_backwardSpeed">Velocidad excéntrica</param>
    public void StartLerp(float _lerpTime, float _forwardSpeed, float _backwardSpeed)
    {

        forwardSpeed = _forwardSpeed;
        backwardSpeed = _backwardSpeed;

        //Comienzo efectuando la aceleración inicial
        currentAnimationState = LerpState.Forward;
        timeTakenDuringLerp = (float)Math.Floor(_lerpTime * 10) / 10;

        //Normalizo el tiempo que tardo en parar
        timeTakenDuringForwardLerp = timeTakenDuringLerp / forwardSpeed;
        timeTakenDuringBackwardLerp = timeTakenDuringLerp / backwardSpeed;

        BeginLerp(0, forwardSpeed);
        RaiseEvent(OnLerpStart);
    }

    /// <summary>
    /// Inicia una interpolación y ajusta los valores iniciales
    /// </summary>
    private void BeginLerp(float startPos, float endPos)
    {
        if(InterpolateCR != null)
        {
            AnimatorScript.instance.StopCoroutine(InterpolateCR);
        }

        InterpolateCR = Interpolate();
        isLerping = true;
        timeStartedLerping = Time.time;
        startPosition = startPos;
        endPosition = endPos;

        AnimatorScript.instance.StartCoroutine(InterpolateCR);
    }

    /// <summary>
    /// Detiene la interpolación que actualmente se está ejecutando
    /// </summary>
    /*
    public void StopLerp()
    {
        if (InterpolateCR != null)
        {
            AnimatorScript.instance.StopCoroutine(InterpolateCR);
        }

        AnimatorScript.anim.speed = ExerciseRunner.DEFAULT_SPEED;
    }*/
    /// <summary>
    /// Realiza la interpolación entre los valores deseados de acuerdo al parámetro "percentageComplete" el cual se modifica para transformar la , se debe llamar en cada frame una vez que se inició un interpolación
    /// </summary>
    private IEnumerator Interpolate()
    {
        bool isRunning = true;
        while (isRunning)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentageComplete = 0f;

            switch (currentAnimationState)
            {
                case LerpState.Forward:
                    percentageComplete = timeSinceStarted / timeTakenDuringForwardLerp;
                    break;

                case LerpState.Stopped:
                    if (lastState == LerpState.Forward)
                    {
                        percentageComplete = timeSinceStarted / timeTakenDuringForwardLerp;
                    }

                    //De ser true, indica que termino una repeticion
                    else if (lastState == LerpState.Backward)
                    {
                        percentageComplete = timeSinceStarted / timeTakenDuringBackwardLerp;
                    }
                    break;

                case LerpState.Backward:
                    percentageComplete = timeSinceStarted / timeTakenDuringBackwardLerp;
                    break;
            }

            //Aplico el suavizado "Smotherstep"
            //percentageComplete = percentageComplete * percentageComplete * (3f - 2f * percentageComplete);
            percentageComplete = percentageComplete * percentageComplete * percentageComplete * (percentageComplete * (6f * percentageComplete - 15f) + 10f);

            //AnimatorScript.anim.speed = Mathf.Lerp(startPosition, endPosition, percentageComplete);
            if (percentageComplete >= 1.0f)
            {
                isRunning = false;
                InterpolationEnd();
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    /// <summary>
    /// Cuando termina una interpolacion se comprueba el estado de la animacion para continuar con el ciclo de aceleración y desaceleracion
    /// </summary>
    private void InterpolationEnd()
    {
        isLerping = false;
        switch (currentAnimationState)
        {
            case LerpState.Forward:
                currentAnimationState = LerpState.Stopped;
                lastState = LerpState.Forward;
                BeginLerp(forwardSpeed, 0);
                break;

            case LerpState.Stopped:
                if (lastState == LerpState.Forward)
                {
                    currentAnimationState = LerpState.Backward;
                    lastState = LerpState.Stopped;
                    BeginLerp(0, -backwardSpeed);
                }

                //De ser true, indica que termino una repeticion
                else if (lastState == LerpState.Backward)
                {
                    RaiseEvent(OnLerpEnd);
                    currentAnimationState = LerpState.Forward;
                    lastState = LerpState.Forward;
                    BeginLerp(0, forwardSpeed);
                }
                break;

            case LerpState.Backward:
                currentAnimationState = LerpState.Stopped;
                lastState = LerpState.Backward;
                BeginLerp(-backwardSpeed, 0);
                break;
        }
    }

    void RaiseEvent(EventHandler eh)
    {
        if(eh != null)
        {
            eh(null, new EventArgs());
        }
    }
}
