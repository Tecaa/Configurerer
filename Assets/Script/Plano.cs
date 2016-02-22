using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets
{
    public class Plano
    {
        GameObject referenciaDerecho;
        GameObject referenciaIzquierdo;
        GameObject centroCorporal;
        GameObject referenciaArriba;
      
        public Plane frontal;
        public Plane sagital;
        public Plane horizontal;
        public Plane horizontalAcostado;
        public planos tipoPlano;

      
        public Plano(GameObject centro, GameObject derecha, GameObject izquierda, GameObject arriba)
        {
            this.centroCorporal = centro;
            this.referenciaDerecho = derecha;
            this.referenciaIzquierdo = izquierda;
            this.referenciaArriba = arriba;
            
            Update();
        }


        public void Update()
        {
            if (!centroCorporal || !referenciaDerecho || !referenciaDerecho || !referenciaArriba) return;

            Vector3 centro = centroCorporal.transform.position;
            Vector3 hombroD = referenciaDerecho.transform.position;
            Vector3 hombroI = referenciaIzquierdo.transform.position;
            Vector3 arriba = referenciaArriba.transform.position;

            frontal = new Plane(centro, hombroD, hombroI);
            sagital = new Plane(centro, centro + frontal.normal, arriba);

            horizontalAcostado = new Plane(Vector3.up, centro);

            frontal.SetNormalAndPosition(Quaternion.AngleAxis(-4.43f, sagital.normal) * frontal.normal, centro);


            var cruz = Vector3.Cross(sagital.normal, frontal.normal);
            horizontal = new Plane(cruz, centro);
 
        }

        public void Draw(Color color, planos tipo)
        {

            Vector3 v3;
            Vector3 normal;
            Vector3 position = centroCorporal.transform.position;

            switch (tipo)
            {
                case planos.planoFrontal: normal = frontal.normal; break;
                case planos.planoHorizontal: normal = horizontal.normal; break;
                case planos.planoSagital: normal = sagital.normal; break;
                case planos.planoHorizontalAcostado: normal = horizontalAcostado.normal; break;
                default : return;
            }


            if (normal != Vector3.forward)
                v3 = Vector3.Cross(normal, Vector3.forward).normalized;// * normal.magnitude;
            else
                v3 = Vector3.Cross(normal, Vector3.up).normalized;// *normal.magnitude;

            var corner0 = position + v3;
            var corner2 = position - v3;

            var q = Quaternion.AngleAxis(90f, normal);
            v3 = q * v3;
            var corner1 = position + v3;
            var corner3 = position - v3;

            Debug.DrawLine(corner0, corner2, color);
            Debug.DrawLine(corner1, corner3, color);
            Debug.DrawLine(corner0, corner1, color);
            Debug.DrawLine(corner1, corner2, color);
            Debug.DrawLine(corner2, corner3, color);
            Debug.DrawLine(corner3, corner0, color);
            Debug.DrawRay(position, normal, color);

        }
        public enum planos { planoFrontal, planoSagital, planoHorizontal, planoHorizontalAcostado };

        

    }
}
