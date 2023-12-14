using System.Collections;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;
using InputManager = MoreMountains.CorgiEngine.InputManager;
using System.Security.Cryptography.X509Certificates;

public class AdditionalMovementSettings : MonoBehaviour
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
    public bool horizontalLadder = false;
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

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Start()
    {
        horizontalMovementCorgi = GetComponent<CharacterHorizontalMovement>();
        theInputManager = FindObjectOfType<InputManager>();
        theCharacterHandleWeapon = FindObjectOfType<CharacterHandleWeapon>();
        theSpecialShootAndRaycastVisualization = GetComponent<SpecialShootAndRaycastVisualization>();
        character = GetComponent<Character>();
        theFirepoint = GameObject.FindWithTag("Firepoint");
        slopesDetector = GameObject.FindWithTag("SlopesDetector");
        theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>();
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
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing && theAnimator.GetBool("isShooting"))
        {
            theAnimator.SetBool("Hold", true);
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
        //This makes the "Death" bool to happen. Death trigger was NOT removed from the original script but I added a bool so it doesn't exit the animator.
        if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
        {
            theAnimator.SetBool("Death", true);
            //theAnimator.SetTrigger("Death");
        }
        else
        {
            theAnimator.SetBool("Death", false);
        }

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
            }

            if (theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponIdle)
            {
                if (player.GetButton(("HoldPosition")) || theSpecialShootAndRaycastVisualization.isShooting)
                {
                    //theAnimator.SetBool("Hold", true);
                    GetComponent<CharacterLadder>().LadderClimbingSpeed = 0f;
                }
                else
                {
                    GetComponent<CharacterLadder>().LadderClimbingSpeed = 5f;
                }
            }
            else
            {
                GetComponent<CharacterLadder>().LadderClimbingSpeed = 0f;
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

    IEnumerator ReinitializeTheBCLadder()
    {
        yield return new WaitForSeconds(0.2f);
        theBCLadder.SetActive(true);
        character.GetComponent<CharacterJump>().AbilityPermitted = true;
    }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This will detect horizontal ladders and assign the bool to the animator.
    void OnTriggerStay2D(Collider2D other)
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

