using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using Rewired.ComponentControls.Data;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleWeapons : MonoBehaviour
{
    public string torsoTag = "Torso";
    public string machineGunLightsTag = "MachineGunLights";
    public string flameGunLightsTag = "FlameGunLights";
    public GameObject torsoObject;
    public GameObject machineGunLights;
    public GameObject flameGunLights;
    public bool torsoNoLights = true;
    public bool machineGunActive = false;
    public bool flameGunActive = false;
    public Inventory weaponInventory;
    public SpriteRenderer torsoSpriteRenderer;
    public SpriteRenderer legsSpriteRenderer;
    public Material originalMaterial;
    public Material flameGunMaterial;
    //public CharacterHandleWeapon theCharacterHandleWeapon;
    //public Animator theAnimator;

    void Start()
    {
        torsoObject = GameObject.FindGameObjectWithTag(torsoTag);
        machineGunLights = GameObject.FindGameObjectWithTag(machineGunLightsTag);
        flameGunLights = GameObject.FindGameObjectWithTag(flameGunLightsTag);
        weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventory").GetComponent<Inventory>();
        //theCharacterHandleWeapon = FindObjectOfType<CharacterHandleWeapon>();
        //theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>();
        torsoSpriteRenderer = GameObject.FindGameObjectWithTag("Torso").GetComponent<SpriteRenderer>();
        legsSpriteRenderer = GameObject.FindGameObjectWithTag("Legs").GetComponent<SpriteRenderer>();
        originalMaterial = torsoSpriteRenderer.material;

        if (torsoObject != null)
        {
            torsoObject.SetActive(torsoNoLights);
        }

        if (machineGunLights != null)
        {
            machineGunLights.SetActive(!torsoNoLights);
            machineGunActive = !torsoNoLights;
        }
    }

    void Update()
    {
        if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            weaponInventory.Content[0].ItemName == "Machine Gun")
        {
            if (GetComponent<CharacterHandleWeapon>().CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle)
            {
                torsoNoLights = false;
                machineGunActive = true;
                flameGunActive = false;
                torsoObject.SetActive(false);
                machineGunLights.SetActive(true);
                flameGunLights.SetActive(false);
                torsoSpriteRenderer.material = originalMaterial;
                legsSpriteRenderer.material = originalMaterial;
            }
            else
            {
                torsoNoLights = true;
                machineGunActive = false;
                flameGunActive = false;
                torsoObject.SetActive(true);
                machineGunLights.SetActive(false);
                flameGunLights.SetActive(false);
            }
        }

        if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            weaponInventory.Content[0].ItemName == "Flame Gun")
        {
            if (GetComponent<CharacterHandleWeapon>().CurrentWeapon.WeaponState.CurrentState !=
                Weapon.WeaponStates.WeaponIdle && GetComponent<Character>().MovementState.CurrentState != CharacterStates.MovementStates.Rolling)
            {
                torsoNoLights = false;
                machineGunActive = false;
                flameGunActive = true;
                torsoObject.SetActive(false);
                machineGunLights.SetActive(false);
                flameGunLights.SetActive(true);
                flameGunLights.GetComponent<SpriteRenderer>().material = flameGunMaterial;
                legsSpriteRenderer.material = flameGunMaterial;
            }
            /*else
            {
                torsoNoLights = true;
                machineGunActive = false;
                flameGunActive = false;
                torsoObject.SetActive(true);
                machineGunLights.SetActive(false);
                flameGunLights.SetActive(false);
                flameGunLights.GetComponent<SpriteRenderer>().material = originalMaterial;
                torsoSpriteRenderer.material = originalMaterial;
                legsSpriteRenderer.material = originalMaterial;
            }*/
        }

        /*else
        {
            torsoNoLights = true;
            machineGunActive = false;
            flameGunActive = false;
            torsoObject.SetActive(true);
            machineGunLights.SetActive(false);
            flameGunLights.SetActive(false);
        }

    else
    {
        torsoNoLights = true;
        machineGunActive = false;
        flameGunActive = false;
        if (torsoObject != null && machineGunLights != null)
        {
            machineGunLights.SetActive(false);
            flameGunLights.SetActive(false);
            torsoObject.SetActive(true);
        }
    }}*/
    }
}