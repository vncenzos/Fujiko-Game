using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private Transform laserStart;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask layerMask;
    // Start is called before the first frame update

    enum LaserDirections{ 
        up,
        down,
        left,
        right
    }

    [SerializeField] private LaserDirections laserDirection;
    private Vector3 directionV;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(laserDirection){
            case LaserDirections.up:
            directionV = Vector3.up;
            break;
            case LaserDirections.down:
            directionV = Vector3.down;
            break;            
            case LaserDirections.left:
            directionV = Vector3.left;
            break;            
            case LaserDirections.right:
            directionV = Vector3.right;
            break;
        }
        if (Physics.Raycast(laserStart.position, directionV, out RaycastHit laserEnd, 10, layerMask)){
        lineRenderer.SetPosition(0, laserStart.position);
        lineRenderer.SetPosition(1, laserEnd.point);
        if(laserEnd.transform.tag == "Player"){
            print("Player hit!");
        }
        }
    }
}
