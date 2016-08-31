using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    class SegmentoPrimario : ArticulacionClass
    {

       
        private UnityEngine.GameObject hombro;
        private UnityEngine.GameObject codo;
        private Plano planosMovimiento;
        private MonoBehaviour parent;

        public SegmentoPrimario(UnityEngine.GameObject puntoInterno, UnityEngine.GameObject puntoExterno, Plano planosMovimiento, 
            Assets.ArticulacionType articulacion, UnityEngine.MonoBehaviour m)
        {
            // TODO: Complete member initialization
         
            this.hombro = puntoInterno;
            this.codo = puntoExterno;
            this.planosMovimiento = planosMovimiento;
            base.articulacion = articulacion;
            this.parent = m;
            Update();
        }

        public override void Update()
        {


            Vector3 nSagital = planosMovimiento.sagital.normal;
            Vector3 nFrontal = planosMovimiento.frontal.normal;
            Vector3 nHorizontal = planosMovimiento.horizontal.normal;
            Vector3 nHorizontalAcostado = planosMovimiento.horizontalAcostado.normal;
            Vector3 segmento = (codo.transform.position - hombro.transform.position).normalized;

            Vector3 segmentoFrontal = segmento;
            Vector3 segmentoSagital = segmento;
            Vector3 segmentoHorizontal = segmento;
            // Correcciones necesarias para que no afecte la rotacón al signo del ángulo medido
            if (parent.transform.eulerAngles.y >= 180)
            {
                segmentoFrontal.x *= -1;
                segmentoFrontal.z *= -1;
                segmentoHorizontal.x *= -1;
                segmentoHorizontal.z *= -1;
            }
            if (parent.transform.eulerAngles.y >= 90   && parent.transform.eulerAngles.y <= 180+90)
            {
                
                segmentoSagital.x *= -1;
                segmentoSagital.z *= -1;
            }
            if (parent.transform.eulerAngles.y >= 90 && parent.transform.eulerAngles.y <= 180 + 90)
            {
                /*
                brazoHorizontal.x *= -1;
                brazoHorizontal.z *= -1;*/
            }


            //if (this.articulacion == ArticulacionType.BrazoDerecho)
            //    Debug.Log("brazo " + segmento + " nFrontal " + nSagital + " nHorizontal " + nHorizontal);

            Vector3 proyBrazoSagital = Vector3.Dot(segmentoSagital, nFrontal) * nFrontal + Vector3.Dot(segmentoSagital, nHorizontal) * nHorizontal;
            Vector3 proyBrazoFrontal = Vector3.Dot(segmentoFrontal, nSagital) * nSagital + Vector3.Dot(segmentoFrontal, nHorizontal) * nHorizontal;
            Vector3 proyBrazoHorizontal = Vector3.Dot(segmentoHorizontal, nSagital) * nSagital + Vector3.Dot(segmentoHorizontal, nFrontal) * nFrontal;



            // Vector3 proyBrazoHorizontalAcostado = hombro - codo; 


            //    Vector3.Dot(brazo, Vector3.right) * Vector3.right + Vector3.Dot(brazo, Vector3.forward) * Vector3.forward;
            //   var cruzHorizontalAcostado = Vector3.Cross(proyBrazoHorizontalAcostado, Vector3.right);


            //   AngleHorizontalAcostado = Vector3.Angle(proyBrazoHorizontalAcostado, nHorizontalAcostado) * (cruzHorizontalAcostado.x / Mathf.Abs(cruzHorizontalAcostado.x));
            AngleHorizontalAcostado = Mathf.Asin(Vector3.Dot(segmentoHorizontal.normalized, Vector3.up));
            AngleHorizontalAcostado = AngleHorizontalAcostado * 180.0f / Mathf.PI;

            var cruzHorizontal = Vector3.Cross(proyBrazoHorizontal, nFrontal);
            AngleHorizontal = Vector3.Angle(proyBrazoHorizontal, nFrontal) * (cruzHorizontal.x / Mathf.Abs(cruzHorizontal.x));
           

            var cruzFrontal = Vector3.Cross(nHorizontal, proyBrazoFrontal);

            AngleFrontal = Vector3.Angle(proyBrazoFrontal, nHorizontal * -1) * (cruzFrontal.x / Mathf.Abs(cruzFrontal.x));

           /* if (this.articulacion == ArticulacionType.BrazoIzquierdo)
                Debug.Log(" cruzFrontal " + cruzFrontal + " nHorizontal " + nHorizontal + " proyBrazoFrontal " + proyBrazoFrontal);*/


            if (this.articulacion == ArticulacionType.BrazoIzquierdo ||
                this.articulacion == ArticulacionType.MusloIzquierda ||
                this.articulacion == ArticulacionType.PiernaIzquierda ||
                this.articulacion == ArticulacionType.AnteBrazoIzquierdo)
            {
                AngleFrontal *= -1;
                AngleHorizontal *= -1;
                AngleHorizontalAcostado *= -1;

            }
            //if (this.articulacion == ArticulacionType.BrazoIzquierdo)
            //    Debug.Log("Angle Frontal " + AngleFrontal);

            var cruzSagital = Vector3.Cross(proyBrazoSagital, nHorizontal);
            AngleSagital = Vector3.Angle(proyBrazoSagital, nHorizontal * -1) * (cruzSagital.x / Mathf.Abs(cruzSagital.x));

            //if (this.articulacion == ArticulacionType.BrazoIzquierdo)
            //    Debug.Log("AngleSagital " + AngleSagital);

            Debug.DrawLine(hombro.transform.position, codo.transform.position, Color.cyan);


            /*  Debug.DrawLine(hombro.transform.position, hombro.transform.position + hombro.transform.right, Color.yellow);
              Debug.DrawLine(hombro.transform.position, hombro.transform.position + hombro.transform.forward, Color.green);

              Debug.DrawLine(pecho.transform.position, pecho.transform.position + pecho.transform.up, Color.red);
              Debug.DrawLine(pecho.transform.position, pecho.transform.position + pecho.transform.right, Color.yellow);
              Debug.DrawLine(pecho.transform.position, pecho.transform.position + pecho.transform.forward, Color.green);


             var rotationRight = Vector3.Angle(hombro.transform.right, pecho.transform.right);
             var rotationForward = Vector3.Angle(hombro.transform.forward, pecho.transform.forward);
             var rotationUp =  Vector3.Angle(hombro.transform.up, pecho.transform.up);*/

            base.Update();
            MovementLimbKey k = new MovementLimbKey(AnimatorScript.instance.CurrentExercise.Movement, AnimatorScript.instance.CurrentExercise.Limb);
            
            if (MovementJointMatch.movementJointMatch.ContainsKey(k))
            {
                //Debug.Log("MOVEMENT LIMB KEY " + MovementJointMatch.movementJointMatch[k].jointType);
                ArticulacionType type = MovementJointMatch.movementJointMatch[k].jointType;
                if (type == articulacion)
                {
                    GameObject.FindGameObjectWithTag("anguloFrontal").GetComponent<Text>().text = "Angulo Frontal : " + AngleFrontal;

                    GameObject.FindGameObjectWithTag("anguloHorizontal").GetComponent<Text>().text = "Angulo Horizontal : " + AngleHorizontal;

                    GameObject.FindGameObjectWithTag("anguloSagital").GetComponent<Text>().text = "Angulo Sagital : " + AngleSagital;

                    GameObject.FindGameObjectWithTag("anguloHorizontalAcostado").GetComponent<Text>().text = "Angulo Horizontal Acostado : " + AngleHorizontalAcostado;
                }
            }
        }
    }
}
