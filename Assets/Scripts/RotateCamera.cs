using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        } else if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);
        }
    }

    
}
