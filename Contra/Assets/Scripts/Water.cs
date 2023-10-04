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

    void Start()
    {

    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isPlayerInWater = true; // Set the flag to true when player enters water
            GetComponentInParent<CharacterCrouch>().AbilityPermitted = false;
            theLegs.SetActive(false);
            // theRippleEffect.SetActive(true);
            thePosition = new Vector3(transform.position.x, transform.position.y,
                other.transform.position.z);
            Instantiate(theSplashIntoTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffsetIntoTheWater.y), transform.rotation);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isPlayerInWater = false; // Set the flag to false when player exits water
            GetComponentInParent<CharacterCrouch>().AbilityPermitted = true;
            theLegs.SetActive(true);
            theRippleEffect.SetActive(false);
            thePosition = new Vector3(transform.position.x, transform.position.y,
                other.transform.position.z);
            Instantiate(theSplashOutOfTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffsetOutOfTheWater.y), transform.rotation);
        }
    }
}