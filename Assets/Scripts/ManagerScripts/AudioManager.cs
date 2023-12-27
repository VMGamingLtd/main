using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip unequipClip;
    public AudioClip energySlotClip;
    public AudioClip oxygenSlotClip;
    public AudioClip waterSlotClip;
    public AudioClip hungerSlotClip;
    public AudioClip equipClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayerEquipSlotSound()
    {
        audioSource.PlayOneShot(equipClip);
    }
    public void PlayUnequipSlotSound()
    {
        audioSource.PlayOneShot(unequipClip);
    }
    public void PlayEnergySlotSound()
    {
        audioSource.PlayOneShot(energySlotClip);
    }
    public void PlayOxygenSlotSound()
    {
        audioSource.PlayOneShot(oxygenSlotClip);
    }
    public void PlayWaterSlotSound()
    {
        audioSource.PlayOneShot(waterSlotClip);
    }
    public void PlayHungerSlotSound()
    {
        audioSource.PlayOneShot(hungerSlotClip);
    }
}
