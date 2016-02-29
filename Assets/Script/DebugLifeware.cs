using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public static class DebugLifeware {
    public static Developer? ActualDeveloper { get { return actualDeveloper; } }
    private static Developer? actualDeveloper;
    private static bool ShowMessages;
    //File must be directly in kinectsiology\KinectiologyGames\KinectsiologyGames folder.
    private static string Filename = "Lifeware_Tools.config";
    
    [Flags]
    public enum Developer
    {
        Fernando_Venegas = 1,
        Alfredo_Gallardo = 2,
        Milton_Pulgar = 4,
        Diego_Cid = 8,
        Jorge_Alviarez = 16,
        Jorge_Basualdo = 32,
        Pablo_Lopez = 64,
        Mauricio_Carpentier= 128,
        Oscar_Echeverria = 256,
        Felipe_Cid = 512,
        Marco_Rojas = 1024
    }

    static DebugLifeware()
    {
        actualDeveloper = ReadActualDeveloper();
    }

    private static Developer? ReadActualDeveloper()
    {
        Developer? dev = null;
        // Handle any problems that might arise when reading the text
         try
         {
             string line;
             // Create a new StreamReader, tell it which file to read and what encoding the file
             // was saved as
             StreamReader theReader = new StreamReader(Filename, Encoding.Default);

             // Immediately clean up the reader after this block of code is done.
             // You generally use the "using" statement for potentially memory-intensive objects
             // instead of relying on garbage collection.
             // (Do not confuse this with the using directive for namespace at the 
             // beginning of a class!)
             using (theReader)
             {
                 // While there's lines left in the text file, do this:
                 
                line = theReader.ReadLine();
                if (line != null)
                {
                    dev = (Developer)Enum.Parse(typeof(Developer), line.Replace(" ", "_"));
                }
             }
         }
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
        catch (Exception e)
        {
            Debug.Log(e.Message);
            dev = null;
        }
         
        return dev;
    }
    /// <summary>
    /// Debug log filtrador por desarrollador, se pueden concatenar varios desarrolladores. 
    /// Ejemplo: LifewareTools.Log("Prueba", LifewareTools.Developer.Marco_Rojas | LifewareTools.Developer.Alfredo_Gallardo);
    /// </summary>
    /// <param name="message"></param>
    /// <param name="logger">Desarrollador o desarrolladores concatenados.</param>
    public static void Log(object message, Developer logger)
    {
        string msg = DebugLifeware.formatMessage(message, logger);
        if (actualDeveloper.HasValue &&  (logger &  actualDeveloper.Value) != 0)
            Debug.Log(msg);
    }

    /// <summary>
    /// Precaución: Mensaje mostrado a todos los developers, usar sólo de ser muy necesario.
    /// </summary>
    /// <param name="message"></param>
    public static void LogAllDevelopers(object message)
    {
        string logg = "[ALL]";
        Debug.Log(logg + " " + message);
    }

    /// <summary>
    /// Precaución: Warning mostrado a todos los developers, usar solo en warnings necesarios.
    /// </summary>
    /// <param name="message"></param>
    public  static void LogWarningAllDevelopers(object message)
    {
        string logg = "[ALL]";
        Debug.LogWarning(logg + " " + message);
    }
    public static void LogWarning(object message, Developer logger)
    {
        string msg = DebugLifeware.formatMessage(message, logger);
        if (actualDeveloper.HasValue && (logger & actualDeveloper.Value) != 0)
            Debug.LogWarning(msg);
    }
    public static void LogError(object message, Developer logger)
    {
        string msg = DebugLifeware.formatMessage(message, logger);
        if (actualDeveloper.HasValue && (logger & actualDeveloper.Value) != 0)
            Debug.LogError(msg);
    }
    private static string formatMessage(object message, Developer logger)
    {
        string logg = "[" + logger.ToString().Replace("_", " ") + "]";
        return logg + " " + message;
    }

}
