using MoreMountains.CorgiEngine;
using UnityEngine;
using Rewired;
using InputManager = MoreMountains.CorgiEngine.InputManager;

public class AdditionalMovementSettings : MonoBehaviour
{
    public Player player;
    public Character character;
    public CharacterHorizontalMovement horizontalMovementCorgi;
    public CorgiController theController;
    public Weapon weapon;
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
        character = GetComponent<Character>();
        weapon = FindObjectOfType<MoreMountains.CorgiEngine.Weapon>();
        theAnimator = GetComponent<Animator>();
        theStandingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y);
        theCrouchingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y + offset.y);
        theController = FindObjectOfType<MoreMountains.CorgiEngine.CorgiController>();
        theController.State.JustGotGrounded = true;
    }


    void Update()
    {
        //This makes the player's firepoint go down if he's crouching.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crouching)
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
        }
        else
        {
            horizontalMovementCorgi.AbilityPermitted = true;
            theController.State.JustGotGrounded = true;
            theAnimator.SetBool("Hold", false);
            theAnimator.SetBool("Walking", false);
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
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Dashing)
        {
            //inputManager.InputDetectionActive = false;
            //handleWeaponCorgi.AbilityPermitted = false;
        }
        else
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
    }
}
