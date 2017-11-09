using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class NetworkController : MonoBehaviour {

    public int port = 7777;
    public string ip = "127.0.0.1";
    public string offlineScene;
    public string onlineScene;

    public GameObject playerPrefab;
    public bool autoSpawn = true;

    NetworkManager netman;

    void Awake()
    {
            netman = gameObject.AddComponent<NetworkManager>();
#if UNITY_EDITOR
            netman.hideFlags = HideFlags.HideInInspector;
#endif
    }

    void Start()
    {
        if (netman == null)
        {
            Debug.Log("NetworkManager singleton is null why?");
            return;
        }
        netman.offlineScene = offlineScene;
        netman.onlineScene = onlineScene;
        netman.playerPrefab = playerPrefab;
        netman.autoCreatePlayer = autoSpawn;
    }
    public void StartHost()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        netman.StartClient();
    }

    public void SetPort()
    {
        netman.networkPort = port;
    }

    public void SetIP()
    {
        netman.networkAddress = ip;
    }

    public void GetPort(Text txt)
    {
        if (txt.text != "")
        {
            port = int.Parse(txt.text);
        }
    }

    public void GetIP(Text txt)
    {
        if (txt.text != "")
        {
            ip = txt.text;
        }
    }
}
