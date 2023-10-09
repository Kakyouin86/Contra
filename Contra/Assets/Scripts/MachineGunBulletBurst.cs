using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class MachineGunBulletBurst : MonoBehaviour
{
    public GameObject theBurst;
    public Vector3 theOffsetPosition;
    public GameObject theFirepoint;
    public WeaponAim weaponAim;
    public Projectile projectile;
    public Vector3 theProjectileFacingRight;
    public Quaternion rotation;
    public LayerMask ObstaclesLayerMask;

    public void Start()
    {
        theFirepoint = GameObject.FindWithTag("Firepoint");
        weaponAim = theFirepoint.GetComponentInChildren<WeaponAim>();
        projectile = GetComponent<Projectile>();
    }
    public void Update()
    {
        theProjectileFacingRight = projectile.Direction;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure that the weaponAim component is not null
        if (weaponAim != null)
        {
            // Check if the collider's layer is included in the ObstaclesLayerMask
            if (((1 << other.gameObject.layer) & ObstaclesLayerMask) != 0)
            {
                // Instantiate theBurst with the specified position and rotation
                GameObject spawnedPrefab = Instantiate(theBurst, transform.position, transform.rotation);

                // Set the newly instantiated GameObject as a child of the current GameObject
                spawnedPrefab.transform.SetParent(transform);

                // Calculate the rotation based on the weaponAim's current angle
                if (theProjectileFacingRight.x >= 0.8f)
                {
                    Debug.Log("De derecha a izquierda");
                    rotation = Quaternion.Euler(0f, 0f, 90f);
                }

                if (theProjectileFacingRight.x <= -0.8f)
                {
                    Debug.Log("De izquierda a derecha");
                    rotation = Quaternion.Euler(0f, 0f, -90f);
                }

                if (theProjectileFacingRight.y <= -0.8f)
                {
                    Debug.Log("De arriba a abajo");
                    rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                if (theProjectileFacingRight.y >= 0.8f)
                {
                    Debug.Log("De abajo a arriba");
                    rotation = Quaternion.Euler(0f, 0f, -180f);
                }

                if (theProjectileFacingRight.x >= 0.5f && theProjectileFacingRight.x <= 0.8f &&
                    theProjectileFacingRight.y >= 0.5f && theProjectileFacingRight.y <= 0.8f)
                {
                    Debug.Log("De abajo a arriba a la derecha");
                    rotation = Quaternion.Euler(0f, 0f, -225f);
                }

                if (theProjectileFacingRight.x >= -0.8f && theProjectileFacingRight.x <= -0.5f &&
                    theProjectileFacingRight.y >= 0.5f && theProjectileFacingRight.y <= 0.8f)
                {
                    Debug.Log("De abajo a arriba a la izquierda");
                    rotation = Quaternion.Euler(0f, 0f, 225f);
                }

                if (theProjectileFacingRight.x >= 0.5f && theProjectileFacingRight.x <= 0.8f &&
                    theProjectileFacingRight.y >= -0.8f && theProjectileFacingRight.y <= -0.5f)
                {
                    Debug.Log("De arriba a abajo a la derecha");
                    rotation = Quaternion.Euler(0f, 0f, 45);
                }

                if (theProjectileFacingRight.x >= -0.8f && theProjectileFacingRight.x <= -0.5f &&
                    theProjectileFacingRight.y >= -0.8f && theProjectileFacingRight.y <= -0.5f)
                {
                    Debug.Log("De arriba a abajo a la izquierda");
                    rotation = Quaternion.Euler(0f, 0f, -45);
                }
                
                transform.DetachChildren();
                spawnedPrefab.transform.rotation = rotation;
                //Instantiate(theBurst, transform.position + theOffsetPosition, rotation);
            }
        }
    }
}