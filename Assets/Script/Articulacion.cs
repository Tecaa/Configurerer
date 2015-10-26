using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.ComponentModel;


namespace Assets
{
    public class ArticulacionClass
    {
        public ArticulacionType articulacion;
        public ArticulacionClass() { }


        float lastSagital = 0;
        public float sagitalSpeed = 0;
        float lastFrontal = 0;
        public float frontalSpeed = 0;
        float lastHorizontal = 0;
        public  float horizontalSpeed = 0;
     

        public virtual void Update() { 
            //calculo de las velocidades angulares

            sagitalSpeed = (lastSagital - angleSagital) / Time.deltaTime;
            frontalSpeed = (lastFrontal - angleFrontal) / Time.deltaTime;
            horizontalSpeed = (lastHorizontal - angleHorizontal) / Time.deltaTime;

            angleSagital = getFixedAngle(angleSagital, lastSagital);
            angleFrontal = getFixedAngle(angleFrontal, lastFrontal);
            angleHorizontal = getFixedAngle(angleHorizontal, lastHorizontal);

            lastSagital = angleSagital;
            lastHorizontal = angleHorizontal;
            lastFrontal = angleFrontal;
        
        }

        /// <summary>
        /// Fixea el ángulo de manera de que no se cambie el signo en las secuencias de ángulos.
        /// Ejemplo: si se tiene la secuencia de ángulos 170, 175, 180, -175
        /// se transforme en 170, 175, 180, 185.
        /// Lo mismo para -170, -175, -180, 175 queda -170, -175, -180, -185
        /// </summary>
        /// <param name="p">Angulo a fixear</param>
        /// <param name="lastAngle">ultimo angulo conocido (frame anterior)</param>
        /// <returns></returns>
        private float getFixedAngle(float p, float lastAngle)
        {
            if (lastAngle >= 170 && p < 0)
                return 360 + p;
            else if (lastAngle <= -170 && p > 0)
                return -360 + p;
            return p;
        }

        private float angleSagital;
        public virtual float AngleSagital
        {
            get { return angleSagital; }
            set { angleSagital = value; }
        }

        private float angleFrontal;
        public virtual float AngleFrontal
        {
            get { return angleFrontal; }
            set { angleFrontal = value; }
        }

      

        private float angleHorizontal;
        public virtual float AngleHorizontal
        {
            get { return angleHorizontal; }
            set
            {
                angleHorizontal = value;
            }
        }

        internal string getDataText()
        {
           
            return this.articulacion.Description() + "> Sg: " + AngleSagital.ToString() + " Fr: " + AngleFrontal.ToString() + " Ho: " + AngleHorizontal.ToString() + "\n";
          
           
        }
    }

    public enum ArticulacionType
    {
        [Description("Rod I")]
        RodillaIzquierda,
         [Description("Rod ")]
        RodillaDerecha ,
         [Description("Cod I")]
        CodoIzquierdo ,
         [Description("Cod D")]
        CodoDerecho ,
         [Description("Bra D")]
        BrazoDerecho ,
         [Description("Bra I")]
        BrazoIzquierdo ,
         [Description("Mus D")]
        MusloDerecha ,
         [Description("Mus I")]
        MusloIzquierda ,
         [Description("Ant I")]
        AnteBrazoIzquierdo,
         [Description("Ant D")]
        AnteBrazoDerecho,
         [Description("Pie I")]
        PiernaIzquierda,
         [Description("Pie D")]
        PiernaDerecha,
        HombroIzquierdo,
        HombroDerecho
    };

    public static class ReflectionHelpers
    {
        public static string GetCustomDescription(object objEnum)
        {
            var fi = objEnum.GetType().GetField(objEnum.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
        }

        public static string Description(this Enum value)
        {
            return GetCustomDescription(value);
        }
    }
}
