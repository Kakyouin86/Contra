using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class SpecialShootController : MonoBehaviour
{
    public bool isAlive = true;
    public bool canShootSpecialShoot = false;
    public bool isShooting = false;
    public bool hasReachedTarget = false;
    public float specialShootCooldown = 3f; // The cooldown time in seconds
    public float timer = 0f;
    public float originalSpecialShootDuration = 4f;   // The original duration of the special shot in seconds
    //public KeyCode specialShootKey = KeyCode.X; // The key to trigger the special shot
    public Image[] counterImages; // Array of images representing the counter progress
    public Image completeBar; // The first new image after reaching the target time
    public Image maxCompleteBar; // The second new image after reaching the target time
    public Image theFlameShoot; // The second new image after reaching the target time
    public Vector3 initialFlameShootPosition; // Initial position of the flame shot
    public Vector3 finalFlameShootPosition; // Final position of the flame shot
    public float currentFlameShootDuration;
    public float flameShootSpeed;
    public Player player;
    public SpecialShootAndRaycastVisualization thePlayer;
    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    void Start()
    {
        // Disable all counter images at the beginning
        foreach (Image image in counterImages)
        {
            image.enabled = false;
        }

        // Disable the new images at the beginning
        completeBar.enabled = false;
        completeBar.GetComponent<Animator>().enabled = false;
        maxCompleteBar.enabled = false;
        maxCompleteBar.GetComponent<Animator>().enabled = false;
        theFlameShoot.enabled = false;
        theFlameShoot.GetComponent<Animator>().enabled = false;

        // Set initial and final positions for the flame shot
        initialFlameShootPosition = theFlameShoot.rectTransform.localPosition;
        finalFlameShootPosition = new Vector3(finalFlameShootPosition.x, initialFlameShootPosition.y, initialFlameShootPosition.z);

        // Set the current flame shot duration to the original duration
        currentFlameShootDuration = originalSpecialShootDuration;
        thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialShootAndRaycastVisualization>();
    }

    void Update()
    {
        // Check if the character is alive
        if (IsCharacterAlive())
        {
            // Check if the special shot is ready to be used
            if (timer >= specialShootCooldown)
            {
                canShootSpecialShoot = true;
                thePlayer.canShoot = true;
            }
            else
            {
                // Update the timer if it's still counting
                timer += Time.deltaTime;
            }

            // Check for user input to shoot the special shot
            if (canShootSpecialShoot && player.GetButton(("SpecialShoot")))
            {
                ShootSpecialShot();
            }

            // Check if the special shot is currently being fired
            if (isShooting)
            {
                maxCompleteBar.enabled = false;
                // Move theFlameShot during shooting
                MoveFlameShot();

                // Countdown the duration of the special shot
                currentFlameShootDuration -= Time.deltaTime;

                // Disable counter images during shooting
                foreach (Image image in counterImages)
                {
                    image.enabled = false;
                }

                completeBar.GetComponent<Animator>().SetBool("IsShooting", true);

                // Enable theFlameShot and its Animator component during shooting
                theFlameShoot.enabled = true;
                theFlameShoot.GetComponent<Animator>().enabled = true;

                if (currentFlameShootDuration <= 0f)
                {
                    // Special shot duration is over, reset variables
                    isShooting = false;
                    currentFlameShootDuration = originalSpecialShootDuration; // Reset the duration
                    timer = 0f; // Start counting again
                    completeBar.GetComponent<Animator>().SetBool("IsShooting", false);
                    completeBar.enabled = false;
                    completeBar.GetComponent<Animator>().enabled = false;
                    maxCompleteBar.enabled = false;
                    maxCompleteBar.GetComponent<Animator>().enabled = false;

                    // Disable theFlameShot and its Animator component after shooting
                    theFlameShoot.enabled = false;
                    theFlameShoot.GetComponent<Animator>().enabled = false;
                    theFlameShoot.rectTransform.localPosition = initialFlameShootPosition;

                    // Reset hasReachedTarget to allow special images on the next shoot
                    hasReachedTarget = false;
                }
            }
            else
            {
                // Update the counter images based on the timer's progress
                UpdateCounterImages();
            }
        }
    }

    void UpdateCounterImages()
    {
        // Calculate the progress percentage based on the timer and cooldown
        float progress = Mathf.Clamp01(timer / specialShootCooldown);

        // Calculate the interval dynamically based on the number of images
        float interval = specialShootCooldown / counterImages.Length;

        // Calculate the index of the image to enable
        int newIndex = Mathf.FloorToInt(progress * (counterImages.Length - 1));

        // Enable the image when the counter passes its corresponding time
        if (progress >= (newIndex + 1) / (float)counterImages.Length)
        {
            EnableImage(newIndex);
        }

        // Check if the target time is reached and handle the transition to new images
        if (!hasReachedTarget && timer >= specialShootCooldown)
        {
            hasReachedTarget = true;

            // Disable the last image in the array
            if (counterImages.Length > 0)
            {
                counterImages[counterImages.Length - 1].enabled = false;
            }

            // Enable the first new image
            completeBar.enabled = true;
            completeBar.GetComponent<Animator>().enabled = true;
            maxCompleteBar.enabled = true;
            maxCompleteBar.GetComponent<Animator>().enabled = true;

            // Set the remaining duration of the special shot
            currentFlameShootDuration = originalSpecialShootDuration;
        }
    }

    void EnableImage(int index)
    {
        // Disable all images first
        foreach (Image image in counterImages)
        {
            image.enabled = false;
        }

        // Enable the specified image
        if (index >= 0 && index < counterImages.Length)
        {
            counterImages[index].enabled = true;
        }
    }

    void ShootSpecialShot()
    {
        // Implement your logic to shoot the special shot here
        // Debug.Log("Special Shot Fired!");

        // Set isShooting to true to indicate that the special shot is currently being fired
        isShooting = true;

        // Calculate the speed of the flame shot movement based on the original duration
        flameShootSpeed = (initialFlameShootPosition.x - finalFlameShootPosition.x) / originalSpecialShootDuration;

        // Reset the timer after shooting
        timer = 0f;
        canShootSpecialShoot = false;
        thePlayer.canShoot = false;
    }

    void MoveFlameShot()
    {
        if (isShooting)
        {
            // Calculate the progress of the flame shot movement
            float progress = 1 - (currentFlameShootDuration / originalSpecialShootDuration);

            // Interpolate between initial and final X positions based on progress
            float targetX = Mathf.Lerp(initialFlameShootPosition.x, finalFlameShootPosition.x, progress);

            // Set the new position while maintaining the Y and Z coordinates
            Vector3 targetPosition = new Vector3(targetX, initialFlameShootPosition.y, initialFlameShootPosition.z);
            theFlameShoot.rectTransform.localPosition = targetPosition;
        }
    }

    bool IsCharacterAlive()
    {
        return isAlive;
    }
}
