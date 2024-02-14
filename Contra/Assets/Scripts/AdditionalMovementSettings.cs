using System.Collections;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
using InputManager = MoreMountains.CorgiEngine.InputManager;
using UnityEngine.SocialPlatforms;
using Unity.VisualScripting;
using UnityEngine.WSA;
using MoreMountains.InventoryEngine;
using static MoreMountains.InventoryEngine.Inventory;
using System.Collections.Generic;
using System.Linq;

public class AdditionalMovementSettings : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public Player player;
    public Character character;
    public CharacterHorizontalMovement horizontalMovementCorgi;
    public CorgiController theController;
    public InputManager theInputManager;
    public CharacterHandleWeapon theCharacterHandleWeapon;
    public SpecialShootAndRaycastVisualization theSpecialShootAndRaycastVisualization;
    public Animator theAnimator;
    public GameObject theFirepoint;
    public WeaponAim weaponAim;
    public OverridesInAnimator theOverridesInAnimator;
    public bool horizontalLadder = false;
    public bool verticalLadder = false;
    public bool canNotDettach = false;
    public BoxCollider2D theBCTrigger;
    public Vector2 theOriginalBoxCollider2DSize;
    public Vector2 theOriginalBoxCollider2DOffset;
    public float jumpingOffsetX = 0f;
    public float jumpingOffsetY = 1.75f;
    public float jumpingSizeX = 1f;
    public float jumpingSizeY = 1.45f;
    public bool isNotCharacterV3 = false;
    public GameObject theRippleEffect;
    public GameObject theTorso;
    public GameObject theLegs;
    public GameObject theTorsoMachineGunLights;
    public GameObject theTorsoFlameGunLights;
    public GameObject theBCLadder;
    public GameObject slopesDetector;
    //public float originalRollDuration;
    public Material originalMaterial;
    public Material flashMaterial;
    public bool characterDead = false;
    public float chargingTimerForRayGun = 1f;
    public GameObject grenadesFillerRegular;
    public GameObject grenadesFillerUpgraded;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Start()
    {
        horizontalMovementCorgi = GetComponent<CharacterHorizontalMovement>();
        theInputManager = FindObjectOfType<InputManager>();
        theCharacterHandleWeapon = GetComponent<CharacterHandleWeapon>();
        theSpecialShootAndRaycastVisualization = GetComponent<SpecialShootAndRaycastVisualization>();
        character = GetComponent<Character>();
        theFirepoint = GameObject.FindWithTag("Firepoint");
        slopesDetector = GameObject.FindWithTag("SlopesDetector");
        theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>();
        theOverridesInAnimator = GetComponent<OverridesInAnimator>();
        theBCTrigger = GetComponent<BoxCollider2D>();
        theController.State.JustGotGrounded = true;
        //weaponAim = GameObject.FindWithTag("WeaponAim").GetComponent<WeaponAim>();
        theOriginalBoxCollider2DSize = new Vector3(theBCTrigger.size.x, theBCTrigger.size.y);
        theOriginalBoxCollider2DOffset = new Vector3(theBCTrigger.offset.x, theBCTrigger.offset.y);
        //originalRollDuration = GetComponent<CharacterRoll>().RollDuration;
        //theTorso.GetComponent<FlashSprites>().totalFlickerDuration = originalRollDuration;
        //theLegs.GetComponent<FlashSprites>().totalFlickerDuration = originalRollDuration;
        theTorso = GameObject.FindWithTag("Torso");
        theLegs = GameObject.FindWithTag("Legs");
        theTorsoMachineGunLights = GameObject.FindWithTag("MachineGunLights");
        theTorsoFlameGunLights = GameObject.FindWithTag("FlameGunLights");
        originalMaterial = theTorso.GetComponent<SpriteRenderer>().material;
    }

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
        if (corgiEngineEvent.EventType == CorgiEngineEventTypes.PlayerDeath)
        {
            //theAnimator.SetBool("Death",true);
            //theTorso.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            theAnimator.SetBool("Death", false);
            theAnimator.SetBool("Death Left", false);
            theAnimator.SetBool("Death Flat", false);
            theAnimator.SetBool("Death Left Flat", false);
            characterDead = false;
            //Physics2D.IgnoreLayerCollision(gameObject.layer, theLayerMaskToExcludeAfterDeath, false);
        }

        if (corgiEngineEvent.EventType == CorgiEngineEventTypes.Respawn)
        {
            StartCoroutine(DeathColliders());
            GameObject grenades = GameObject.FindGameObjectWithTag("Grenade");

            if (grenades != null)
            {
                Inventory theInventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
                int grenadeCount = 0;
                int occupiedSlotsCount = 0;

                // Count the number of grenades and occupied slots
                for (int i = 0; i < theInventory.Content.Length; i++)
                {
                    InventoryItem item = theInventory.Content[i];
                    if (item != null)
                    {
                        occupiedSlotsCount++;
                        if (item.ItemName == "Grenade")
                        {
                            grenadeCount++;
                        }
                    }
                }

                // Check if the number of grenades is exactly 5
                if (grenadeCount != 5)
                {
                    // Clear slots with "Grenade" if there are any
                    for (int i = 0; i < theInventory.Content.Length; i++)
                    {
                        if (theInventory.Content[i]?.ItemName == "Grenade")
                        {
                            theInventory.Content[i] = null;
                        }
                    }
                    // Instantiate appropriate filler based on UI state
                    UIAndUpgradesController theUIAndUpgradesController = GameObject.FindGameObjectWithTag("UI").GetComponent<UIAndUpgradesController>();
                    if (theUIAndUpgradesController != null && !theUIAndUpgradesController.grenadePlus)
                    {
                        Instantiate(grenadesFillerRegular, transform.position, transform.rotation);
                    }
                    else
                    {
                        Instantiate(grenadesFillerUpgraded, transform.position, transform.rotation);
                    }
                }
            }
        }
    }

    IEnumerator DeathColliders()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, 12, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 13, true);
        // Flash effect for 3 seconds
        float flashDuration = 0.1f; // Duration for each flash
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            // Toggle sprite renderer colors to create a flash effect
            ToggleSpriteRendererColor(theTorsoMachineGunLights.GetComponent<SpriteRenderer>());
            ToggleSpriteRendererColor(theTorsoFlameGunLights.GetComponent<SpriteRenderer>());
            ToggleSpriteRendererColor(theTorso.GetComponent<SpriteRenderer>());
            ToggleSpriteRendererColor(theLegs.GetComponent<SpriteRenderer>());

            // Wait for a short duration before the next flash
            yield return new WaitForSeconds(flashDuration);

            elapsedTime += flashDuration;
        }

        // Reset the sprite renderer colors at the end of the flashing duration
        ResetSpriteRendererColor(theTorsoMachineGunLights.GetComponent<SpriteRenderer>());
        ResetSpriteRendererColor(theTorsoFlameGunLights.GetComponent<SpriteRenderer>());
        ResetSpriteRendererColor(theTorso.GetComponent<SpriteRenderer>());
        ResetSpriteRendererColor(theLegs.GetComponent<SpriteRenderer>());
        Physics2D.IgnoreLayerCollision(gameObject.layer, 12, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, 13, false);
    }

    public void ToggleSpriteRendererColor(SpriteRenderer spriteRenderer)
    {
        // Toggle the sprite renderer color (example: toggle between red and white)
        // spriteRenderer.color = spriteRenderer.color == Color.red ? Color.white : Color.red;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a == 1.0f ? 0.5f : 1.0f);
    }

    public void ResetSpriteRendererColor(SpriteRenderer spriteRenderer)
    {
        // Reset the sprite renderer color to its original color (e.g., white)
        // spriteRenderer.color = Color.white;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
    }

    void Update()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Just the "Hold".
        if (player.GetButton(("HoldPosition")))
        {
            theAnimator.SetBool("Hold", true);
        }

        else
        {
            theAnimator.SetBool("Hold", false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //If I'm in the ladder and shooting, then I'm holding the "Hold Position". Now I can't move.

        /*if (character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing && theAnimator.GetBool("isShooting"))
        {
            theAnimator.SetBool("Hold", true);
        }*/

        if (character.MovementState.CurrentState != CharacterStates.MovementStates.LadderClimbing)
        {
            verticalLadder = false;
        }

        if (character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing && theAnimator.GetBool("isShooting"))
        {
            if (theAnimator.GetBool("Charging"))
            {
                theAnimator.SetBool("Hold", false);
            }
            else
            {
                theAnimator.SetBool("Hold", true);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Makes the collider smaller when jumping.
        if (theController.State.IsJumping)
        {
            if (!isNotCharacterV3)
            {
                theBCTrigger.offset = new Vector2(0, 1.3f);
                theBCTrigger.size = new Vector2(1, 1.8f);
            }

            else
            {
                theBCTrigger.offset = new Vector2(jumpingOffsetX, jumpingOffsetY);
                theBCTrigger.size = new Vector2(jumpingSizeX, jumpingSizeY);
            }
        }
        else
        {
            theBCTrigger.offset = new Vector2(theOriginalBoxCollider2DOffset.x, theOriginalBoxCollider2DOffset.y);
            theBCTrigger.size = new Vector2(theOriginalBoxCollider2DSize.x, theOriginalBoxCollider2DSize.y);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes the Hold position impossible to move and aim without walking.
        if (theController.State.IsGrounded && player.GetButton(("HoldPosition")) && !theController.State.IsJumping && character.MovementState.CurrentState != CharacterStates.MovementStates.Rolling)
        {
            theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
            theController.SetHorizontalForce(0);
            theController.SetVerticalForce(0);
            //theAnimator.SetBool("Hold", true);
            theAnimator.SetBool("Walking", false);
            if (!slopesDetector.GetComponent<Water>().isPlayerInWater)
            {
                //theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = false;
                weaponAim.IgnoreDownWhenGrounded = false;
            }
            else
            {
                //theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = true;
                weaponAim.IgnoreDownWhenGrounded = true;
            }
        }
        else
        {
            horizontalMovementCorgi.AbilityPermitted = true;
            theController.State.JustGotGrounded = true;
            //theAnimator.SetBool("Hold", false);
            theAnimator.SetBool("Walking", false);
            //theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = true;
            weaponAim.IgnoreDownWhenGrounded = true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes the "ShootCrouch" bool to happen.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crouching && theAnimator.GetBool("isShooting"))
        {
            theAnimator.SetBool("ShootCrouch", true);
        }
        else
        {
            theAnimator.SetBool("ShootCrouch", false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes that, when crouching, you can fire diagonally.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crawling)
        {
            //theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = false;
            weaponAim.IgnoreDownWhenGrounded = false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes that, when climbing a ladder, you stop if you are shooting or not. Original climbing speed: 2. Had to modify in CharacterLadder this:
        //_condition.ChangeState(CharacterStates.CharacterConditions.ControlledMovement)

        if (character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing)
        {
            if (!horizontalLadder)
            {
                horizontalMovementCorgi.AbilityPermitted = false;
                verticalLadder = true;
            }
            CharacterLadder characterLadder = GetComponent<CharacterLadder>();
            bool isWeaponIdle = theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle;
            bool isWeaponUse = theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse;
            bool isWeaponDelay = theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponDelayBetweenUses;
            bool isWeaponStop = theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponStop;
            bool isWeaponCooldown = theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponInCooldown;
            bool isHoldPosition = player.GetButton("HoldPosition");
            bool isShootingTheSpecialShoot = theSpecialShootAndRaycastVisualization.isShooting;
            bool isCharging = theAnimator.GetBool("Charging");
            bool chargingPause = theAnimator.GetBool("ChargingPause");
            //Debug.Log(theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState);
            //Debug.Log(chargingTimerForRayGun);

            if (isCharging && chargingTimerForRayGun > 0)
            {
                chargingTimerForRayGun -= Time.deltaTime;
            }

            if (!isCharging || isWeaponCooldown)
            {
                chargingTimerForRayGun = 0.1f;
            }
            
            if (isWeaponIdle)
            {
                if (isHoldPosition || isShootingTheSpecialShoot)
                {
                    characterLadder.LadderClimbingSpeed = 0f;
                    if (theOverridesInAnimator.rayGun)
                    {
                        theAnimator.SetBool("ChargingPause", true);
                    }
                    else
                    {
                        theAnimator.SetBool("ChargingPause", false);
                    }
                }
                else
                {
                    characterLadder.LadderClimbingSpeed = 5f;
                    theAnimator.SetBool("ChargingPause", false);
                }
            }

            else
            {
                if ((isWeaponDelay || isWeaponUse) && isCharging && chargingTimerForRayGun < 0f && !isHoldPosition && theOverridesInAnimator.rayGun)
                {
                    characterLadder.LadderClimbingSpeed = 5f;
                    theAnimator.SetBool("ChargingPause", false);
                }
                else
                {
                    characterLadder.LadderClimbingSpeed = 0f;
                    if (theOverridesInAnimator.rayGun)
                    {
                        theAnimator.SetBool("ChargingPause", true);
                    }
                    else
                    {
                        theAnimator.SetBool("ChargingPause", false);
                    }
                }
            }
        }

        if (horizontalLadder && player.GetButton(("HoldPosition")))
        {
            character.FlipModelOnDirectionChange = false;
        }
        else
        {
            character.FlipModelOnDirectionChange = true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This will detect horizontal ladders and assign the bool "HorizontalLadder" to the animator.
        if (horizontalLadder)
        {
            theAnimator.SetBool("HorizontalLadder", true);
        }
        else
        {
            theAnimator.SetBool("HorizontalLadder", false);
        }

        //This makes that if the player is hanging and presses down+Jump, it will go down. Also, the CoRoutine associated to this.
        if ((player.GetAxis("Vertical") < 0) && (player.GetButton("Jump")) && horizontalLadder && !canNotDettach)
        {
            character.GetComponent<CharacterJump>().AbilityPermitted = false;
            theBCLadder.SetActive(false);
            StartCoroutine(ReinitializeTheBCLadder());
        }

        if (theController.State.IsGrounded)
        {
            character.GetComponent<CharacterJump>().AbilityPermitted = true;
            theBCLadder.SetActive(true);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes that if you are NOT in the water, then you can crouch.
        if (!slopesDetector.GetComponent<Water>().isPlayerInWater)
        {
            GetComponentInParent<CharacterCrouch>().AbilityPermitted = true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes that if the character is rolling, then activate the flickers in Torso and Legs.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Rolling)
        {
            //theTorso.GetComponent<FlashSprites>().run = true;
            //theLegs.GetComponent<FlashSprites>().run = true;
            theTorso.GetComponent<SpriteRenderer>().material = flashMaterial;
            theLegs.GetComponent<SpriteRenderer>().material = flashMaterial;
            theTorsoMachineGunLights.GetComponent<SpriteRenderer>().material = flashMaterial;
            theTorsoFlameGunLights.GetComponent<SpriteRenderer>().material = flashMaterial;
        }
        else
        {
            //theTorso.GetComponent<FlashSprites>().run = false;
            //theLegs.GetComponent<FlashSprites>().run = false;
            theTorso.GetComponent<SpriteRenderer>().material = originalMaterial;
            theLegs.GetComponent<SpriteRenderer>().material = originalMaterial;
            theTorsoMachineGunLights.GetComponent<SpriteRenderer>().material = originalMaterial;
            theTorsoFlameGunLights.GetComponent<SpriteRenderer>().material = originalMaterial;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This finishes the Death animation when hitting the ground.

        if (theController.State.IsFalling && characterDead)
        {
            theAnimator.SetBool("Death Flat", false);
            theAnimator.SetBool("Death Left Flat", false);
        }
        else if (!theController.State.IsFalling && theController.State.IsGrounded && characterDead && theAnimator.GetBool("Death Left"))
        {
            theAnimator.SetBool("Death Left Flat", true);
            if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead &&
                !GetComponent<Health>().PostDamageInvulnerable)
            {
                character.GetComponent<CorgiController>().SetForce(Vector2.zero);
            }
        }
        else if (!theController.State.IsFalling && theController.State.IsGrounded && characterDead && theAnimator.GetBool("Death") && !theAnimator.GetBool("Death Left"))
        {
            theAnimator.SetBool("Death Flat", true);
            if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead &&
                !GetComponent<Health>().PostDamageInvulnerable)
            {
                character.GetComponent<CorgiController>().SetForce(Vector2.zero);
            }
        }
        else if (!theController.State.IsFalling && !theController.State.IsGrounded)
        {
            theAnimator.SetBool("Death Flat", false);
            theAnimator.SetBool("Death Left Flat", false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This doesn't allow to throw grenades if rolling or shooting the primary Weapon. The climbing part is taken in consideration within Corgi's bool "Can Shoot From Ladders".

        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Rolling)
        {
            character.GetComponent<CharacterHandleSecondaryWeapon>().AbilityPermitted = false;
        }
        else
        {
            character.GetComponent<CharacterHandleSecondaryWeapon>().AbilityPermitted = true;
        }

        /*///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This checks if the Throw Grenade animation is playing.
        if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Throw Grenade Straight"))
        {
            theAnimator.SetBool("ThrowGrenade", true);
        }
        else
        {
            //theAnimator.SetBool("ThrowGrenade", false);
        }
        if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Throw Grenade Crouch"))
        {
            theAnimator.SetBool("ThrowGrenade", true);
        }*/
    }

    public IEnumerator ReinitializeTheBCLadder()
    {
        yield return new WaitForSeconds(0.2f);
        theBCLadder.SetActive(true);
        character.GetComponent<CharacterJump>().AbilityPermitted = true;
    }

    public IEnumerator LittleWaitForRayGun()
    {
        //Debug.Log("Here7");
        yield return new WaitForSeconds(0.2f);
        theAnimator.SetBool("ChargingPause", true);
        chargingTimerForRayGun = 0.2f;
        CharacterLadder characterLadder = GetComponent<CharacterLadder>();
        characterLadder.LadderClimbingSpeed = 5f;
    }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This will detect horizontal ladders and assign the bool to the animator.
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "HorizontalLadder" && character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing)
        {
            horizontalLadder = true;
            if (other.gameObject.GetComponent<AdditionalLadderOverride>().canNotDettach)
            {
                canNotDettach = true;
            }
            else
            {
                canNotDettach = false;
            }
        }
        else
        {
            horizontalLadder = false;
            canNotDettach = false;
            verticalLadder = false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //This defines to which direction the player should go upon death.

    public void OnTriggerEnter2D(Collider2D other)
    {
        DamageOnTouch damageOnTouch = other.GetComponent<DamageOnTouch>();
        if (damageOnTouch != null & !characterDead)
        {
            Vector2 relativePosition = other.transform.position - transform.position;

            // Determine if it's a horizontal collision
            if (relativePosition.x > 0)
            {
                theAnimator.SetBool("Death", true);
                theAnimator.SetBool("Death Left", true);
                characterDead = true;
                character.GetComponent<CorgiController>().SetForce(new Vector2(-5, 10));
            }
            else
            {
                theAnimator.SetBool("Death", true);
                theAnimator.SetBool("Death Left", false);
                characterDead = true;
                character.GetComponent<CorgiController>().SetForce(new Vector2(5, 10));
            }
        }
    }
}


//Useless as of now.


//if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationName"))

// Avoid any reload.

//Debug.Log(theCharacterHandleWeapon.BufferInput);
//if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead && (theInputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonDown) || (theInputManager.ShootAxis == MMInput.ButtonStates.ButtonDown))
//{
//    theCharacterHandleWeapon.ShootStart();
//}

//if (character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing)

//inputManager.InputDetectionActive = false;
//handleWeaponCorgi.AbilityPermitted = false;

//else

//inputManager.InputDetectionActive = true;
//handleWeaponCorgi.AbilityPermitted = true;


//if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crouching && player.GetButtonDown("Jump"))
//{
//FindObjectOfType<CharacterCrouch>().AbilityPermitted = false;
//theAnimator.SetBool("Jumping From Crouching", true);
//theAnimator.SetTrigger("Jumping 0");
//}

//Debug.Log(player.GetButton(("HoldPosition")));
//Debug.Log(character.MovementState.CurrentState);
//Debug.Log(theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState);
//Debug.Log(MMInput.ButtonStates.ButtonDown + "Down");
//Debug.Log(MMInput.ButtonStates.ButtonPressed + "Pressed");
//Debug.Log(MMInput.ButtonStates.ButtonUp + "Up");
//Debug.Log(MMInput.AxisTypes.Positive);
//Debug.Log(MMInput.ButtonStates.ButtonPressed);
//Debug.Log(player.GetButtonDown(nameof(transform)));

