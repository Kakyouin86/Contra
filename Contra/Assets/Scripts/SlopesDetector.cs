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
        //theAnimator = GetComponentInParent<Animator>();
        //theController = FindObjectOfType<MoreMountains.CorgiEngine.CorgiController>();
        theController = GetComponentInParent<CorgiController>();
    }

    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "UpwardSlope")
        {
            //Debug.Log("Estoy en el Upward Slope");
            //This is because the default 5 in the offset will make the character jump too high when in between the platform and slope.
            //character.GetComponent<CorgiController>().StickToSlopesOffsetY = 5f;
            //character.GetComponent<CorgiController>().StickToSlopes = true;
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

        else if (other.gameObject.tag == "DownwardSlope")
        {
            //Debug.Log("Estoy en el Downward Slope");
            //character.GetComponent<CorgiController>().StickToSlopes = true;
            //character.GetComponent<CorgiController>().StickToSlopesOffsetY = 5f;
            //character.GetComponent<CorgiController>().DefaultParameters.Gravity = -35;
            if (character.IsFacingRight)
            {
                theAnimator.SetBool("UpwardSlope", false);
                theAnimator.SetBool("DownwardSlope", true);
            }
            else
            {
                theAnimator.SetBool("UpwardSlope", true);
                theAnimator.SetBool("DownwardSlope", false);
            }
        }

        else
        {
            //Debug.Log("Me fui del Downward Slope");
            //character.GetComponent<CorgiController>().DefaultParameters.Gravity = -30;
            theAnimator.SetBool("UpwardSlope", false);
            theAnimator.SetBool("DownwardSlope", false);
            //character.GetComponent<CorgiController>().StickToSlopes = false;
            //character.GetComponent<CorgiController>().StickToSlopesOffsetY = 0f;
        }
    }

   void OnTriggerExit2D(Collider2D other)
    {
        //character.GetComponent<CorgiController>().DefaultParameters.Gravity = -30;
    }
}
