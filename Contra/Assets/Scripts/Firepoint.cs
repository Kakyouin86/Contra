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
    //public Vector3 theStandingPosition;
    //public Vector3 theCrouchingPosition;
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
        //theStandingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y);
        //theCrouchingPosition = new Vector3(theFirepoint.transform.position.x, theFirepoint.transform.position.y + offset.y);
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
                //////////////////////////////////////////////////////////////////////////////////////
                //Idle and straight
                //Torso idle while facing right
                if (animationNames.name == "Torso Idle" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.05f, 2.03f);
                }
                //Torso idle while facing left
                if (animationNames.name == "Torso Idle" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.05f, 2.03f);
                }
                //Shoot straight while facing right
                if (animationNames.name == "Shoot Straight" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.05f, 2.03f);
                }
                //Shoot straight while facing left
                if (animationNames.name == "Shoot Straight" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.05f, 2.03f);
                }
                //Shoot straight while walking and facing right
                if (animationNames.name == "Shoot Straight Walking" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.05f, 2.03f);
                }
                //Shoot straight while walking and facing left
                if (animationNames.name == "Shoot Straight Walking" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.05f, 2.03f);
                }
                //Shoot Up-Right
                if (((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up") || (animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.7f, 3.5f);
                }
                //Shoot Up-Left
                if (((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up") || (animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.7f, 3.5f);
                }
                //Shoot up while facing right
                if (((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(- 0.4f, 4.3f);
                }
                //Shoot up while facing left
                if (((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.4f, 4.3f);
                }
                //Shoot Down-Right
                if (((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down") || (animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.7f, 0.5f);
                }
                //Shoot Down-Left
                if (((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down") || (animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.7f, 0.5f);
                }
                //Shoot down while facing right
                if (((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.75f, 0f);
                }
                //Shoot down while facing left
                if (((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.75f, 0f);
                }


                //////////////////////////////////////////////////////////////////////////////////////

                //Jumps
                //Right while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.7f, 1.75f);
                }
                //Left while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.7f, 1.75f);
                }
                //Up while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 3.0f);
                }
                //Up while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 3f);
                }
                //Down while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 0.5f);
                }
                //Down while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 0.5f);
                }
                //Right-Up while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0) && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 2.7f);
                }
                //Left-Up while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0) && (player.GetAxisRaw("Horizontal") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.35f,2.7f);
                }
                //Right-Down while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0) && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 0.85f);
                } 
                //Left-Down while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0) && (player.GetAxisRaw("Horizontal") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(- 0.35f, 0.85f);
                }
                
                //////////////////////////////////////////////////////////////////////////////////////
                
                //Crouch
                if (animationNames.name == "Crouch" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 0.75f);
                }
                if (animationNames.name == "Crouch" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.35f, 0.75f);
                }
                if (animationNames.name == "Crouch Shooting" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 0.75f);
                }
                if (animationNames.name == "Crouch Shooting" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.35f, 0.75f);
                }

                //////////////////////////////////////////////////////////////////////////////////////
                //Vertical ladder
                //Vertical ladder idle while facing right
                if (((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.85f,2.1f);
                }
                //Vertical ladder idle while facing left
                if (((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.85f, 2.1f);
                }
                //Vertical ladder idle while facing right but aiming left
                if (((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-3.5f, 2.1f);
                }
                //Vertical ladder idle while facing left but aiming right
                if (((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && !character.IsFacingRight && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(3.5f, 2.1f);
                }
                //Vertical ladder idle while facing right and aiming diagonal up-right
                if (((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 3.5f);
                }
                //Vertical ladder idle while facing left and aiming diagonal up-right
                if (((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 3.5f);
                }
                //Vertical ladder idle while facing right and aiming diagonal up-left
                if (((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-2.9f, 3.5f);
                }
                //Vertical ladder idle while facing left and aiming diagonal up-left
                if (((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(2.9f, 3.5f);
                }
                //Vertical ladder idle while facing right and aiming up
                if (((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.7f, 4.35f);
                }
                //Vertical ladder idle while facing left and aiming up
                if (((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.6f, 4.35f);
                }
                //Vertical ladder idle while facing right and aiming down-back
                if (((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-3f, 0.75f);
                }
                //Vertical ladder idle while facing left and aiming down-back
                if (((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(3f, 0.6f);
                }

                //Vertical ladder idle while facing right and aiming down-forward
                if (((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.4f, 0.4f);
                }
                //Vertical ladder idle while facing left and aiming down-forward
                if (((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 0.55f);
                }
                //Vertical ladder idle while facing right and aiming down
                if (((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1f, 0f);
                }
                //Vertical ladder idle while facing left and aiming down
                if (((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1f, -0.2f);
                }

                //////////////////////////////////////////////////////////////////////////////////////
                //Horizontal ladder
                //Horizontal ladder idle while facing right
            }
        }
    }
    private bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo currentState = theAnimator.GetCurrentAnimatorStateInfo(1);
        return currentState.IsName(animationName);
    }
}
