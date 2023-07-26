using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using BuildingManagement;
using System.Collections;
using Cysharp.Threading.Tasks;

public class BuildingOptionsInterface : MonoBehaviour
{
    [Header("Reactor display elements")]
    public TextMeshProUGUI efficiency;
    public TextMeshProUGUI powerOutput;
    public TextMeshProUGUI totalTime;
    public TextMeshProUGUI[] consumedSlotQuantity;
    public Image[] consumedSlotImage;
    public GameObject[] consumedSlotObjects;
    public Animation reactorStatus;
    public GameObject mainObj;
    public GameObject playButton;
    public GameObject pauseButton;
    public Image fillImg;
    private bool isUpdatingUI = false;
    private BuildingItemData itemData;
    private BuildingCycles buildingCycles;
    private AudioSource audioSource;
    public AudioClip reactorAudioClip;
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
        bool audioPlaying = false;

        int consumedSlots = itemData.consumedSlotCount;
        for (int i = 0; i < consumedSlotObjects.Length; i++)
        {
            if (i < consumedSlots)
            {
                consumedSlotObjects[i].SetActive(true);
                var consumedItem = itemData.consumedItems[i];
                consumedSlotImage[i].sprite = AssignSpriteToSlot(consumedItem.itemName);
            }
            else
            {
                consumedSlotObjects[i].SetActive(false); // Deactivate the rest of the consumedSlotObjects
            }
        }


        while (isUpdatingUI)
        {
            efficiency.text = itemData.efficiencySetting.ToString() + '%';
            string formattedPower = formatter.FormatEnergyInThousands(itemData.powerOutput);
            powerOutput.text = formattedPower;
            string formattedTime = formatter.FormatTimeInTens(itemData.totalTime);
            totalTime.text = formattedTime;

            for (int i = 0; i < consumedSlots; i++)
            {
                consumedSlotQuantity[i].text = itemData.consumedItems[i].quantity.ToString();
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
                    pauseButton.SetActive(true);
                    playButton.SetActive(false);
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
                pauseButton.SetActive(false);
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
            BuildingCycles buildingCycles = mainObj.GetComponent<BuildingCycles>();
            if (mainObj.layer == LayerMask.NameToLayer("Energy"))
            {
                buildingCycles.StartBuildingCycleEnergy().Forget();
            }
            else if (mainObj.layer == LayerMask.NameToLayer("NoConsume"))
            {
                buildingCycles.StartNoConsumeCycle().Forget();
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
