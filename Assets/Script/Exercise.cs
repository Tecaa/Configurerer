using UnityEngine;
using System.Collections;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Exercise : INotifyPropertyChanged
{
    public Exercise() { }
    public Exercise(Movement m, Limb l)
    {
        this.movement = m;
        this.limb = l;
    }
    private Movement movement;
    public Movement Movement
    {
        get
        {
            return movement;
        }
        set
        {
            movement = value;
            OnPropertyChanged(AnimatorParams.Movement);
        }
    }

    private Limb limb;
    public Limb Limb
    {
        get
        {
            return limb;
        }
        set
        {
            limb = value;
            OnPropertyChanged(AnimatorParams.Limb);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null)
        {
            handler(this, new PropertyChangedEventArgs(name));
        }
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static explicit operator Exercise(string s)
    {
        Exercise e = JsonConvert.DeserializeObject<Exercise>(s);
        return e;
    }


    public override bool Equals(object obj)
    {
        Exercise e = obj as Exercise;
        return (this.Movement == e.Movement && this.Limb == e.Limb);
    }

    public bool Equals(Exercise e)
    {
        return (this.Movement == e.Movement && this.Limb == e.Limb);
    }

    public override int GetHashCode()
    {
        return String.Format("{0}{1}", (int)this.Movement, (int)this.Limb).GetHashCode();
    }
}
public class Coverage
{
    /*
    public KSGame KsGame
    {
        get
        {
            return KSGame.ByMovement(movement);
        }
    }*/
    private Movement movement;
    public Movement Movement
    {
        get
        {
            return movement;
        }
        set
        {
            movement = value;
        }
    }

    public Coverage(Movement m)
    {
        this.movement = m;
    }

    /*
    public static Dictionary<int, Coverage> ByCoverageId = new Dictionary<int, Coverage>()
    {
        {53, new Coverage(Movement.BipedElbowFlexionAndExtension, Laterality.Single)},
        {54, new Coverage(Movement.BipedElbowFlexionAndExtension, Laterality.Single)},
        {55, new Coverage(Movement.BipedElbowFlexionAndExtension, Laterality.Double)},
        {56, new Coverage(Movement.BipedElbowFlexionAndExtension, Laterality.Double)},
        {4, new Coverage(Movement.Step)},
        {5, new Coverage(Movement.Step)},
        {6, new Coverage(Movement.Step)},
        {7, new Coverage(Movement.Step)},
        {15, new Coverage(Movement.Stride)},
        {16, new Coverage(Movement.Stride)},
        {17, new Coverage(Movement.Stride)},
        {18, new Coverage(Movement.Stride)},
        {19, new Coverage(Movement.Stride)},
        {20, new Coverage(Movement.Stride)},
        {21, new Coverage(Movement.Stride)}
    };*/
}
