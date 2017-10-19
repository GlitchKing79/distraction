using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCanvas.transform.parent = null;
        playerCanvas.transform.position = Vector3.zero;
    }

    void Update()
    {

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


                            hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                            hit.transform.parent = playerCamera.transform;
                            pickedObject = hit.transform;


                        }
                        else
                        {
                            pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                            pickedObject.parent = null;
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
    }

    void FixedUpdate()
    {
        
        if (Input.GetButton("Vertical"))
        {
            playerRigidbody.AddForce((transform.forward * Input.GetAxis("Vertical")) * playerSpeed);
        }
        if (Input.GetButton("Horizontal"))
        {
            playerRigidbody.AddForce((transform.right * Input.GetAxis("Horizontal")) * playerSpeed);
        }

        Vector3 clampVector = playerRigidbody.velocity;
        clampVector.x = Mathf.Clamp(clampVector.x, -5, 5);
        clampVector.z = Mathf.Clamp(clampVector.z, -5, 5);

        playerRigidbody.velocity = clampVector;
    }
}
