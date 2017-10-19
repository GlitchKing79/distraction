using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public Camera playerCamera;

    Rigidbody playerRigidbody;

    public float playerSpeed = 10;
    public float cameraAngle = 0;
    public float cameraSensitivity = 1;
    public float playerAngle = 0;
    public float playerReach = 2;

    Vector3 moveDirection = Vector3.zero;

    public Transform pickedObject = null;

    public bool inMenu = false;

    public Canvas playerCanvas;

    NetworkIdentity playerIdentity;
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
            Destroy(playerCanvas);
        }
    }

    void Update()
    {
        if (!playerIdentity.isLocalPlayer)
        {
            transform.name = transform.name.Replace("(Clone)", "(NET) " + playerIdentity.netId);
            return;
        } else
        {
            transform.name = transform.name.Replace("(Clone)", "(LOCAL) " + playerIdentity.netId);
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
                       
                        if (pickedObject == null)
                        {

                            if (hit.transform.parent == null)
                            {
                                PickUpObject(playerCamera.transform, hit.transform, true);
                                pickedObject = hit.transform;
                            }

                        }
                        else
                        {
                            PickUpObject(transform, hit.transform, false);
                            pickedObject = null;
                        }
                    }

                    playerCanvas.transform.Find("Redicle").gameObject.SetActive(true);
                } else
                {
                    playerCanvas.transform.Find("Redicle").gameObject.SetActive(false);
                }
            }
        }
        else
        {
            playerCanvas.transform.Find("Redicle").gameObject.SetActive(false);
        }

        if (pickedObject != null)
        {
            if (Input.GetMouseButton(1))
            {
                pickedObject.rotation = Quaternion.Slerp(pickedObject.rotation, Quaternion.identity, Time.deltaTime);
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
            playerRigidbody.AddForce((transform.forward * Input.GetAxis("Vertical")) * playerSpeed);
        }
        if (Input.GetButton("Horizontal"))
        {
            playerRigidbody.AddForce((transform.right * Input.GetAxis("Horizontal")) * playerSpeed);
        }
        RaycastHit downHit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out downHit, GetComponent<MeshFilter>().mesh.bounds.size.y));
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
        clampVector.x = Mathf.Clamp(clampVector.x, -5, 5);
        clampVector.z = Mathf.Clamp(clampVector.z, -5, 5);

        playerRigidbody.velocity = clampVector;
    }

    
    public virtual void PickUpObject(Transform sender,Transform targetObject,bool hold)
    {
        if (hold)
        {
            targetObject.GetComponent<Rigidbody>().isKinematic = true;
            targetObject.parent = sender;
        }else
        {
            targetObject.GetComponent<Rigidbody>().isKinematic = false;
            targetObject.parent = null;
        }
    }
}
