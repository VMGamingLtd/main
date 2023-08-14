using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using BuildingManagement;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Globalization;

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
    private BuildingCycles buildingCycles;
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

    private IEnumerator UpdateUI(BuildingItemData itemData)
    {
        SystemLanguage language = Application.systemLanguage;
        buildingCycles = mainObj.GetComponent<BuildingCycles>();
        audioSource = transform.gameObject.GetComponent<AudioSource>();
        NumberFormater formatter = new NumberFormater();
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
            if (itemData.basePowerOutput > 0)
            {
                string formattedPower = formatter.FormatEnergyInThousands(itemData.powerOutput);
                powerOutput.text = formattedPower;
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
    public void ToggleProduction()
    {
        itemData = mainObj.GetComponent<BuildingItemData>();
        audioSource = transform.gameObject.GetComponent<AudioSource>();

        if (itemData.isPaused == false)
        {
            itemData = mainObj.GetComponent<BuildingItemData>();
            itemData.efficiency = 0;
            itemData.isPaused = true;
        }
        else
        {
            itemData.efficiency = itemData.efficiencySetting;
            itemData.isPaused = false;
            BuildingCycles buildingCycles = mainObj.GetComponent<BuildingCycles>();
            if (mainObj.CompareTag("Energy"))
            {
                buildingCycles.StartBuildingCycleEnergy().Forget();
            }
            else if (mainObj.CompareTag("NoConsume"))
            {
                buildingCycles.StartNoConsumeCycle().Forget();
            }
            else if (mainObj.CompareTag("Consume"))
            {
                buildingCycles.StartConsumeCycle().Forget();
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
