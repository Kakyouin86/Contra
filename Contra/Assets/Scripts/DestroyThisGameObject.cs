using UnityEngine;
using System.Collections;

public class DestroyThisGameObject : MonoBehaviour
{
    //public bool itIsParticleSystem;
    public float delay = 0f;

    public void Start()
    {
        //if (!itIsParticleSystem)
        {
            if (this.GetComponent<Animator>() != null)
            {
                Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
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