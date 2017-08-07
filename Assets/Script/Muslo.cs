using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class Muslo : ArticulacionClass
    {
        GameObject cadera;
        GameObject rodilla;
        private Plano planosMovimiento;
        //public Assets.articulaciones_ articulaciones;

        public Muslo(GameObject cadera, GameObject rodilla, Plano planosMovimiento, ArticulacionType articulacion)
        {
            this.cadera = cadera;
            this.rodilla = rodilla;
            this.planosMovimiento = planosMovimiento;
            base.articulacion = articulacion;
            Update();
        }

        override public void  Update()
        {
            Vector3 nSagital = planosMovimiento.sagital.normal;
            Vector3 nFrontal = planosMovimiento.frontal.normal;
            Vector3 nHorizontal = planosMovimiento.horizontal.normal;

            Vector3 femur = rodilla.transform.position - cadera.transform.position;


            Vector3 proyBrazoSagital = Vector3.Dot(femur, nFrontal) * nFrontal + Vector3.Dot(femur, nHorizontal) * nHorizontal;
            Vector3 proyBrazoFrontal = Vector3.Dot(femur, nSagital) * nSagital + Vector3.Dot(femur, nHorizontal) * nHorizontal;
            Vector3 proyBrazoHorizontal = Vector3.Dot(femur, nSagital) * nSagital + Vector3.Dot(femur, nFrontal) * nFrontal;



            var cruzHorizontal = Vector3.Cross(proyBrazoHorizontal, nFrontal);
            AngleHorizontal = Vector3.Angle(proyBrazoHorizontal, nFrontal) * (cruzHorizontal.x / Mathf.Abs(cruzHorizontal.x));

            var cruzFrontal = Vector3.Cross(nHorizontal, proyBrazoFrontal);
            AngleFrontal = Vector3.Angle(proyBrazoFrontal, nHorizontal * -1) * (cruzFrontal.x / Mathf.Abs(cruzFrontal.x));

            if (this.articulacion == ArticulacionType.MusloIzquierda)
            {
                AngleFrontal *= -1;
                AngleHorizontal *= -1;
            }
            
            var cruzSagital = Vector3.Cross(proyBrazoSagital, nHorizontal);
            AngleSagital = Vector3.Angle(proyBrazoSagital, nHorizontal * -1) * (cruzSagital.x / Mathf.Abs(cruzSagital.x));


            Debug.DrawLine(cadera.transform.position, rodilla.transform.position, Color.cyan);
            base.Update();

        }
    }
}
