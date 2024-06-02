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
        if (gameObject.name == "ExplorationMenu")
        {
            PlanetControl.isPlayerInExplorationMenu = true;
        }
        animator.Play("Panel In");
        audioManager.PlayPanelInSound();
    }

    private void OnDisable()
    {
        if (gameObject.name == "ExplorationMenu")
        {
            PlanetControl.isPlayerInExplorationMenu = false;
        }
    }
}
