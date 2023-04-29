using UnityEngine;

public class LoadPlanetAnimation : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private string animationName;

    private Animator animator;

    private void Awake()
    {
        animator = mainUI.transform.Find("ButtonList/Planets").GetComponent<Animator>();
    }

    private void Start()
    {
        animator.Play(animationName);
    }
}