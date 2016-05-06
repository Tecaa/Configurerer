using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
            MovementLimbKey k = new MovementLimbKey(AnimatorScript.instance.CurrentExercise.Movement, AnimatorScript.instance.CurrentExercise.Laterality, AnimatorScript.instance.CurrentExercise.Limb);

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

        public Visagra(UnityEngine.GameObject UpperArm, UnityEngine.GameObject Arm, UnityEngine.GameObject Hand, ArticulacionType articulacion)
        {
            base.articulacion = articulacion;

            this.inicio = UpperArm;
            this.centro = Arm;
            this.fin = Hand;
        }
    }
}
