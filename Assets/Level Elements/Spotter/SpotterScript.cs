using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotterScript : MonoBehaviour
{
    private Transform playerTransform;
    void Start()
    {
        
    }

    
    void Update()
    {
                    gameObject.transform.LookAt(playerTransform);

    }

    private void OnTriggerStay(Collider other) {
        if(other.transform.tag == "Player")
        {
            playerTransform = other.transform;
            SnapToPlayer();
        }
    }

    void SnapToPlayer()
    {
        if(playerTransform != null)
        {
            gameObject.transform.LookAt(playerTransform);
        }
    }
}
