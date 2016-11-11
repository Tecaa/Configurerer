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
        speedMultiplier.Add(Movement.DesplazamientoLateralConPaso_75, 1.5f);
        speedMultiplier.Add(Movement.AbducciónDeCaderaEnDecúbitoLateral, 1.5f);
        speedMultiplier.Add(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_CIMA, 1.5f);
        speedMultiplier.Add(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_FRENTE, 1.5f);
        speedMultiplier.Add(Movement.FlexiónDeHombroConElongaciónCápsulaArticularEnSedente_NUCA, 1.5f);
        speedMultiplier.Add(Movement.FlexiónHorizontalResistidaDeHombros_BípedoBilateral, 2);
        speedMultiplier.Add(Movement.RotaciónDeHombrosAsistidaConBastón_DecúbitoSupino, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConAmbasManos_BrazosAbajoDerecha, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConAmbasManos_BrazosAbajoIzquierda, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConAmbasManos_BrazosArribaDerecha, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConAmbasManos_BrazosArribaIzquierda, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoDerecha, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConUnaMano_BrazoAbajoIzquierda, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConUnaMano_BrazoArribaDerecha, 1.5f);
        speedMultiplier.Add(Movement.RecogiendoYGuardandoConUnaMano_BrazoArribaIzquierda, 1.5f);
        speedMultiplier.Add(Movement.ExtensiónHorizontalDeHombrosEnSupino, 1.5f);
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