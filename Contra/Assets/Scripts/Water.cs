using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject theSplashIntoTheWater;
    public GameObject theSplashOutOfTheWater;
    public Vector3 thePosition;
    public Vector3 yOffsetIntoTheWater;
    public Vector3 yOffsetOutOfTheWater;
    public GameObject theLegs;
    public GameObject theRippleEffect;
    public bool isPlayerInWater = false; // Flag to track whether the player is in water
    public CharacterHorizontalMovement characterHorizontalMovement;
    public CharacterCrouch characterCrouch;
    public CharacterRoll characterRoll;

    void Start()
    {
        characterHorizontalMovement = GetComponentInParent<CharacterHorizontalMovement>();
        characterCrouch = GetComponentInParent<CharacterCrouch>();
        characterRoll = GetComponentInParent<CharacterRoll>();
    }

    void Update()
    {
        if (!isPlayerInWater)
        {
            theRippleEffect.SetActive(false);
            characterCrouch.AbilityPermitted = true;
            characterRoll.AbilityPermitted = true;
            theLegs.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isPlayerInWater = true;
            characterHorizontalMovement.AbilityMovementSpeedMultiplier = 0f;
            StartCoroutine(DisableControls());
            characterCrouch.AbilityPermitted = false;
            characterRoll.AbilityPermitted = false;
            theLegs.SetActive(false);
            // theRippleEffect.SetActive(true);
            thePosition = new Vector3(transform.position.x, transform.position.y,
                other.transform.position.z);
            Instantiate(theSplashIntoTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffsetIntoTheWater.y), transform.rotation);
        }
    }

    IEnumerator DisableControls()
    {
        //This makes a short stop upon entering on the water
        yield return new WaitForSeconds(0.05f);
        characterHorizontalMovement.AbilityMovementSpeedMultiplier = 1f;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isPlayerInWater = false;
            characterCrouch.AbilityPermitted = true;
            characterRoll.AbilityPermitted = true;
            theLegs.SetActive(true);
            theRippleEffect.SetActive(false);
            thePosition = new Vector3(transform.position.x, transform.position.y,
                other.transform.position.z);
            Instantiate(theSplashOutOfTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffsetOutOfTheWater.y), transform.rotation);
        }
    }
}