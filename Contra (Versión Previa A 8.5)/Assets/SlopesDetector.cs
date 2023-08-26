using MoreMountains.CorgiEngine;
using UnityEngine;


public class SlopesDetector : MonoBehaviour
{
    public Animator theAnimator;
    public Character character;
    public CorgiController theController;
    void Start()
    {
        character = GetComponentInParent<Character>();
        theAnimator = GetComponentInParent<Animator>();
        theController = FindObjectOfType<MoreMountains.CorgiEngine.CorgiController>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "UpwardSlope" && theController.State.IsGrounded)
        {
            if (character.IsFacingRight)
            {
                theAnimator.SetBool("UpwardSlope", true);
                theAnimator.SetBool("DownwardSlope", false);
            }
            else
            {
                theAnimator.SetBool("UpwardSlope", false);
                theAnimator.SetBool("DownwardSlope", true);
            }
        }
        else
        {
            theAnimator.SetBool("UpwardSlope", false);
            theAnimator.SetBool("DownwardSlope", false);
        }
    }
}
