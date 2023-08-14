using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSwitcher : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        animator.Play("Panel In");
    }

}
