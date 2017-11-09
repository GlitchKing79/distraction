using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {

    public Camera playerCamera;

    Rigidbody playerRigidbody;

    public float playerSpeed = 10;
    public float cameraAngle = 0;
    public float cameraSensitivity = 1;
    public float playerAngle = 0;
    public float playerReach = 2;
    float playerSpeedMultiplyer = 1;

    Vector3 moveDirection = Vector3.zero;

    public Transform pickedObject = null;

    public bool inMenu = false;

    public Canvas playerCanvas;

    public ItemType plyerType = ItemType.player;

    NetworkIdentity playerIdentity;

    bool pickedUpItem = false;

    Color redacleFull = new Color(0, 0, 0, 1);
    Color reacleSmall = new Color(0, 0, 0, .25f);
    void Start()
    {
        playerIdentity = GetComponent<NetworkIdentity>();
        if (playerIdentity.isLocalPlayer)
        {
            playerRigidbody = GetComponent<Rigidbody>();
            playerCanvas.transform.parent = null;
            playerCanvas.transform.position = Vector3.zero;
            playerCamera.enabled = true;
            playerCamera.GetComponent<AudioListener>().enabled = true;
        } else
        {
            Destroy(playerCanvas.gameObject);
        }
    }

    void Update()
    {
        transform.Find("New Text").GetComponent<TextMesh>().text = transform.name;
        if (!playerIdentity.isLocalPlayer)
        {
            transform.name = transform.name.Replace("(Clone)", "(NET) " + playerIdentity.netId);
            return;
        } else
        {
            transform.name = transform.name.Replace("(Clone)", "(LOCAL) " + playerIdentity.netId);
        }
        if (transform.position.y < -50)
        {
            transform.position = Vector3.zero;
        }
        playerCanvas.transform.Find("Player Name").GetComponent<Text>().text = transform.name;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeedMultiplyer = 2;
        } else
        {
            playerSpeedMultiplyer = 1;
        }

        if (inMenu)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inMenu = !inMenu;
            Cmd_SendTextMessage("Test");
        }

        cameraAngle += (-Input.GetAxis("Mouse Y") * cameraSensitivity) * Time.deltaTime;
        playerAngle += (Input.GetAxis("Mouse X") * cameraSensitivity) * Time.deltaTime;
        cameraAngle = Mathf.Clamp(cameraAngle, -60, 60);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraAngle, 0, 0);

        transform.localRotation = Quaternion.Euler(0, playerAngle, 0);


        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, playerReach))
        {
            if (hit.transform.tag != "Player")
            {
                if (hit.transform.tag == "PickUp")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                       
                        if (!pickedUpItem)
                        {

                            if (hit.transform.parent == null)
                            {
                                if (hit.transform.GetComponent<Itemhandler>())
                                {
                                    Itemhandler item = hit.transform.GetComponent<Itemhandler>();

                                    if (item.isPickup && item.complete && item.itmtype != ItemType.heavy)
                                    {
                                        PickUpObject(transform.Find("Hand"), hit.transform, true);
                                        hit.transform.localPosition = Vector3.zero;
                                        pickedUpItem = true;
                                    }
                                }
                                else
                                {
                                    PickUpObject(playerCamera.transform, hit.transform, true);
                                }
                                pickedObject = hit.transform;
                            }

                        }
                        else
                        {
                            PickUpObject(transform, hit.transform, false);
                            pickedObject = null;
                        }
                    }

                    playerCanvas.transform.Find("Redicle").GetComponent<Image>().color = redacleFull;
                } else
                {
                    playerCanvas.transform.Find("Redicle").GetComponent<Image>().color = reacleSmall;
                }
            }
        }
        else
        {
            playerCanvas.transform.Find("Redicle").GetComponent<Image>().color = reacleSmall;
            if (pickedUpItem)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    
                    PickUpObject(playerCamera.transform, pickedObject, false);

                    pickedObject.position = playerCamera.transform.position + playerCamera.transform.forward;
                    pickedObject = null;
                    pickedUpItem = false;
                }
            }
        }
        
        if (pickedObject != null)
        {
            if (Input.GetMouseButton(1))
            {
                pickedObject.forward = Vector3.Lerp(transform.forward, transform.forward,Time.deltaTime);
                pickedObject.transform.position =Vector3.Lerp(pickedObject.transform.position, transform.position + transform.forward * 2,Time.deltaTime);
            }

            if (!transform.Find("Hand").Find(pickedObject.name))
            {
                pickedObject = null;
                pickedUpItem = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (!playerIdentity.isLocalPlayer)
        {
            return;
        }
        if (Input.GetButton("Vertical"))
        {
            playerRigidbody.AddForce((transform.forward * Input.GetAxis("Vertical")) * playerSpeed * playerSpeedMultiplyer);
        }
        if (Input.GetButton("Horizontal"))
        {
            playerRigidbody.AddForce((transform.right * Input.GetAxis("Horizontal")) * playerSpeed * playerSpeedMultiplyer);
        }
        RaycastHit downHit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out downHit, GetComponent<MeshFilter>().mesh.bounds.size.y))
        {
            if (downHit.transform != null)
            {


                if (downHit.transform.tag != "Player")
                {
                    if (Input.GetButton("Jump"))
                    {
                        playerRigidbody.AddForce(transform.up * playerSpeed * 1.5f);
                    }
                }
            }
        }

        Vector3 clampVector = playerRigidbody.velocity;
        clampVector.x = Mathf.Clamp(clampVector.x, -5 * playerSpeedMultiplyer, 5 * playerSpeedMultiplyer);
        clampVector.z = Mathf.Clamp(clampVector.z, -5 * playerSpeedMultiplyer, 5 * playerSpeedMultiplyer);

        playerRigidbody.velocity = clampVector;
    }

    
    public Transform cm_targetObject = null;
    public Transform cm_sender = null;
    public virtual void PickUpObject(Transform sender,Transform targetObject,bool hold)
    {
        Cmd_SetMovingObjects(transform.name,sender.name,targetObject.name);
        Cmd_PickUpObject(hold);
    }
    [Command]
    public virtual void Cmd_SetMovingObjects(string parent, string child, string targetObject)
    {
        
        try
        {
            cm_sender = GameObject.Find(parent).transform.Find(child);
        }
        catch
        {
            parent = parent.Replace("(LOCAL)", "(NET)");
            cm_sender = GameObject.Find(parent).transform.Find(child);
        }
        cm_targetObject = GameObject.Find(targetObject).transform;

        Debug.Log(parent + "/" + child + ":" + targetObject);
    }

    [Command]
    public virtual void Cmd_PickUpObject(bool hold)
    {
        if (hold)
        {
            cm_targetObject.GetComponent<Rigidbody>().isKinematic = true;
            cm_targetObject.GetComponent<Rigidbody>().useGravity = false;
            cm_targetObject.parent = cm_sender;
            cm_targetObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            cm_targetObject.GetComponent<Rigidbody>().isKinematic = false;
            cm_targetObject.GetComponent<Rigidbody>().useGravity = true;
            cm_targetObject.parent = null;
            cm_targetObject.transform.position = cm_sender.transform.position + cm_sender.transform.forward;
        }
    }

    [Command]
    public virtual void Cmd_SendTextMessage(string message)
    {
        Debug.Log(message);

    } 
}
