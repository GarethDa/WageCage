using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] float _turnSpeed = 50000f;
    [SerializeField] float _forwardSpeed = 4000f;
    [SerializeField] float _reverseSpeed = 4000f;
    [SerializeField] float _mouseSensitivity = 1f;
    [SerializeField] int _vertLookInvert = 1;

    float _xRotation = 0f;
    float _yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
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
        float x = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime * _vertLookInvert;

        _xRotation -= y;
        _xRotation = Mathf.Clamp(_xRotation, -50f, 40f);
        _yRotation += x;
        _yRotation = Mathf.Clamp(_yRotation, -55f, 55f);

        Debug.Log("Mouse X: " + x + ", Mouse Y: " + y + ", xRot: " + _xRotation + ", yRot: " + _yRotation);

        Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }
}
