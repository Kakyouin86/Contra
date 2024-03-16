using MoreMountains.CorgiEngine;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using UnityEngine;

public class ItemMessageToUI : MonoBehaviour, MMEventListener<CorgiEngineEvent>
{
    public bool isPlayer1;
    public bool flameGun;
    public bool rayGun;
    public bool shotGun;
    public bool spreadGun;
    public bool grenade;
    public UIAndUpgradesController theUIAndUpgradesController;

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
        // theUIController = GameObject.FindWithTag("UIPlayer1").GetComponent<UIAndUpgradesController>(); This is the 1 player version. The next line is the 2 players version.
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.gameObject.GetComponent<Character>() != null && collider.gameObject.GetComponent<Character>().PlayerID == "Player1")
            {
                isPlayer1 = true;
                theUIAndUpgradesController = GameObject.FindGameObjectWithTag("UIPlayer1").GetComponent<UIAndUpgradesController>();
            }
            else
            {
                isPlayer1 = false;
                theUIAndUpgradesController = GameObject.FindGameObjectWithTag("UIPlayer2").GetComponent<UIAndUpgradesController>();
            }
            if (flameGun)
            {
                theUIAndUpgradesController.FlameGun();
            }

            if (rayGun)
            {
                theUIAndUpgradesController.RayGun();
            }

            if (shotGun)
            {
                theUIAndUpgradesController.ShotGun();
            }

            if (spreadGun)
            {
                theUIAndUpgradesController.SpreadGun();
            }

            /*if (grenade)
            {
                theUIAndUpgradesController.Grenade();
            }*/
        }
    }
}