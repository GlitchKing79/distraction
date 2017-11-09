using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    light, heavy, normal, player
}
public class Itemhandler : MonoBehaviour{

    public string name = "";
    public bool isPickup = false;
    public bool complete = false;
    public float boyancy = 0;
    public ItemType itmtype = ItemType.normal;

    void Awake()
    {
        if (complete == false)
        {
            complete = isPickup;
        }

        if (isPickup)
        {
            transform.tag = "PickUp";
            if (!GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>();
            }
        }
    }
}
