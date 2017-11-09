using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SensorType
{
    pressure, radar, laser, toggle
}

public class SensorController : MonoBehaviour {

    public SensorType sensorType;
    public List<ItemType> switchType = new List<ItemType>();
    public Transform linkedTarget = null;

    public string enterArgument = "";
    public string exitArgument = "";


    void OnTriggerEnter(Collider c)
    {
        if (sensorType == SensorType.pressure)
        {
            if (c.tag == "Player" || c.tag == "PickUp")
            {
                for (int i = 0; i < switchType.Count; i++)
                {
                    if (switchType[i] == c.GetComponent<Itemhandler>().itmtype || switchType[i] == c.GetComponent<PlayerController>().plyerType)
                    {
                        transform.GetComponent<MeshRenderer>().enabled = false;
                        linkedTarget.SendMessage("Detector", enterArgument, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }

    void OnTriggerExit (Collider c)
    {
        if (sensorType == SensorType.pressure)
        {
            if (c.tag == "Player" || c.tag == "PickUp")
            {
                for (int i = 0; i < switchType.Count; i++)
                {
                    if (switchType[i] == c.GetComponent<Itemhandler>().itmtype || switchType[i] == c.GetComponent<PlayerController>().plyerType)
                    {
                        transform.GetComponent<MeshRenderer>().enabled = true;
                        linkedTarget.SendMessage("Detector", exitArgument, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }
}
