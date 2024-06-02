using Michsky.UI.Shift;
using UnityEngine;

public class SwitchSettingsManager : MonoBehaviour
{
    public SwitchManager[] Switches;

    void Awake()
    {
        InitializeAllSwitches();
    }

    public void InitializeAllSwitches()
    {
        if (Player.CavesSwitch) { Switches[0].isOn = true; }
        else { Switches[0].isOn = false; }
        Switches[0].AnimateSwitch(false);

        if (Player.VolcanicCaveSwitch) { Switches[1].isOn = true; }
        else { Switches[1].isOn = false; }
        Switches[1].AnimateSwitch(false);

        if (Player.IceCaveSwitch) { Switches[2].isOn = true; }
        else { Switches[2].isOn = false; }
        Switches[2].AnimateSwitch(false);

        if (Player.HiveNestSwitch) { Switches[3].isOn = true; }
        else { Switches[3].isOn = false; }
        Switches[3].AnimateSwitch(false);

        if (Player.CyberHideoutSwitch) { Switches[4].isOn = true; }
        else { Switches[4].isOn = false; }
        Switches[4].AnimateSwitch(false);

        if (Player.MissionsSwitch) { Switches[5].isOn = true; }
        else { Switches[5].isOn = false; }
        Switches[5].AnimateSwitch(false);

        if (Player.AlienBaseSwitch) { Switches[6].isOn = true; }
        else { Switches[6].isOn = false; }
        Switches[6].AnimateSwitch(false);

        if (Player.WormTunnelsSwitch) { Switches[7].isOn = true; }
        else { Switches[7].isOn = false; }
        Switches[7].AnimateSwitch(false);

        if (Player.ShipwreckSwitch) { Switches[8].isOn = true; }
        else { Switches[8].isOn = false; }
        Switches[8].AnimateSwitch(false);

        if (Player.MysticTempleSwitch) { Switches[9].isOn = true; }
        else { Switches[9].isOn = false; }
        Switches[9].AnimateSwitch(false);

        if (Player.MonstersSwitch) { Switches[10].isOn = true; }
        else { Switches[10].isOn = false; }
        Switches[10].AnimateSwitch(false);

        if (Player.XenoSpiderSwitch) { Switches[11].isOn = true; }
        else { Switches[11].isOn = false; }
        Switches[11].AnimateSwitch(false);

        if (Player.SporeBehemothSwitch) { Switches[12].isOn = true; }
        else { Switches[12].isOn = false; }
        Switches[12].AnimateSwitch(false);

        if (Player.ElectroBeastSwitch) { Switches[13].isOn = true; }
        else { Switches[13].isOn = false; }
        Switches[13].AnimateSwitch(false);

        if (Player.VoidReaperSwitch) { Switches[14].isOn = true; }
        else { Switches[14].isOn = false; }
        Switches[14].AnimateSwitch(false);

        if (Player.ResourcesDiscoverySwitch) { Switches[15].isOn = true; }
        else { Switches[15].isOn = false; }
        Switches[15].AnimateSwitch(false);

        if (Player.PlantsDiscoverySwitch) { Switches[16].isOn = true; }
        else { Switches[16].isOn = false; }
        Switches[16].AnimateSwitch(false);

        if (Player.LiquidsDiscoverySwitch) { Switches[17].isOn = true; }
        else { Switches[17].isOn = false; }
        Switches[17].AnimateSwitch(false);

        if (Player.MineralsDiscoverySwitch) { Switches[18].isOn = true; }
        else { Switches[18].isOn = false; }
        Switches[18].AnimateSwitch(false);

        if (Player.GasDiscoverySwitch) { Switches[19].isOn = true; }
        else { Switches[19].isOn = false; }
        Switches[19].AnimateSwitch(false);

        if (Player.FoamsDiscoverySwitch) { Switches[20].isOn = true; }
        else { Switches[20].isOn = false; }
        Switches[20].AnimateSwitch(false);

        if (Player.MeatDiscoverySwitch) { Switches[21].isOn = true; }
        else { Switches[21].isOn = false; }
        Switches[21].AnimateSwitch(false);

        if (Player.AnomalySwitch) { Switches[22].isOn = true; }
        else { Switches[22].isOn = false; }
        Switches[22].AnimateSwitch(false);

        if (Player.MysteryDevicesSwitch) { Switches[23].isOn = true; }
        else { Switches[23].isOn = false; }
        Switches[23].AnimateSwitch(false);

        if (Player.ItemsCollectionSwitch) { Switches[24].isOn = true; }
        else { Switches[24].isOn = false; }
        Switches[24].AnimateSwitch(false);

        if (Player.ResourcesCollectionSwitch) { Switches[25].isOn = true; }
        else { Switches[25].isOn = false; }
        Switches[25].AnimateSwitch(false);
    }
}