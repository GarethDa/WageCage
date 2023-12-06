using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Win : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    bool win;

    float timer = 2f;
    private void Start()
    {
        _canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            win = true;
        }
    }

    private void Update()
    {
        if (win)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0)
        {
            _canvas.enabled = true;
        }
    }
}
