using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace Assets
{
    public class HumanoidUtils : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }
        public bool drawPlanes;

        public GameObject hips;
        public GameObject spine;

        public GameObject leftShoulder;
        public GameObject leftArm;
        public GameObject leftForeArm;
        public GameObject leftHand;

        public GameObject leftUpperLeg;
        public GameObject leftLeg;
        public GameObject leftFoot;

        public GameObject rightShoulder;
        public GameObject rightArm;
        public GameObject rightForeArm;
        public GameObject rightHand;

        public GameObject rightUpperLeg;
        public GameObject rightLeg;
        public GameObject rightFoot;




        public List<ArticulacionClass> segmentoCorporal = new List<ArticulacionClass>();
        Plano planosMovimiento;

        void Awake()
        {
            if (hips == null ||
                spine == null ||             
                leftForeArm == null ||
                leftFoot == null ||
                leftHand == null ||
                leftLeg == null ||
                leftShoulder == null ||
                leftArm == null ||
                leftUpperLeg == null ||
                rightForeArm == null ||
                rightFoot == null ||
                rightHand == null ||
                rightLeg == null ||
                rightShoulder == null ||
                rightArm == null ||
                rightUpperLeg == null
                )
            {
                return;
            }

             
            
         
            planosMovimiento = new Plano(hips, rightShoulder, leftShoulder, spine);

            segmentoCorporal.Add(new Visagra(leftUpperLeg, leftLeg, leftFoot, ArticulacionType.RodillaIzquierda));
            segmentoCorporal.Add(new Visagra(rightUpperLeg, rightLeg, rightFoot, ArticulacionType.RodillaDerecha));
            
            segmentoCorporal.Add(new Visagra(leftArm, leftForeArm, leftHand, ArticulacionType.CodoIzquierdo));
            segmentoCorporal.Add(new Visagra(rightArm, rightForeArm, rightHand, ArticulacionType.CodoDerecho));

            //TODO: Prueba
            //segmentoCorporal.Add(new Visagra(rightArm, leftArm, leftForeArm, ArticulacionType.HombroIzquierdo));
            //segmentoCorporal.Add(new Visagra(leftArm, rightArm, rightForeArm, ArticulacionType.HombroDerecho));


            segmentoCorporal.Add(new SegmentoPrimario(rightArm, rightForeArm, planosMovimiento, ArticulacionType.BrazoDerecho));
            segmentoCorporal.Add(new SegmentoSecundario(rightForeArm, rightHand, planosMovimiento, ArticulacionType.AnteBrazoDerecho));
            
            segmentoCorporal.Add(new SegmentoPrimario(leftArm, leftForeArm, planosMovimiento, ArticulacionType.BrazoIzquierdo));
            segmentoCorporal.Add(new SegmentoSecundario(leftForeArm, leftHand, planosMovimiento, ArticulacionType.AnteBrazoIzquierdo));
            
            segmentoCorporal.Add(new SegmentoPrimario(rightUpperLeg, rightLeg, planosMovimiento, ArticulacionType.MusloDerecha));
            segmentoCorporal.Add(new SegmentoSecundario(rightLeg, rightFoot, planosMovimiento, ArticulacionType.PiernaDerecha));

            segmentoCorporal.Add(new SegmentoPrimario(leftUpperLeg, leftLeg, planosMovimiento, ArticulacionType.MusloIzquierda));
            segmentoCorporal.Add(new SegmentoSecundario(leftLeg, leftFoot, planosMovimiento, ArticulacionType.PiernaIzquierda));            
            
        }


        public ArticulacionClass getArticulacion(ArticulacionType tipo)
        {
            if (segmentoCorporal.Count == 0)
            { 
            
            }
            foreach (var x in segmentoCorporal)
            {
                if (x.articulacion == tipo)
                {
                    return x;
                }
            }
            throw new Exception();
   
        }
        
        



        // Update is called once per frame
        void Update()
        {

           // head.transform.localRotation

            planosMovimiento.Update();

            foreach (var x in segmentoCorporal)
            {
                x.Update();
            }

            if (this.drawPlanes)
            {
                planosMovimiento.Draw(Color.red, Plano.planos.planoFrontal);
                planosMovimiento.Draw(Color.green, Plano.planos.planoHorizontal);
                planosMovimiento.Draw(Color.yellow, Plano.planos.planoSagital);
            }
        }
    }
}