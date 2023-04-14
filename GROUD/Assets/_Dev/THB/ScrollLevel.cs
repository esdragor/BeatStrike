using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollLevel : MonoBehaviour
{
    public float baseMoveDistance = 1f;
    public float perfectMoveDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, baseMoveDistance);
            print(transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, -baseMoveDistance);
            print(transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            transform.position += new Vector3(0, 0, perfectMoveDistance);
            print(transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            transform.position += new Vector3(0, 0, -perfectMoveDistance);
            print(transform.position.z);
        }
    }
}
