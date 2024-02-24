using UnityEngine;

public class AnimationSwitcher : MonoBehaviour
{
    private Animator animator;
    private AudioManager audioManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    private void OnEnable()
    {
        animator.Play("Panel In");
        audioManager.PlayPanelInSound();
    }
}
