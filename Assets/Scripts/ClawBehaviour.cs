using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawBehaviour : MonoBehaviour
{
    public GameObject targetObject = null;//the object that we are hovering over

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Grabbable") //if we are over a grabbable object
        {
            Debug.Log("Target Found");
            targetObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Grabbable")//This makes sure we don't get false-positives for other objects leaving the trigger, like walls
        {
            Debug.Log("Target Lost");
            targetObject = null;
        }

    }
}
