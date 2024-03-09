using MoreMountains.CorgiEngine;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class BeamController : MonoBehaviour
{
    public bool canInstantiateThePrefabs = false;
    public GameObject middlePrefabToInstantiate;
    public GameObject prefabToInstantiate;
    public bool canInstantiate = false;
    public float stopInstantiating = 0.7f;
    public float timerToStopInstantiating = 0f;
    public GameObject[] topBeamSegments;
    public GameObject[] bottomBeamSegments;
    public float delayBetweenSegments = 0.2f;
    public float delayBetweenReEnable = 1.0f;
    public LayerMask LaserCollisionMask = LayerManager.ObstaclesLayerMask;
    public bool[] isTopCollidingArray;
    public bool[] isBottomCollidingArray;
    public bool reEnableInProgress;
    public float maxRaycastLength = 10f;
    public bool topPrefabInstantiated = false;
    public bool bottomPrefabInstantiated = false;

    void Start()
    {
        canInstantiate = false;
        isTopCollidingArray = new bool[topBeamSegments.Length];
        isBottomCollidingArray = new bool[bottomBeamSegments.Length];
        StartCoroutine(EnableSegmentsSequentially(topBeamSegments, true, isTopCollidingArray));
        StartCoroutine(EnableSegmentsSequentially(bottomBeamSegments, false, isBottomCollidingArray));
        canInstantiate = true;
    }

    void Update()
    {
        timerToStopInstantiating += Time.deltaTime;

        if (timerToStopInstantiating >= stopInstantiating)
        {
            canInstantiate = false;
        }
    }

    public void CanInstantiate()
    {
        canInstantiateThePrefabs = true;
    }

    IEnumerator EnableSegmentsSequentially(GameObject[] beamSegments, bool isTop, bool[] isCollidingArray)
    {
        int length = beamSegments.Length;

        while (true)
        {
            for (int i = 0; i < length; i++)
            {
                var segment = beamSegments[i];

                if (isTop ? !isCollidingArray[i] : !isCollidingArray[i])
                {
                    segment.GetComponent<SpriteRenderer>().enabled = true;
                    segment.GetComponent<BoxCollider2D>().enabled = true;
                }

                yield return new WaitForSeconds(delayBetweenSegments);

                var collisionInfo = CheckCollision(segment);

                isCollidingArray[i] = collisionInfo.isColliding;

                if (collisionInfo.isColliding)
                {
                    if (isTop && !topPrefabInstantiated && canInstantiate)
                    {
                        // Check for prior collisions before the current index
                        if (!HasPriorCollisions(isBottomCollidingArray, i))
                        {
                            // Check if both top and bottom beams are colliding at the same index
                            if (isBottomCollidingArray[i])
                            {
                                // Calculate the middle point between the top and bottom beams at the colliding index
                                InstantiateMiddlePrefab(collisionInfo.hitPoint, topBeamSegments[i].transform.position, bottomBeamSegments[i].transform.position);
                                topPrefabInstantiated = true;
                                bottomPrefabInstantiated = true;
                                continue; // Skip regular prefab instantiation
                            }
                            else
                            {
                                InstantiatePrefab(collisionInfo.hitPoint, segment.transform.up);
                                topPrefabInstantiated = true;
                            }
                        }
                        else
                        {
                            InstantiatePrefab(collisionInfo.hitPoint, segment.transform.up);
                            topPrefabInstantiated = true;
                        }
                    }

                    if (!isTop && !bottomPrefabInstantiated && canInstantiate)
                    {
                        // Check for prior collisions before the current index
                        if (!HasPriorCollisions(isTopCollidingArray, i))
                        {
                            // Check if both top and bottom beams are colliding at the same index
                            if (isTopCollidingArray[i])
                            {
                                // Calculate the middle point between the top and bottom beams at the colliding index
                                InstantiateMiddlePrefab(collisionInfo.hitPoint, topBeamSegments[i].transform.position, bottomBeamSegments[i].transform.position);
                                topPrefabInstantiated = true;
                                bottomPrefabInstantiated = true;
                                continue; // Skip regular prefab instantiation
                            }
                            else
                            {
                                InstantiatePrefab(collisionInfo.hitPoint, segment.transform.up);
                                bottomPrefabInstantiated = true;
                            }
                        }
                        else
                        {
                            InstantiatePrefab(collisionInfo.hitPoint, segment.transform.up);
                            bottomPrefabInstantiated = true;
                        }
                    }

                    for (int j = i; j < length; j++)
                    {
                        beamSegments[j].GetComponent<SpriteRenderer>().enabled = false;
                        beamSegments[j].GetComponent<BoxCollider2D>().enabled = false;
                    }

                    for (int k = i + 1; k < length; k++)
                    {
                        isCollidingArray[k] = true;
                    }

                    reEnableInProgress = true;
                }
            }

            if (!reEnableInProgress)
            {
                EnableSegmentsAgainSequentially(beamSegments, isTop, isCollidingArray);
            }
            else
            {
                ReEnableSegmentsInOrder(beamSegments, isTop, isCollidingArray);
                yield return new WaitForSeconds(delayBetweenReEnable);
                reEnableInProgress = false;

                topPrefabInstantiated = false;
                bottomPrefabInstantiated = false;
            }
        }
    }

    bool HasPriorCollisions(bool[] collisionArray, int currentIndex)
    {
        for (int i = 0; i < currentIndex; i++)
        {
            if (collisionArray[i])
            {
                return true;
            }
        }
        return false;
    }

    void InstantiatePrefab(Vector3 position, Vector2 beamDirection)
    {
        float angle = Mathf.Atan2(beamDirection.y, beamDirection.x) * Mathf.Rad2Deg;
        if (canInstantiateThePrefabs)
        {
            Instantiate(prefabToInstantiate, position, Quaternion.Euler(0f, 0f, angle));
        }
    }

    void InstantiateMiddlePrefab(Vector3 position, Vector3 topPosition, Vector3 bottomPosition)
    {
        Vector3 middlePoint = (topPosition + bottomPosition) / 2f;
        float angle = Mathf.Atan2((topPosition - bottomPosition).y, (topPosition - bottomPosition).x) * Mathf.Rad2Deg;

        if (canInstantiateThePrefabs)
        {
            Instantiate(middlePrefabToInstantiate, middlePoint, Quaternion.Euler(0f, 0f, angle));
        }
    }

    void EnableSegmentsAgainSequentially(GameObject[] beamSegments, bool isTop, bool[] isCollidingArray)
    {
        int length = beamSegments.Length;

        for (int i = 0; i < length; i++)
        {
            var segment = beamSegments[i];

            if (isTop ? !isCollidingArray[i] : !isCollidingArray[i])
            {
                segment.GetComponent<SpriteRenderer>().enabled = true;
                segment.GetComponent<BoxCollider2D>().enabled = true;
                StartCoroutine(WaitAndEnableNextSegment(segment, delayBetweenSegments));
            }
        }
    }

    IEnumerator ReEnableSegmentsInOrder(GameObject[] beamSegments, bool isTop, bool[] isCollidingArray)
    {
        int length = beamSegments.Length;

        for (int i = 0; i < length; i++)
        {
            if (isTop ? !isCollidingArray[i] : !isCollidingArray[i])
            {
                beamSegments[i].GetComponent<SpriteRenderer>().enabled = true;
                beamSegments[i].GetComponent<BoxCollider2D>().enabled = true;
                yield return new WaitForSeconds(delayBetweenSegments);
            }
        }
    }

    IEnumerator WaitAndEnableNextSegment(GameObject segment, float delay)
    {
        yield return new WaitForSeconds(delay);
        segment.GetComponent<SpriteRenderer>().enabled = true;
        segment.GetComponent<BoxCollider2D>().enabled = true;
    }

    struct CollisionInfo
    {
        public bool isColliding;
        public Vector3 hitPoint;
    }

    CollisionInfo CheckCollision(GameObject beamSegment)
    {
        Vector3 startPos = beamSegment.transform.position;
        Vector3 endPos = startPos + beamSegment.transform.up * Mathf.Min(beamSegment.transform.localScale.y, maxRaycastLength);

        Debug.DrawRay(startPos, endPos - startPos, Color.red);

        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, LaserCollisionMask);

        return new CollisionInfo
        {
            isColliding = hit.collider != null,
            hitPoint = hit.collider != null ? hit.point : Vector3.zero
        };
    }
}