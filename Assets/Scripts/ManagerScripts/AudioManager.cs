using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField, Range(0f, 1f)]
    public float GlobalVolume;

    public AudioMixer audioMixer;
    public AudioClip hoverSound;
    public AudioClip pressedSound;
    public AudioClip panelInSound;
    public AudioClip unequipClip;
    public AudioClip energySlotClip;
    public AudioClip oxygenSlotClip;
    public AudioClip waterSlotClip;
    public AudioClip hungerSlotClip;
    public AudioClip equipClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateVolume();
    }

    private void OnValidate()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        if (audioMixer != null)
        {
            // Set the MasterVolume parameter in the AudioMixer
            audioMixer.SetFloat("Master", GlobalVolume);
        }
        else
        {
            Debug.LogWarning("AudioMixer not assigned in the Inspector!");
        }
    }

    public void UpdateMusicVolume(Slider slider)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Music", slider.value);
        }
        else
        {
            Debug.LogWarning("AudioMixer not assigned in the Inspector!");
        }
    }

    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(hoverSound, GlobalVolume);
    }
    public void PlayPressedSound()
    {
        audioSource.PlayOneShot(pressedSound, GlobalVolume);
    }
    public void PlayPanelInSound()
    {
        audioSource.PlayOneShot(panelInSound, GlobalVolume);
    }
    public void PlayerEquipSlotSound()
    {
        audioSource.PlayOneShot(equipClip, GlobalVolume);
    }
    public void PlayUnequipSlotSound()
    {
        audioSource.PlayOneShot(unequipClip, GlobalVolume);
    }
    public void PlayEnergySlotSound()
    {
        audioSource.PlayOneShot(energySlotClip, GlobalVolume);
    }
    public void PlayOxygenSlotSound()
    {
        audioSource.PlayOneShot(oxygenSlotClip, GlobalVolume);
    }
    public void PlayWaterSlotSound()
    {
        audioSource.PlayOneShot(waterSlotClip, GlobalVolume);
    }
    public void PlayHungerSlotSound()
    {
        audioSource.PlayOneShot(hungerSlotClip, GlobalVolume);
    }
}
