using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
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
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);
        }
    }

    void Start()
    {
        theUIController = GameObject.FindWithTag("UIPlayer1").GetComponent<UIAndUpgradesController>();
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