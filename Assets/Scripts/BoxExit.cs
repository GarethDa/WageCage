using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxExit : MonoBehaviour
{
    int destroyedBoxes = 0;
    [SerializeField] int boxScore = 2;
    [SerializeField] GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Destroy(other.gameObject);
            destroyedBoxes++;
            checkWin();
        }
    }

    void checkWin()
    {
        if (destroyedBoxes == boxScore)
        {
            canvas.SetActive(true);
        }
    }
}

