using MoreMountains.CorgiEngine;
using UnityEngine;

public class ActivateDeactivateRippleEffect : MonoBehaviour
{
    public GameObject theRippleEffect;
    public GameObject theLegs;
    public bool isPlayer1;

    public void Start()
    {
        //theRippleEffect = GameObject.FindGameObjectWithTag("Player").GetComponent<AdditionalMovementSettings>().theRippleEffect; This is the 1 player version. The next line is the 2 players version.
        //theLegs = GameObject.FindGameObjectWithTag("Player").GetComponent<AdditionalMovementSettings>().theLegs; This is the 1 player version. The next line is the 2 players version.
        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            if (isPlayer1 && character.PlayerID == "Player1")
            {
                theRippleEffect = character.GetComponent<AdditionalMovementSettings>().theRippleEffect;
                theLegs = character.GetComponent<AdditionalMovementSettings>().theLegs;
                break;
            }
            if (!isPlayer1 && character.PlayerID == "Player2")
            {
                theRippleEffect = character.GetComponent<AdditionalMovementSettings>().theRippleEffect;
                theLegs = character.GetComponent<AdditionalMovementSettings>().theLegs;
                break;
            }
        }
    }


    public void Update()
    {
        
    }

    public void ActivateRippleEffect()
    {
        theRippleEffect.SetActive(true);
    }

    public void DeactivateRippleEffect()
    {
        theRippleEffect.SetActive(false);
        theLegs.SetActive(true);
    }
}
