using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;
using InputManager = MoreMountains.CorgiEngine.InputManager;


public class AdditionalMovementSettings : MonoBehaviour
{
    public Player player;
    public Character character;
    public CharacterHorizontalMovement horizontalMovementCorgi;
    public CorgiController theController;
    public InputManager theInputManager;
    public CharacterHandleWeapon theCharacterHandleWeapon;
    public Animator theAnimator;
    public GameObject theFirepoint;
    public Vector3 theStandingPosition;
    public Vector3 theCrouchingPosition;
    public Vector3 offset = new Vector3(0f, -2f, 0f);
    public Weapon theWeapon;
    public bool horizontalLadder = false;
    public BoxCollider2D theBCTrigger;
    public Vector2 theOriginalBoxCollider2DSize;
    public Vector2 theOriginalBoxCollider2DOffset;

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
        theAnimator = GetComponent<Animator>();
        theStandingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y);
        theCrouchingPosition =
            new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y + offset.y);
        theController = FindObjectOfType<MoreMountains.CorgiEngine.CorgiController>();
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
            theBCTrigger.offset = new Vector2(0, 1.3f);
            theBCTrigger.size = new Vector2(1, 1.8f);
        }
        else
        {
            theBCTrigger.offset = new Vector2(theOriginalBoxCollider2DOffset.x, theOriginalBoxCollider2DOffset.y);
            theBCTrigger.size = new Vector2(theOriginalBoxCollider2DSize.x, theOriginalBoxCollider2DSize.y);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //This makes the player's firepoint go down if he's crouching.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crouching &&
            !player.GetButton(("HoldPosition")))
        {
            theFirepoint.gameObject.transform.localPosition =
                new Vector3(theCrouchingPosition.x, theCrouchingPosition.y);
        }
        else
        {
            theFirepoint.gameObject.transform.localPosition = new Vector3(theStandingPosition.x, theStandingPosition.y);
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

