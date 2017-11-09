using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerController : MonoBehaviour {

    public Transform child;
    int state = 1;
    public float speed = 1;
    public Transform targetObject;
    public string voidMessage = "MarkerDetect";

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "Player")
        {
            if (targetObject != null)
            {
                targetObject.SendMessage(voidMessage, SendMessageOptions.DontRequireReceiver);
            }
            Destroy(gameObject);
        }
    }

    void Update()
    { 
        if (child.localPosition.y >= 1)
        {
            state = -1;

        } else if (child.localPosition.y <= -1)
        {
            state = 1;
        }

        child.localPosition += new Vector3(0, state, 0) * Time.deltaTime * speed;
    }
}
