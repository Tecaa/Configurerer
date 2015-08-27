using UnityEngine;
using System.Collections;
using Assets;

public class ImplementationExample : MonoBehaviour
{

    // Use this for initialization


    Assets.HumanoidUtils utils;


    void Awake()
    {
        utils = GetComponent<HumanoidUtils>();
        
   
    }


    Assets.ArticulacionClass rodilladerecha;
    Assets.ArticulacionClass rodillaizquierda;
    Assets.ArticulacionClass cododerecha;
    Assets.ArticulacionClass codoizquierdo;
    Assets.ArticulacionClass brazoIzquierdo;
    Assets.ArticulacionClass brazoDerecho;
    Assets.ArticulacionClass musloIzquierdo;
    Assets.ArticulacionClass musloDerecho;
    Assets.ArticulacionClass anteBrazoIzquierdo;
    Assets.ArticulacionClass antebrazoDerecho;
    Assets.ArticulacionClass piernaIzquierdo;
    Assets.ArticulacionClass piernaDerecho;
    

    void Start()
    {

        rodilladerecha = utils.getArticulacion(Assets.ArticulacionType.RodillaDerecha);
        rodillaizquierda = utils.getArticulacion(Assets.ArticulacionType.RodillaIzquierda);
        cododerecha = utils.getArticulacion(Assets.ArticulacionType.CodoDerecho);
        codoizquierdo = utils.getArticulacion(Assets.ArticulacionType.CodoIzquierdo);

        brazoDerecho = utils.getArticulacion(Assets.ArticulacionType.BrazoDerecho);
        brazoIzquierdo = utils.getArticulacion(Assets.ArticulacionType.BrazoIzquierdo);
        musloIzquierdo = utils.getArticulacion(Assets.ArticulacionType.MusloIzquierda);
        musloDerecho = utils.getArticulacion(Assets.ArticulacionType.MusloDerecha);

        antebrazoDerecho = utils.getArticulacion(Assets.ArticulacionType.AnteBrazoDerecho);
        anteBrazoIzquierdo = utils.getArticulacion(Assets.ArticulacionType.AnteBrazoIzquierdo);
        piernaIzquierdo = utils.getArticulacion(Assets.ArticulacionType.PiernaIzquierda);
        piernaDerecho = utils.getArticulacion(Assets.ArticulacionType.PiernaDerecha);
    }

    public UnityEngine.UI.Text cuadroTexto;

    
    // Update is called once per frame
    void Update()
    {
        string granCadena;
        if (cuadroTexto != null)
        {

            granCadena = "Variables \n";
            granCadena += Time.frameCount.ToString() + "\n";

            granCadena += musloDerecho.getDataText();
            granCadena += musloIzquierdo.getDataText();

            granCadena += brazoDerecho.getDataText();
            granCadena += brazoIzquierdo.getDataText();

            granCadena += rodilladerecha.getDataText();
            granCadena += rodillaizquierda.getDataText();

            granCadena += cododerecha.getDataText();
            granCadena += codoizquierdo.getDataText();

            granCadena += antebrazoDerecho.getDataText();
            granCadena += anteBrazoIzquierdo.getDataText();

            granCadena += piernaDerecho.getDataText();
            granCadena += piernaIzquierdo.getDataText();


            cuadroTexto.text = granCadena;
        }
    }
}
