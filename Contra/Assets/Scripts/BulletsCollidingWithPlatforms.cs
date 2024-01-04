using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoreMountains.CorgiEngine;
using Rewired;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BulletsCollidingWithPlatforms : MonoBehaviour
{
    public GameObject objectToInstantiate;
    public GameObject objectToInstantiateWater;
    public Vector3 spawnPosition = new Vector3(0.4f, 0f, 0f);
    public Vector3 spawnPositionWater = new Vector3(-0.5f, 0f, 0f);
    public Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
    public bool doNotInstantiateTheBurst = false;
    public LayerMask ObstaclesLayerMask;
    public float distanceToPlayer;
    public float thresholdNotToShowBurst = 2f;
    public float thresholdNotToShowBurstWater = 2f;
    public GameObject thePlayer;
    public WeaponAim weaponAim;

    public void OnEnable()
    {
        //GetComponent<Animator>().enabled = false;
        //GetComponent<Animator>().Play(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);
        //GetComponent<Animator>().enabled = true;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        doNotInstantiateTheBurst = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void OnDisable()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        //GetComponent<Animator>().enabled = false;
        //GetComponent<Animator>().Play(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 0f);
        //GetComponent<Animator>().enabled = true;
    }

    public void Update()
    {
        
    }

    public void Start()
    {
        weaponAim = GameObject.FindWithTag("Firepoint").GetComponentInChildren<WeaponAim>();
        thePlayer = GameObject.FindWithTag("Player");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Water")
        {
            doNotInstantiateTheBurst = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GameObject spawnedObject = Instantiate(objectToInstantiateWater, Vector3.zero, Quaternion.identity);
            spawnedObject.transform.SetParent(transform);
            //if (GetComponent<Projectile>().Direction.x > 0)
            if (!GetComponent<SpriteRenderer>().flipX)
            {
                spawnedObject.transform.localPosition = spawnPositionWater;
                spawnedObject.transform.localRotation = rotation;
            }
            else
            {
                spawnedObject.transform.localPosition = -spawnPositionWater;
                spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
            }

            if (weaponAim == null)
            {
                weaponAim = GameObject.FindWithTag("Firepoint").GetComponentInChildren<WeaponAim>();
            }
            distanceToPlayer = Vector3.Distance(transform.position, weaponAim.transform.position);
            if (distanceToPlayer > thresholdNotToShowBurstWater)
            {
                transform.DetachChildren();
            }
            else
            {
                spawnedObject.IsDestroyed();
            }
            //GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = null;
        }

        if (((1 << other.gameObject.layer) & ObstaclesLayerMask) != 0 && !doNotInstantiateTheBurst)
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

            if (weaponAim == null)
            {
                weaponAim = GameObject.FindWithTag("Firepoint").GetComponentInChildren<WeaponAim>();
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
            //GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}