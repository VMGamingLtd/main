using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityUpdater : MonoBehaviour
{
    public RefreshCurrentElectricity currentElectricity;
    public RefreshMaxElectricity maxElectricity;

    public void UpdateCurrentElectricity()
    {
        currentElectricity.enableRefreshCurrentElectricity();
    }
    public void UpdateMaxElectricity()
    {
        maxElectricity.enableRefreshMaxElectricity();
    }


}
