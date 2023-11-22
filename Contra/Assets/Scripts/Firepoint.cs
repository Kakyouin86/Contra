using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;


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
    public Inventory weaponInventory;
    public GameObject theFireIndicator;
    public GameObject theSmokeIndicator;
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
        weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventory").GetComponent<Inventory>();
    }

    void Update()
    {
        //////////////////////////////////////////////////////////////////////////////////////
        //I'm not sure why I did this... Something related to climbing the ladder and changing the cooldowns
        //originalCoolDown = projectileWeapon.CooldownDuration;
        /*if (character.MovementState.CurrentState == CharacterStates.MovementStates.LadderClimbing)
        {
            projectileWeapon.CooldownDuration = shortCooldown;
        }
        else
        {
            projectileWeapon.CooldownDuration = originalCoolDown;
        }*/

        foreach (AnimationClip animationNames in animationNames)
        {
            if (IsAnimationPlaying(animationNames.name))
            {
                //////////////////////////////////////////////////////////////////////////////////////
                //Idle and straight
                //Torso IDLE while facing RIGHT
                if (animationNames.name == "Torso Idle" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }
                //Torso IDLE while facing LEFT
                if (animationNames.name == "Torso Idle" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }

                ///////

                //Shoot STRAIGHT while facing RIGHT
                if (animationNames.name == "Shoot Straight" && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }
                //Shoot STRAIGHT while facing LEFT
                if (animationNames.name == "Shoot Straight" && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }

                ///////

                //Shoot UP while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.8f);
                }
                //Shoot UP while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.8f);
                }

                //Shoot UP while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.6f, 2.95f);
                }
                //Shoot UP while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.6f, 2.95f);
                }

                ///////

                //Shoot UP-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.5f);
                }
                //Shoot UP-LEFT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.5f);
                }

                //Shoot UP-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.25f, 2.6f);
                }
                //Shoot UP-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.25f, 2.6f);
                }

                ///////

                //Shoot DOWN while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 1.8f);
                }
                //Shoot DOWN while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.8f);
                }

                //Shoot DOWN while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 0.8f);
                }
                //Shoot DOWN while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 0.8f);
                }

                ///////

                //Shoot DOWN-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.6f);
                }
                //Shoot DOWN-LEFT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.6f);
                }

                //Shoot DOWN-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.15f);
                }
                //Shoot DOWN-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.15f);
                }

                ///////

                //Shoot STRAIGHT while WALKING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Shoot Straight Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 2.03f);
                }
                //Shoot STRAIGHT while WALKING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Shoot Straight Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 2.03f);
                }

                //Shoot STRAIGHT while WALKING and facing RIGHT Flame Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Shoot Straight Walking") || (animationNames.name == "Shoot Straight Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 2.03f);
                }
                //Shoot STRAIGHT while WALKING and facing LEFT Flame Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Shoot Straight Walking") || (animationNames.name == "Shoot Straight Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 2.03f);
                }

                ///////

                //Shoot UP-RIGHT while WALKING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.9f, 3.1f);
                }
                //Shoot UP-LEFT while WALKING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.9f, 3.1f);
                }

                //Shoot UP-RIGHT while WALKING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2.5f);
                }
                //Shoot UP-LEFT while WALKING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 2.5f);
                }

                ///////

                //Shoot DOWN-RIGHT while WALKING while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.84f, 0.94f);
                }
                //Shoot DOWN-LEFT while WALKING while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.84f, 0.94f);
                }

                //Shoot DOWN-RIGHT while WALKING while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.1f);
                }
                //Shoot DOWN-LEFT while WALKING while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.2f);
                }

                ///////

                //////////////////////////////////////////////////////////////////////////////////////

                //Jumps
                //Shoot RIGHT while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 1.85f);
                }
                //Shoot LEFT while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.85f);
                }
                //Shoot RIGHT while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.4f, 1.6f);
                }
                //Shoot LEFT while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.4f, 1.6f);
                }

                ///////

                //Shoot UP while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.5f);
                }
                //Shoot UP while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.5f);
                }

                //Shoot UP while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 1.6f);
                }
                //Shoot UP while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 1.6f);
                }

                ///////

                //Shoot DOWN while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2f);
                }
                //Shoot DOWN while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2f);
                }

                //Shoot DOWN while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.4f, 1.3f);
                }
                //Shoot DOWN while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.4f, 1.3f);
                }

                ///////

                //Shoot UP-RIGHT while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 1.5f);
                }
                //Shoot UP-LEFT while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 1.5f);
                }

                //Shoot UP-RIGHT while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 1.6f);
                }
                //Shoot UP-LEFT while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 1.6f);
                }

                ///////

                //Shoot RIGHT-DOWN while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2f);
                }
                //Shoot LEFT-DOWN while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2f);
                }

                //Shoot RIGHT-DOWN while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 1.2f);
                }
                //Shoot LEFT-DOWN while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 1.2f);
                }

                ///////
                
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

                //Vertical ladder shoot STRAIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 2.1f);
                }
                //Vertical ladder shoot STRAIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 2.1f);
                }

                //Vertical ladder shoot STRAIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2);
                }
                //Vertical ladder shoot STRAIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2);
                }

                ///////

                //Vertical ladder shoot BACK while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.4f, 2.1f);
                }
                //Vertical ladder shoot BACK while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 2.1f);
                }

                //Vertical ladder shoot BACK while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-2.4f, 1.8f);
                }
                //Vertical ladder shoot BACK while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(2.4f, 1.8f);
                }

                ///////

                //Vertical ladder shoot UP while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2.8f);
                }
                //Vertical ladder shoot UP while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2.8f);
                }

                //Vertical ladder shoot UP while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.9f, 3.0f);
                }
                //Vertical ladder shoot UP while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.9f, 3.0f);
                }

                ///////

                //Vertical ladder shoot DOWN while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 1.3f);
                }
                //Vertical ladder shoot DOWN while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 1.3f);
                }

                //Vertical ladder shoot DOWN while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.75f, 0.5f);
                }
                //Vertical ladder shoot DOWN while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.75f, 0.5f);
                }

                ///////

                //Vertical ladder shoot UP-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.5f);
                }
                //Vertical ladder shoot UP-LEFT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.5f);
                }

                //Vertical ladder shoot UP-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 2.65f);
                }
                //Vertical ladder shoot UP-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.4f, 2.65f);
                }

                ///////

                //Vertical ladder shoot DOWN-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.3f, 2.4f);
                }
                //Vertical ladder shoot DOWN-RIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.3f, 2.4f);
                }

                //Vertical ladder shoot DOWN-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-2.25f, 2.35f);
                }
                //Vertical ladder shoot DOWN-RIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(2.25f, 2.35f);
                }

                ///////

                //Vertical ladder shoot DOWN-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 1.6f);
                }
                //Vertical ladder shoot DOWN-RIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 1.6f);
                }

                //Vertical ladder shoot DOWN-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 1.15f);
                }
                //Vertical ladder shoot DOWN-RIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.15f);
                }

                ///////

                //Vertical ladder shoot DOWN-LEFT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.5f, 1.7f);
                }
                //Vertical ladder shoot DOWN-LEFT while facing LEFT  Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.5f, 1.7f);
                }

                //Vertical ladder shoot DOWN-LEFT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-2.3f, 0.95f);
                }
                //Vertical ladder shoot DOWN-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName != "Machine Gun" && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(2.3f, 1.0f);
                }

                ///////
                
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
                //Change the Weapon Rotation Speed. This is done so that if you change positions rapidly, it doesn't show as it's a circular movement on some animations. It creates all kind of crazy shoots in diagonals.

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

                //////////////////////////////////////////////////////////////////////////////////////
                //This moves the Fire Burst when walking so that it doesn't create the effect that the shot is way away from the burst.

                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Machine Gun")
                {
                    theFireIndicator = GameObject.FindWithTag("FireIndicator");
                    if ((animationNames.name == "Shoot Diagonal Up Walking") || (animationNames.name == "Torso Walking Diagonal Up"))
                    {
                        theFireIndicator.transform.localPosition = new Vector3(0.29f, 0.08f, 0.0f);
                    }
                    else if ((animationNames.name == "Shoot Diagonal Down Walking") || (animationNames.name == "Torso Walking Diagonal Down"))

                    {
                        theFireIndicator.transform.localPosition = new Vector3(0.25f, -0.16f, 0.0f);
                    }
                    else
                    {
                        theFireIndicator.transform.localPosition = new Vector3(1.5f, 0.0f, 0.0f);
                    }
                }

                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && weaponInventory.Content[0].ItemName == "Flame Gun")
                {
                    theSmokeIndicator = GameObject.FindWithTag("SmokeIndicator");
                    if (((animationNames.name == "Hold Up") || (animationNames.name == "Shoot Up")))
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.7f, 0.4f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                    }
                    else if (((animationNames.name == "Hold Down") || (animationNames.name == "Shoot Down")))
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.8f, 0.4f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                    }
                    else if ((animationNames.name == "Shoot Diagonal Up Walking Flame Gun") && character.IsFacingRight)
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(-0.35f, 0.35f, 0.0f);
                    }
                    else if ((animationNames.name == "Shoot Diagonal Up Walking Flame Gun") && !character.IsFacingRight)
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(-0.2f, 0.3f, 0.0f);
                    }
                    else if (((animationNames.name == "Climb Shooting Back") || (animationNames.name == "Climb Hold Back")) && character.IsFacingRight)
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.6f, -0.2f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(-180f, 90f, 0f);
                    }
                    else if (((animationNames.name == "Climb Shooting Back") || (animationNames.name == "Climb Hold Back")) && !character.IsFacingRight)
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.625f, -0.2f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(-180f, 90f, 0f);
                    }
                    else if (((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")))
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.7f, 0.5f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                    }
                    else if (((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")))
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.7f, 0.5f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                    }
                    else if (((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")))
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(1.2f, -0.3f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
                    }
                    else if (((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")))
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.0f, -0.15f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 180f, 180f);
                    }
                    else
                    {
                        theSmokeIndicator.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                        theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                }
            }
        }
    }
    private bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo currentState = theAnimator.GetCurrentAnimatorStateInfo(1);
        return currentState.IsName(animationName);
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
