using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
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
        theCrouchingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y + offset.y);
        theController = FindObjectOfType<MoreMountains.CorgiEngine.CorgiController>();
        theController.State.JustGotGrounded = true;
    }
    void Update()
    {
        //This makes the player's firepoint go down if he's crouching.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crouching && !player.GetButton(("HoldPosition")))
        {
            theFirepoint.gameObject.transform.localPosition = new Vector3(theCrouchingPosition.x, theCrouchingPosition.y);
        }
        else
        {
            theFirepoint.gameObject.transform.localPosition = new Vector3(theStandingPosition.x, theStandingPosition.y);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //This makes the Hold Position impossible to move and aim without walking.
        if (theController.State.IsGrounded && player.GetButton(("HoldPosition")) && !theController.State.IsJumping)
        {
            theController.SetHorizontalForce(0);
            theController.SetVerticalForce(0);
            theAnimator.SetBool("Hold", true);
            theAnimator.SetBool("Walking", false);
            theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = false;
        }
        else
        {
            horizontalMovementCorgi.AbilityPermitted = true;
            theController.State.JustGotGrounded = true;
            theAnimator.SetBool("Hold", false);
            theAnimator.SetBool("Walking", false);
            theFirepoint.GetComponentInChildren<WeaponAim>().IgnoreDownWhenGrounded = true;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //This makes the Death bool to happen. Death Trigger was NOT removed from the original script.
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

        //Useless as of now.

        //if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationName"))
        {
            // Avoid any reload.
        }
        //Debug.Log(theCharacterHandleWeapon.BufferInput);
        //if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead && (theInputManager.ShootButton.State.CurrentState == MMInput.ButtonStates.ButtonDown) || (theInputManager.ShootAxis == MMInput.ButtonStates.ButtonDown))
        //{
        //    theCharacterHandleWeapon.ShootStart();
        //}

        //if (character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing)
        {
            //inputManager.InputDetectionActive = false;
            //handleWeaponCorgi.AbilityPermitted = false;
        }
        //else
        {
            //inputManager.InputDetectionActive = true;
            //handleWeaponCorgi.AbilityPermitted = true;
        }

        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crouching && player.GetButtonDown("Jump"))
        {
            //FindObjectOfType<CharacterCrouch>().AbilityPermitted = false;
            //theAnimator.SetBool("Jumping From Crouching", true);
            //theAnimator.SetTrigger("Jumping 0");
        }

        //Debug.Log(player.GetButton(("HoldPosition")));
        //Debug.Log(character.MovementState.CurrentState);
        //Debug.Log(MMInput.ButtonStates.ButtonDown + "Down");
        //Debug.Log(MMInput.ButtonStates.ButtonPressed + "Pressed");
        //Debug.Log(MMInput.ButtonStates.ButtonUp + "Up");
        //Debug.Log(MMInput.AxisTypes.Positive);
        //Debug.Log(MMInput.ButtonStates.ButtonPressed);
        //Debug.Log(player.GetButtonDown(nameof(transform)));
    }
}
