using System.Buffers;
using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using Rewired;
using UnityEngine;
public class OverridesInAnimator : MonoBehaviour
{
    public bool machineGun;
    public bool flameGun;
    public bool rayGun;
    public bool startTimerBeforeNextAnim = false;
    public float customTimeBeforeNextAnim = 0.03f;
    public float initialTimeBeforeNextAnim = 0.03f;
    public float currentTimeBeforeNextAnim = 0.03f;
    //public bool timerBeforeNextAnim;
   // public bool modifyTheMirror;
    public Animator theAnimator;
    public Inventory weaponInventory;
    public CharacterHandleWeapon theCharacterHandleWeapon;
    public Player player;
    public ChargeWeapon theChargeWeapon;
    public AnimationClip[] animationNames;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Start()
    {
        theAnimator = GameObject.FindGameObjectWithTag("PlayerSprites").GetComponent<Animator>();
        animationNames = Resources.LoadAll<AnimationClip>("Player Animations");
        weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventoryPlayer1").GetComponent<Inventory>();
        theCharacterHandleWeapon = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterHandleWeapon>();
    }

    void Update()
    {
        if (GetComponent<Character>().ConditionState.CurrentState != CharacterStates.CharacterConditions.Dead)
        {
            if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun"))
            {
                machineGun = true;
                flameGun = false;
                rayGun = false;
                //modifyTheMirror = false;
                //modifyTheMirror = true;
                theAnimator.SetBool("Flame Gun", false);
                theAnimator.SetBool("Charging", false);
                //foreach (AnimationClip clip in animationNames)
                {
                    theAnimator.SetFloat("Delay", 1f);
                    ////timerBeforeNextAnim = theAnimator.GetBool("TimerBeforeNextAnim");
                    //theAnimator.SetBool("Mirror", timerBeforeNextAnim);
                }
            }

            if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")))
            {
                flameGun = true;
                machineGun = false;
                rayGun = false;
                theAnimator.SetBool("Charging", false);
                //modifyTheMirror = true;
                SpecialShootAndRaycastVisualization theSpecialShootAndRaycastVisualization = GetComponent<SpecialShootAndRaycastVisualization>();

                if (theSpecialShootAndRaycastVisualization.isShooting)
                {
                    theAnimator.SetBool("Flame Gun", false);
                    machineGun = true;
                    theAnimator.SetFloat("Delay", 1f);
                }
                else
                {
                    theAnimator.SetBool("Flame Gun", true);
                    theAnimator.SetFloat("Delay", 0f);
                }

                //foreach (AnimationClip clip in animationNames)
                {
                    //theAnimator.SetFloat("Delay", 0f);
                    ////timerBeforeNextAnim = theAnimator.GetBool("TimerBeforeNextAnim");
                    //theAnimator.SetBool("Mirror", timerBeforeNextAnim);
                }
            }

            if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun"))
            {
                rayGun = true;
                machineGun = false;
                flameGun = false;
                //modifyTheMirror = false;
                //modifyTheMirror = true;
                theAnimator.SetBool("Flame Gun", false);
                //foreach (AnimationClip clip in animationNames)
                {
                    theAnimator.SetFloat("Delay", 1f);
                    ////timerBeforeNextAnim = theAnimator.GetBool("TimerBeforeNextAnim");
                    //theAnimator.SetBool("Mirror", timerBeforeNextAnim);
                }
                theChargeWeapon = GameObject.FindWithTag("WeaponAim").GetComponent<ChargeWeapon>();
                if (theChargeWeapon != null && theChargeWeapon.Charging)
                {
                    theAnimator.SetBool("Charging", true);
                }
                else
                {
                    theAnimator.SetBool("Charging", false);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes the animator stay on "shooting" if the "Fire" of the weapon is still active. For the Machine Gun it should be 0 always.
        if (theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse && weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun"))
        {
            initialTimeBeforeNextAnim = 0.0f;
            currentTimeBeforeNextAnim = initialTimeBeforeNextAnim;
            startTimerBeforeNextAnim = false;
        }

        if (theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse && weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun"))
        {
            initialTimeBeforeNextAnim = customTimeBeforeNextAnim;
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
        //This makes that if you are in the middle of the animation using the Machine Gun, all clips down below will be reset
        //if (Input.GetKeyDown(KeyCode.E))
        if (theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse &&
            weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && ((weaponInventory.Content[0].ItemName == "Flame Gun") || (weaponInventory.Content[0].ItemName == "Super Flame Gun")))
        {
            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Straight"))
            {
                theAnimator.PlayInFixedTime("Shoot Straight", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Up"))
            {
                theAnimator.PlayInFixedTime("Shoot Up", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Up"))
            {
                theAnimator.PlayInFixedTime("Shoot Diagonal Up", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Down"))
            {
                theAnimator.PlayInFixedTime("Shoot Down", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Down"))
            {
                theAnimator.PlayInFixedTime("Shoot Diagonal Down", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Crouch Shooting"))
            {
                theAnimator.PlayInFixedTime("Crouch Shooting", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Up"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Up", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Diagonal Up Forward"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Diagonal Up Forward", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Forward"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Forward", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Diagonal Down Forward"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Diagonal Down Forward", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Down"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Down", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Diagonal Down Back"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Diagonal Down Back", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Back"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Back", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Horizontal Ladder Shooting Diagonal Up Back"))
            {
                theAnimator.PlayInFixedTime("Horizontal Ladder Shooting Diagonal Up Back", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Up"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Up", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Diagonal Up Back"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Diagonal Up Back", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Forward"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Forward", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Diagonal Down Back"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Diagonal Down Back", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Down"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Down", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Diagonal Down Forward"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Diagonal Down Forward", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Back"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Back", 1, 0f);
            }

            if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Climb Shooting Diagonal Up Forward"))
            {
                theAnimator.PlayInFixedTime("Climb Shooting Diagonal Up Forward", 1, 0f);
            }
        }
    }
}
