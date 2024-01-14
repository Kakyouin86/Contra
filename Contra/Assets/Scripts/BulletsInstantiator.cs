using UnityEngine;
using static MoreMountains.CorgiEngine.CorgiController;

public class BulletsInstantiator : MonoBehaviour
{
    public GameObject theExplosion;
    public GameObject theNewBulletPrefab;
    public BulletsDirection bulletsDirection;
    public LayerMask layerMask;
    public float bulletForce = 10;
    public bool cancelTheInstantiation = false;
    public float raycastDistance = 0.1f;

    public void Start()
    {
        bulletsDirection = GetComponent<BulletsDirection>();
    }

    public void Update()
    {
    }

    public void OnEnable()
    {
        cancelTheInstantiation = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            cancelTheInstantiation = true;
            return;
        }

        if (((1 << other.gameObject.layer) & layerMask) != 0 && !cancelTheInstantiation)
        {
            if (bulletsDirection.isMovingRight && bulletsDirection.isMovingDown)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position) + new Vector2(1f,0f), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                Debug.Log("1");
            }

            else if (bulletsDirection.isMovingLeft && bulletsDirection.isMovingDown && !bulletsDirection.isMovingUp)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.GetComponent<SpriteRenderer>().flipX = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed =
                    -newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().travellingRight = false;
                Debug.Log("2");
            }

            else if (bulletsDirection.isMovingRight && !bulletsDirection.isMovingDown && !bulletsDirection.isMovingUp)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(other.bounds.min.x - yOffset, transform.position.y, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position) - new Vector2(0f, 0f), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.localRotation = Quaternion.Euler(0f, 0f, 90);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().rightRaycast = true;
                Debug.Log("3");
            }

            else if (bulletsDirection.isMovingLeft && !bulletsDirection.isMovingDown && !bulletsDirection.isMovingUp)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(other.bounds.max.x + yOffset, transform.position.y, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position) - new Vector2(0f, 0f), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.localRotation = Quaternion.Euler(0f, 0f, -90);
                newBullet.GetComponent<SpriteRenderer>().flipX = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed =
                    -newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().leftRaycast = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().travellingRight = false;
                Debug.Log("4");
            }

            else if (bulletsDirection.isMovingDown && Mathf.Approximately(transform.rotation.eulerAngles.z, 90f))
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.GetComponent<SpriteRenderer>().flipX = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed =
                    -newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().travellingRight = false;
                Debug.Log("5");
            }

            else if (bulletsDirection.isMovingDown && transform.localRotation.z < 0)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position) + new Vector2(1f, 0f), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                Debug.Log("6");
            }

            else if (bulletsDirection.isMovingUp && Mathf.Approximately(transform.rotation.eulerAngles.z, 90f))
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.min.y - yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().upRaycast = true;
                Debug.Log("7");
            }

            else if (bulletsDirection.isMovingUp && transform.localRotation.z < 0)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.min.y - yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().upRaycast = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().travellingRight = false;
                Debug.Log("8");
            }

            else
            {
                if (!bulletsDirection.facingRight)
                {
                    float yOffset = 0.01f;
                    Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);

                    if ((bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hold Down") || bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Down") || bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Jumping")))
                    {
                        Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(theExplosion, other.ClosestPoint(transform.position) - new Vector2(0.5f, 0f), Quaternion.identity);
                    }

                    GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                    newBullet.GetComponent<SpriteRenderer>().flipX = true;
                    newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed =
                        -newBullet.GetComponent<BulletControllerRayGunPlatform>().bulletSpeed;
                    newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                    newBullet.GetComponent<BulletControllerRayGunPlatform>().travellingRight = false;
                    Debug.Log("9");//ESTUDIAR POR QUÉ SE JUNTA A LA PARED
                }

                if (bulletsDirection.facingRight && !bulletsDirection.isMovingUp)
                {
                    float yOffset = 0.01f;
                    Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);

                    if ((bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hold Down") || bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Down") || bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Jumping")))
                    {
                        Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(theExplosion, other.ClosestPoint(transform.position) - new Vector2(1f, 0f), Quaternion.identity);
                    }

                    GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                    newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                    Debug.Log("10");//ESTUDIAR POR QUÉ SE JUNTA A LA PARED
                }

                if (bulletsDirection.facingRight && bulletsDirection.isMovingUp)
                {
                    float yOffset = 0.01f;
                    Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.min.y - yOffset, transform.position.z);

                    if ((bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hold Up") || bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Up") || bulletsDirection.thePlayer.GetComponent<OverridesInAnimator>().theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Jumping")))
                    {
                        Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(theExplosion, other.ClosestPoint(transform.position) + new Vector2(0.5f, 0f), Quaternion.identity);
                    }

                    GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                    newBullet.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
                    newBullet.GetComponent<BulletControllerRayGunPlatform>().upRaycast = true;
                    Debug.Log("11");
                }
            }
        }
    }
}