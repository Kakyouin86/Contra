using Unity.VisualScripting;
using UnityEngine;

public class RandomAnimationDelay : MonoBehaviour
{
    public Animator theAnimator;
    public float minimum = 0.0f;
    public float maximum = 0.6f;

    void Start()
    {
        theAnimator = GetComponent<Animator>();
        Invoke("PlayRandomDelayedAnimation", Random.Range(minimum, maximum));
    }

    void PlayRandomDelayedAnimation()
    {
        // Get the current state information
        AnimatorStateInfo stateInfo = theAnimator.GetCurrentAnimatorStateInfo(0);
        // Play the current animation state
        theAnimator.Play(stateInfo.fullPathHash, 0, Random.Range(0f, 1f));
    }
}