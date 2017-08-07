using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class CaptureFrame : MonoBehaviour
{

    public Animator anim;
    public Slider slider;
    public InputField InputAngle;
    public Movement movement;
    private Dictionary<Movement, SortedList<float, int>> angulos = new Dictionary<Movement, SortedList<float, int>>();
    private static string Filename = "Angles-New";
    // Use this for initialization
    void Start()
    {
        movement = Movement.ExtensiónHorizontalDeHombrosEnSupino;
        if (!ReadAnglesFile())
            angulos = new Dictionary<Movement, SortedList<float, int>>();
        if (this.gameObject.GetComponentInParent<CanvasGroup>().alpha != 0)
            OrbitCamera.ctrlClick = true;
    }

    private bool ReadAnglesFile()
    {
        try
        {
            TextAsset anglesData = Resources.Load(Filename) as TextAsset;
            angulos = (Dictionary<Movement, SortedList<float, int>>)JsonConvert.DeserializeObject(anglesData.text, typeof(Dictionary<Movement, SortedList<float, int>>));
        }
        catch
        {
            return false;
        }
        return true;
    }

    public void GoFrame()
    {
        if (angulos.ContainsKey(movement))
            angulos[movement].Add(slider.value, Convert.ToInt32(InputAngle.text));
        else
        {
            angulos.Add(movement, new SortedList<float, int>());
            angulos[movement].Add(slider.value, Convert.ToInt32(InputAngle.text));
        }
        SaveAnglesFile();
    }

    private void SaveAnglesFile()
    {
        string str = JsonConvert.SerializeObject(angulos, Formatting.Indented);
        using (FileStream fs = new FileStream("Assets/Resources/" + Filename + ".json", FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(str);
                writer.Close();
                writer.Dispose();
            }
            fs.Close();
            fs.Dispose();
        }
    }
    

    public void Slider()
    {
        if (anim.GetInteger("Movement") != (int)movement)
            anim.SetInteger("Movement", (int)movement);
        anim.speed = 0;
        anim.PlayInFixedTime(0, 0, slider.value * anim.GetCurrentAnimatorStateInfo(0).length);
        
    }
    
}