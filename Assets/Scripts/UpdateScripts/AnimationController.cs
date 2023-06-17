using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject myObject;
    private Animation myAnimation;

    private void Start()
    {
        // Assign the GameObject and Animation references
        myAnimation = GetComponent<Animation>();

        // Start the animation
        myAnimation.Play();

        // Subscribe to the animation event
        AnimationClip clip = myAnimation.GetClip("LevelUp");
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = "AnimationFinished";
        animationEvent.time = myAnimation.clip.length;
        myAnimation.clip.AddEvent(animationEvent);
    }

    private void AnimationFinished()
    {
        myObject.SetActive(false);
    }
}
