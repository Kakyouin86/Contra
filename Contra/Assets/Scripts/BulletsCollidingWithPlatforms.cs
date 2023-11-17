using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using Rewired;
using Unity.VisualScripting;
using UnityEngine;

public class BulletsCollidingWithPlatforms : MonoBehaviour
{
    public GameObject objectToInstantiate;
    public Vector3 spawnPosition = new Vector3(0.4f, 0f, 0f);
    public Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
    public LayerMask ObstaclesLayerMask;
    public float distanceToPlayer;
    public float thresholdNotToShowBurst = 2f;
    public GameObject thePlayer;
    public WeaponAim weaponAim;

    public void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Start()
    {
        weaponAim = GameObject.FindWithTag("Firepoint").GetComponentInChildren<WeaponAim>();
        thePlayer = GameObject.FindWithTag("Player");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & ObstaclesLayerMask) != 0)
        {
            GameObject spawnedObject = Instantiate(objectToInstantiate, Vector3.zero, Quaternion.identity);
            spawnedObject.transform.SetParent(transform);
            //if (GetComponent<Projectile>().Direction.x > 0)
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                spawnedObject.transform.localPosition = spawnPosition;
                spawnedObject.transform.localRotation = rotation;
            }
            else
            {
                spawnedObject.transform.localPosition = -spawnPosition;
                spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
            }

            distanceToPlayer = Vector3.Distance(transform.position, weaponAim.transform.position);
            if (distanceToPlayer > thresholdNotToShowBurst)
            {
                transform.DetachChildren();
            }
            else
            {
                spawnedObject.IsDestroyed();
            }
            spawnedObject = null;
        }
    }
}