using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using Rewired;
using UnityEngine;

public class ToggleWeapons : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public bool isPlayer1;
    public string torsoTag = "Torso";
    public string machineGunLightsTag = "MachineGunLights";
    public string flameGunLightsTag = "FlameGunLights";
    public GameObject torsoObject;
    public GameObject machineGunLights;
    public GameObject flameGunLights;
    public bool torsoNoLights = true;
    public bool machineGunActive = false;
    public bool flameGunActive = false;
    public bool rayGunActive = false;
    public bool specialShootActive = false;
    public UIAndUpgradesController theUIAndUpgradesController;
    public SpecialShootAndRaycastVisualization theSpecialShootAndRaycastVisualization;
    public Inventory weaponInventory;
    public Animator theAnimator;
    public SpriteRenderer torsoSpriteRenderer;
    public SpriteRenderer legsSpriteRenderer;
    public Material originalMaterial;
    public Material flameGunMaterial;
    public Material rayGunMaterial;
    public Material specialShootMaterial;
    //public CharacterHandleWeapon theCharacterHandleWeapon;
    //public Animator theAnimator;

    public CharacterInventory theCharacterInventory;

    void Start()
    {
        //torsoObject = GameObject.FindGameObjectWithTag(torsoTag); This is the 1 player version. The next line is the 2 players version.
        //machineGunLights = GameObject.FindGameObjectWithTag(machineGunLightsTag); This is the 1 player version. The next line is the 2 players version.
        //flameGunLights = GameObject.FindGameObjectWithTag(flameGunLightsTag); This is the 1 player version. The next line is the 2 players version.
        //theUIAndUpgradesController = GameObject.FindGameObjectWithTag("UIPlayer1").GetComponent<UIAndUpgradesController>(); This is the 1 player version. The next line is the 2 players version.
        //weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventoryPlayer1").GetComponent<Inventory>(); This is the 1 player version. The next line is the 2 players version.
        //theAnimator = GameObject.FindGameObjectWithTag("PlayerSprites").GetComponent<Animator>(); This is the 1 player version. The next line is the 2 players version.
        //torsoSpriteRenderer = GameObject.FindGameObjectWithTag("Torso").GetComponent<SpriteRenderer>(); This is the 1 player version. The next line is the 2 players version.
        //legsSpriteRenderer = GameObject.FindGameObjectWithTag("Legs").GetComponent<SpriteRenderer>(); This is the 1 player version. The next line is the 2 players version.
        theCharacterInventory = GetComponent<CharacterInventory>();
        theSpecialShootAndRaycastVisualization = GetComponent<SpecialShootAndRaycastVisualization>();
        originalMaterial = torsoSpriteRenderer.material;

        if (GetComponent<Character>() != null && GetComponent<Character>().PlayerID == "Player1")
        {
            isPlayer1 = true;
            theUIAndUpgradesController = GameObject.FindGameObjectWithTag("UIPlayer1").GetComponent<UIAndUpgradesController>();
            weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventoryPlayer1").GetComponent<Inventory>();
        }
        else
        {
            isPlayer1 = false;
            theUIAndUpgradesController = GameObject.FindGameObjectWithTag("UIPlayer2").GetComponent<UIAndUpgradesController>();
            weaponInventory = GameObject.FindGameObjectWithTag("WeaponInventoryPlayer2").GetComponent<Inventory>();
        }

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

    protected virtual void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }

    /// <summary>
    /// OnDisable, we stop listening to events.
    /// </summary>
    protected virtual void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public void OnMMEvent(CorgiEngineEvent corgiEngineEvent)
    {
        if (corgiEngineEvent.EventType == CorgiEngineEventTypes.PlayerDeath)
        {
            GameObject theMainInventory = GameObject.FindWithTag("InventoryPlayer1");
            string weaponName = weaponInventory.Content[0].ItemName;

            if ((weaponName != "Machine Gun" && weaponName != "Super Machine Gun") && weaponInventory.Content[0].ItemName != null)
            { 
                weaponInventory.DestroyItem(0);
            }

            if (theUIAndUpgradesController.machineGunUpgrade)
            {
                theCharacterInventory.EquipWeapon("Inventory 02 - Super Machine Gun");
            }
            else
            {
                theCharacterInventory.EquipWeapon("Inventory 01 - Machine Gun");
            }
            theUIAndUpgradesController.PlayerIsDead(weaponName);
        }
    }

    public void Update()
    {
        if (theSpecialShootAndRaycastVisualization.isShooting)
        {
            specialShootActive = true;
            torsoNoLights = true;
            machineGunActive = false;
            flameGunActive = false;
            rayGunActive = false;
            torsoObject.SetActive(true);
            machineGunLights.SetActive(false);
            flameGunLights.SetActive(false);
            torsoSpriteRenderer.material = specialShootMaterial;
            legsSpriteRenderer.material = specialShootMaterial;
        }

        else
        {
            specialShootActive = false;
            if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Machine Gun" || weaponInventory.Content[0].ItemName == "Super Machine Gun"))
            {
                if (GetComponent<CharacterHandleWeapon>().CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle)
                {
                    torsoNoLights = false;
                    machineGunActive = true;
                    flameGunActive = false;
                    rayGunActive = false;
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
                    rayGunActive = false;
                    torsoObject.SetActive(true);
                    machineGunLights.SetActive(false);
                    flameGunLights.SetActive(false);
                }
            }

            if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Flame Gun" || weaponInventory.Content[0].ItemName == "Super Flame Gun") && GetComponent<Character>().MovementState.CurrentState != CharacterStates.MovementStates.Rolling)
            {
                if (GetComponent<CharacterHandleWeapon>().CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle && theAnimator.GetBool("isShooting"))
                {
                    torsoNoLights = false;
                    machineGunActive = false;
                    flameGunActive = true;
                    rayGunActive = false;
                    torsoObject.SetActive(false);
                    machineGunLights.SetActive(false);
                    flameGunLights.SetActive(true);
                    flameGunLights.GetComponent<SpriteRenderer>().material = flameGunMaterial;
                    legsSpriteRenderer.material = flameGunMaterial;
                }
                else
                {
                    torsoNoLights = true;
                    machineGunActive = false;
                    flameGunActive = false;
                    rayGunActive = false;
                    torsoObject.SetActive(true);
                    machineGunLights.SetActive(false);
                    flameGunLights.SetActive(false);
                    flameGunLights.GetComponent<SpriteRenderer>().material = originalMaterial;
                    torsoSpriteRenderer.material = originalMaterial;
                    legsSpriteRenderer.material = originalMaterial;
                }
            }

            if (weaponInventory.Content.Length > 0 && weaponInventory.Content[0] != null && (weaponInventory.Content[0].ItemName == "Ray Gun" || weaponInventory.Content[0].ItemName == "Super Ray Gun") && GetComponent<Character>().MovementState.CurrentState != CharacterStates.MovementStates.Rolling)
            {
                if (GetComponent<CharacterHandleWeapon>().CurrentWeapon.WeaponState.CurrentState != Weapon.WeaponStates.WeaponIdle && theAnimator.GetBool("Charging"))
                {
                    torsoNoLights = true;
                    machineGunActive = false;
                    flameGunActive = false;
                    rayGunActive = true;
                    torsoObject.SetActive(true);
                    machineGunLights.SetActive(false);
                    flameGunLights.SetActive(false);
                    torsoSpriteRenderer.material = rayGunMaterial;
                    legsSpriteRenderer.material = originalMaterial;
                }
                else
                {
                    torsoNoLights = true;
                    machineGunActive = false;
                    flameGunActive = false;
                    rayGunActive = false;
                    torsoObject.SetActive(true);
                    machineGunLights.SetActive(false);
                    flameGunLights.SetActive(false);
                }
            }
        }
    }
}