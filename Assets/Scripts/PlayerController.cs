using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject clawObject;
    GameObject heldObject;
    bool clawMode = false;
    bool holdingObject = false;
    [SerializeField] float clawX_Min = -1f, clawX_Max = 1f, clawZ_Min = 1.5f, clawZ_Max = 2.5f;

    Rigidbody _rb;

    [SerializeField] float _turnSpeed = 50000f;
    [SerializeField] float _forwardSpeed = 4000f;
    [SerializeField] float _reverseSpeed = 4000f;
    [SerializeField] float _mouseSensitivity = 1f;
    [SerializeField] int _vertLookInvert = 1;

    float _xRotation = 0f;
    float _yRotation = 0f;

    float mouseX, mouseY;//moved your variables so i could reuse your mouse values in the arm script

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!clawMode)//only move when not in claw mode
        {
            if (Input.GetKey(KeyCode.S))
            {
                _rb.AddForce(-transform.forward * _reverseSpeed);
                if (Input.GetKey(KeyCode.A))
                {
                    _rb.AddTorque(0, _turnSpeed, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    _rb.AddTorque(0, -_turnSpeed, 0);
                }
            }
            else if (Input.GetKey(KeyCode.W))
            {
                _rb.AddForce(transform.forward * _forwardSpeed);

                if (Input.GetKey(KeyCode.A))
                {
                    _rb.AddTorque(0, -_turnSpeed, 0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    _rb.AddTorque(0, _turnSpeed, 0);
                }
            }
        }
        else //if we're in claw mode, change the mouse to move the claw
        {
            Vector3 clawPos = clawObject.transform.localPosition;//the current position of the claw as of this frame

            //the x axis
            clawPos.x += mouseX * 0.002f;
            clawPos.x = Mathf.Clamp(clawPos.x, clawX_Min, clawX_Max);

            //the z axis
            clawPos.z += mouseY * 0.002f;
            clawPos.z = Mathf.Clamp(clawPos.z, clawZ_Min, clawZ_Max);

            //set the new position
            clawObject.transform.localPosition = clawPos;
        }
        /*else
        {
            if (Input.GetKey(KeyCode.A))
            {
                _rb.AddTorque(0, -_turnSpeed, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                _rb.AddTorque(0, _turnSpeed, 0);
            }
        }*/

    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime * _vertLookInvert;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -50f, 40f);
        _yRotation += mouseX;
        _yRotation = Mathf.Clamp(_yRotation, -55f, 55f);

        //Debug.Log("Mouse X: " + mouseX + ", Mouse Y: " + mouseY + ", xRot: " + _xRotation + ", yRot: " + _yRotation);

        if (Input.GetKeyDown(KeyCode.Mouse1))//mouse 1 is right click
        {
            clawMode = !clawMode; //toggle claw mode
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && clawMode)//only can grab and drop in claw mode
        {
            //if we are not holding something already and if we have a valid target, grab it
            if (!holdingObject && clawObject.GetComponentInChildren<ClawBehaviour>().targetObject != null)
            {
                heldObject = clawObject.GetComponentInChildren<ClawBehaviour>().targetObject; //set our held object
                heldObject.GetComponent<Rigidbody>().isKinematic = true;//make it kinematic so it doesn't have gravity
                heldObject.transform.parent = clawObject.transform;//make our claw the parent so it follows
                heldObject.transform.localPosition = clawObject.transform.localPosition;//update the local position to the claw's local
                holdingObject = true;
            }
            //else, drop what we are holding
            else
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;//no longer kinematic, so it falls down
                heldObject.transform.parent = null; //remove it as a child
                holdingObject = false;
            }
        }

        //only move the camera while claw mode is disabled
        if (!clawMode)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        }
    }
}
