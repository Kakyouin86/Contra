using System.Collections;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;
using Rewired;
using Rewired.ComponentControls.Data;
using InputManager = MoreMountains.CorgiEngine.InputManager;
using System.Security.Cryptography.X509Certificates;

public class OverridesInPlayer : MonoBehaviour
{
    public Character character;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindWithTag("Player").GetComponent<Character>();
        AdditionalMovementSettings additionalMovementSettings = character.GetComponent<AdditionalMovementSettings>();
        additionalMovementSettings.weaponAim = this.gameObject.GetComponent<WeaponAim>();
        Firepoint firepoint = character.GetComponent<Firepoint>();
        firepoint.weaponAim = this.gameObject.GetComponent<WeaponAim>();
        firepoint.projectileWeapon = this.gameObject.GetComponent<ProjectileWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
