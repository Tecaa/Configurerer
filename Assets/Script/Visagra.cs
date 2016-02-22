using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Visagra : ArticulacionClass
    {
        //AnguloSimple flexoExtension;

      
        GameObject inicio;
        GameObject centro;
        GameObject fin;

        public override void Update()
        {
            Vector3 vectorInicial = inicio.transform.position - centro.transform.position;
            Vector3 vectorFinal = fin.transform.position - centro.transform.position;

            AngleSagital = Vector3.Angle(vectorInicial, vectorFinal) * -1 + 180;

            base.Update();
        }

        public Visagra(UnityEngine.GameObject UpperArm, UnityEngine.GameObject Arm, UnityEngine.GameObject Hand, ArticulacionType articulacion)
        {
            base.articulacion = articulacion;

            this.inicio = UpperArm;
            this.centro = Arm;
            this.fin = Hand;
        }
    }
}
