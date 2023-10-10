using System.Collections;
using UnityEngine;

namespace BarthaSzabolcs.Tutorial_SpriteFlash
{
    public class FlashSprites : MonoBehaviour
    {
        [Tooltip("Material to switch to during the flash.")]
        public Material flashMaterial;
        [Tooltip("Duration of each flash.")]
        public float flashDuration = 0.2f; // Duration of each flash
        [Tooltip("Total duration of the flicker effect.")]
        public float totalFlickerDuration = 0.35f; // Total duration of the flicker effect (overwritten by the Roll ability)
        public float flickerTimer = 0.0f; // Timer for controlling the flicker duration
        public bool run;
        public SpriteRenderer spriteRenderer;
        public Material originalMaterial;
        public Coroutine flashRoutine;

        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalMaterial = spriteRenderer.material;
            flickerTimer = 0f; // Initialize the flicker timer
        }

        public void Update()
        {
            if (run)
            {
                // Check if the flicker timer is within the total flicker duration
                if (flickerTimer < totalFlickerDuration)
                {
                    // Start or continue the flicker effect
                    if (flashRoutine == null)
                    {
                        flashRoutine = StartCoroutine(FlashRoutine());
                    }
                    else
                    {
                        flickerTimer += Time.deltaTime;
                    }
                }
                else
                {
                    // Stop the flicker effect when the total duration is reached
                    run = false;
                }
            }
            else
            {
                // If 'run' is false, reset the spriteRenderer's material to the original material and the timer.
                spriteRenderer.material = originalMaterial;
                flickerTimer = 0f; // Reset the flicker timer
                if (flashRoutine != null)
                {
                    // If a flashRoutine was running, stop it.
                    StopCoroutine(flashRoutine);
                    flashRoutine = null;
                }
            }
        }

        public IEnumerator FlashRoutine()
        {
            while (true)
            {
                // Swap to the flashMaterial.
                spriteRenderer.material = flashMaterial;

                // Pause for "flashDuration" seconds.
                yield return new WaitForSeconds(flashDuration);

                // Swap back to the original material.
                spriteRenderer.material = originalMaterial;

                // Pause for "flashDuration" seconds.
                yield return new WaitForSeconds(flashDuration);
            }
        }
    }
}
