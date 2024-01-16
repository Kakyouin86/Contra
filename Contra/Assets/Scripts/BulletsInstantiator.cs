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
    public bool hitWater = false;
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
        hitWater  = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            cancelTheInstantiation = true;
            hitWater = true;
            return;
        }

        WhereItHit(other);
        
        if (((1 << other.gameObject.layer) & layerMask) != 0 && !cancelTheInstantiation)
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            // Down-Right Shoot Hanging collides with floor and moves right.
            if (bulletsDirection.isMovingRight && bulletsDirection.isMovingDown)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position) + new Vector2(1f,0f), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                //Debug.Log("1");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Down-Left Shoot Hanging collides with floor and moves left.
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
                //Debug.Log("2");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Right Shoot collides with wall and goes up.
            else if (bulletsDirection.isMovingRight && !bulletsDirection.isMovingDown && !bulletsDirection.isMovingUp)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(other.bounds.min.x - yOffset, transform.position.y, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position) - new Vector2(0f, 0f), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.localRotation = Quaternion.Euler(0f, 0f, 90);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().rightRaycast = true;
                //Debug.Log("3");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Left Shoot collides with wall and goes up.
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
                //Debug.Log("4");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Down Shoot Hanging collides with floor and moves left.
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
               //Debug.Log("5");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Down Shoot Hanging collides with floor and moves right.
            else if (bulletsDirection.isMovingDown && transform.localRotation.z < 0)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.max.y + yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position) + new Vector2(0f, 0f), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().downRaycast = true;
                //Debug.Log("6");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Right Shoot-Up while facing right.
            else if (bulletsDirection.isMovingUp && Mathf.Approximately(transform.rotation.eulerAngles.z, 90f))
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.min.y - yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().upRaycast = true;
                //Debug.Log("7");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Left Shoot-Up collides with wall and goes up. Also Shoot Up while facing Left.
            else if (bulletsDirection.isMovingUp && transform.localRotation.z < 0)
            {
                float yOffset = 0.01f;
                Vector3 spawnPosition = new Vector3(transform.position.x, other.bounds.min.y - yOffset, transform.position.z);
                Instantiate(theExplosion, other.ClosestPoint(transform.position), Quaternion.identity);
                GameObject newBullet = Instantiate(theNewBulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                newBullet.GetComponent<BulletControllerRayGunPlatform>().upRaycast = true;
                newBullet.GetComponent<BulletControllerRayGunPlatform>().travellingRight = false;
                //Debug.Log("8");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Down-Left Shoot Standing collides with floor and moves right. Doesn't work well when next to a wall to the left.
            else if (!bulletsDirection.facingRight) 
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
                //Debug.Log("9");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Down Or Down-Right Shoot Standing collides with floor and moves right. Doesn't work well when next to a wall to the right.
            else if (bulletsDirection.facingRight && !bulletsDirection.isMovingUp) 
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
                //Debug.Log("10");
            }

            else if (bulletsDirection.facingRight && bulletsDirection.isMovingUp)
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
                //Debug.Log("11");
            }
        }

        if (((1 << other.gameObject.layer) & layerMask) != 0 && cancelTheInstantiation && !hitWater)
        {
            Instantiate(GetComponent<BulletsCollidingWithPlatforms>().objectToInstantiate, other.ClosestPoint(transform.position) + new Vector2(0.0f, 0f), Quaternion.identity);
        }
    }

    public void WhereItHit(Collider2D other)
    {
        Vector2 collisionPoint = other.ClosestPoint(transform.position);
        if (collisionPoint.y >= other.bounds.max.y - 0.01f)
        {
            // Hit on the top part of the collider
            //Debug.Log("Hit on top");
        }
        else if (collisionPoint.y <= other.bounds.min.y + 0.01f)
        {
            // Hit on the bottom part of the collider
            //Debug.Log("Hit on bottom");
            cancelTheInstantiation = true;
        }
        else if (collisionPoint.x <= other.bounds.min.x + 0.01f)
        {
            // Hit on the left part of the collider
            //Debug.Log("Hit on left");
            cancelTheInstantiation = true;
        }
        else if (collisionPoint.x >= other.bounds.max.x - 0.01f)
        {
            // Hit on the right part of the collider
            //Debug.Log("Hit on right");
            cancelTheInstantiation = true;
        }
    }
}