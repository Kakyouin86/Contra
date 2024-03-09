using System.Collections;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Character = MoreMountains.CorgiEngine.Character;

public class Water : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public GameObject theSplashIntoTheWater;
    public GameObject theSplashOutOfTheWater;
    public Vector3 thePosition;
    public Vector3 yOffsetIntoTheWater;
    public Vector3 yOffsetOutOfTheWater;
    public GameObject theTorso;
    public GameObject theLegs;
    public GameObject theRippleEffect;
    public bool isPlayerInWater = false;
    public float exitThreshold = 6f;
    public float distance;
    public CharacterHorizontalMovement characterHorizontalMovement;
    public CharacterCrouch characterCrouch;
    public CharacterRoll characterRoll;
    public UIAndUpgradesController theUIController;
    public Character character;
    public bool runFirstBoxColliderOff = false;

    protected virtual void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }

    /// <summary>
    /// OnDisable, we stop listening to events.
    /// </summary>
    protected virtual void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public void OnMMEvent(CorgiEngineEvent corgiEngineEvent)
    {
        if (corgiEngineEvent.EventType == CorgiEngineEventTypes.Respawn)
        {
            isPlayerInWater = false;
            characterCrouch.AbilityPermitted = true;
            GetComponent<BoxCollider2D>().enabled = true;
            runFirstBoxColliderOff = false;
            if (theUIController.GetComponent<UIAndUpgradesController>().dash)
            {
                characterRoll.AbilityPermitted = true;
            }
            theLegs.SetActive(true);
            theRippleEffect.SetActive(false);
            theTorso.GetComponent<SpriteRenderer>().enabled = true;
            theLegs.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void Start()
    {
        characterHorizontalMovement = GetComponentInParent<CharacterHorizontalMovement>();
        characterCrouch = GetComponentInParent<CharacterCrouch>();
        characterRoll = GetComponentInParent<CharacterRoll>();
        theUIController = GameObject.FindWithTag("UIPlayer1").GetComponent<UIAndUpgradesController>();
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    void Update()
    {
        if (!isPlayerInWater && character.ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead)
        {
            theRippleEffect.SetActive(false);
            characterCrouch.AbilityPermitted = true;
            if (theUIController.GetComponent<UIAndUpgradesController>().dash)
            {
                characterRoll.AbilityPermitted = true;
            }
            theLegs.SetActive(true);
        }

        if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
        {
            theTorso.GetComponent<SpriteRenderer>().enabled = false;
            theLegs.GetComponent<SpriteRenderer>().enabled = true;
            isPlayerInWater = false;
            if (!runFirstBoxColliderOff)
            {
                theLegs.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(BringBackTheCollider());
            }
        }
    }

    IEnumerator BringBackTheCollider()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().enabled = true;
        runFirstBoxColliderOff = true;
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isPlayerInWater = true;
            characterHorizontalMovement.AbilityMovementSpeedMultiplier = 0f;
            StartCoroutine(DisableControls());
            characterCrouch.AbilityPermitted = false;
            characterRoll.AbilityPermitted = false;
            theLegs.SetActive(false);
            theRippleEffect.SetActive(true);
            thePosition = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
            Instantiate(theSplashIntoTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffsetIntoTheWater.y), transform.rotation);
        }
    }

    IEnumerator DisableControls()
    {
        //This makes a short stop upon entering on the water
        yield return new WaitForSeconds(0.05f);
        characterHorizontalMovement.AbilityMovementSpeedMultiplier = 1f;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            distance = Vector2.Distance(transform.position, other.transform.position);
            isPlayerInWater = false;
            characterCrouch.AbilityPermitted = true;
            if (theUIController.GetComponent<UIAndUpgradesController>().dash)
            {
                characterRoll.AbilityPermitted = true;
            }
            theLegs.SetActive(true);
            theRippleEffect.SetActive(false);
            thePosition = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
            if (distance < exitThreshold)
            {
                Instantiate(theSplashOutOfTheWater, new Vector3(thePosition.x, other.transform.position.y + yOffsetOutOfTheWater.y), transform.rotation);
            }
        }
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