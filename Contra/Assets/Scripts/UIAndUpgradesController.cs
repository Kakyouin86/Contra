using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using Rewired;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIAndUpgradesController : MonoBehaviour
{
    [Header("Components")]
    public Image avatarPlayer1;
    public Image theUIImagePlayer1;
    public Image allTheEmptySlotsPlayer1;
    public Image slot1MachineGunPlayer1;
    public Image slot2FlameGunPlayer1;
    public Image slot2FlameGunPlusPlayer1;
    public Image slot3RayGunPlayer1;
    public Image slot3RayGunPlusPlayer1;
    public Image slot4ShotGunPlayer1;
    public Image slot4ShotGunPlusPlayer1;
    public Image slot5SpreadGunPlayer1;
    public Image slot5SpreadGunPlusPlayer1;
    public Image slot6GrenadePlayer1;
    public Image slot6GrenadePlusPlayer1;
    public TextMeshProUGUI grenadesPlayer1;
    public TextMeshProUGUI livesPlayer1;
    public Inventory theInventory;
    public Inventory theWeaponInventory;
    public ToggleWeapons thePlayer;
    [ColorUsage(true, true)]
    public Color fadingColor = new Color(130 / 255f, 130 / 255f, 130 / 255f, 255 / 255f);
    [ColorUsage(true, true)]
    public Color originalColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);

    [Header("Gauge components")]
    public Image theGauge;

    [Header("Weapon parameters")]
    public string machineGunName = "Machine Gun";
    public string flameGunName = "Flame Gun";
    public string flameGunPlusName = "Super Flame Gun";
    public bool flameGunUpgrade = false;
    public string rayGunName = "Ray Gun";
    public string rayGunPlusName = "Super Ray Gun";
    public bool rayGunUpgrade = false;
    public string shotGunName = "Shot Gun";
    public string shotGunPlusName = "Super Shot Gun";
    public bool shotGunUpgrade = false;
    public string spreadGunName = "Spread Gun";
    public string spreadGunPlusName = "Super Spread Gun";
    public bool spreadGunUpgrade = false;
    public string grenadePlusName = "Super Grenade";
    public bool grenadePlus = false;

    [Header("Weapon In Use & Acquired Power Ups")]
    public bool machineGun = false;
    public bool flameGun = false;
    public bool flameGunPlus = false;
    public bool rayGun = false;
    public bool rayGunPlus = false;
    public bool shotGun = false;
    public bool shotGunPlus = false;
    public bool spreadGun = false;
    public bool spreadGunPlus = false;
    public bool doubleJump = false;
    public bool dash = false;
    public bool specialShot = false;

    void Start()
    {
        theInventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        theWeaponInventory = GameObject.FindWithTag("WeaponInventory").GetComponent<Inventory>();
        thePlayer = GameObject.FindWithTag("Player").GetComponent<ToggleWeapons>();
        avatarPlayer1.enabled = true;
        theUIImagePlayer1.enabled = true;
        allTheEmptySlotsPlayer1.enabled = true;
        slot1MachineGunPlayer1.enabled = true;
        slot2FlameGunPlayer1.enabled = false;
        slot2FlameGunPlusPlayer1.enabled = false;
        slot3RayGunPlayer1.enabled = false;
        slot3RayGunPlusPlayer1.enabled = false;
        slot4ShotGunPlayer1.enabled = false;
        slot4ShotGunPlusPlayer1.enabled = false;
        slot5SpreadGunPlayer1.enabled = false;
        slot5SpreadGunPlusPlayer1.enabled = false;
        if (!grenadePlus)
        {
            slot6GrenadePlayer1.enabled = true;
            slot6GrenadePlusPlayer1.enabled = false;
        }
        else
        {
            slot6GrenadePlayer1.enabled = false;
            slot6GrenadePlusPlayer1.enabled = true;
        }

        //This will check default weapons only. If there are some power ups, it will disable the legacy versions.
        for (int i = 0; i < theInventory.Content.Length; i++)
        {
            //Debug.Log("Content[" + i + "] = " + theInventory.Content[i]);
            if (theInventory.Content[i] != null && theInventory.Content[i].ItemName == flameGunName)
            {
                slot2FlameGunPlayer1.enabled = true;
            }
            if (theInventory.Content[i] != null && theInventory.Content[i].ItemName == rayGunName)
            {
                slot3RayGunPlayer1.enabled = true;
            }
            if (theInventory.Content[i] != null && theInventory.Content[i].ItemName == shotGunName)
            {
                slot4ShotGunPlayer1.enabled = true;
            }
            if (theInventory.Content[i] != null && theInventory.Content[i].ItemName == spreadGunName)
            {
                slot5SpreadGunPlayer1.enabled = true;
            }
        }

        //Here we check for the upgraded versions and remove the legacy ones if needed.
        for (int i = 0; i < theInventory.Content.Length; i++)
        {
            //Debug.Log("Content[" + i + "] = " + theInventory.Content[i]);
            if (theInventory.Content[i] != null && flameGunUpgrade && (theInventory.Content[i].ItemName == flameGunName || theInventory.Content[i].ItemName == flameGunPlusName))
            {
                slot2FlameGunPlayer1.enabled = false;
                slot2FlameGunPlusPlayer1.enabled = true;
            }
            if (theInventory.Content[i] != null && rayGunUpgrade && (theInventory.Content[i].ItemName == rayGunName || theInventory.Content[i].ItemName == rayGunPlusName))
            {
                slot3RayGunPlayer1.enabled = false;
                slot3RayGunPlusPlayer1.enabled = true;
            }
            if (theInventory.Content[i] != null && shotGunUpgrade && (theInventory.Content[i].ItemName == shotGunName || theInventory.Content[i].ItemName == shotGunPlusName))
            {
                slot4ShotGunPlayer1.enabled = false;
                slot4ShotGunPlusPlayer1.enabled = true;
            }
            if (theInventory.Content[i] != null && spreadGunUpgrade && (theInventory.Content[i].ItemName == spreadGunName || theInventory.Content[i].ItemName == spreadGunPlusName))
            {
                slot5SpreadGunPlayer1.enabled = false;
                slot5SpreadGunPlusPlayer1.enabled = true;
            }
            if (theInventory.Content[i] != null && grenadePlus && (theInventory.Content[i].ItemName == grenadePlusName))
            {
                slot6GrenadePlayer1.enabled = false;
                slot6GrenadePlusPlayer1.enabled = true;
            }
        }

        //Here I pass the upgrade in the grenades to the Player so it autoloads the correct grenade in his inventory.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CharacterInventory playerInventory = player.GetComponent<CharacterInventory>();
        if (playerInventory != null && grenadePlus)
        {
            playerInventory.hasUpgradedGrenades = true;
        }
        else
        {
            playerInventory.hasUpgradedGrenades = false;
        }
        CharacterHandleSecondaryWeapon playerSecondaryWeapon = player.GetComponent<CharacterHandleSecondaryWeapon>();
        if (playerSecondaryWeapon != null && grenadePlus)
        {
            playerSecondaryWeapon.hasUpgradedGrenades = true;
        }
        else
        {
            playerSecondaryWeapon.hasUpgradedGrenades = false;
        }
        playerSecondaryWeapon.ChangeWeapon();

        //Here we disable all the power ups that should not be present if I purchased a power up.
        DisableWeapons();

        //Related to power ups and gauge.
        if (doubleJump)
        {
            thePlayer.GetComponent<CharacterJump>().NumberOfJumps = 2;
        }
        else
        {
            thePlayer.GetComponent<CharacterJump>().NumberOfJumps = 1;
        }
        if (dash)
        {
            thePlayer.GetComponent<CharacterRoll>().AbilityPermitted = true;
        }
        else
        {
            thePlayer.GetComponent<CharacterRoll>().AbilityPermitted = false;
        }
        if (specialShot)
        {
            theGauge.enabled = true;
        }
        else
        {
            theGauge.enabled = false;
        }
    }

    void Update()
    {
        GameObject grenadesLeft = GameObject.FindGameObjectWithTag("Grenade");
        grenadesPlayer1.text = grenadesLeft.GetComponent<WeaponAmmo>().CurrentAmmoAvailable.ToString();
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        livesPlayer1.text = "X" + gameController.GetComponent<GameManager>().CurrentLives;

        if (thePlayer.machineGunActive || theWeaponInventory.Content[0] != null && theWeaponInventory.Content[0].ItemName == machineGunName)
        {
            slot1MachineGunPlayer1.color = originalColor;
            slot2FlameGunPlayer1.color = fadingColor;
            slot2FlameGunPlusPlayer1.color = fadingColor;
            slot3RayGunPlayer1.color = fadingColor;
            slot3RayGunPlusPlayer1.color = fadingColor;
            slot4ShotGunPlayer1.color = fadingColor;
            slot4ShotGunPlusPlayer1.color = fadingColor;
            slot5SpreadGunPlayer1.color = fadingColor;
            slot5SpreadGunPlusPlayer1.color = fadingColor;
        }

        if (thePlayer.flameGunActive && !flameGunPlus || theWeaponInventory.Content[0] != null && theWeaponInventory.Content[0].ItemName == flameGunName)
        {
            slot1MachineGunPlayer1.color = fadingColor;
            slot2FlameGunPlayer1.color = originalColor;
            slot2FlameGunPlusPlayer1.color = fadingColor;
            slot3RayGunPlayer1.color = fadingColor;
            slot3RayGunPlusPlayer1.color = fadingColor;
            slot4ShotGunPlayer1.color = fadingColor;
            slot4ShotGunPlusPlayer1.color = fadingColor;
            slot5SpreadGunPlayer1.color = fadingColor;
            slot5SpreadGunPlusPlayer1.color = fadingColor;
        }

        if (thePlayer.flameGunActive && flameGunPlus || theWeaponInventory.Content[0] != null && theWeaponInventory.Content[0].ItemName == flameGunPlusName)
        {
            slot1MachineGunPlayer1.color = fadingColor;
            slot2FlameGunPlayer1.color = fadingColor;
            slot2FlameGunPlusPlayer1.color = originalColor;
            slot3RayGunPlayer1.color = fadingColor;
            slot3RayGunPlusPlayer1.color = fadingColor;
            slot4ShotGunPlayer1.color = fadingColor;
            slot4ShotGunPlusPlayer1.color = fadingColor;
            slot5SpreadGunPlayer1.color = fadingColor;
            slot5SpreadGunPlusPlayer1.color = fadingColor;
        }
    }

    public void DisableWeapons()
    {
        // Get all game objects with the "NormalWeapon" tag
        GameObject[] normalWeapons = GameObject.FindGameObjectsWithTag("NormalWeapon");
        foreach (var normalWeapon in normalWeapons)
        {
            // Check if the GameObject has the "ItemMessageToUI" script
            ItemMessageToUI itemMessageScript = normalWeapon.GetComponent<ItemMessageToUI>();

            // If the script is found, set active accordingly
            if (itemMessageScript != null)
            {
                normalWeapon.SetActive(!ShouldDisableWeapon(itemMessageScript));
            }
        }

        // Get all game objects with the "UpgradedWeapon" tag
        GameObject[] upgradedWeapons = GameObject.FindGameObjectsWithTag("UpgradedWeapon");
        foreach (var upgradedWeapon in upgradedWeapons)
        {
            // Check if the GameObject has the "ItemMessageToUI" script
            ItemMessageToUI itemMessageScript = upgradedWeapon.GetComponent<ItemMessageToUI>();

            // If the script is found, set active accordingly
            if (itemMessageScript != null)
            {
                upgradedWeapon.SetActive(ShouldDisableWeapon(itemMessageScript));
            }
        }
    }

    bool ShouldDisableWeapon(ItemMessageToUI itemMessageScript)
    {
        // Add more conditions as needed for other weapons
        return (flameGunUpgrade && itemMessageScript.flameGun) ||
               (rayGunUpgrade && itemMessageScript.rayGun) ||
               (shotGunUpgrade && itemMessageScript.shotGun) ||
               (spreadGunUpgrade && itemMessageScript.spreadGun) ||
               (grenadePlus && itemMessageScript.grenade);
    }
    
    public void FlameGun()
    {
        if (!flameGunUpgrade)
        {
            slot2FlameGunPlayer1.enabled = true;
            slot2FlameGunPlusPlayer1.enabled = false;
        }
        else
        {
            slot2FlameGunPlayer1.enabled = false;
            slot2FlameGunPlusPlayer1.enabled = true;
        }
    }

    public void RayGun()
    {
        if (!rayGunUpgrade)
        {
            slot3RayGunPlayer1.enabled = true;
            slot3RayGunPlusPlayer1.enabled = false;
        }
        else
        {
            slot3RayGunPlayer1.enabled = false;
            slot3RayGunPlusPlayer1.enabled = true;
        }
    }

    public void ShotGun()
    {
        if (!shotGunUpgrade)
        {
            slot4ShotGunPlayer1.enabled = true;
            slot4ShotGunPlusPlayer1.enabled = false;
        }
        else
        {
            slot4ShotGunPlayer1.enabled = false;
            slot4ShotGunPlusPlayer1.enabled = true;
        }
    }

    public void SpreadGun()
    {
        if (!shotGunUpgrade)
        {
            slot5SpreadGunPlayer1.enabled = true;
            slot5SpreadGunPlusPlayer1.enabled = false;
        }
        else
        {
            slot5SpreadGunPlayer1.enabled = false;
            slot5SpreadGunPlusPlayer1.enabled = true;
        }
    }

    public void Grenade()
    {
        if (!grenadePlus)
        {
            slot6GrenadePlayer1.enabled = true;
            slot6GrenadePlusPlayer1.enabled = false;
        }
        else
        {
            slot6GrenadePlayer1.enabled = false;
            slot6GrenadePlusPlayer1.enabled = true;
        }
    }
}
