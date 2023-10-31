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
    public GameObject torsoObject;
    public GameObject machineGunLights;
    public bool torsoNoLights = true;
    public bool machineGunActive = false;
    public Inventory weaponInventory;
    //public CharacterHandleWeapon theCharacterHandleWeapon;
    //public Animator theAnimator;

    void Start()
    {
        torsoObject = GameObject.FindGameObjectWithTag(torsoTag);
        machineGunLights = GameObject.FindGameObjectWithTag(machineGunLightsTag);
        weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventory").GetComponent<Inventory>();
        //theCharacterHandleWeapon = FindObjectOfType<CharacterHandleWeapon>();
        //theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>();
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
                torsoObject.SetActive(false);
                machineGunLights.SetActive(true);
            }
            else
            {
                torsoNoLights = true;
                machineGunActive = false;
                torsoObject.SetActive(true);
                machineGunLights.SetActive(false);
            }
        }
        else
        {
            machineGunActive = false;
            torsoNoLights = true;
            if (torsoObject != null && machineGunLights != null)
            {
                machineGunLights.SetActive(false);
                torsoObject.SetActive(true);
            }
        }
    }
}