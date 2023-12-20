using MoreMountains.CorgiEngine;
using System.Collections;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    public bool canInstantiate = true;
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
    public GameObject prefabToInstantiate; // Add the prefab to instantiate

    // Flags to track prefab instantiation state for top and bottom beams separately
    private bool topPrefabInstantiated = false;
    private bool bottomPrefabInstantiated = false;

    void Start()
    {
        canInstantiate = true;
        isTopCollidingArray = new bool[topBeamSegments.Length];
        isBottomCollidingArray = new bool[bottomBeamSegments.Length];

        StartCoroutine(EnableSegmentsSequentially(topBeamSegments, true, isTopCollidingArray));
        StartCoroutine(EnableSegmentsSequentially(bottomBeamSegments, false, isBottomCollidingArray));
    }

    void Update()
    {
        timerToStopInstantiating += Time.deltaTime;
        if (timerToStopInstantiating >= stopInstantiating)
        {
            canInstantiate = false;
        }
    }

    IEnumerator EnableSegmentsSequentially(GameObject[] beamSegments, bool isTop, bool[] isCollidingArray)
    {
        int length = beamSegments.Length;

        while (true)
        {
            for (int i = 0; i < length; i++)
            {
                var segment = beamSegments[i];

                // If not colliding, enable rendering for the segment
                if (isTop ? !isCollidingArray[i] : !isCollidingArray[i])
                {
                    segment.GetComponent<SpriteRenderer>().enabled = true;
                }

                // Wait for the specified delay
                yield return new WaitForSeconds(delayBetweenSegments);

                // Check for collisions after each segment is instantiated
                var collisionInfo = CheckCollision(segment);

                // Update collision status based on the current collision
                isCollidingArray[i] = collisionInfo.isColliding;

                // If colliding and prefab not instantiated, instantiate prefab at the hit point and disable rendering for the segment and all the ones to the right
                if (collisionInfo.isColliding)
                {
                    if (isTop && !topPrefabInstantiated && canInstantiate)
                    {
                        InstantiatePrefab(collisionInfo.hitPoint, segment.transform.up);
                        topPrefabInstantiated = true;
                    }

                    if (!isTop && !bottomPrefabInstantiated && canInstantiate)
                    {
                        InstantiatePrefab(collisionInfo.hitPoint, segment.transform.up);
                        bottomPrefabInstantiated = true;
                    }

                    // Disable rendering for the segment and all the ones to the right (this part remains the same as before)
                    for (int j = i; j < length; j++)
                    {
                        beamSegments[j].GetComponent<SpriteRenderer>().enabled = false;
                    }

                    // Make it impossible to render the rest of the array next to the colliding segment
                    for (int k = i + 1; k < length; k++)
                    {
                        isCollidingArray[k] = true;
                    }

                    reEnableInProgress = true;
                }
            }

            // Enable segments again (sequentially) if no collision
            if (!reEnableInProgress)
            {
                EnableSegmentsAgainSequentially(beamSegments, isTop, isCollidingArray);
            }
            else
            {
                // Re-enable segments in order after the specified delay
                ReEnableSegmentsInOrder(beamSegments, isTop, isCollidingArray);
                yield return new WaitForSeconds(delayBetweenReEnable);
                reEnableInProgress = false;

                // Reset the prefab instantiation flags when re-enabling segments
                topPrefabInstantiated = false;
                bottomPrefabInstantiated = false;
            }
        }
    }

    void InstantiatePrefab(Vector3 position, Vector2 beamDirection)
    {
        // Calculate the rotation based on the beam's direction
        float angle = Mathf.Atan2(beamDirection.y, beamDirection.x) * Mathf.Rad2Deg;

        // Instantiate the prefab at the hit point with the calculated rotation
        Instantiate(prefabToInstantiate, position, Quaternion.Euler(0f, 0f, angle));
    }

    void EnableSegmentsAgainSequentially(GameObject[] beamSegments, bool isTop, bool[] isCollidingArray)
    {
        int length = beamSegments.Length;

        for (int i = 0; i < length; i++)
        {
            var segment = beamSegments[i];

            // If not colliding, enable rendering for the segment
            if (isTop ? !isCollidingArray[i] : !isCollidingArray[i])
            {
                segment.GetComponent<SpriteRenderer>().enabled = true;
                // Wait for the specified delay
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
                yield return new WaitForSeconds(delayBetweenSegments); // Optional: introduce a delay between re-enabling segments
            }
        }
    }

    IEnumerator WaitAndEnableNextSegment(GameObject segment, float delay)
    {
        yield return new WaitForSeconds(delay);
        segment.GetComponent<SpriteRenderer>().enabled = true;
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

        // Visual representation of the raycast
        Debug.DrawRay(startPos, endPos - startPos, Color.red);

        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, LaserCollisionMask);

        // If hit, set isColliding to true and return the hit point
        return new CollisionInfo
        {
            isColliding = hit.collider != null,
            hitPoint = hit.collider != null ? hit.point : Vector3.zero
        };
    }
}