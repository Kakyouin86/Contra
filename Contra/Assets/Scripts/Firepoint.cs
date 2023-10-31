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
    //Machine gun's Weapon Attachment Offset was originally at 0.5. Switched to 0.
    // Box collider in character:
    // Original:
    // x offset 0f / y offset = 1.650963
    // x size: 1f / y size: 2.498392
    // 
    // After changing all Firepoints:
    // x offset 0f / y offset = 1.634014f
    // x size: 1f / y size: 2.53229f
    public Player player;
    public Character character;
    public GameObject theFirepoint;
    public Animator theAnimator;
    public AnimationClip[] animationNames;
    public AnimationClip[] animationNamesForInstantRotation;
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    public WeaponAim weaponAim;
    public float originalWeaponAimRotSpeed;
    public float instantWeaponAimRotSpeed = 0f;
    public ProjectileWeapon projectileWeapon;
    public float originalCoolDown = 0f;
    public float shortCooldown = 0.1f;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }
    void Start()
    {
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
        theFirepoint = GameObject.FindWithTag("Firepoint");
        theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>();
        weaponAim = GameObject.FindWithTag("WeaponAim").GetComponent<WeaponAim>();
        projectileWeapon = GameObject.FindWithTag("WeaponAim").GetComponent<ProjectileWeapon>();
        originalWeaponAimRotSpeed = weaponAim.WeaponRotationSpeed;
        animationNames = Resources.LoadAll<AnimationClip>("Player Animations");
    }

    void Update()
    {
        if (character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing)
        {
            projectileWeapon.CooldownDuration = shortCooldown;
        }
        else
        {
            projectileWeapon.CooldownDuration = originalCoolDown;
        }

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
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);//Was new Vector3(1.05f, 2.03f);
                }
                //Torso idle while facing left
                if (animationNames.name == "Torso Idle" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);//Was new Vector3(-1.05f, 2.03f);
                }
                //Shoot straight while facing right
                if (animationNames.name == "Shoot Straight" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);//Was new Vector3(1.05f, 2.03f);
                }
                //Shoot straight while facing left
                if (animationNames.name == "Shoot Straight" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);//Was new Vector3(-1.05f, 2.03f);
                }
                //Shoot straight while walking and facing right
                if (((animationNames.name == "Shoot Straight Walking") || (animationNames.name == "Shoot Straight Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);//Was new Vector3(1.05f, 2.03f);
                }
                //Shoot straight while walking and facing left
                if (((animationNames.name == "Shoot Straight Walking") || (animationNames.name == "Shoot Straight Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);//Was new Vector3(-1.05f, 2.03f);
                }
                //Shoot Up-Right
                if (((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.5f);//Was new Vector3(0.7f, 3.5f);
                }
                //Shoot Up-Right Walking
                if (((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2.4f);//Was new Vector3(0.7f, 3.5f);
                }
                //Shoot Up-Left
                if (((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.5f);//Was new Vector3(-0.7f, 3.5f);
                }
                //Shoot Up-Left Walking
                if (((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2.4f);//Was new Vector3(-0.7f, 3.5f);
                }
                //Shoot up while facing right
                if (((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.8f);//Was new Vector3(-0.4f, 4.3f);
                }
                //Shoot up while facing left
                if (((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.8f);//Was new Vector3(0.4f, 4.3f);
                }
                //Shoot Down-Right
                if (((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 1.6f);//Was new Vector3(0.7f, 0.5f);
                }
                //Shoot Down-Right Walking
                if (((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking") || (animationNames.name == "Shoot Diagonal Down Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.7f);//Was new Vector3(0.7f, 0.5f);
                }
                //Shoot Down-Left
                if (((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 1.6f);//Was new Vector3(-0.7f, 0.5f);
                }
                //Shoot Down-Left Walking
                if (((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking") || (animationNames.name == "Shoot Diagonal Down Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.0f, 1.7f);//Was new Vector3(-0.7f, 0.5f);
                }
                //Shoot down while facing right
                if (((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 1.8f);//Was new Vector3(-0.75f, 0f);
                }
                //Shoot down while facing left
                if (((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.8f);//Was new Vector3(0.75f, 0f);
                }

                //////////////////////////////////////////////////////////////////////////////////////

                //Jumps
                //Right while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 1.85f);//Was new Vector3(0.7f, 1.75f);
                }
                //Left while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.85f);//Was new Vector3(-0.7f, 1.75f);
                }
                //Up while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.5f); ;//Was new Vector3(-0.5f, 3.0f);
                }
                //Up while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.5f); ;//Was new Vector3(0.5f, 3f);
                }
                //Down while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2f);//Was new Vector3(-0.5f, 0.5f);
                }
                //Down while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2f);//Was new Vector3(0.5f, 0.5f);
                }
                //Right-Up while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0) && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 1.5f); //Was new Vector3(0.3f, 2.7f);
                }
                //Left-Up while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0) && (player.GetAxisRaw("Horizontal") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 1.5f);//Was new Vector3(-0.35f,2.7f);
                }
                //Right-Down while jumping and facing right
                if (animationNames.name == "Jumping" && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0) && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2f);//Was new Vector3(0.3f, 0.85f);
                } 
                //Left-Down while jumping and facing left
                if (animationNames.name == "Jumping" && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0) && (player.GetAxisRaw("Horizontal") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2f);//Was new Vector3(- 0.35f, 0.85f);
                }
                
                //////////////////////////////////////////////////////////////////////////////////////
                
                //Crouch
                if (animationNames.name == "Crouch" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 0.75f);//Was new Vector3(1.4f, 0.75f);
                }
                if (animationNames.name == "Crouch" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 0.75f);//Was new Vector3(-1.35f, 0.75f);
                }
                if (animationNames.name == "Crouch Shooting" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 0.75f);//Was new Vector3(1.4f, 0.75f);
                }
                if (animationNames.name == "Crouch Shooting" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 0.75f);//Was new Vector3(-1.35f, 0.75f);
                }

                //////////////////////////////////////////////////////////////////////////////////////
                //Vertical ladder
                //Vertical ladder idle while facing right
                if (((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 2.1f);//Was new Vector3(0.85f,2.1f);
                }
                //Vertical ladder idle while facing left
                if (((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 2.1f);//Was new Vector3(-0.85f, 2.1f);
                }
                //Vertical ladder idle while facing right but aiming left
                if (((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.4f, 2.1f);//Was new Vector3(-3.5f, 2.1f);
                }
                //Vertical ladder idle while facing left but aiming right
                if (((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && !character.IsFacingRight && (player.GetAxisRaw("Horizontal") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 2.1f);//Was new Vector3(3.5f, 2.1f);
                }
                //Vertical ladder idle while facing right and aiming diagonal up-right
                if (((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.5f);//Was new Vector3(0.5f, 3.5f);
                }
                //Vertical ladder idle while facing left and aiming diagonal up-right
                if (((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.5f);//Was new Vector3(-0.5f, 3.5f);
                }
                //Vertical ladder idle while facing right and aiming up
                if (((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2.8f);//Was new Vector3(-0.7f, 4.35f);
                }
                //Vertical ladder idle while facing left and aiming up
                if (((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2.8f);//Was new Vector3(0.6f, 4.35f);
                }
                //Vertical ladder idle while facing right and aiming diagonal up-left
                if (((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.3f, 2.4f);//Was new Vector3(-2.9f, 3.5f);
                }
                //Vertical ladder idle while facing left and aiming diagonal up-left
                if (((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.3f, 2.4f);//Was new Vector3(2.9f, 3.5f);
                }
                //Vertical ladder idle while facing right and aiming down-forward
                if (((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 1.6f);//Was new Vector3(0.4f, 0.4f);
                }
                //Vertical ladder idle while facing left and aiming down-forward
                if (((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 1.6f);//Was new Vector3(-0.3f, 0.55f);
                }
                //Vertical ladder idle while facing right and aiming down
                if (((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 1.3f);//Was new Vector3(-1f, 0f);
                }
                //Vertical ladder idle while facing left and aiming down
                if (((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 1.3f);//Was new Vector3(1f, -0.2f);
                }
                //Vertical ladder idle while facing right and aiming down-back
                if (((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.5f, 1.7f);//Was new Vector3(-3f, 0.75f);
                }
                //Vertical ladder idle while facing left and aiming down-back
                if (((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.5f, 1.7f);//Was new Vector3(3f, 0.6f);
                }

                //////////////////////////////////////////////////////////////////////////////////////
                //Horizontal ladder
                //Horizontal ladder idle while facing right
                if (((animationNames.name == "Horizontal Ladder Idle") || (animationNames.name == "Horizontal Ladder Hold Forward") || (animationNames.name == "Horizontal Ladder Shooting Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.85f);//Was new Vector3(1.0f, 1.85f);
                }
                //Horizontal ladder idle while facing left
                if (((animationNames.name == "Horizontal Ladder Idle") || (animationNames.name == "Horizontal Ladder Hold Forward") || (animationNames.name == "Horizontal Ladder Shooting Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.85f);//Was new Vector3(-1.0f, 1.85f);
                }
                //Horizontal ladder idle while facing right but aiming left
                if (((animationNames.name == "Horizontal Ladder Hold Back") || (animationNames.name == "Horizontal Ladder Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.1f, 2.1f);//Was new Vector3(-3.1f, 2.15f);
                }
                //Horizontal ladder idle while facing left but aiming right
                if (((animationNames.name == "Horizontal Ladder Hold Back") || (animationNames.name == "Horizontal Ladder Shooting Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.1f, 2.1f);//Was new Vector3(3.25f, 2.15f);
                }
                //Horizontal ladder idle while facing right and aiming diagonal up-right
                if (((animationNames.name == "Horizontal Ladder Hold Diagonal Up Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2.3f);//Was new Vector3(0.65f, 3.4f);
                }
                //Horizontal ladder idle while facing left and aiming diagonal up-right
                if (((animationNames.name == "Horizontal Ladder Hold Diagonal Up Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2.3f);//new Vector3(-0.65f, 3.4f);
                }
                //Horizontal ladder idle while facing right and aiming up
                if (((animationNames.name == "Horizontal Ladder Hold Up") || (animationNames.name == "Horizontal Ladder Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.7f);//Was new Vector3(-0.55f, 4.2f);
                }
                //Horizontal ladder idle while facing left and aiming up
                if (((animationNames.name == "Horizontal Ladder Hold Up") || (animationNames.name == "Horizontal Ladder Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.7f);//Was new Vector3(0.6f, 4.2f);
                }
                //Horizontal ladder idle while facing right and aiming diagonal up-left
                if (((animationNames.name == "Horizontal Ladder Hold Back Diagonal Up") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.1f, 2.3f);//Was new Vector3(-2.5f, 3.4f);
                }
                //Horizontal ladder idle while facing left and aiming diagonal up-left
                if (((animationNames.name == "Horizontal Ladder Hold Back Diagonal Up") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.1f, 2.3f);//Was new Vector3(2.5f, 3.4f);
                }
                //Horizontal ladder idle while facing right and aiming down-forward
                if (((animationNames.name == "Horizontal Ladder Hold Diagonal Down Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 1.4f);//Was new Vector3(0.35f, 0.4f);
                }
                //Horizontal ladder idle while facing left and aiming down-forward
                if (((animationNames.name == "Horizontal Ladder Hold Diagonal Down Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 1.4f);//Was new Vector3(-0.5f, 0.4f);
                }
                //Horizontal ladder idle while facing right and aiming down
                if (((animationNames.name == "Horizontal Ladder Hold Down") || (animationNames.name == "Horizontal Ladder Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.4f, 1.1f);//Was new Vector3(-0.8f, -0.5f);
                }
                //Horizontal ladder idle while facing left and aiming down
                if (((animationNames.name == "Horizontal Ladder Hold Down") || (animationNames.name == "Horizontal Ladder Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.4f, 1.1f);//Was new Vector3(0.8f, -0.5f);
                }
                //Horizontal ladder idle while facing right and aiming down-back
                if (((animationNames.name == "Horizontal Ladder Hold Diagonal Down Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.3f, 1.7f);//Was new Vector3(-2.8f, 0.65f);
                }
                //Horizontal ladder idle while facing left and aiming down-back
                if (((animationNames.name == "Horizontal Ladder Hold Diagonal Down Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.3f, 1.7f);//Was new Vector3(2.8f, 0.65f);
                }

                //////////////////////////////////////////////////////////////////////////////////////
                //Change the Weapon Rotation Speed

                bool animationFound = false;
                if (animationNamesForInstantRotation != null)
                {
                    foreach (var clip in animationNamesForInstantRotation)
                    {
                        if (clip.name == animationNames.name)
                        {
                            weaponAim.WeaponRotationSpeed = instantWeaponAimRotSpeed;
                            animationFound = true;
                            break; // Exit the loop once the animation is found
                        }
                    }
                }

                if (!animationFound)
                {
                    weaponAim.WeaponRotationSpeed = originalWeaponAimRotSpeed;
                }
                
                /*if (
                    ((animationNames.name == "Shoot Straight"
                      || animationNames.name == "Shoot Straight Walking"
                      || animationNames.name == "Shoot Diagonal Down Walking"
                      || animationNames.name == "Torso Walking Diagonal Up"
                      || animationNames.name == "Climb Hold Back"
                      || animationNames.name == "Climb Shooting Back" 
                      || animationNames.name == "Climb Shooting Forward"
                      || animationNames.name == "Horizontal Ladder Shooting Back"
                      || animationNames.name == "Horizontal Ladder Shooting Forward")))
                {
                    weaponAim.WeaponRotationSpeed = instantWeaponAimRotSpeed;
                }
                else
                {
                    weaponAim.WeaponRotationSpeed = originalWeaponAimRotSpeed;
                }*/
            }
        }
    }
    private bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo currentState = theAnimator.GetCurrentAnimatorStateInfo(1);
        return currentState.IsName(animationName);
    }
}
