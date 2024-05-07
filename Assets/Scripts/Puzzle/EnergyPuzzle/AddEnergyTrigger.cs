using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEnergyTrigger : MonoBehaviour, InteractObject
{
    [SerializeField] EnergyPuzzleController energyPuzzleController;
    public void OnInteract()
    {
        if(energyPuzzleController == null)
            return;

        energyPuzzleController.AddUnUseEnergy();

        gameObject.SetActive(false);
    }
}
