using UnityEngine;
using MoreMountains.CorgiEngine;

public class BoxColliderExample : MonoBehaviour
{
    public Animator theAnimator;

    public void Start()
    {
        theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        theAnimator.SetBool("Death Left", false);
        DamageOnTouch damageOnTouch = other.GetComponent<DamageOnTouch>();
        if (damageOnTouch != null)
        {
            // Get the relative position of the colliding object
            Vector2 relativePosition = other.transform.position - transform.position;

            // Determine if it's a horizontal collision
            if (relativePosition.x > 0)
            {
                
                //theAnimator.SetBool("Death Left", true);
            }
            else
            {
                //theAnimator.SetBool("Death Left", false);
            }
        }
    }
}