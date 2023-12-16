using MoreMountains.CorgiEngine;
using System.Collections;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    public GameObject[] topBeamSegments;
    public GameObject[] bottomBeamSegments;
    public float delayBetweenSegments = 0.0f;
    public float delayBetweenReEnable = 0.05f; // Adjust this value for the delay between re-enabling segments
    public LayerMask LaserCollisionMask = LayerManager.ObstaclesLayerMask;
    public bool[] isTopCollidingArray;
    public bool[] isBottomCollidingArray;
    public bool reEnableInProgress;
    public float maxRaycastLength = 10f; // Adjust this value to limit the raycast length

    void Start()
    {
        isTopCollidingArray = new bool[topBeamSegments.Length];
        isBottomCollidingArray = new bool[bottomBeamSegments.Length];

        // Enable the segments sequentially with a delay
        StartCoroutine(EnableSegmentsSequentially(topBeamSegments, true, isTopCollidingArray));
        StartCoroutine(EnableSegmentsSequentially(bottomBeamSegments, false, isBottomCollidingArray));
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

                // If colliding, disable rendering for the segment and all the ones to the right
                if (collisionInfo.isColliding)
                {
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
            }
        }
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
    }

    CollisionInfo CheckCollision(GameObject beamSegment)
    {
        Vector3 startPos = beamSegment.transform.position;
        Vector3 endPos = startPos + beamSegment.transform.up * Mathf.Min(beamSegment.transform.localScale.y, maxRaycastLength);

        // Visual representation of the raycast
        Debug.DrawRay(startPos, endPos - startPos, Color.red);

        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, LaserCollisionMask);

        // If hit, set isColliding to true
        return new CollisionInfo
        {
            isColliding = hit.collider != null
        };
    }
}
