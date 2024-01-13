using System.Collections;
using UnityEngine;

public class BulletsDirection : MonoBehaviour
{
    public Vector3 lastPosition;
    public bool hasCheckedDirection = false;

    public bool isMovingRight = false;
    public bool isMovingLeft = false;
    public bool isMovingUp = false;
    public bool isMovingDown = false;

    // Adjust this threshold based on the acceptable deviation
    private float deviationThreshold = 0.1f;

    void Start()
    {

    }

    void OnEnable()
    {
        hasCheckedDirection = false;
        ResetDirectionBools();
        lastPosition = transform.position;
        StartCoroutine(CheckDirectionAfterDelay(0.1f));
    }

    void OnDisable()
    {
        hasCheckedDirection = true;
    }

    void ResetDirectionBools()
    {
        isMovingRight = false;
        isMovingLeft = false;
        isMovingUp = false;
        isMovingDown = false;
    }

    IEnumerator CheckDirectionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!hasCheckedDirection)
        {
            Vector3 currentPosition = transform.position;

            // Compare current position with the last position to determine direction
            float deltaX = currentPosition.x - lastPosition.x;
            float deltaY = currentPosition.y - lastPosition.y;

            // Check for diagonal directions with a deviation threshold
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) + deviationThreshold)
            {
                if (deltaX > 0)
                {
                    isMovingRight = true;
                }
                else if (deltaX < 0)
                {
                    isMovingLeft = true;
                }
            }
            else if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX) + deviationThreshold)
            {
                if (deltaY > 0)
                {
                    isMovingUp = true;
                }
                else if (deltaY < 0)
                {
                    isMovingDown = true;
                }
            }
            // Check for primary directions
            else
            {
                if (deltaX > 0 && deltaY > 0)
                {
                    isMovingUp = true;
                    isMovingRight = true;
                }
                else if (deltaX < 0 && deltaY > 0)
                {
                    isMovingUp = true;
                    isMovingLeft = true;
                }
                else if (deltaX > 0 && deltaY < 0)
                {
                    isMovingDown = true;
                    isMovingRight = true;
                }
                else if (deltaX < 0 && deltaY < 0)
                {
                    isMovingDown = true;
                    isMovingLeft = true;
                }
            }

            // Debug statement for the detected direction
            // Debug.Log("Projectile is moving " + bulletDirection);

            lastPosition = currentPosition;
            hasCheckedDirection = true;
        }
    }
}