using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using UnityEngine;
public class OverridesInAnimator : MonoBehaviour
{
    public bool machineGun;
    public bool flameGun;
    public bool startTimerBeforeNextAnim = false;
    public float initialTimeBeforeNextAnim = 0.05f;
    public float currentTimeBeforeNextAnim = 0.05f;
    public bool timerBeforeNextAnim;
    public bool modifyTheMirror;
    public Animator theAnimator;
    public AnimationClip[] animationNames;
    public Inventory weaponInventory;
    public CharacterHandleWeapon theCharacterHandleWeapon;

    void Start()
    {
        theAnimator = GameObject.FindGameObjectWithTag("PlayerSprites").GetComponent<Animator>();
        animationNames = Resources.LoadAll<AnimationClip>("Player Animations");
        weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventory").GetComponent<Inventory>();
        theCharacterHandleWeapon = FindObjectOfType<CharacterHandleWeapon>();
    }

    void Update()
    {
        if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            weaponInventory.Content[0].ItemName == "Flame Gun")
        {
            flameGun = true;
            machineGun = false;
            //modifyTheMirror = true;
            theAnimator.SetBool("Flame Gun", true);
            //foreach (AnimationClip clip in animationNames)
            {
                theAnimator.SetFloat("Delay", 0f);
                ////timerBeforeNextAnim = theAnimator.GetBool("TimerBeforeNextAnim");
                //theAnimator.SetBool("Mirror", timerBeforeNextAnim);
            }
        }

        if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            weaponInventory.Content[0].ItemName == "Machine Gun")
        {
            machineGun = true;
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
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //This makes the animator stay on "shooting" if the "Fire" of the weapon is still active. For the Machine Gun it should be 0 always.
        if (theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse && weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            weaponInventory.Content[0].ItemName == "Machine Gun")
        {
            initialTimeBeforeNextAnim = 0.0f;
            currentTimeBeforeNextAnim = initialTimeBeforeNextAnim;
            startTimerBeforeNextAnim = false;
        }

        if (theCharacterHandleWeapon.CurrentWeapon.WeaponState.CurrentState == Weapon.WeaponStates.WeaponUse && weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            weaponInventory.Content[0].ItemName != "Machine Gun")
        {
            initialTimeBeforeNextAnim = 0.05f;
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
    }
}
