using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public bool interactable = true;
    bool openClosed = false;
    public void OpenClose()
    {
        if (interactable)
        {
            if (!openClosed)
            {
                transform.GetComponent<MeshRenderer>().enabled = false;
                transform.GetComponent<Collider>().enabled = false;
                openClosed = false;
            } else
            {
                transform.GetComponent<MeshRenderer>().enabled = true;
                transform.GetComponent<Collider>().enabled = true;
                openClosed = true;
            }
        }
    }

	public void Detector(string arg)
    {
        if (arg == "open")
        {
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetComponent<Collider>().enabled = false;
        }

        if (arg == "close")
        {
            transform.GetComponent<MeshRenderer>().enabled = true;
            transform.GetComponent<Collider>().enabled = true;
        }
    }
}
