using System.Linq.Expressions;
using MoreMountains.CorgiEngine;
using Unity.VisualScripting;
using UnityEngine;

public class BulletsCollidingWithPlatforms : MonoBehaviour
{
    public GameObject objectToInstantiate;
    public GameObject objectToInstantiateWater;
    public Vector3 spawnPositionArista = new Vector3(-1.0f, 0.0f, 0f);
    public Vector3 spawnPosition = new Vector3(0.4f, 0.0f, 0f);
    public Vector3 spawnPositionDiagonalsPlatforms = new Vector3(0.6f, 0.0f, 0f);
    public Vector3 spawnPositionWater = new Vector3(0.4f, 0.0f, 0f);
    public Vector3 spawnPositionDiagonalsWater = new Vector3(0.0f, 0.0f, 0f);
    public float thresholdNotToShowBurst = 2f;
    public float thresholdNotToShowBurstWater = 2f;
    public bool doNotInstantiateTheBurst = false;
    public bool doNotInstantiateWithAngleWater = true;
    public bool doNotInstantiateWithAnglePlatform = false;
    public bool applyOffsetToDiagonals = true;
    public Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
    public float distanceToPlayer;
    public float offsetMagnitude = 0.1f;
    public LayerMask ObstaclesLayerMask;
    public GameObject thePlayer;
    public WeaponAim weaponAim;

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
        ////////////////////////////////////////////////////////////////////////////////////////////////
        // Water related
        if (other.tag == "Water")
        {
            //Debug.Log("Water");
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
                if (GetComponent<BulletsDirection>() != null && applyOffsetToDiagonals)
                {
                    if ((GetComponent<BulletsDirection>().isMovingDown ||
                         GetComponent<BulletsDirection>().isMovingUp) &&
                        !GetComponent<BulletsDirection>().isMovingLeft &&
                        !GetComponent<BulletsDirection>().isMovingRight)
                    {
                        if (gameObject.transform.rotation.z < 0)
                        {
                            spawnedObject.transform.localPosition = Vector3.zero + spawnPositionWater;
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = Vector3.zero - spawnPositionWater;
                        }
                    }

                    else if (GetComponent<BulletsDirection>().isMovingDown &&
                             GetComponent<BulletsDirection>().isMovingLeft)
                    {
                        if (gameObject.transform.rotation.z < 0)
                        {
                            spawnedObject.transform.localPosition = new Vector3(spawnPositionDiagonalsWater.x + 0.0f, spawnPositionDiagonalsWater.y, spawnPositionDiagonalsWater.z);
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = new Vector3(-spawnPositionDiagonalsWater.x + 0.0f, -spawnPositionDiagonalsWater.y, -spawnPositionDiagonalsWater.z);
                        }
                    }

                    else if (GetComponent<BulletsDirection>().isMovingDown &&
                             GetComponent<BulletsDirection>().isMovingRight)
                    {
                        if (gameObject.transform.rotation.z < 0)
                        {
                            spawnedObject.transform.localPosition = new Vector3(spawnPositionDiagonalsWater.x + 0.0f, spawnPositionDiagonalsWater.y, spawnPositionDiagonalsWater.z);
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = new Vector3(-spawnPositionDiagonalsWater.x + 0.0f, -spawnPositionDiagonalsWater.y, -spawnPositionDiagonalsWater.z);
                        }
                    }

                    else if (GetComponent<BulletsDirection>().isMovingUp &&
                             GetComponent<BulletsDirection>().isMovingLeft)
                    {
                        spawnedObject.transform.localPosition = new Vector3(spawnPositionDiagonalsWater.x + 0.2f,
                            spawnPositionDiagonalsWater.y, spawnPositionDiagonalsWater.z);
                    }

                    else if (GetComponent<BulletsDirection>().isMovingUp &&
                             GetComponent<BulletsDirection>().isMovingRight)
                    {
                        spawnedObject.transform.localPosition = spawnPositionDiagonalsWater;
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = spawnPositionWater;

                    }
                }
                else
                {
                    if (gameObject.transform.rotation.z < 0)
                    {
                        spawnedObject.transform.localPosition = new Vector3(spawnPositionWater.x + 0.8f, spawnPositionWater.y, spawnPositionWater.z);
                        Debug.Log("Here");
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = new Vector3(-spawnPositionWater.x + 0.0f, -spawnPositionWater.y, -spawnPositionWater.z);
                        Debug.Log("H3ere");
                    }
                }
            }
            else
            {
                if (!GetComponent<SpriteRenderer>().flipX)
                {
                    if (GetComponent<BulletsDirection>() != null && applyOffsetToDiagonals)
                    {
                        if ((GetComponent<BulletsDirection>().isMovingDown ||
                             GetComponent<BulletsDirection>().isMovingUp) &&
                            !GetComponent<BulletsDirection>().isMovingLeft &&
                            !GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = Vector3.zero + spawnPositionWater;
                            spawnedObject.transform.localRotation = rotation;
                            Debug.Log("Here");
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            spawnedObject.transform.localPosition = new Vector3(-spawnPositionDiagonalsWater.x + 1.5f, -spawnPositionDiagonalsWater.y, -spawnPositionDiagonalsWater.z);
                            spawnedObject.transform.localRotation = rotation;
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = spawnPositionDiagonalsWater;
                            spawnedObject.transform.localRotation = rotation;
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            spawnedObject.transform.localPosition = new Vector3(spawnPositionDiagonalsWater.x + 0.2f, spawnPositionDiagonalsWater.y, spawnPositionDiagonalsWater.z);
                            spawnedObject.transform.localRotation = rotation;
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = spawnPositionDiagonalsWater;
                            spawnedObject.transform.localRotation = rotation;
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = spawnPositionWater;
                            spawnedObject.transform.localRotation = rotation;
                        }
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = spawnPositionWater;
                        spawnedObject.transform.localRotation = rotation;
                    }
                }
                else
                {
                    if (GetComponent<BulletsDirection>() != null && applyOffsetToDiagonals)
                    {
                        if ((GetComponent<BulletsDirection>().isMovingDown ||
                             GetComponent<BulletsDirection>().isMovingUp) &&
                            !GetComponent<BulletsDirection>().isMovingLeft &&
                            !GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = Vector3.zero - spawnPositionWater;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            spawnedObject.transform.localPosition = new Vector3(spawnPositionDiagonalsWater.x - 0.6f, spawnPositionDiagonalsWater.y, spawnPositionDiagonalsWater.z);
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = -spawnPositionDiagonalsWater;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            spawnedObject.transform.localPosition = -spawnPositionDiagonalsWater;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = -spawnPositionDiagonalsWater;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = -spawnPositionWater;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = -spawnPositionWater;
                        spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                    }
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

        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// Arista related

        if (other.tag == "Arista")
        {
            //Debug.Log("Arista");
            if (GetComponent<BulletsDirection>() != null && applyOffsetToDiagonals)
            {
                if (!(GetComponent<BulletsDirection>().isMovingDown ^ GetComponent<BulletsDirection>().isMovingUp ^
                      GetComponent<BulletsDirection>().isMovingLeft ^ GetComponent<BulletsDirection>().isMovingRight))
                {
                    GameObject spawnedObject = Instantiate(objectToInstantiate, Vector3.zero, Quaternion.identity);
                    spawnedObject.transform.SetParent(transform);
                    doNotInstantiateTheBurst = true;
                    if (gameObject.transform.rotation.z < 0)
                    {
                        spawnedObject.transform.localPosition = -spawnPositionArista;
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = spawnPositionArista;
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
                    return;
                }

                else
                {
                    doNotInstantiateTheBurst = false;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// Platforms related

        if (((1 << other.gameObject.layer) & ObstaclesLayerMask) != 0 && !doNotInstantiateTheBurst)
        {
            //Debug.Log("Plaform");
            GameObject spawnedObject = Instantiate(objectToInstantiate, Vector3.zero, Quaternion.identity);
            spawnedObject.transform.SetParent(transform);

            if (doNotInstantiateWithAnglePlatform)
            {
                if (GetComponent<BulletsDirection>() != null && applyOffsetToDiagonals)
                {
                    if ((GetComponent<BulletsDirection>().isMovingDown ||
                         GetComponent<BulletsDirection>().isMovingUp) &&
                        !GetComponent<BulletsDirection>().isMovingLeft &&
                        !GetComponent<BulletsDirection>().isMovingRight)
                    {
                        if (gameObject.transform.rotation.z < 0)
                        {
                            spawnedObject.transform.localPosition = Vector3.zero + spawnPosition;
                            Debug.Log("Here");
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = Vector3.zero - spawnPosition;
                            Debug.Log("Here");
                        }
                    }

                    else if (GetComponent<BulletsDirection>().isMovingDown &&
                             GetComponent<BulletsDirection>().isMovingLeft)
                    {
                        spawnedObject.transform.localPosition = new Vector3(-spawnPositionDiagonalsPlatforms.x + 1.5f, -spawnPositionDiagonalsPlatforms.y, -spawnPositionDiagonalsPlatforms.z);
                    }

                    else if (GetComponent<BulletsDirection>().isMovingDown &&
                             GetComponent<BulletsDirection>().isMovingRight)
                    {
                        spawnedObject.transform.localPosition = spawnPositionDiagonalsPlatforms;
                    }

                    else if (GetComponent<BulletsDirection>().isMovingUp &&
                             GetComponent<BulletsDirection>().isMovingLeft)
                    {
                        spawnedObject.transform.localPosition = new Vector3(spawnPositionDiagonalsPlatforms.x + 0.2f, spawnPositionDiagonalsPlatforms.y, spawnPositionDiagonalsPlatforms.z);
                    }

                    else if (GetComponent<BulletsDirection>().isMovingUp &&
                             GetComponent<BulletsDirection>().isMovingRight)
                    {
                        spawnedObject.transform.localPosition = spawnPositionDiagonalsPlatforms;
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = spawnPosition;
                    }
                }
                else
                {
                    spawnedObject.transform.localPosition = spawnPosition;
                }
            }
            else
            {
                if (!GetComponent<SpriteRenderer>().flipX)
                {
                    if (GetComponent<BulletsDirection>() != null && applyOffsetToDiagonals)
                    {
                        if ((GetComponent<BulletsDirection>().isMovingDown ||
                             GetComponent<BulletsDirection>().isMovingUp) &&
                            !GetComponent<BulletsDirection>().isMovingLeft &&
                            !GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = Vector3.zero + spawnPosition;
                            spawnedObject.transform.localRotation = rotation;
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            if (gameObject.transform.rotation.z < 0)
                            {
                                spawnedObject.transform.localPosition = new Vector3(-spawnPositionDiagonalsPlatforms.x + 1.5f, -spawnPositionDiagonalsPlatforms.y, -spawnPositionDiagonalsPlatforms.z);
                                spawnedObject.transform.localRotation = rotation;
                            }
                            else
                            {
                                spawnedObject.transform.localPosition = new Vector3(-spawnPositionDiagonalsPlatforms.x + 1.5f, -spawnPositionDiagonalsPlatforms.y, -spawnPositionDiagonalsPlatforms.z);
                                spawnedObject.transform.localRotation = rotation;
                            }
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = spawnPositionDiagonalsPlatforms;
                            spawnedObject.transform.localRotation = rotation;
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            spawnedObject.transform.localPosition = new Vector3(
                                spawnPositionDiagonalsPlatforms.x + 0.2f, spawnPositionDiagonalsPlatforms.y,
                                spawnPositionDiagonalsPlatforms.z);
                            spawnedObject.transform.localRotation = rotation;
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = spawnPositionDiagonalsPlatforms;
                            spawnedObject.transform.localRotation = rotation;
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = spawnPosition;
                            spawnedObject.transform.localRotation = rotation;
                        }
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = spawnPosition;
                        spawnedObject.transform.localRotation = rotation;
                    }
                }
                else
                {
                    if (GetComponent<BulletsDirection>() != null && applyOffsetToDiagonals)
                    {
                        if ((GetComponent<BulletsDirection>().isMovingDown ||
                             GetComponent<BulletsDirection>().isMovingUp) &&
                            !GetComponent<BulletsDirection>().isMovingLeft &&
                            !GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = Vector3.zero - spawnPosition;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            spawnedObject.transform.localPosition = new Vector3(spawnPositionDiagonalsPlatforms.x - 0.6f, spawnPositionDiagonalsPlatforms.y, spawnPositionDiagonalsPlatforms.z);
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingDown &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = -spawnPositionDiagonalsPlatforms;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingLeft)
                        {
                            spawnedObject.transform.localPosition = -spawnPositionDiagonalsPlatforms;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }

                        else if (GetComponent<BulletsDirection>().isMovingUp &&
                                 GetComponent<BulletsDirection>().isMovingRight)
                        {
                            spawnedObject.transform.localPosition = -spawnPositionDiagonalsPlatforms;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }
                        else
                        {
                            spawnedObject.transform.localPosition = -spawnPosition;
                            spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                        }
                    }
                    else
                    {
                        spawnedObject.transform.localPosition = -spawnPosition;
                        spawnedObject.transform.localRotation = Quaternion.Euler(0f, 0f, -rotation.eulerAngles.z);
                    }
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
