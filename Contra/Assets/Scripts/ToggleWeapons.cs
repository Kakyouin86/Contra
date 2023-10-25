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
    public GameObject machineGunObject;
    public bool torsoObjectActive = true;
    public bool machineGunActive = false;
    public Inventory weaponInventory;

    void Start()
    {
        torsoObject = GameObject.FindGameObjectWithTag(torsoTag);
        machineGunObject = GameObject.FindGameObjectWithTag(machineGunLightsTag);
        weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventory").GetComponent<Inventory>();
        if (torsoObject != null)
        {
            torsoObject.SetActive(torsoObjectActive);
        }

        if (machineGunObject != null)
        {
            machineGunObject.SetActive(!torsoObjectActive);
            machineGunActive = !torsoObjectActive;
        }
    }

    void Update()
    {
        if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null &&
            weaponInventory.Content[0].ItemName == "Machine Gun")
        {
            if (GetComponent<CharacterHandleWeapon>().CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle)

            {
                torsoObjectActive = false;
                machineGunActive = true;
                torsoObject.SetActive(false);
                machineGunObject.SetActive(true);
            }
            else
            {
                torsoObjectActive = true;
                machineGunActive = false;
                torsoObject.SetActive(true);
                machineGunObject.SetActive(false);
            }
        }
        else
        {
            machineGunActive = false;
            torsoObjectActive = true;
            if (torsoObject != null && machineGunObject != null)
            {
                machineGunObject.SetActive(false);
                torsoObject.SetActive(true);
            }
        }
    }
}