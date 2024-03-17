using UnityEngine;

public class CopyOfUpgrades : MonoBehaviour
{
    public bool isPlayer1;
    [Header("Acquired Power Ups")]
    public bool grenadePlus = false;
    public bool machineGunUpgrade = false;
    public bool flameGunUpgrade = false;
    public bool rayGunUpgrade = false;
    public bool shotGunUpgrade = false;
    public bool spreadGunUpgrade = false;
    public bool doubleJump = false;
    public bool dash = false;
    public bool specialShot = false;

    void Start()
    {
        if (gameObject.tag == "WeaponInventoryPlayer1")
        {
            isPlayer1 = true;
        }
    }
}
