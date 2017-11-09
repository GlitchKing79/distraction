using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour {

    public MarkerController[] markers;
    int nextMarker = 0;
    void Start()
    {
        for (int i = 0; i < markers.Length; i++)
        {
            markers[i].gameObject.SetActive(false);
        }
    }

	public void MarkerDetect()
    {
        try
        {
            markers[nextMarker].gameObject.SetActive(true);
            nextMarker++;
        }
        catch
        {
            if (nextMarker < markers.Length)
            {
                nextMarker++;
                MarkerDetect();
            }
            //ignore if error
        }
    }
}
