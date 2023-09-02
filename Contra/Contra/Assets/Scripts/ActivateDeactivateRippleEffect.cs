using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateRippleEffect : MonoBehaviour
{
    public GameObject theRippleEffect;
    public GameObject theLegs;
    // Start is called before the first frame update
    void Start()
    {
        theRippleEffect = GameObject.FindGameObjectWithTag("Player").GetComponent<AdditionalMovementSettings>().theRippleEffect;
        theLegs = GameObject.FindGameObjectWithTag("Player").GetComponent<AdditionalMovementSettings>().theLegs;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRippleEffect()
    {
        theRippleEffect.SetActive(true);
    }

    public void DeactivateRippleEffect()
    {
        theRippleEffect.SetActive(false);
        theLegs.SetActive(true);
    }
}
