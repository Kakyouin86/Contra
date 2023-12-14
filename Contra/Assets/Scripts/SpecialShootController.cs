using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class SpecialShootController : MonoBehaviour
{
    public bool isAlive = true;
    public bool canShootSpecialShoot = false;
    public bool isShooting = false;
    public bool hasReachedTarget = false;
    public bool enableAnimation = true;
    public float specialShootCooldown = 3f;
    public float timer = 0f;
    public float originalSpecialShootDuration = 1.167f;
    public Image[] counterImages;
    public Image completeBar;
    public Image maxCompleteBar;
    public Image theAnimationThatTranslates;
    public Image electricAnimation;
    public Vector3 initialFlameShootPosition;
    public Vector3 finalFlameShootPosition;
    public float currentFlameShootDuration;
    public float theAnimationThatTranslatesSpeed;
    public Player player;
    public SpecialShootAndRaycastVisualization theSpecialShootAndRaycastVisualization;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(0);
    }

    public void Start()
    {
        foreach (Image image in counterImages)
        {
            image.enabled = false;
        }

        completeBar.enabled = false;
        completeBar.GetComponent<Animator>().enabled = false;
        maxCompleteBar.enabled = false;
        maxCompleteBar.GetComponent<Animator>().enabled = false;
        if (theAnimationThatTranslates != null)
        {
            theAnimationThatTranslates.enabled = false;
            theAnimationThatTranslates.GetComponent<Animator>().enabled = false;
            initialFlameShootPosition = theAnimationThatTranslates.rectTransform.localPosition;
        }

        if (electricAnimation != null)
        {
            electricAnimation.enabled = false;
        }

        finalFlameShootPosition = new Vector3(finalFlameShootPosition.x, initialFlameShootPosition.y, initialFlameShootPosition.z);
        currentFlameShootDuration = originalSpecialShootDuration;
        theSpecialShootAndRaycastVisualization = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialShootAndRaycastVisualization>();
    }

    public void Update()
    {
        if (IsCharacterAlive())
        {
            if (timer >= specialShootCooldown)
            {
                canShootSpecialShoot = true;
                theSpecialShootAndRaycastVisualization.canShoot = true;
            }
            else
            {
                timer += Time.deltaTime;
            }

            if (isShooting)
            {
                maxCompleteBar.enabled = false;
                MoveFlameShot();
                currentFlameShootDuration -= Time.deltaTime;

                foreach (Image image in counterImages)
                {
                    image.enabled = false;
                }

                completeBar.GetComponent<Animator>().SetBool("IsShooting", true);

                if (enableAnimation && theAnimationThatTranslates != null)
                {
                    theAnimationThatTranslates.enabled = true;
                    theAnimationThatTranslates.GetComponent<Animator>().enabled = true;
                }

                if (electricAnimation != null)
                {
                    electricAnimation.enabled = true;
                }

                if (currentFlameShootDuration <= 0f)
                {
                    isShooting = false;
                    currentFlameShootDuration = originalSpecialShootDuration;
                    timer = 0f;
                    completeBar.GetComponent<Animator>().SetBool("IsShooting", false);
                    completeBar.enabled = false;
                    completeBar.GetComponent<Animator>().enabled = false;
                    maxCompleteBar.enabled = false;
                    maxCompleteBar.GetComponent<Animator>().enabled = false;

                    if (enableAnimation && theAnimationThatTranslates != null)
                    {
                        theAnimationThatTranslates.enabled = false;
                        theAnimationThatTranslates.GetComponent<Animator>().enabled = false;
                        theAnimationThatTranslates.rectTransform.localPosition = initialFlameShootPosition;
                    }

                    if (electricAnimation != null)
                    {
                        electricAnimation.enabled = false;
                    }

                    hasReachedTarget = false;
                }
            }
            else
            {
                UpdateCounterImages();
            }
        }
    }

    public void UpdateCounterImages()
    {
        float progress = Mathf.Clamp01(timer / specialShootCooldown);
        float interval = specialShootCooldown / counterImages.Length;
        int newIndex = Mathf.FloorToInt(progress * (counterImages.Length - 1));

        if (progress >= (newIndex + 1) / (float)counterImages.Length)
        {
            EnableImage(newIndex);
        }

        if (!hasReachedTarget && timer >= specialShootCooldown)
        {
            hasReachedTarget = true;

            if (counterImages.Length > 0)
            {
                counterImages[counterImages.Length - 1].enabled = false;
            }

            completeBar.enabled = true;
            completeBar.GetComponent<Animator>().enabled = true;
            maxCompleteBar.enabled = true;
            maxCompleteBar.GetComponent<Animator>().enabled = true;

            currentFlameShootDuration = originalSpecialShootDuration;
        }
    }

    public void EnableImage(int index)
    {
        foreach (Image image in counterImages)
        {
            image.enabled = false;
        }

        if (index >= 0 && index < counterImages.Length)
        {
            counterImages[index].enabled = true;
        }
    }

    public void ShootSpecialShot()
    {
        isShooting = true;
        theAnimationThatTranslatesSpeed = (initialFlameShootPosition.x - finalFlameShootPosition.x) / originalSpecialShootDuration;
        timer = 0f;
        canShootSpecialShoot = false;
        theSpecialShootAndRaycastVisualization.canShoot = false;
    }

    public void MoveFlameShot()
    {
        if (isShooting)
        {
            float progress = 1 - (currentFlameShootDuration / originalSpecialShootDuration);
            float targetX = Mathf.Lerp(initialFlameShootPosition.x, finalFlameShootPosition.x, progress);
            Vector3 targetPosition = new Vector3(targetX, initialFlameShootPosition.y, initialFlameShootPosition.z);
            if (theAnimationThatTranslates != null)
            {
                theAnimationThatTranslates.rectTransform.localPosition = targetPosition;
            }
        }
    }

    bool IsCharacterAlive()
    {
        return isAlive;
    }
}
