using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour {

    public MatchInfo matchInfo;
    public NetworkConnection netCon;
    public Text ipAdress;

    static MenuController instance;

    bool isAtStartup = true;

    public NetworkClient myClient;

    public GameObject playerPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(instance);
        }

        DontDestroyOnLoad(instance);
    }

    public void StartServer()
    {
        NetworkServer.Listen(7777);
        Network.InitializeServer(8, 7777,false);
        isAtStartup = false;
    }

    

    public void SetupClient(string ip)
    {
        string conIP;

        if (ip == "")
        {
            if (ipAdress.text != "")
            {
                conIP = ipAdress.text;
            }
            else
            {
                conIP = "127.0.0.1";
            }
        }
        else
        {
            conIP = ip;
        }
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect(conIP, 7777);
        isAtStartup = false;
    }

    public void SetupLocalClient()
    {
        myClient = new NetworkClient();
        SceneManager.LoadScene("Scenes/Main/Main");
        myClient.Connect("127.0.0.1", 7777);
        myClient = ClientScene.ConnectLocalServer();
        
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartup = false;
    }

    public void SetupLan()
    {
        StartServer();
        SetupLocalClient();
    }

    public void OnConnected(NetworkMessage netMsg)
    {
        
        Debug.Log("Connected");
        ClientScene.Ready(netMsg.conn);

        netCon = netMsg.conn;
        SpawnNewPlayer();
    }
    

    
    void SpawnNewPlayer()
    {
        StartCoroutine(spawnPlayer(playerPrefab));
    }

    IEnumerator spawnPlayer(GameObject player)
    {
        
        while (SceneManager.GetActiveScene().name != "Main")
        {
            Debug.Log("Cant Spawn player, Wrong Scene");
            yield return new WaitForEndOfFrame();
        }
        System.Collections.ObjectModel.ReadOnlyCollection<NetworkConnection> cons = NetworkServer.connections;
        List<int> allIds = new List<int>();
        for (int i = 0; i < cons.Count; i++)
        {
            allIds.Add(cons[i].connectionId);
        }
        
        if (!netCon.isReady)
        {
            ClientScene.Ready(netCon);
        }
        else
        {
            Debug.Log("Client is already ready");
        }
        
        GameObject newPlayer = Instantiate(player);
        NetworkServer.SpawnWithClientAuthority(newPlayer, netCon); 
       //newPlayer.GetComponent<NetworkIdentity>().
       //if (newPlayer)
       //{
       //
       //    //NetworkServer.AddPlayerForConnection(netCon, newPlayer, 1);
       //        GameObject.Find(newPlayer.name).GetComponent<NetworkIdentity>().AssignClientAuthority(netCon);
       //}
       //
       //
       //    yield return new WaitForEndOfFrame();
        
    }
    
}
