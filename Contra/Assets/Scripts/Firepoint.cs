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
    public SpecialShootAndRaycastVisualization theSpecialShootAndRaycastVisualization;
    public GameObject theFireIndicator;
    public GameObject theSmokeIndicator;
    public GameObject[] theRayIndicator;
    public GameObject[] sparks;
    public float originalWeaponAimRotSpeed;
    public float instantWeaponAimRotSpeed = 0f;
    public ProjectileWeapon projectileWeapon;
    //public float originalCoolDown = 0f;
    //public float shortCooldown = 0.1f;

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
        theSpecialShootAndRaycastVisualization = GetComponent<SpecialShootAndRaycastVisualization>();
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
            if (IsAnimationPlaying(animationNames.name) && character.ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead)
            {
                //////////////////////////////////////////////////////////////////////////////////////
                //Idle and straight
                //Torso IDLE while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Torso Idle") || (animationNames.name == "Torso Idle")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }
                //Torso IDLE while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Torso Idle") || (animationNames.name == "Torso Idle")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }

                //Torso IDLE while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Idle") || (animationNames.name == "Torso Idle")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.50f, 2.3f);
                }
                //Torso IDLE  while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Idle") || (animationNames.name == "Torso Idle")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.50f, 2.3f);
                }

                ///////

                //Shoot STRAIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Straight") || (animationNames.name == "Shoot Straight")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }
                //Shoot STRAIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Straight") || (animationNames.name == "Shoot Straight")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0, 2.03f);
                }

                //Shoot STRAIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Straight") || (animationNames.name == "Shoot Straight")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.50f, 2.3f);
                }
                //Shoot STRAIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Straight") || (animationNames.name == "Shoot Straight")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.50f, 2.3f);
                }

                ///////

                //Shoot UP while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.8f);
                }
                //Shoot UP while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.8f);
                }

                //Shoot UP while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.25f, 3.30f);
                }
                //Shoot UP while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 3.30f);
                }

                ///////

                //Shoot UP-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.5f);
                }
                //Shoot UP-LEFT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.5f);
                }

                //Shoot UP-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.25f, 2.90f);
                }
                //Shoot UP-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Hold Diagonal Up") || (animationNames.name == "Shoot Diagonal Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.25f, 2.90f);
                }

                ///////

                //Shoot DOWN while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 1.8f);
                }
                //Shoot DOWN while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.8f);
                }

                //Shoot DOWN while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.05f, 1.1f);
                }
                //Shoot DOWN while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.00f, 1.1f);
                }

                ///////

                //Shoot DOWN-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.6f);
                }
                //Shoot DOWN-LEFT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.6f);
                }

                //Shoot DOWN-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 1.4f);
                }
                //Shoot DOWN-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Hold Diagonal Down") || (animationNames.name == "Shoot Diagonal Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 1.4f);
                }

                ///////

                //Shoot STRAIGHT while WALKING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Straight Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 2.03f);
                }
                //Shoot STRAIGHT while WALKING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Shoot Straight Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 2.03f);
                }

                //Shoot STRAIGHT while WALKING and facing RIGHT Flame Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Straight Walking") || (animationNames.name == "Shoot Straight Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.55f, 2.25f);
                }
                //Shoot STRAIGHT while WALKING and facing LEFT Flame Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Shoot Straight Walking") || (animationNames.name == "Shoot Straight Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.60f, 2.25f);
                }

                ///////

                //Shoot UP-RIGHT while WALKING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun") && !theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.9f, 3.1f);
                }
                //Shoot UP-LEFT while WALKING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun") && !theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.9f, 3.1f);
                }

                //Shoot UP-RIGHT while WALKING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 2.9f);
                }
                //Shoot UP-LEFT while WALKING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 2.85f);
                }

                if (theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.5f);
                }
                if (theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking") || (animationNames.name == "Shoot Diagonal Up Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.5f);
                }

                //Shoot UP-RIGHT while WALKING and facing LEFT Ray Gun ***
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Ray Gun") || (weaponInventory.Content[0].ItemName == "Super Ray Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.8f, 2.9f);
                }
                //Shoot UP-LEFT while WALKING and facing LEFT Ray Gun ***
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Ray Gun") || (weaponInventory.Content[0].ItemName == "Super Ray Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Up") || (animationNames.name == "Shoot Diagonal Up Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.8f, 2.85f);
                }

                ///////

                //Shoot DOWN-RIGHT while WALKING while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.84f, 0.94f);
                }
                //Shoot DOWN-LEFT while WALKING while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.84f, 0.94f);
                }

                //Shoot DOWN-RIGHT while WALKING while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking Flame Gun")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 1.4f);
                }
                //Shoot DOWN-LEFT while WALKING while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Torso Walking Diagonal Down") || (animationNames.name == "Shoot Diagonal Down Walking Flame Gun")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 1.4f);
                }

                ///////

                //////////////////////////////////////////////////////////////////////////////////////

                //Jumps
                //Shoot RIGHT while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 1.85f);
                }
                //Shoot LEFT while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.85f);
                }
                //Shoot RIGHT while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 2.00f);
                }
                //Shoot LEFT while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2.00f);
                }

                ///////

                //Shoot UP while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.5f);
                }
                //Shoot UP while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.5f);
                }

                //Shoot UP while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.9f);
                }
                //Shoot UP while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") > 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.9f);
                }

                ///////

                //Shoot DOWN while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2f);
                }
                //Shoot DOWN while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2f);
                }

                //Shoot DOWN while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 1.6f);
                }
                //Shoot DOWN while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 1.6f);
                }

                ///////

                //Shoot UP-RIGHT while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 1.5f);
                }
                //Shoot UP-LEFT while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 1.5f);
                }

                //Shoot UP-RIGHT while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.9f);
                }
                //Shoot UP-LEFT while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 1.9f);
                }

                ///////

                //Shoot RIGHT-DOWN while JUMPING and facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2f);
                }
                //Shoot LEFT-DOWN while JUMPING and facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2f);
                }

                //Shoot RIGHT-DOWN while JUMPING and facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") > 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 1.5f);
                }
                //Shoot LEFT-DOWN while JUMPING and facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Jumping")) && !character.IsFacingRight && (player.GetAxisRaw("Vertical") < 0 && (player.GetAxisRaw("Horizontal") < 0)))
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.7f, 1.5f);
                }

                ///////

                //////////////////////////////////////////////////////////////////////////////////////

                //Crouch
                //Crouch IDLE while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting)  && ((animationNames.name == "Crouch") || (animationNames.name == "Crouch")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 0.75f);
                }
                //Crouch IDLE while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting)  && ((animationNames.name == "Crouch") || (animationNames.name == "Crouch")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 0.75f);
                }

                //Crouch IDLE  while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Crouch") || (animationNames.name == "Crouch")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.0f, 0.9f);
                }
                //Crouch IDLE  while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Crouch") || (animationNames.name == "Crouch")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.0f, 0.9f);
                }

                //Crouch SHOOTING while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Crouch Shooting") || (animationNames.name == "Crouch Shooting")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 0.75f);
                }
                //Crouch SHOOTING while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Crouch Shooting") || (animationNames.name == "Crouch Shooting")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 0.75f);
                }

                //Crouch SHOOTING while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Crouch Shooting") || (animationNames.name == "Crouch Shooting")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.0f, 0.9f);
                }
                //Crouch SHOOTING while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Crouch Shooting") || (animationNames.name == "Crouch Shooting")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.0f, 0.9f);
                }

                ///////

                //////////////////////////////////////////////////////////////////////////////////////
                //Vertical ladder

                //Vertical ladder shoot STRAIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward") || (animationNames.name == "Climb")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.3f, 2.1f);
                }
                //Vertical ladder shoot STRAIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward") || (animationNames.name == "Climb")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.3f, 2.1f);
                }

                //Vertical ladder shoot STRAIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward") || (animationNames.name == "Climb")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.35f, 2.3f);
                }
                //Vertical ladder shoot STRAIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Idle") || (animationNames.name == "Climb Shooting Forward") || (animationNames.name == "Climb")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.35f, 2.3f);
                }

                ///////

                //Vertical ladder shoot BACK while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.4f, 2.1f);
                }
                //Vertical ladder shoot BACK while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 2.1f);
                }

                //Vertical ladder shoot BACK while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.9f, 2.1f);
                }
                //Vertical ladder shoot BACK while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Back") || (animationNames.name == "Climb Shooting Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.8f, 2.1f);
                }

                ///////

                //Vertical ladder shoot UP while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 2.8f);
                }
                //Vertical ladder shoot UP while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 2.8f);
                }

                //Vertical ladder shoot UP while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.4f, 3.3f);
                }
                //Vertical ladder shoot UP while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Up") || (animationNames.name == "Climb Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.4f, 3.3f);
                }

                ///////

                //Vertical ladder shoot DOWN while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 1.3f);
                }
                //Vertical ladder shoot DOWN while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 1.3f);
                }

                //Vertical ladder shoot DOWN while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.25f, 0.8f);
                }
                //Vertical ladder shoot DOWN while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Down") || (animationNames.name == "Climb Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.25f, 0.8f);
                }

                ///////

                //Vertical ladder shoot UP-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.5f);
                }
                //Vertical ladder shoot UP-LEFT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.5f);
                }

                //Vertical ladder shoot UP-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.05f, 2.95f);
                }
                //Vertical ladder shoot UP-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Up Forward") || (animationNames.name == "Climb Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.11f, 2.95f);
                }

                ///////

                //Vertical ladder shoot UP-LEFT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.3f, 2.4f);
                }
                //Vertical ladder shoot UP-RIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.3f, 2.4f);
                }

                //Vertical ladder shoot UP-LEFT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.7f, 2.6f);
                }
                //Vertical ladder shoot UP-RIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Up Back") || (animationNames.name == "Climb Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.7f, 2.5f);
                }

                ///////

                //Vertical ladder shoot DOWN-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 1.6f);
                }
                //Vertical ladder shoot DOWN-RIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 1.6f);
                }

                //Vertical ladder shoot DOWN-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.25f, 1.5f);
                }
                //Vertical ladder shoot DOWN-RIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Down Forward") || (animationNames.name == "Climb Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.25f, 1.5f);
                }

                ///////

                //Vertical ladder shoot DOWN-LEFT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.5f, 1.7f);
                }
                //Vertical ladder shoot DOWN-LEFT while facing LEFT  Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.5f, 1.7f);
                }

                //Vertical ladder shoot DOWN-LEFT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.7f, 1.35f);
                }
                //Vertical ladder shoot DOWN-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Climb Hold Diagonal Down Back") || (animationNames.name == "Climb Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.7f, 1.35f);
                }

                ///////

                //////////////////////////////////////////////////////////////////////////////////////

                //Horizontal ladder

                //Horizontal ladder shoot STRAIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Idle") || (animationNames.name == "Horizontal Ladder Shooting Forward") || (animationNames.name == "Horizontal Ladder")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.85f);
                }
                //Horizontal ladder shoot STRAIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Idle") || (animationNames.name == "Horizontal Ladder Shooting Forward") || (animationNames.name == "Horizontal Ladder")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 1.85f);
                }

                //Horizontal ladder shoot STRAIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Idle") || (animationNames.name == "Horizontal Ladder Shooting Forward") || (animationNames.name == "Horizontal Ladder")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.5f, 2.15f);
                }
                //Horizontal ladder shoot STRAIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Idle") || (animationNames.name == "Horizontal Ladder Shooting Forward") || (animationNames.name == "Horizontal Ladder")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.5f, 2.15f);
                }
                

                ///////

                //Horizontal ladder shoot BACK while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Back") || (animationNames.name == "Horizontal Ladder Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.1f, 2.1f);
                }
                //Horizontal ladder shoot BACK while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Back") || (animationNames.name == "Horizontal Ladder Shooting Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.1f, 2.1f);
                }

                //Horizontal ladder shoot BACK while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Back") || (animationNames.name == "Horizontal Ladder Shooting Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.6f, 2.1f);
                }
                //Horizontal ladder shoot BACK while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Back") || (animationNames.name == "Horizontal Ladder Shooting Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.6f, 2.15f);
                }

                ///////

                //Horizontal ladder shoot UP while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Up") || (animationNames.name == "Horizontal Ladder Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 2.7f);
                }
                //Horizontal ladder shoot UP while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Up") || (animationNames.name == "Horizontal Ladder Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 2.7f);
                }

                //Horizontal ladder shoot UP while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Up") || (animationNames.name == "Horizontal Ladder Shooting Up")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.25f, 3.2f);
                }
                //Horizontal ladder shoot UP while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Up") || (animationNames.name == "Horizontal Ladder Shooting Up")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 3.2f);
                }

                ///////

                //Horizontal ladder shoot DOWN while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Down") || (animationNames.name == "Horizontal Ladder Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.2f, 1.1f);
                }
                //Horizontal ladder shoot DOWN while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Down") || (animationNames.name == "Horizontal Ladder Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 1.1f);
                }

                //Horizontal ladder shoot DOWN while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Down") || (animationNames.name == "Horizontal Ladder Shooting Down")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.0f, 0.55f);
                }
                //Horizontal ladder shoot DOWN while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Down") || (animationNames.name == "Horizontal Ladder Shooting Down")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.1f, 0.55f);
                }

                ///////

                //Horizontal ladder shoot UP-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2.3f);
                }
                //Horizontal ladder shoot UP-LEFT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0f, 2.3f);
                }

                //Horizontal ladder shoot UP-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.25f, 2.75f);
                }
                //Horizontal ladder shoot UP-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.25f, 2.70f);
                }

                ///////

                //Horizontal ladder shoot UP-LEFT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.1f, 2.3f);
                }
                //Horizontal ladder shoot UP-RIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.1f, 2.3f);
                }

                //Horizontal ladder shoot UP-LEFT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.4f, 2.55f);
                }
                //Horizontal ladder shoot UP-RIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Up Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.3f, 2.65f);
                }

                ///////

                //Horizontal ladder shoot DOWN-RIGHT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 1.4f);
                }
                //Horizontal ladder shoot DOWN-RIGHT while facing LEFT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.1f, 1.4f);
                }

                //Horizontal ladder shoot DOWN-RIGHT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Forward")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(0.2f, 1.25f);
                }
                //Horizontal ladder shoot DOWN-RIGHT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Forward") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Forward")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-0.35f, 1.3f);
                }

                ///////

                //Horizontal ladder shoot DOWN-LEFT while facing RIGHT Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.3f, 1.7f);
                }
                //Horizontal ladder shoot DOWN-LEFT while facing LEFT  Machine Gun ---
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun" || theSpecialShootAndRaycastVisualization.isShooting) && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.3f, 1.7f);
                }

                //Horizontal ladder shoot DOWN-LEFT while facing RIGHT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back")) && character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(-1.45f, 1.35f);
                }
                //Horizontal ladder shoot DOWN-LEFT while facing LEFT Flame Gun |||
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")) && !theSpecialShootAndRaycastVisualization.isShooting && ((animationNames.name == "Horizontal Ladder Hold Diagonal Down Back") || (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back")) && !character.IsFacingRight)
                {
                    theFirepoint.gameObject.transform.localPosition = new Vector3(1.4f, 1.3f);
                }

                ///////

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
                //This moves the Fire Burst or Smoke Particle Effect when playing some animations, so that it doesn't create the effect that the shot is way away from the burst.
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun" || theSpecialShootAndRaycastVisualization.isShooting))
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
                
                //////////////////////////////////////////////////////////////////////////////////////
                // Edits in the Fire Indicator
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")))
                {
                    theSmokeIndicator = GameObject.FindWithTag("SmokeIndicator");
                    if (theSmokeIndicator != null)
                    {
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

                        else if (((animationNames.name == "Hold Down") || (animationNames.name == "Shoot Down")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.8f, 0.4f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                        }
                        else if ((animationNames.name == "Shoot Diagonal Up Walking Flame Gun") &&
                                 character.IsFacingRight)
                        {
                            //theSmokeIndicator.transform.localPosition = new Vector3(-0.35f, 0.35f, 0.0f);
                        }
                        else if ((animationNames.name == "Shoot Diagonal Up Walking Flame Gun") &&
                                 !character.IsFacingRight)
                        {
                            //theSmokeIndicator.transform.localPosition = new Vector3(-0.2f, 0.3f, 0.0f);
                        }
                        else if (((animationNames.name == "Climb Shooting Back") ||
                                  (animationNames.name == "Climb Hold Back")) && character.IsFacingRight)
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.6f, -0.2f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(-180f, 90f, 0f);
                        }
                        else if (((animationNames.name == "Climb Shooting Back") ||
                                  (animationNames.name == "Climb Hold Back")) && !character.IsFacingRight)
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.625f, -0.2f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(-180f, 90f, 0f);
                        }
                        else if (((animationNames.name == "Climb Hold Up") ||
                                  (animationNames.name == "Climb Shooting Up")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.7f, 0.5f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                        }
                        else if (((animationNames.name == "Climb Hold Down") ||
                                  (animationNames.name == "Climb Shooting Down")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.7f, 0.5f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                        }
                        else if (((animationNames.name == "Climb Hold Diagonal Up Back") ||
                                  (animationNames.name == "Climb Shooting Diagonal Up Back")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(1.2f, -0.3f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
                        }
                        else if (((animationNames.name == "Climb Hold Diagonal Down Back") ||
                                  (animationNames.name == "Climb Shooting Diagonal Down Back")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.0f, -0.15f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 180f, 180f);
                        }
                        else if (((animationNames.name == "Horizontal Ladder Hold Back") ||
                                  (animationNames.name == "Horizontal Ladder Shooting Back")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.0f, -0.15f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 180f, 180f);
                        }
                        else if (((animationNames.name == "Horizontal Ladder Hold Up") ||
                                  (animationNames.name == "Horizontal Ladder Shooting Up")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.7f, 0.5f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                        }
                        else if (((animationNames.name == "Horizontal Ladder Hold Down") ||
                                  (animationNames.name == "Horizontal Ladder Shooting Down")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(0.7f, 0.5f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                        }
                        else if (((animationNames.name == "Horizontal Ladder Hold Diagonal Up Back") ||
                                  (animationNames.name == "Horizontal Ladder Shooting Diagonal Up Back")))
                        {
                            theSmokeIndicator.transform.localPosition = new Vector3(1.2f, -0.3f, 0.0f);
                            theSmokeIndicator.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
                        }
                        else if (((animationNames.name == "Horizontal Ladder Hold Diagonal Down Back") ||
                                  (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back")))
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

                //////////////////////////////////////////////////////////////////////////////////////
                // Edits in the Ray Indicator
                if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Ray Gun") || (weaponInventory.Content[0].ItemName == "Super Ray Gun")))
                {
                    theRayIndicator = GameObject.FindGameObjectsWithTag("RayIndicator");
                    foreach (var rayIndicator in theRayIndicator)
                    {
                        foreach (var weapon in weaponInventory.Content)
                        {
                            if (weapon != null && (weapon.ItemName == "Ray Gun" || weapon.ItemName == "Super Ray Gun"))
                            {
                                if ((animationNames.name == "Shoot Diagonal Up Walking") ||
                                    (animationNames.name == "Torso Walking Diagonal Up"))
                                {
                                    rayIndicator.transform.localPosition = new Vector3(0.45f, 0.2f, 0.0f);
                                }
                                else if ((animationNames.name == "Shoot Diagonal Down Walking") ||
                                         (animationNames.name == "Torso Walking Diagonal Down"))
                                {
                                    rayIndicator.transform.localPosition = new Vector3(0.05f, -0.1f, 0.0f);
                                }
                                else if ((animationNames.name == "Shoot Up") || (animationNames.name == "Hold Up"))
                                {
                                    rayIndicator.transform.localPosition = new Vector3(0.1f, 0.05f, 0.0f);
                                }
                                else if ((animationNames.name == "Shoot Down") || (animationNames.name == "Hold Down"))
                                {
                                    rayIndicator.transform.localPosition = new Vector3(0.2f, 0.00f, 0.0f);
                                }
                                else
                                {
                                    rayIndicator.transform.localPosition = new Vector3(1.0f, 0.0f, 0.0f);
                                }
                            }
                        }
                    }

                    sparks = GameObject.FindGameObjectsWithTag("Spark");
                    if (animationNames.name == "Horizontal Ladder")
                    {
                        foreach (var spark in sparks)
                        {
                            if (spark != null)
                            {
                                float horizontalInput = player.GetAxisRaw("Horizontal");
                                float verticalInput = player.GetAxisRaw("Vertical");
                                if (horizontalInput > 0 && verticalInput == 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.7f, 0.2f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90);
                                }
                                if (horizontalInput < 0 && verticalInput == 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.35f, 0.2f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90);
                                }
                                if (horizontalInput > 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.30f, 0.5f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 45);
                                }
                                if (horizontalInput > 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.5f, -0.2f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -45);
                                }
                                if (horizontalInput < 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.1f, 0.45f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 45);
                                }
                                if (horizontalInput < 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.5f, -0.2f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -45);
                                }
                            }
                        }
                    }
                    else if (animationNames.name == "Horizontal Ladder Shooting Diagonal Down Back" || animationNames.name == "Horizontal Ladder Hold Diagonal Down Back")
                    {
                        foreach (var spark in sparks)
                        {
                            if (spark != null)
                            {
                                spark.transform.localPosition = new Vector3(-0.1f, 0.2f, 0.0f);
                            }
                        }
                    }
                    else if (animationNames.name == "Climb" && character.IsFacingRight)
                    {
                        foreach (var spark in sparks)
                        {
                            if (spark != null)
                            {
                                float horizontalInput = player.GetAxisRaw("Horizontal");
                                float verticalInput = player.GetAxisRaw("Vertical");
                                if (horizontalInput == 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.30f, 0.40f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0);
                                }
                                if (horizontalInput == 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(0.2f, -0.4f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0);
                                }
                                if (horizontalInput > 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.5f, 0.00f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 45);
                                }
                                if (horizontalInput < 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(0.10f, 0.50f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -45);
                                }
                                if (horizontalInput > 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.1f, -0.4f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -45);
                                }
                                if (horizontalInput < 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(0.35f, -0.15f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 45);
                                }
                            }
                        }
                    }

                    else if (animationNames.name == "Climb" && !character.IsFacingRight)
                    {
                        foreach (var spark in sparks)
                        {
                            if (spark != null)
                            {
                                float horizontalInput = player.GetAxisRaw("Horizontal");
                                float verticalInput = player.GetAxisRaw("Vertical");
                                if (horizontalInput == 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.35f, 0.40f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0);
                                }
                                if (horizontalInput == 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(0.2f, -0.4f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0);
                                }
                                if (horizontalInput > 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(0.0f, 0.50f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -45);
                                }
                                if (horizontalInput < 0 && verticalInput > 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.60f, 0.00f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 45);
                                }
                                if (horizontalInput > 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(0.40f, -0.1f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 45);
                                }
                                if (horizontalInput < 0 && verticalInput < 0)
                                {
                                    spark.transform.localPosition = new Vector3(-0.05f, -0.40f, 0.0f);
                                    spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -45);
                                }
                            }
                        }
                    }

                    else if (animationNames.name == "Climb Shooting Diagonal Down Back" || animationNames.name == "Climb Hold Diagonal Down Back")
                    {
                        foreach (var spark in sparks)
                        {
                            if (spark != null)
                            {
                                spark.transform.localPosition = new Vector3(-0.15f, 0.15f, 0.0f);
                            }
                        }
                    }
                    
                    else
                    {
                        foreach (var spark in sparks)
                        {
                            if (spark != null)
                            {
                                float horizontalInput = player.GetAxisRaw("Horizontal");
                                float verticalInput = player.GetAxisRaw("Vertical");
                                spark.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                                spark.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                            }
                        }
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
