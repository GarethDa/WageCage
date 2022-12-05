using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.Animations;
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

    [SerializeField] Animator joystickAnimator;
    [SerializeField] Animator armAnimator;
    [SerializeField] Transform armTransform;
    //[SerializeField] BlendTree blendtree;

    float _armExtension = 0.4f;
    Vector3 _armXOffset = Vector3.zero;

    bool _lookMode = true;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        //_armXOffset.x = 0f;
        //_armXOffset.y = 0.3f;
        //_armXOffset.z = 1.056f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.S))
        {
            joystickAnimator.SetFloat("Forward", -1);
            _rb.AddForce(-transform.forward * _reverseSpeed);
            if (Input.GetKey(KeyCode.A))
            {
                joystickAnimator.SetFloat("Right", -1);
                _rb.AddTorque(0, _turnSpeed, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                joystickAnimator.SetFloat("Right", 1);
                _rb.AddTorque(0, -_turnSpeed, 0);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            joystickAnimator.SetFloat("Forward", 1);
            _rb.AddForce(transform.forward * _forwardSpeed);

            if (Input.GetKey(KeyCode.A))
            {
                joystickAnimator.SetFloat("Right", -1);
                _rb.AddTorque(0, -_turnSpeed, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                joystickAnimator.SetFloat("Right", 1);
                _rb.AddTorque(0, _turnSpeed, 0);
            }
        }
        else
        {
            joystickAnimator.SetFloat("Forward", 0);
            joystickAnimator.SetFloat("Right", 0);
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _lookMode = !_lookMode;
            Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        float x = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime * _vertLookInvert;

        if (_lookMode)
        {
            _xRotation -= y;
            _xRotation = Mathf.Clamp(_xRotation, -50f, 40f);
            _yRotation += x;
            _yRotation = Mathf.Clamp(_yRotation, -55f, 55f);

            //Debug.Log("Mouse X: " + x + ", Mouse Y: " + y + ", xRot: " + _xRotation + ", yRot: " + _yRotation);

            Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        }
        else
        {
            _armXOffset.x += x * 0.01f;
            _armXOffset.x = Mathf.Clamp(_armXOffset.x, -0.83f, 0.83f);

            armTransform.position = _armXOffset;

            Debug.Log(_armXOffset);

            _armExtension += y * 0.01f;
            _armExtension = Mathf.Clamp(_armExtension, 0, 1);
            armAnimator.SetFloat("Blend", _armExtension);
        }
    }
}
