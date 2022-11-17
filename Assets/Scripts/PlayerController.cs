using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] float _turnSpeed = 50000f;
    [SerializeField] float _forwardSpeed = 4000f;
    [SerializeField] float _reverseSpeed = 4000f;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
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
}
