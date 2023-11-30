using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMessageToUI : MonoBehaviour
{
    public bool flameGun;
    public bool rayGun;
    public bool shotGun;
    public bool spreadGun;
    public bool grenade;
    public UIAndUpgradesController theUIController;

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

            if (grenade)
            {
                theUIController.Grenade();
            }
        }
    }
}