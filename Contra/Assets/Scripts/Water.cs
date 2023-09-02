using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;
using InputManager = MoreMountains.CorgiEngine.InputManager;
using MMConsole = MoreMountains.Tools.MMConsole;

public class Water : MonoBehaviour
{
    public GameObject theSplashIntoTheWater;
    public GameObject theSplashOutOfTheWater;
    public Vector3 thePosition;
    public Vector3 yOffset;
    public GameObject theLegs;
    public GameObject theRippleEffect;

    void Start()
    {

    }

    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            theLegs.SetActive(false);
            //theRippleEffect.SetActive(true);
            thePosition = new Vector3(transform.position.x, transform.position.y,
                other.transform.position.z);
            Instantiate(theSplashIntoTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffset.y), transform.rotation);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        theLegs.SetActive(true);
        theRippleEffect.SetActive(false);
        if (other.tag == "Water")
        {
            thePosition = new Vector3(transform.position.x, transform.position.y,
                other.transform.position.z);
            Instantiate(theSplashOutOfTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffset.y), transform.rotation);
        }
    }
}
