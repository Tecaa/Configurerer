using System;
using System.Collections.Generic;

internal static class SpeedFixer
{
    private const float NO_SET_SPEED = 1;
    private static Dictionary<Movement, float> speedMultiplier;

    private static void FillDictionaries()
    {
        speedMultiplier.Add(Movement.ExtensiónDeHombrosConFlexiónDeCodosEnBípedo_Bilateral_45, 0.5f);
        speedMultiplier.Add(Movement.FlexiónDeHombrosEnBípedoConBastón_Bilateral, 3);
    }

    static SpeedFixer()
    {
        speedMultiplier = new Dictionary<Movement, float>();
        FillDictionaries();
    }
    internal static float FixSpeed(Movement movement)
    {
        if (speedMultiplier.ContainsKey(movement))
            return speedMultiplier[movement];
        else
            return NO_SET_SPEED;
    }
}