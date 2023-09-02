using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableColliderOnSlopes : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && GetComponent<BoxCollider2D>().isTrigger == true)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
