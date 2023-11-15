using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject theThingToInstantiate;
    public Vector3 theOffset = new Vector3(0.0f, 0.0f, 0.0f);

    void OnEnable()
    {
        Instantiate(theThingToInstantiate, transform.position + theOffset, transform.rotation);
    }

    void Start()
    {
        Instantiate(theThingToInstantiate, transform.position + theOffset, transform.rotation);
    }
}