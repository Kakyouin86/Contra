using UnityEngine;

public class BulletControllerRayGunPlatform : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float destroyDelay = 3f;
    public LayerMask platformLayer;
    public float destroyTimer;
    public GameObject theExplosion;
    public bool downRaycast;
    public bool upRaycast;
    public bool leftRaycast;
    public bool rightRaycast;
    public float raycastLength = 1f;
    public RaycastHit2D hit;
    public Vector3 offset = new Vector3(0.2f, 0, 0);
    public bool bigExplosion = false;
    public Vector3 offsetBigExplosion = new Vector3(1f, 1f, 0);
    public bool travellingRight = true;

    void Start()
    {
        destroyTimer = destroyDelay;
    }

    void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
        destroyTimer -= Time.deltaTime;

        if (downRaycast)
        {
            if (bulletSpeed > 0)
            {
                offset = new Vector3(offset.x, 0, offset.z);
            }
            else
            {
                offset = new Vector3(offset.x * -1, 0, offset.z);
            }

            hit = Physics2D.Raycast(transform.position + offset, Vector2.down, raycastLength, platformLayer);
            Debug.DrawRay(transform.position, Vector2.down * 10f, Color.green);
        }
        if (upRaycast)
        {
            if (bulletSpeed > 0)
            {
                offset = new Vector3(offset.x * -1, 0, offset.z);
            }
            else
            {
                offset = new Vector3(offset.x, 0, offset.z);
            }
            hit = Physics2D.Raycast(transform.position + offset, Vector2.up, raycastLength, platformLayer);
            //Debug.DrawRay(transform.position, Vector2.up * 10f, Color.green);
        }
        if (leftRaycast)
        {
            if (bulletSpeed > 0)
            {
                offset = new Vector3(offset.x, offset.y, offset.z);
            }
            else
            {
                offset = new Vector3(0, offset.y, offset.z);
            }
            hit = Physics2D.Raycast(transform.position + offset, Vector2.left, raycastLength, platformLayer);
            //Debug.DrawRay(transform.position, Vector2.left * 10f, Color.green);
        }
        if (rightRaycast)
        {
            if (bulletSpeed > 0)
            {
                offset = new Vector3(offset.x, offset.y, offset.z);
            }
            else
            {
                offset = new Vector3(offset.x, offset.y , offset.z);
            }
            hit = Physics2D.Raycast(transform.position + offset, Vector2.right, raycastLength, platformLayer);
            //Debug.DrawRay(transform.position, Vector2.right * 10f, Color.green);
        }

        if (destroyTimer <= 0f)
        {
            // Instantiate the explosion prefab before destroying the current object
            if (!bigExplosion)
            {
                Instantiate(theExplosion, transform.position, transform.rotation);
            }
            else
            {
                if (bulletSpeed > 0 && downRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(offsetBigExplosion.x, 0, 0), transform.rotation);
                }
                else if (bulletSpeed < 0 && downRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(-offsetBigExplosion.x, 0), transform.rotation);
                }
                else if (bulletSpeed > 0 && rightRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(0, offsetBigExplosion.y, 0), transform.rotation);
                }
                else if (bulletSpeed > 0 && leftRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(0, -offsetBigExplosion.y, 0), transform.rotation);
                }
                else if (bulletSpeed > 0 && upRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(offsetBigExplosion.x, 0, 0), transform.rotation);
                }
                else if (bulletSpeed < 0 && upRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(-offsetBigExplosion.x, 0, 0), transform.rotation);
                }

                else
                {
                    Instantiate(theExplosion, transform.position, transform.rotation);
                }
            }

            Destroy(gameObject);
        }

        if (hit.collider != null)
        {
            //Debug.Log("Platform below: true");
        }
        else
        {
            //Debug.Log("Platform below: false");
            // Instantiate the explosion prefab before destroying the current object
            if (!bigExplosion)
            {
                Instantiate(theExplosion, transform.position, transform.rotation);
            }
            else
            {
                if (travellingRight && downRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(offsetBigExplosion.x, 0, 0), transform.rotation);
                    Debug.Log("Here 1");
                }
                else if (!travellingRight && downRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(-offsetBigExplosion.x, 0), transform.rotation);
                    Debug.Log("Here 2");
                }
                else if (travellingRight && rightRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(0, offsetBigExplosion.y, 0), transform.rotation);
                    Debug.Log("Here 3");
                }
                else if (!travellingRight && leftRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(0, offsetBigExplosion.y, 0), transform.rotation);
                    Debug.Log("Here 4");
                }
                else if (travellingRight && upRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(offsetBigExplosion.x, 0, 0), transform.rotation);
                    Debug.Log("Here 5");
                }
                else if (!travellingRight && upRaycast)
                {
                    Instantiate(theExplosion, transform.position + new Vector3(-offsetBigExplosion.x, 0, 0), transform.rotation);
                    Debug.Log("Here 6");
                }

                else
                {
                    Instantiate(theExplosion, transform.position, transform.rotation);
                    Debug.Log("Here 7");
                }
            }

            Destroy(gameObject);
        }
    }
}