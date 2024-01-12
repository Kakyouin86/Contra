using MoreMountains.CorgiEngine;
using Unity.VisualScripting;
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
    public bool doNotInstantiateWithAngleWater = true;
    public bool doNotInstantiateWithAnglePlatform = false;
    public float offsetMagnitude = 0.1f;

    public void OnEnable()
    {
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

            /*if (doNotInstantiateWithAngleWater)
            {
                spawnedObject.transform.position = other.ClosestPoint(transform.position);
            }*/
            if (doNotInstantiateWithAngleWater)
            {
                spawnedObject.transform.position = other.ClosestPoint(transform.position);
                Vector2 hitNormal = other.ClosestPoint(transform.position) - (Vector2)transform.position;
                Vector3 offset = new Vector3(Mathf.Sign(hitNormal.x) * offsetMagnitude, 0f, 0f);
                spawnedObject.transform.position += offset;
            }
            else
            {
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

            GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        if (((1 << other.gameObject.layer) & ObstaclesLayerMask) != 0 && !doNotInstantiateTheBurst)
        {
            GameObject spawnedObject = Instantiate(objectToInstantiate, Vector3.zero, Quaternion.identity);
            spawnedObject.transform.SetParent(transform);

            if (doNotInstantiateWithAnglePlatform)
            {
                spawnedObject.transform.position = other.ClosestPoint(transform.position);
            }
            /*if (doNotInstantiateWithAnglePlatform)
            {
                spawnedObject.transform.position = other.ClosestPoint(transform.position);

                // Apply a custom offset for diagonal hits on the floor
                Vector2 hitNormal = other.ClosestPoint(transform.position) - (Vector2)transform.position;

                // Check if the hit is diagonal
                if (Mathf.Abs(hitNormal.x) > 0.1f && Mathf.Abs(hitNormal.y) > 0.1f)
                {
                    Vector3 offset = new Vector3(offsetMagnitude, 0f, 0f);
                    spawnedObject.transform.position += offset;
                }
            }*/
            else
            {
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
                Vector2 hitNormal = other.ClosestPoint(transform.position) - (Vector2)transform.position;
                if (Mathf.Abs(hitNormal.x) > 0.1f && Mathf.Abs(hitNormal.y) > 0.1f)
                {
                    spawnedObject.transform.localPosition += new Vector3(0f, 0f, 0.1f);
                }
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

            GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}