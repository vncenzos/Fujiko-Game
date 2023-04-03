using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] new private Collider collider;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] public int damage;
    [SerializeField] public float knockback;
    private HealthSystem characterHealth;
    private GameObject otherObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        otherObject = collision.gameObject;
        characterHealth = otherObject.transform.GetComponent<HealthSystem>();
        characterHealth.onKnockback(knockback);
        characterHealth.onDamage(damage);
    }

    private void OnCollisionExit(Collision collision)
    {
        otherObject = null;
        characterHealth = null;
    }
}

