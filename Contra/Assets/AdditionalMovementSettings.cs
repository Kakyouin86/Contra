using System.Linq.Expressions;
using MoreMountains.CorgiEngine;
using UnityEngine;
using Rewired;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using Rewired.ComponentControls.Data;
using UnityEngine;
using InputManager = MoreMountains.CorgiEngine.InputManager;

public class AdditionalMovementSettings : MonoBehaviour
{
    public Player player;
    public CharacterHorizontalMovement horizontalMovementCorgi;
    public CharacterHandleWeapon handleWeaponCorgi;
    //public CharacterDash dashCorgi;
    public Character character;
    public InputManager inputManager;
    public Weapon weapon;
    public Animator theAnimator;
    public GameObject theFirepoint;
    public Vector3 theStandingPosition;
    public Vector3 theCrouchingPosition;
    public Vector3 offset = new Vector3(0f, -2f, 0f);
    public InputManager inputManagerCorgi;
    //public bool adjustPosition = false;

    // Start is called before the first frame update
    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }
    void Start()
    {
        horizontalMovementCorgi = GetComponent<CharacterHorizontalMovement>();
        handleWeaponCorgi = GetComponent<CharacterHandleWeapon>();
        character = GetComponent<Character>();
        inputManager = FindObjectOfType<MoreMountains.CorgiEngine.InputManager>();
        weapon = FindObjectOfType<MoreMountains.CorgiEngine.Weapon>();
        theAnimator = GetComponent<Animator>();
        theStandingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y);
        theCrouchingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y + offset.y);
        inputManagerCorgi = FindObjectOfType<MoreMountains.CorgiEngine.InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.GetButton(("HoldPosition")));
        Debug.Log(character.MovementState.CurrentState);
        //This makes the player's firepoint go down if he's crouching.
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.Crouching)
        {
            theFirepoint.gameObject.transform.localPosition = new Vector3(theCrouchingPosition.x, theCrouchingPosition.y);
        }
        else
        {
            theFirepoint.gameObject.transform.localPosition = new Vector3(theStandingPosition.x, theStandingPosition.y);
        }

        //This makes the Hold Position impossible to move and aim without walking.
        if (player.GetButton(("HoldPosition")))
        {
            horizontalMovementCorgi.AbilityPermitted = false;
            //horizontalMovementCorgi.ReadInput = false;
            ////handleWeaponCorgi.AbilityPermitted = false;
            //horizontalMovementCorgi.WalkSpeed = 0f;
            //inputManagerCorgi.InputDetectionActive = false;
            //inputManagerCorgi.ResetButtonStatesOnFocusLoss = false;
        }
        else
        {
            horizontalMovementCorgi.AbilityPermitted = true;
            //horizontalMovementCorgi.ReadInput = true;
            //handleWeaponCorgi.AbilityPermitted = true;
            //horizontalMovementCorgi.WalkSpeed = 6f;
            //inputManagerCorgi.InputDetectionActive = true;
            //inputManagerCorgi.ResetButtonStatesOnFocusLoss = true;

        }

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
    }
}
