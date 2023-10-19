using UnityEngine;

public class ToggleWeapons : MonoBehaviour
{
    public string torsoTag = "Torso";
    public string machineGunLightsTag = "MachineGunLights";
    public GameObject torsoObject;
    public GameObject machineGunObject;
    public bool torsoObjectActive = true;
    public bool machineGunActive = false;

    void Start()
    {
        torsoObject = GameObject.FindGameObjectWithTag(torsoTag);
        machineGunObject = GameObject.FindGameObjectWithTag(machineGunLightsTag);

        // Ensure both objects are initially in the correct state if found
        if (torsoObject != null)
        {
            torsoObject.SetActive(torsoObjectActive);
        }

        if (machineGunObject != null)
        {
            machineGunObject.SetActive(!torsoObjectActive);
            machineGunActive = !torsoObjectActive;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (torsoObject != null && machineGunObject != null)
            {
                if (torsoObjectActive)
                {
                    torsoObjectActive = false;
                    machineGunActive = true;

                    // Set object states accordingly
                    torsoObject.SetActive(false);
                    machineGunObject.SetActive(true);
                }
                else
                {
                    machineGunActive = false;
                    torsoObjectActive = true;

                    // Set object states accordingly
                    machineGunObject.SetActive(false);
                    torsoObject.SetActive(true);
                }
            }
        }
    }
}