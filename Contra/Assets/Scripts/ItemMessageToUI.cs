using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMessageToUI : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public bool flameGun;
    public bool rayGun;
    public bool shotGun;
    public bool spreadGun;
    public bool grenade;
    public UIAndUpgradesController theUIController;

    protected virtual void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }

    /// <summary>
    /// OnDisable, we stop listening to events.
    /// </summary>
    protected virtual void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public void OnMMEvent(CorgiEngineEvent corgiEngineEvent)
    {
        if (corgiEngineEvent.EventType == CorgiEngineEventTypes.PlayerDeath)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        theUIController = GameObject.FindWithTag("UI").GetComponent<UIAndUpgradesController>();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (flameGun)
            {
                theUIController.FlameGun();
            }

            if (rayGun)
            {
                theUIController.RayGun();
            }

            if (shotGun)
            {
                theUIController.ShotGun();
            }

            if (spreadGun)
            {
                theUIController.SpreadGun();
            }

            /*if (grenade)
            {
                theUIController.Grenade();
            }*/
        }
    }
}