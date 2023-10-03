using System.Collections;
using System.Globalization;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;
using InputManager = MoreMountains.CorgiEngine.InputManager;

public class Firepoint : MonoBehaviour
{
    public Player player;
    public Character character;
    public GameObject theFirepoint;
    public Animator theAnimator;
    public Vector3 theStandingPosition;
    public Vector3 theCrouchingPosition;
    public AnimationClip[] animationNames;
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    //public WeaponAim aimableWeapon;
    //public float theAngle;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }
    void Start()
    {
        character = GetComponent<Character>();
        theFirepoint = GameObject.FindWithTag("Firepoint");
        theAnimator = gameObject.transform.GetChild(0).GetComponent<Animator>();
        theStandingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y);
        theCrouchingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y + offset.y);
    }

    void Update()
    {
        //theStandingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y);
        //theCrouchingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y + offset.y);
        //theAngle = aimableWeapon.CurrentAngleRelative;

        foreach (AnimationClip animationNames in animationNames)
        {
            // Check if the animation is playing
            if (IsAnimationPlaying(animationNames.name))
            {
                if (animationNames.name == "Torso Idle" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.05f, 2.03f);
                }
                if (animationNames.name == "Torso Idle" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.05f, 2.03f);
                }
                //////////////////////////////////////////////////////////////////////////////////////
                if (animationNames.name == "Shoot Straight" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.05f, 2.03f);
                }
                if (animationNames.name == "Shoot Straight" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.05f, 2.03f);
                }
                //////////////////////////////////////////////////////////////////////////////////////
                if (animationNames.name == "Jumping" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.7f, 1.75f);
                }
                if (animationNames.name == "Jumping" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.7f, 1.75f);
                }
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 3.0f);
                }
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 3f);
                }
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxis("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 0.5f);
                }
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxis("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 0.5f);
                }
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0) && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 2.7f);
                }
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0) && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(offset.x, offset.y);
                }
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxis("Vertical") < 0) && (player.GetAxis("Horizontal") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(offset.x, offset.y);
                }
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxis("Vertical") < 0) && (player.GetAxis("Horizontal") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(- 0.35f, 0.85f);
                }
                
                //////////////////////////////////////////////////////////////////////////////////////
                if (animationNames.name == "Crouch" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 0.75f);
                }
                if (animationNames.name == "Crouch" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.35f, 0.75f);
                }
                //////////////////////////////////////////////////////////////////////////////////////
                if (animationNames.name == "Crouch Shooting" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 0.75f);
                }
                if (animationNames.name == "Crouch Shooting" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.35f, 0.75f);
                }
                //////////////////////////////////////////////////////////////////////////////////////
            }
        }
    }
    private bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo currentState = theAnimator.GetCurrentAnimatorStateInfo(1);
        return currentState.IsName(animationName);
    }
}
