using System;
using System.Collections;
using UnityEngine;
using Rewired;
using MoreMountains.CorgiEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class SpecialShootAndRaycastVisualization : MonoBehaviour
{
    public bool isPlayer1;
    public bool hasSpecialShoot = false;
    public bool canShoot = false;
    public bool enableLaser = false;
    public bool isShooting = false;

    [Header("Animation Timer")]
    public float animationTimer = 0.0f; // Set this value in the inspector
    public float currentTimer = 0.0f;

    [Header("Raycasts")]
    public Vector3 RaycastOriginOffset;
    public Vector3 LaserOriginOffset;
    public float LaserMaxDistance = 50;
    public LayerMask LaserCollisionMask = LayerManager.ObstaclesLayerMask;
    public Vector2 LaserWidth = new Vector2(0.05f, 0.05f);

    [Header("Appearance")]
    public Material LaserMaterial;
    public Vector3 LaserOrigin => _origin;
    public Vector3 LaserDestination => _destination;
    public Player player;
    public GameObject theSpecialShot;
    public GameObject theSpecialShotInstantiated;
    public Weapon _weapon;
    private Vector3 _direction;
    private LineRenderer _line;
    private RaycastHit2D _hit;
    private Vector3 _origin;
    private Vector3 _destination;
    private Vector3 _laserOffset;
    private Vector3 _weaponPosition;
    private Quaternion _weaponRotation;
    public GameObject theFirepoint;
    public Animator theAnimator;

    public void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    public void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _line.receiveShadows = true;
        _line.startWidth = LaserWidth.x;
        _line.endWidth = LaserWidth.y;
        _line.material = LaserMaterial;
        if (GetComponent<Character>().PlayerID == "Player1")
        {
            isPlayer1 = true;
        }
    }
    
    public void Update()
    {
        theFirepoint = transform.Find("Firepoint").gameObject;
        //_weapon = GameObject.FindGameObjectWithTag("WeaponAim").GetComponent<Weapon>(); This is the 1 player version. The next line is the 2 players version.
        // Get the transform of theFirepoint's GameObject
        Transform firepointTransform = theFirepoint.transform;

        // Iterate through each child of theFirepoint
        foreach (Transform child in firepointTransform)
        {
            // Check if the child has the "WeaponAim" tag
            if (child.CompareTag("WeaponAim"))
            {
                // Get the Weapon component from the child
                _weapon = child.GetComponentInChildren<Weapon>();
                // Exit the loop once found
                break;
            }
        }

        ShootLaser();

        if (isShooting)
        {
            if (GetComponent<Firepoint>().weaponAim.GetComponent<ChargeWeapon>() != null) //CAREFUL HERE! It's because if you are shooting the Special Shot, it will continue charging.
            {
                ChargeWeapon theChargeWeapon = GetComponent<Firepoint>().weaponAim.GetComponent<ChargeWeapon>();
                theChargeWeapon.enabled = false;
                Transform chargeWeaponTransform = theChargeWeapon.transform;
                if (chargeWeaponTransform != null)
                {
                    foreach (Transform child in chargeWeaponTransform)
                    {
                        if (child.GetComponent<MMF_Player>())
                        {
                            child.GetComponent<MMF_Player>().CanPlay = false;
                        }
                        if (child.CompareTag("Spark"))
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }
                theChargeWeapon.enabled = false;
                theChargeWeapon.Charging = false;
            }
            
            // Update the timer while shooting
            currentTimer += Time.deltaTime;

            // Check if the timer exceeds the animation length
            if (currentTimer >= animationTimer)
            {
                ResetEverything();
            }
        }

        if (player.GetButtonDown("SpecialShoot") && hasSpecialShoot && !isShooting && canShoot)
        {
            // Set isShooting to true
            isShooting = true;
            //Animator theAnimator = GameObject.FindGameObjectWithTag("PlayerSprites").GetComponent<Animator>(); This is the 1 player version. The next line is the 2 players version.
            // Get the transform of the current GameObject
            Transform currentTransform = transform;

            // Iterate through each child of the current GameObject
            foreach (Transform child in currentTransform)
            {
                // Check if the child has the "PlayerSprites" tag
                if (child.CompareTag("PlayerSprites"))
                {
                    // Get the Animator component from the child
                    theAnimator = child.GetComponent<Animator>();
                    // Exit the loop once found
                    break;
                }
            }

            theAnimator.SetBool("isShootingSpecialShot", true);
            AdditionalCharacterHandleWeaponOverride theAdditionalCharacterHandleWeaponOverride = GetComponent<AdditionalCharacterHandleWeaponOverride>();
            theAdditionalCharacterHandleWeaponOverride.AbilityPermitted = false;
            //SpecialShootController theSpecialShootController = GameObject.FindGameObjectWithTag("UI").GetComponent<SpecialShootController>(); This is the 1 player version. The next line is the 2 players version.
            if (GetComponent<Character>().PlayerID == "Player1")
            { 
                SpecialShootController theSpecialShootController = GameObject.FindGameObjectWithTag("UIPlayer1").GetComponent<SpecialShootController>();
                theSpecialShootController.ShootSpecialShot();
            }
            if (GetComponent<Character>().PlayerID == "Player2")
            {
                SpecialShootController theSpecialShootController = GameObject.FindGameObjectWithTag("UIPlayer2").GetComponent<SpecialShootController>();
                theSpecialShootController.ShootSpecialShot();
            }

            _weapon.WeaponInputStop();
            _weapon.TurnWeaponOff();

            // Check if thePickerInstance is not already instantiated and laser is enabled
            if (theSpecialShotInstantiated == null)
            {
                // Instantiate thePickerPrefab and store the reference
                theSpecialShotInstantiated = Instantiate(theSpecialShot, transform.position, transform.rotation);

                // Set thePickerInstance's position and rotation to match _weapon
                theSpecialShotInstantiated.transform.position = _weapon.transform.position;
                theSpecialShotInstantiated.transform.rotation = _weapon.transform.rotation;
            }
        }

        if (theSpecialShotInstantiated != null)
        {
            // Update thePickerInstance's position and rotation to match _weapon
            theSpecialShotInstantiated.transform.position = _weapon.transform.position;

            // Adjust thePickerInstance's rotation based on _weapon.Flipped
            if (_weapon.Flipped)
            {
                // If flipped, rotate 180 degrees
                theSpecialShotInstantiated.transform.rotation = _weapon.transform.rotation * Quaternion.Euler(0f, 0f, 180f);
            }
            else
            {
                theSpecialShotInstantiated.transform.rotation = _weapon.transform.rotation;
            }
        }
    }

    public void ResetEverything()
    {
        // Animation is done, reset the timer and set isShooting to false
        currentTimer = 0.0f;
        isShooting = false;
        //Animator theAnimator = GameObject.FindGameObjectWithTag("PlayerSprites").GetComponent<Animator>(); This is the 1 player version. The next line is the 2 players version.

        Transform currentTransform = transform;
        foreach (Transform child in currentTransform)
        {
            if (child.CompareTag("PlayerSprites"))
            {
                theAnimator = child.GetComponent<Animator>();
                break;
            }
        }

        theAnimator.SetBool("isShootingSpecialShot", false);
        AdditionalCharacterHandleWeaponOverride theAdditionalCharacterHandleWeaponOverride = GetComponent<AdditionalCharacterHandleWeaponOverride>();
        theAdditionalCharacterHandleWeaponOverride.AbilityPermitted = true;

        if (GetComponent<Firepoint>().weaponAim.GetComponent<ChargeWeapon>() != null) //CAREFUL HERE! It's because if you are shooting the Special Shot, it will continue charging.
        {
            ChargeWeapon theChargeWeapon = GetComponent<Firepoint>().weaponAim.GetComponent<ChargeWeapon>();
            theChargeWeapon.enabled = true;
            StartCoroutine(KillTheSparks());
        }
    }

    public IEnumerator KillTheSparks()
    {
        yield return new WaitForSeconds(0.1f);
        ChargeWeapon theChargeWeapon = GetComponent<Firepoint>().weaponAim.GetComponent<ChargeWeapon>();
        Transform chargeWeaponTransform = theChargeWeapon.transform;
        if (chargeWeaponTransform != null)
        {
            foreach (Transform child in chargeWeaponTransform)
            {
                if (child.GetComponent<MMF_Player>())
                {
                    child.GetComponent<MMF_Player>().CanPlay = true;
                }
                if (child.CompareTag("Spark"))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ShootLaser()
    {
        _weaponPosition = _weapon.transform.position;
        _weaponRotation = _weapon.transform.rotation;

        _direction = _weapon.Flipped ? Vector3.left : Vector3.right;

        _laserOffset = LaserOriginOffset;
        if (_direction == Vector3.left)
        {
            _laserOffset.x = -LaserOriginOffset.x;
        }

        _origin = MMMaths.RotatePointAroundPivot(_weaponPosition + _laserOffset, _weaponPosition, _weaponRotation);

        _hit = MMDebug.RayCast(_origin, _weaponRotation * _direction, LaserMaxDistance, LaserCollisionMask, Color.yellow, true);

        if (_hit)
        {
            _destination = _hit.point;
        }
        else
        {
            _destination = _origin;
            _destination.x = _destination.x + LaserMaxDistance * _direction.x;
            _destination = MMMaths.RotatePointAroundPivot(_destination, _weaponPosition, _weaponRotation);
        }

        // Set our laser's line's start and end coordinates only if the laser is enabled
        _line.enabled = enableLaser;
        if (enableLaser)
        {
            _line.SetPosition(0, _origin);
            _line.SetPosition(1, _destination);
        }
    }

    // Method to enable/disable the laser
    public void SetLaserEnabled(bool enabled)
    {
        enableLaser = enabled;

        // If disabling the laser, also destroy the picker instance
        if (!enabled && theSpecialShotInstantiated != null)
        {
            Destroy(theSpecialShotInstantiated);
            theSpecialShotInstantiated = null;
        }
    }
}
