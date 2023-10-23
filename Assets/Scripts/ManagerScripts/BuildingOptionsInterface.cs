using BuildingManagement;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingOptionsInterface : MonoBehaviour
{
    [Header("Reactor display elements")]
    public TextMeshProUGUI efficiency;
    public TextMeshProUGUI powerOutput;
    public TextMeshProUGUI powerConsumption;
    public TextMeshProUGUI totalTime;
    public TextMeshProUGUI title;
    public TextMeshProUGUI[] consumedSlotQuantity;
    public Image[] consumedSlotImage;
    public GameObject[] consumedSlotObjects;
    public TextMeshProUGUI[] producedSlotQuantity;
    public Image[] producedSlotImage;
    public GameObject[] producedSlotObjects;
    public GameObject ConsumeModel;
    public GameObject PowerConumeModel;
    public GameObject PowerOutputModel;
    public GameObject ProductionModel;
    public Animation reactorStatus;
    public GameObject mainObj;
    public GameObject playButton;
    public GameObject pauseButton;
    public Image fillImg;
    public AudioClip reactorAudioClip;
    private bool isUpdatingUI = false;
    private BuildingItemData itemData;
    private EnergyBuildingItemData itemDataEnergy;
    private BuildingCycles buildingCycles;
    private EnergyBuildingCycles buildingCyclesEnergy;
    private AudioSource audioSource;

    public void StartUpdatingUI(BuildingItemData itemData, GameObject refObj)
    {
        if (!isUpdatingUI)
        {
            isUpdatingUI = true;
            gameObject.SetActive(true);
            mainObj = refObj;
            StartCoroutine(UpdateUI(itemData));
        }
    }
    public void StartUpdatingEnergyUI(EnergyBuildingItemData itemData, GameObject refObj)
    {
        if (!isUpdatingUI)
        {
            isUpdatingUI = true;
            gameObject.SetActive(true);
            mainObj = refObj;
            StartCoroutine(UpdateEnergyUI(itemData));
        }
    }

    private IEnumerator UpdateUI(BuildingItemData itemData)
    {
        buildingCycles = mainObj.GetComponent<BuildingCycles>();
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        NumberFormater formatter = new();
        audioSource.clip = reactorAudioClip;
        title.text = itemData.buildingName;
        bool audioPlaying = false;

        if (itemData.powerConsumption > 0)
        {
            PowerConumeModel.SetActive(true);
        }
        else
        {
            PowerConumeModel.SetActive(false);
        }

        int consumedSlots = itemData.consumedSlotCount;
        if (consumedSlots > 0)
        {
            ConsumeModel.SetActive(true);
            for (int i = 0; i < consumedSlotObjects.Length; i++)
            {
                if (i < consumedSlots)
                {
                    consumedSlotObjects[i].SetActive(true);
                    var item = itemData.consumedItems[i];
                    consumedSlotImage[i].sprite = AssignSpriteToSlot(item.itemName);
                    consumedSlotQuantity[i].text = itemData.consumedItems[i].quantity.ToString();
                }
                else
                {
                    consumedSlotObjects[i].SetActive(false); // Deactivate the rest of the consumedSlotObjects
                }
            }
        }
        else
        {
            ConsumeModel.SetActive(false);
        }
        int producedSlots = itemData.producedSlotCount;
        if (producedSlots > 0)
        {
            ProductionModel.SetActive(true);
            for (int i = 0; i < producedSlotObjects.Length; i++)
            {
                if (i < producedSlots)
                {
                    producedSlotObjects[i].SetActive(true);
                    var item = itemData.producedItems[i];
                    producedSlotImage[i].sprite = AssignSpriteToSlot(item.itemName);
                }
                else
                {
                    producedSlotObjects[i].SetActive(false); // Deactivate the rest of the consumedSlotObjects
                }
            }
        }
        else
        {
            ProductionModel.SetActive(false);
        }

        while (isUpdatingUI)
        {
            efficiency.text = itemData.efficiencySetting.ToString() + '%';
            string formattedTime = formatter.FormatTimeInTens(itemData.totalTime);
            totalTime.text = formattedTime;

            if (consumedSlots > 0)
                for (int i = 0; i < consumedSlotQuantity.Length; i++)
                {
                    if (i < consumedSlots)
                    {
                        consumedSlotQuantity[i].text = itemData.consumedItems[i].quantity.ToString("F2", CultureInfo.InvariantCulture);
                    }
                }

            if (producedSlots > 0)
            {
                for (int i = 0; i < producedSlotQuantity.Length; i++)
                {
                    if (i < producedSlots)
                    {
                        producedSlotQuantity[i].text = itemData.producedItems[i].quantity.ToString("F2", CultureInfo.InvariantCulture);
                    }

                }
            }
            if (itemData.powerConsumption > 0)
            {
                string formattedPower = formatter.FormatEnergyInThousands(itemData.powerConsumption);
                powerConsumption.text = formattedPower;
            }
            if (itemData.isPaused == true)
            {
                pauseButton.SetActive(false);
                playButton.SetActive(true);
                audioPlaying = false;
                audioSource.Stop();
                reactorStatus.Play("ReactorOff");
            }
            else if (itemData.efficiency > 0)
            {
                if (!audioPlaying)
                {
                    audioSource.Play();
                    audioPlaying = true;
                }
                pauseButton.SetActive(true);
                playButton.SetActive(false);
                fillImg.fillAmount = buildingCycles.currentFillAmount;
                reactorStatus.Play("ReactorOn");
                float targetSpeed = itemData.efficiencySetting / 100f;
                reactorStatus["ReactorOn"].speed = targetSpeed;
                float adjustedPitch = 1f + targetSpeed;
                audioSource.pitch = adjustedPitch;
            }
            else
            {
                pauseButton.SetActive(true);
                playButton.SetActive(false);
                audioPlaying = false;
                audioSource.Stop();
                reactorStatus.Play("ReactorOff");
                fillImg.fillAmount = 0f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator UpdateEnergyUI(EnergyBuildingItemData itemData)
    {
        buildingCyclesEnergy = mainObj.GetComponent<EnergyBuildingCycles>();
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        NumberFormater formatter = new();
        audioSource.clip = reactorAudioClip;
        title.text = itemData.buildingName;
        bool audioPlaying = false;

        if (itemData.basePowerOutput > 0)
        {
            PowerOutputModel.SetActive(true);
        }
        else
        {
            PowerOutputModel.SetActive(false);
        }

        int consumedSlots = itemData.consumedSlotCount;
        if (consumedSlots > 0)
        {
            ConsumeModel.SetActive(true);
            for (int i = 0; i < consumedSlotObjects.Length; i++)
            {
                if (i < consumedSlots)
                {
                    consumedSlotObjects[i].SetActive(true);
                    var item = itemData.consumedItems[i];
                    consumedSlotImage[i].sprite = AssignSpriteToSlot(item.itemName);
                    consumedSlotQuantity[i].text = itemData.consumedItems[i].quantity.ToString();
                }
                else
                {
                    consumedSlotObjects[i].SetActive(false); // Deactivate the rest of the consumedSlotObjects
                }
            }
        }
        else
        {
            ConsumeModel.SetActive(false);
        }
        ProductionModel.SetActive(false);
        while (isUpdatingUI)
        {
            efficiency.text = itemData.efficiencySetting.ToString() + '%';
            string formattedTime = formatter.FormatTimeInTens(itemData.totalTime);
            totalTime.text = formattedTime;

            if (consumedSlots > 0)
                for (int i = 0; i < consumedSlotQuantity.Length; i++)
                {
                    if (i < consumedSlots)
                    {
                        consumedSlotQuantity[i].text = itemData.consumedItems[i].quantity.ToString("F2", CultureInfo.InvariantCulture);
                    }
                }

            if (itemData.basePowerOutput > 0)
            {
                string formattedPower = formatter.FormatEnergyInThousands(itemData.powerOutput);
                powerOutput.text = formattedPower;
            }
            if (itemData.isPaused == true)
            {
                pauseButton.SetActive(false);
                playButton.SetActive(true);
                audioPlaying = false;
                audioSource.Stop();
                reactorStatus.Play("ReactorOff");
            }
            else if (itemData.efficiency > 0)
            {
                if (!audioPlaying)
                {
                    audioSource.Play();
                    audioPlaying = true;
                }
                pauseButton.SetActive(true);
                playButton.SetActive(false);
                fillImg.fillAmount = buildingCyclesEnergy.currentFillAmount;
                reactorStatus.Play("ReactorOn");
                float targetSpeed = itemData.efficiencySetting / 100f;
                reactorStatus["ReactorOn"].speed = targetSpeed;
                float adjustedPitch = 1f + targetSpeed;
                audioSource.pitch = adjustedPitch;
            }
            else
            {
                pauseButton.SetActive(true);
                playButton.SetActive(false);
                audioPlaying = false;
                audioSource.Stop();
                reactorStatus.Play("ReactorOff");
                fillImg.fillAmount = 0f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void ToggleProduction()
    {
        itemData = mainObj.GetComponent<BuildingItemData>();
        itemDataEnergy = mainObj.GetComponent<EnergyBuildingItemData>();
        audioSource = transform.gameObject.GetComponent<AudioSource>();

        // check if the building is running, then stop it
        if (itemData != null && itemData.isPaused == false)
        {
            itemData = mainObj.GetComponent<BuildingItemData>();
            itemData.efficiency = 0;
            itemData.isPaused = true;
        }
        else if (itemDataEnergy != null && itemDataEnergy.isPaused == false)
        {
            itemDataEnergy = mainObj.GetComponent<EnergyBuildingItemData>();
            itemDataEnergy.efficiency = 0;
            itemDataEnergy.isPaused = true;
        }
        else
        {
            // if the building is already stopped, then start the building back
            if (mainObj.CompareTag("Energy"))
            {
                EnergyBuildingCycles buildingCyclesEnergy = mainObj.GetComponent<EnergyBuildingCycles>();
                itemDataEnergy.efficiency = itemDataEnergy.efficiencySetting;
                itemDataEnergy.isPaused = false;
                _ = buildingCyclesEnergy.Start();
            }
            else if (mainObj.CompareTag("NoConsume"))
            {
                BuildingCycles buildingCycles = mainObj.GetComponent<BuildingCycles>();
                itemData.efficiency = itemData.efficiencySetting;
                itemData.isPaused = false;
                _ = buildingCycles.Start();
            }
            else if (mainObj.CompareTag("Consume"))
            {
                BuildingCycles buildingCycles = mainObj.GetComponent<BuildingCycles>();
                itemData.efficiency = itemData.efficiencySetting;
                itemData.isPaused = false;
                _ = buildingCycles.Start();
            }
        }

    }

    private Sprite AssignSpriteToSlot(string spriteName)
    {
        Sprite sprite = AssetBundleManager.LoadAssetFromBundle<Sprite>("resourceicons", spriteName);
        return sprite;
    }

    public void StopUpdatingUI()
    {
        isUpdatingUI = false;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopUpdatingUI();
    }
}
