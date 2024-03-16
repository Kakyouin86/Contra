using MoreMountains.CorgiEngine;
using UnityEngine;

public class OverridesInPlayerOld : MonoBehaviour
{
    public Character character;
    void Start()
    {
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
        AdditionalMovementSettings additionalMovementSettings = character.GetComponent<AdditionalMovementSettings>();
        additionalMovementSettings.weaponAim = this.gameObject.GetComponent<WeaponAim>();
        Firepoint firepoint = character.GetComponent<Firepoint>();
        firepoint.weaponAim = this.gameObject.GetComponent<WeaponAim>();
        firepoint.projectileWeapon = this.gameObject.GetComponent<ProjectileWeapon>();
    }
}
