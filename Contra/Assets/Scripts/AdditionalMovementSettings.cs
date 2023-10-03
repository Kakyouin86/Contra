using System.Collections;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;
using InputManager = MoreMountains.CorgiEngine.InputManager;


public class AdditionalMovementSettings : MonoBehaviour
{
    //These are the things we need to change in each script:

    //Ladder: 14
    //public enum LadderTypes { Simple, BiDirectional, Horizontal } //Leo Monge: Need to ALWAYS bring it after update.

    //CharacterLadder: 75
    /*
     protected override void Initialization()
       {
       base.Initialization();
       CurrentLadderClimbingSpeed = Vector2.zero;
       //_boxCollider = this.gameObject.GetComponentInParent<BoxCollider2D>();//Leo Monge: Need to ALWAYS bring it after update.
       _boxCollider = GameObject.FindWithTag("LadderCollider").GetComponent<BoxCollider2D>();//Leo Monge: Need to ALWAYS bring it after update. This adds the collider of the Ladder Collider only
       _colliders = new List<Collider2D>();
       _characterHandleWeapon = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterHandleWeapon>();
       }
     */

    //CharacterWallClinging: 21
    //[Range(0.0000000001f, 1)]//Leo Monge: Need to ALWAYS bring it after update. it was 0.01f originally. With this small value, you can't detach from the wall.

    public Player player;
    public Character character;
    public CharacterHorizontalMovement horizontalMovementCorgi;
    public CorgiController theController;
    public InputManager theInputManager;
    public CharacterHandleWeapon theCharacterHandleWeapon;
    public Animator theAnimator;
    public GameObject theFirepoint;
    public Weapon theWeapon;
    public bool horizontalLadder = false;
    public BoxCollider2D theBCTrigger;
    public Vector2 theOriginalBoxCollider2DSize;
    public Vector2 theOriginalBoxCollider2DOffset;
    public float jumpingOffsetX;
    public float jumpingOffsetY;
    public float jumpingSizeX;
    public float jumpingSizeY;
    public bool isNotCharacterV3 = false;
    public GameObject theRippleEffect;
    public GameObject theLegs;
    public GameObject theBCLadder;
    public bool startTimerBeforeNextAnim = false;
    public float initialTimeBeforeNextAnim = 0.25f;
    public float currentTimeBeforeNextAnim = 0.25f;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Start()
    {
        horizontalMovementCorgi = GetComponent<CharacterHorizontalMovement>();
        theInputManager = FindObjectOfType<InputManager>();
        theCharacterHandleWeapon = FindObjectOfType<CharacterHandleWeapon>();
        character = GetComponent<Character>();
        theFirepoint = GameObject.FindWithTag("Firepoint");
        //theAnimator = GetComponent<Animator>();
        theBCTrigger = GetComponent<BoxCollider2D>();
        theController.State.JustGotGrounded = true;
        theWeapon = FindObjectOfType<Weapon>();
        theOriginalBoxCollider2DSize = new Vector3(theBCTrigger.size.x, theBCTrigger.size.y);
        theOriginalBoxCollider2DOffset = new Vector3(theBCTrigger.offset.x, theBCTrigger.offset.y);
    }
    void Update()
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Just the "Hold"
        if (player.GetButton(("HoldPosition")))
        {
            theAnimator.SetBool("Hold", true);
        }

        else
        {
            theAnimator.SetBool("Hold", false);
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
        if (theController.State.IsGrounded && player.GetButton(("HoldPosition")) && !theController.State.IsJumping)
        {
            theController.SetHorizontalForce(0);
            theController.SetVerticalForce(0);
            //theAnimator.SetBool("Hold", true);
            theAnimator.SetBool("Walking", false);
            theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = false;
        }
        else
        {
            horizontalMovementCorgi.AbilityPermitted = true;
            theController.State.JustGotGrounded = true;
            //theAnimator.SetBool("Hold", false);
            theAnimator.SetBool("Walking", false);
            theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes the Death bool to happen. Death trigger was NOT removed from the original script but I added a bool so it doesn't exit the animator.
        if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
        {
            theAnimator.SetBool("Death", true);
            //theAnimator.SetTrigger("Death");
        }
        else
        {
            theAnimator.SetBool("Death", false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes that, when crouching, you can fire diagonally.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crawling)
        {
            theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = false;
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
                if (player.GetButton(("HoldPosition")))
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
            if (horizontalLadder && player.GetButton(("HoldPosition")))
            {
                character.FlipModelOnDirectionChange = false;
            }
            else
            {
                character.FlipModelOnDirectionChange = true;
            }

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This will detect horizontal ladders and assign the bool to the animator.
        if (horizontalLadder)
        {
            theAnimator.SetBool("HorizontalLadder", true);
        }
        else
        {
            theAnimator.SetBool("HorizontalLadder", false);
        }

        //This makes that if the player is hang and presses down and Jump, it will go through. Also, the Coroutine associated to this.
        if ((player.GetAxis("Vertical") < 0) && (player.GetButton("Jump")) && horizontalLadder)
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
        //This makes the animator stay on "shooting" if the "Fire" of the weapon is still active
        if (theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse)
        {
            currentTimeBeforeNextAnim = initialTimeBeforeNextAnim;
            startTimerBeforeNextAnim = true;
        }

        if (startTimerBeforeNextAnim)
        {
            currentTimeBeforeNextAnim -= Time.deltaTime;
            theAnimator.SetBool("TimerBeforeNextAnim", true);
        }

        if (currentTimeBeforeNextAnim <= 0.0f)
        {
            currentTimeBeforeNextAnim = initialTimeBeforeNextAnim;
            startTimerBeforeNextAnim = false;
            theAnimator.SetBool("TimerBeforeNextAnim", false);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    IEnumerator ReinitializeTheBCLadder()
    {
        yield return new WaitForSeconds(0.2f);
        theBCLadder.SetActive(true);
        character.GetComponent<CharacterJump>().AbilityPermitted = true;
    }

    //This will detect horizontal ladders and assign the bool to the animator.
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "HorizontalLadder" && character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing)
        {
            horizontalLadder = true;
        }
        else
        {
            horizontalLadder = false;
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

