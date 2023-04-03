using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider[] collisions = Physics.OverlapSphere(attackCheck.position, 2f, enemyLayer);
            foreach (var collision in collisions)
            {
                collision.GetComponent<EnemyBase>().enemyDamage(1);
            }
        }
    }
}
