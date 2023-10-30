using MoreMountains.InventoryEngine;
using UnityEngine;
public class OverridesInAnimator : MonoBehaviour
{
    public bool machineGun;
    public bool flameGun;
    public bool timerBeforeNextAnim;
    public bool modifyTheMirror;
    public Animator theAnimator;
    public AnimationClip[] animationNames;
    public Inventory weaponInventory;

    void Start()
    {
        theAnimator = GameObject.FindGameObjectWithTag("PlayerSprites").GetComponent<Animator>();
        animationNames = Resources.LoadAll<AnimationClip>("Player Animations");
        weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventory").GetComponent<Inventory>();
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
                timerBeforeNextAnim = theAnimator.GetBool("TimerBeforeNextAnim");
                theAnimator.SetBool("Mirror", timerBeforeNextAnim);
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
                timerBeforeNextAnim = theAnimator.GetBool("TimerBeforeNextAnim");
                theAnimator.SetBool("Mirror", timerBeforeNextAnim);
            }
        }
    }
}
