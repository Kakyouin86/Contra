using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.WSA;


public class OverridesInSmokeFirepoint : MonoBehaviour
{
    public Animator theAnimator;
    public Vector3 originalVector3;

    void Start()
    {
        theAnimator = GameObject.FindWithTag("PlayerSprites").GetComponent<Animator>();
        originalVector3 = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Up") || theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Up Walking Flame Gun"))
        {
            transform.localPosition = new Vector3(-0.10f, -0.07f, 0f);
        }

        else if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Down") || theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Down Walking Flame Gun"))
        {
            transform.localPosition = new Vector3(-0.11f, -0.11f, 0f);
        }
        else
        {
            transform.localPosition = originalVector3;
        }

        /*if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Straight") || theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Torso Idle"))
        {
            transform.localPosition = new Vector3(0.0f, 0.0f, 0f);
        }

         if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Up") || theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Hold Up"))
        {
            transform.localPosition = new Vector3(-0.08f, 0.08f, 0f);
        }

         if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Down"))
        {
            transform.localPosition = new Vector3(0.25f, 0.0f, 0f);
        }

         if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Up") || theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Up Walking Flame Gun"))
        {
            transform.localPosition = new Vector3(-0.10f, -0.07f, 0f);
        }

         if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Down") || theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot Diagonal Down Walking Flame Gun"))
        {
            transform.localPosition = new Vector3(-0.11f, -0.11f, 0f);
        }

         if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Crouch Shooting"))
        {
            transform.localPosition = new Vector3(0.1f, 0.0f, 0f);
        }

         if (theAnimator.GetCurrentAnimatorStateInfo(1).IsName("Crouch Shooting"))
        {
            transform.localPosition = new Vector3(0.1f, 0.0f, 0f);*/
    }
}