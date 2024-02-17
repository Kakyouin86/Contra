using System.Collections;
using MoreMountains.CorgiEngine;
using UnityEngine;
using static MoreMountains.CorgiEngine.Character;


public class BulletsDirection : MonoBehaviour
{
    public Vector3 lastPosition;
    public float deltaX;
    public float deltaY;
    public bool hasCheckedDirection = false;
    public bool isMovingRight = false;
    public bool isMovingLeft = false;
    public bool isMovingUp = false;
    public bool isMovingDown = false;
    public float deviationThreshold = 0.1f;
    public float delayInCalculation = 0.1f;
    public GameObject thePlayer;
    public bool facingRight = false;
    public bool cantCalculate = false;
    public RaycastHit2D hitLeft;
    public RaycastHit2D hitRight;
    public RaycastHit2D hitUp;
    public RaycastHit2D hitDown;
    public float raycastLength = 10f;
    public LayerMask platformLayer;
    public bool leftWall = false;
    public bool rightWall = false; 
    public bool topWall = false;
    public bool bottomWall = false;
    public bool closeToTwoWalls = false;
    public bool hangingShoot = false;

    public void Start()
    {
        thePlayer = GetComponent<Projectile>().GetOwner().gameObject;
        if (thePlayer != null)
        {
            facingRight = thePlayer.GetComponent<Character>().IsFacingRight;
        }
        if (thePlayer.GetComponent<AdditionalMovementSettings>().verticalLadder || thePlayer.GetComponent<AdditionalMovementSettings>().horizontalLadder)
        {
            hangingShoot = true;
        }
    }

    public void Update()
    {
        //Debug.Log(hitLeft + " hit Left");
        //Debug.Log(hitRight + " hit Right");
        //Debug.Log(hitUp + " hit Up");
        //Debug.Log(hitDown + " hit Down");
    }

    public void OnEnable()
    {
        cantCalculate = false;
        leftWall = false;
        rightWall = false;
        topWall = false;
        bottomWall = false;
        closeToTwoWalls = false;
        hangingShoot = false;

        if (thePlayer != null)
        {
            facingRight = thePlayer.GetComponent<Character>().IsFacingRight;
        }
        hasCheckedDirection = false;
        ResetDirectionBools();
        lastPosition = transform.position;

        hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastLength, platformLayer);
        hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastLength, platformLayer);
        hitUp = Physics2D.Raycast(transform.position, Vector2.up, raycastLength, platformLayer);
        hitDown = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, platformLayer);
        Debug.DrawRay(transform.position, Vector2.left * 10f, Color.green);
        Debug.DrawRay(transform.position, Vector2.right * 10f, Color.blue);
        Debug.DrawRay(transform.position, Vector2.up * 10f, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 10f, Color.yellow);

        if (hitLeft.collider != null && hitLeft.distance < 3f)
        {
            //Debug.Log("Left Wall");
            leftWall = true;
        }
        if (hitRight.collider != null && hitRight.distance < 3f)
        {
            //Debug.Log("Right Wall");
            rightWall = true;
        }
        if (hitUp.collider != null && hitUp.distance < 3f)
        {
            //Debug.Log("Up Wall");
            topWall = true;
        }
        if (hitDown.collider != null && hitDown.distance < 1.5f)
        {
            //Debug.Log("Down Wall");
            bottomWall = true;
        }

        if (hitDown.collider != null && hitDown.distance < 1.5f && hitRight.collider != null && hitRight.distance < 1.5f)
        {
            //Debug.Log("Extremely close to right and bottom walls");
            closeToTwoWalls = true;
        }

        if (thePlayer != null && thePlayer.GetComponent<AdditionalMovementSettings>().verticalLadder || thePlayer != null && thePlayer.GetComponent<AdditionalMovementSettings>().horizontalLadder)
        {
            hangingShoot = true;
        }

        StartCoroutine(CheckDirectionAfterDelay(delayInCalculation));
        //CheckDirectionAfterDelay();
    }

    public void OnDisable()
    {
        hasCheckedDirection = true;
        if (!isMovingDown && !isMovingUp && !isMovingLeft && !isMovingRight)
        {
            cantCalculate = true;
            hitLeft = Physics2D.Raycast(transform.position, Vector2.left, raycastLength, platformLayer);
            hitRight = Physics2D.Raycast(transform.position, Vector2.right, raycastLength, platformLayer);
            hitUp = Physics2D.Raycast(transform.position, Vector2.up, raycastLength, platformLayer);
            hitDown = Physics2D.Raycast(transform.position, Vector2.down, raycastLength, platformLayer);

            if (hitLeft.collider != null && hitLeft.distance < 2f)
            {
                //Debug.Log("Left Wall Ahead");
                leftWall = true;
            }
            if (hitRight.collider != null && hitRight.distance < 2f)
            {
                //Debug.Log("Right Wall Ahead");
                rightWall = true;
            }
            if (hitUp.collider != null && hitUp.distance < 1f)
            {
                //Debug.Log("Up Wall Ahead");
                topWall = true;
            }
            if (hitDown.collider != null && hitDown.distance < 1f)
            {
                //Debug.Log("Bottom Wall Ahead");
                bottomWall = true;
            }
        }
    }

    IEnumerator RunSecondRaycast()
    {
        yield return new WaitForSeconds(0.0f);
    }

    public void ResetDirectionBools()
    {
        isMovingRight = false;
        isMovingLeft = false;
        isMovingUp = false;
        isMovingDown = false;
    }

    IEnumerator CheckDirectionAfterDelay(float delay)
    //public void CheckDirectionAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        //if (!hasCheckedDirection)
        //{
            Vector3 currentPosition = transform.position;

            // Compare current position with the last position to determine direction
            deltaX = currentPosition.x - lastPosition.x;
            deltaY = currentPosition.y - lastPosition.y;

            // Check for diagonal directions with a deviation threshold
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) + deviationThreshold)
            {
                //Debug.Log("Here 1");
                if (deltaX > 0)
                {
                    //Debug.Log("Here 2");
                    isMovingRight = true;
                }
                else if (deltaX < 0)
                {
                    //Debug.Log("Here 3");
                    isMovingLeft = true;
                }
            }
            else if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX) + deviationThreshold)
            {
                //Debug.Log("Here 4");
                if (deltaY > 0)
                {
                    //Debug.Log("Here 5");
                    isMovingUp = true;
                }
                else if (deltaY < 0)
                {
                    //Debug.Log("Here 6");
                    isMovingDown = true;
                }
            }
            // Check for primary directions
            else
            {
                //Debug.Log("Here 7");
                if (deltaX > 0 && deltaY > 0)
                {
                    //Debug.Log("Here 8");
                    isMovingUp = true;
                    isMovingRight = true;
                }
                else if (deltaX < 0 && deltaY > 0)
                {
                    //Debug.Log("Here 9");
                    isMovingUp = true;
                    isMovingLeft = true;
                }
                else if (deltaX > 0 && deltaY < 0)
                {
                    //Debug.Log("Here 10");
                    isMovingDown = true;
                    isMovingRight = true;
                }
                else if (deltaX < 0 && deltaY < 0)
                {
                    //Debug.Log("Here 11");
                    isMovingDown = true;
                    isMovingLeft = true;
                }
            }

            // Debug statement for the detected direction
            // Debug.Log("Projectile is moving " + bulletDirection);

            lastPosition = currentPosition;
            hasCheckedDirection = true;
        }
    //}
}