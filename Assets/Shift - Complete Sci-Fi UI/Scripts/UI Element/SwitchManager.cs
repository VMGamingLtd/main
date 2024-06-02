using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.Shift
{
    public class SwitchManager : MonoBehaviour
    {
        public bool isOn;
        public UnityEvent OnEvents;
        public UnityEvent OffEvents;
        public Animation AnimationComponent;

        void Awake()
        {
            AnimationComponent = gameObject.GetComponent<Animation>();
        }

        public void AnimateSwitch(bool overrideSettings)
        {
            if (isOn == true)
            {
                AnimationComponent.Play("SwitchOff");
                isOn = false;
                OffEvents.Invoke();
            }

            else
            {
                AnimationComponent.Play("SwitchOn");
                isOn = true;
                OnEvents.Invoke();
            }

            if (overrideSettings)
            {
                ReflectPlayerSettings();
            }

        }

        public void ReflectPlayerSettings()
        {
            if (gameObject.name == "CavesSwitch")
            {
                Player.CavesSwitch = isOn;
            }
            else if (gameObject.name == "VolcanicCaveSwitch")
            {
                Player.VolcanicCaveSwitch = isOn;
            }
            else if (gameObject.name == "IceCaveSwitch")
            {
                Player.IceCaveSwitch = isOn;
            }
            else if (gameObject.name == "HiveNestSwitch")
            {
                Player.HiveNestSwitch = isOn;
            }
            else if (gameObject.name == "CyberHideoutSwitch")
            {
                Player.CyberHideoutSwitch = isOn;
            }
            else if (gameObject.name == "MissionsSwitch")
            {
                Player.MissionsSwitch = isOn;
            }
            else if (gameObject.name == "AlienBaseSwitch")
            {
                Player.AlienBaseSwitch = isOn;
            }
            else if (gameObject.name == "WormTunnelsSwitch")
            {
                Player.WormTunnelsSwitch = isOn;
            }
            else if (gameObject.name == "ShipwreckSwitch")
            {
                Player.ShipwreckSwitch = isOn;
            }
            else if (gameObject.name == "MysticTempleSwitch")
            {
                Player.MysticTempleSwitch = isOn;
            }
            else if (gameObject.name == "MonstersSwitch")
            {
                Player.MonstersSwitch = isOn;
            }
            else if (gameObject.name == "XenoSpiderSwitch")
            {
                Player.XenoSpiderSwitch = isOn;
            }
            else if (gameObject.name == "SporeBehemothSwitch")
            {
                Player.SporeBehemothSwitch = isOn;
            }
            else if (gameObject.name == "ElectroBeast")
            {
                Player.ElectroBeastSwitch = isOn;
            }
            else if (gameObject.name == "VoidReaperSwitch")
            {
                Player.VoidReaperSwitch = isOn;
            }
            else if (gameObject.name == "ResourcesDiscoverySwitch")
            {
                Player.ResourcesDiscoverySwitch = isOn;
            }
            else if (gameObject.name == "PlantsDiscoverySwitch")
            {
                Player.PlantsDiscoverySwitch = isOn;
            }
            else if (gameObject.name == "LiquidsDiscoverySwitch")
            {
                Player.LiquidsDiscoverySwitch = isOn;
            }
            else if (gameObject.name == "MineralsDiscoverySwitch")
            {
                Player.MineralsDiscoverySwitch = isOn;
            }
            else if (gameObject.name == "GasDiscoverySwitch")
            {
                Player.GasDiscoverySwitch = isOn;
            }
            else if (gameObject.name == "FoamsDiscoverySwitch")
            {
                Player.FoamsDiscoverySwitch = isOn;
            }
            else if (gameObject.name == "MeatDiscoverySwitch")
            {
                Player.MeatDiscoverySwitch = isOn;
            }
            else if (gameObject.name == "AnomalySwitch")
            {
                Player.AnomalySwitch = isOn;
            }
            else if (gameObject.name == "MysteryDevicesSwitch")
            {
                Player.MysteryDevicesSwitch = isOn;
            }
            else if (gameObject.name == "ItemsCollectionSwitch")
            {
                Player.ItemsCollectionSwitch = isOn;
            }
            else if (gameObject.name == "ResourcesCollectionSwitch")
            {
                Player.ResourcesCollectionSwitch = isOn;
            }
        }
    }
}