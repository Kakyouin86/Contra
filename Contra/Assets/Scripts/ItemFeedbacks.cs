using MoreMountains.CorgiEngine;
using UnityEngine;

public class ItemFeedbacks : MonoBehaviour
{
    public GameObject thePickup;
    public Health health;
    public bool hasInstantiated = false;

    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.CurrentHealth <= 0 && !hasInstantiated)
        {
            GetComponent<Animator>().SetBool("Broken", true);
            Instantiate(thePickup, transform.position, transform.rotation);
            hasInstantiated = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
