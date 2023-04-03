using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject followObject;

    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(followObject.transform.position.x , followObject.transform.position.y + 2 , -12.5f);
    }
}
