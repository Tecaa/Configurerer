using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class SegmentoSecundario : ArticulacionClass
    {


        private UnityEngine.GameObject puntoInterno;
        private UnityEngine.GameObject puntoExterno;
        private Plano planosMovimiento;

        public SegmentoSecundario(UnityEngine.GameObject puntoInterno, UnityEngine.GameObject puntoExterno, Plano planosMovimiento, Assets.ArticulacionType articulacion)
        {
            // TODO: Complete member initialization

            this.puntoInterno = puntoInterno;
            this.puntoExterno = puntoExterno;
            this.planosMovimiento = planosMovimiento;
            base.articulacion = articulacion;

            Update();
        }

        public override void Update()
        {


            Vector3 nSagital = planosMovimiento.sagital.normal;
            Vector3 nFrontal = planosMovimiento.frontal.normal;
            Vector3 nHorizontal = planosMovimiento.horizontal.normal;
            Vector3 nHorizontalAcostado = planosMovimiento.horizontalAcostado.normal;
            Vector3 segmento = puntoExterno.transform.position - puntoInterno.transform.position;

            Vector3 proyBrazoSagital = Vector3.Dot(segmento, nFrontal) * nFrontal + Vector3.Dot(segmento, nHorizontal) * nHorizontal;
            Vector3 proyBrazoFrontal = Vector3.Dot(segmento, nSagital) * nSagital + Vector3.Dot(segmento, nHorizontal) * nHorizontal;
            Vector3 proyBrazoHorizontal = Vector3.Dot(segmento, nSagital) * nSagital + Vector3.Dot(segmento, nFrontal) * nFrontal;


            AngleHorizontalAcostado = Mathf.Asin(Vector3.Dot(segmento.normalized, Vector3.up));
            AngleHorizontalAcostado = AngleHorizontalAcostado * 180.0f / Mathf.PI;

            var cruzHorizontal = Vector3.Cross(proyBrazoHorizontal, nFrontal);
            AngleHorizontal = Vector3.Angle(proyBrazoHorizontal, nFrontal) * (cruzHorizontal.x / Mathf.Abs(cruzHorizontal.x));

            var cruzFrontal = Vector3.Cross(nHorizontal, proyBrazoFrontal);
            AngleFrontal = Vector3.Angle(proyBrazoFrontal, nHorizontal * -1) * (cruzFrontal.x / Mathf.Abs(cruzFrontal.x));

            if (this.articulacion == ArticulacionType.BrazoIzquierdo ||
                this.articulacion == ArticulacionType.MusloIzquierda ||
                this.articulacion == ArticulacionType.PiernaIzquierda ||
                this.articulacion == ArticulacionType.AnteBrazoIzquierdo)
            {
                AngleFrontal *= -1;
                AngleHorizontal *= -1;

            }


            var cruzSagital = Vector3.Cross(proyBrazoSagital, nHorizontal);
            AngleSagital = Vector3.Angle(proyBrazoSagital, nHorizontal * -1) * (cruzSagital.x / Mathf.Abs(cruzSagital.x));


            /* Debug.DrawLine(puntoInterno.transform.position, puntoInterno.transform.position + puntoInterno.transform.up, Color.red);
             Debug.DrawLine(puntoInterno.transform.position, puntoInterno.transform.position + puntoInterno.transform.right, Color.yellow);
             Debug.DrawLine(puntoInterno.transform.position, puntoInterno.transform.position + puntoInterno.transform.forward, Color.green);*/

            
            /*

            var rotationRight = Vector3.Angle(hombro.transform.right, pecho.transform.right);
            var rotationForward = Vector3.Angle(hombro.transform.forward, pecho.transform.forward);
            var rotationUp =  Vector3.Angle(hombro.transform.up, pecho.transform.up);*/

            base.Update();

        }
    }
}
