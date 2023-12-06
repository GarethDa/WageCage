using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forkscript : MonoBehaviour
{
    [Header("Forklift Movement Variables")]
    [SerializeField] float _turnSpeed = 50000f;
    [SerializeField] float _forwardSpeed = 4000f;
    [SerializeField] float _reverseSpeed = 4000f;

    [Header("Camera Settings")]
    [SerializeField] float _mouseSensitivity = 1f;
    [SerializeField] int _vertLookInvert = 1;
    float _camXRotation = 0f;
    float _camYRotation = 0f;

    [Header("Fork You")]
    [SerializeField] Transform _forkTransform;
    [SerializeField] BoxCollider _ForkBoxCollider;
    Vector3 _ForkPosition = Vector3.zero;
    [SerializeField] float _forkRiseSpeed;
    [SerializeField] float _forkMaxx; //https://www.youtube.com/watch?v=lLxAbevUyIQ

    [Header("Cargo")]
    [SerializeField] Transform CargoVolume;
    [SerializeField] Vector3 CargoBounds = new Vector3(0.1f, 0.1f, 0.1f);

    List<Collider> _cargo = new List<Collider>();

    Vector3 CoG;

    //References
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        _ForkPosition = _forkTransform.localPosition;
        ForkOff();
        CoG = _rb.centerOfMass;
    }

    void FixedUpdate()
    {
        ForkliftMove();
    }

    // Update is called once per frame
    void Update()
    {
        CameraUpdate();
        ForkMoveUpdate();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        //_rb.centerOfMass = CoG;
        //Debug.Log(_rb.centerOfMass);
    }

    void ForkliftMove()
    {
        if (Input.GetKey(KeyCode.S))
        {
            _rb.AddForce(-transform.forward * _reverseSpeed);
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up, -_turnSpeed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            _rb.AddForce(transform.forward * _forwardSpeed);

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.up, -_turnSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up, _turnSpeed * Time.deltaTime);
            }
        }
    }

    void CameraUpdate()
    {
        float x = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime * _vertLookInvert;

        _camXRotation -= y;
        _camXRotation = Mathf.Clamp(_camXRotation, -50f, 40f);
        _camYRotation += x;
        _camYRotation = Mathf.Clamp(_camYRotation, -55f, 55f);

        Camera.main.transform.localRotation = Quaternion.Euler(_camXRotation, _camYRotation, 0);
    }

    void ForkMoveUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            _ForkPosition.z = _ForkPosition.z + _forkRiseSpeed * Time.deltaTime;
            _ForkPosition.z = Mathf.Clamp(_ForkPosition.z, 0, _forkMaxx);
            _forkTransform.localPosition = _ForkPosition;
            ForkOff();
        }
        else if (Input.GetKey(KeyCode.F))
        {
            _ForkPosition.z = _ForkPosition.z - _forkRiseSpeed * Time.deltaTime;
            _ForkPosition.z = Mathf.Clamp(_ForkPosition.z, 0, _forkMaxx);
            _forkTransform.localPosition = _ForkPosition;
            ForkOff();
        }
    }

    void ForkOff() //Checks to see if the fork is lowered and the collider should be off, allowing objects to be picked up.
    {
        if (_ForkPosition.z <= 0)
        {
            _ForkBoxCollider.enabled = false;
            DropCargo();
        }
        else
        {
            //If the collider is already enabled, we don't need to do anything.
            //If it is not yet enabled, we need to do a collision check and grab anything
            if (!_ForkBoxCollider.enabled)
            {
                GrabCargo();
            }
            _ForkBoxCollider.enabled = true;
        }
    }

    void GrabCargo()
    {
        Collider[] hitColliders = Physics.OverlapBox(CargoVolume.position, CargoBounds, transform.rotation);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Box"))
            {
                Debug.Log(collider.name);
                collider.enabled = false;
                collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                collider.transform.SetParent(_forkTransform, true);
                _cargo.Add(collider);
            }
        }
    }

    void DropCargo()
    {
        Debug.Log("Dumping");
        foreach (Collider collider in _cargo)
        {
            collider.enabled = true;
            collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            collider.transform.SetParent(null, true);
        }
        _cargo.Clear();
    }

    void OnDrawGizmos()
    {
        // Set the color of the Gizmos
        Gizmos.color = Color.red;

        // Create a matrix that represents the position and rotation of the CargoVolume
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(CargoVolume.position, CargoVolume.rotation, Vector3.one);

        // Apply this matrix to the Gizmos
        Gizmos.matrix = rotationMatrix;

        // Draw a cube centered at the origin (since the matrix already moves and rotates it)
        // Use DrawWireCube for a wireframe cube
        Gizmos.DrawWireCube(Vector3.zero, CargoBounds);
    }
}
