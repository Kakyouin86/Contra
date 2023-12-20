using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using MoreMountains.Tools;

public class DestroyThisGameObject : MonoBehaviour
{
    //public bool itIsParticleSystem;
    public float delay = 0f;
    public bool destroyByTime = false; //If this is true, then the GO will be destroyed when hitting that time. If not, then the animator gets to the final position of the animation currently playing.
    public bool destroyTheParent = false;

    public void Start()
    {
        //if (!itIsParticleSystem)
        {
            if (this.GetComponent<Animator>() != null && !destroyByTime)
            {
                if (destroyTheParent)
                {
                    Destroy(transform.parent.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
                }
                else
                {
                    Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
                }
            }
        }
    }

    public void Update()
    {
        if (destroyByTime)
        {
            delay -= Time.deltaTime;

            if (delay <= 0f)
            {
                if (destroyTheParent)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void OnTransformParentChanged()
    {
        //if (itIsParticleSystem)
        {
            //Destroy(gameObject, delay);
            //gameObject.SetActive(false);
        }
    }
}