using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Itemhandler))]
public class IncompleteObject : MonoBehaviour {

    Itemhandler itemController;

    public Material incomplete;
    public Material complete;

    public string[] usableitems;
    bool[] isItemUsed;

    void Start ()
    {
        itemController = GetComponent<Itemhandler>();
        transform.GetComponent<MeshRenderer>().material = incomplete;
        isItemUsed = new bool[usableitems.Length];
	}

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.transform.GetComponent<Itemhandler>())
        {
            for (int i = 0; i < usableitems.Length; i++)
            {
                if (usableitems[i] == c.transform.GetComponent<Itemhandler>().name)
                {
                    isItemUsed[i] = true;
                    Destroy(c.gameObject);
                }
            }

            for (int i = 0; i < isItemUsed.Length; i++)
            {
                if (isItemUsed[i] == false)
                {
                    return;
                }
            }

            itemController.complete = true;
            transform.GetComponent<MeshRenderer>().material = complete;
        }


    }

    
}
