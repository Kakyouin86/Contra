using MoreMountains.CorgiEngine;
using UnityEngine;

public class ItemFeedbacks : MonoBehaviour
{
    public GameObject thePickup;
    public GameObject theSmokeEffect;
    public Health health;
    public bool hasInstantiated = false;
    public Vector3 offset1 = new Vector3(0.35f, 1.5f, 0f);
    public Vector3 offset2 = new Vector3(-0.35f, 1.5f, 0f);
    public float jumpForce = 5f;

    void Start()
    {
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (health.CurrentHealth <= 0 && !hasInstantiated)
        {
            GetComponent<Animator>().SetBool("Broken", true);
            if (theSmokeEffect != null)
            {
                Instantiate(theSmokeEffect, transform.position + offset1, transform.rotation);
                Instantiate(theSmokeEffect, transform.position + offset2, transform.rotation);
            }
            if (thePickup != null)
            {
                GameObject pickupInstance = Instantiate(thePickup, transform.position, transform.rotation);
                Rigidbody2D rb = pickupInstance.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            }
            hasInstantiated = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}