using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth;
    int health;
    bool canBeDamaged = true;
    bool canBeKnockedback= true;

    [SerializeField] Rigidbody rb;
    [SerializeField] Transform respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            gameObject.transform.position = respawnPoint.transform.position;
            health = maxHealth;
        }
    }

    public void onDamage(int damage)
    {
        if (canBeDamaged)
        {
            canBeDamaged = false;
            health -= damage;
            StartCoroutine(damageCooldown());
        }
    }

    public void onHeal(int heal)
    {
        health += heal;
        health = Mathf.Min(maxHealth, health);
    }

    public void onKnockback(float knockbackForce)
    {
        if (canBeKnockedback)
        {
            canBeKnockedback = false;
            Vector3 knockbackVector = new Vector3(rb.transform.localScale.x * knockbackForce * 10 , 0 , 0);
            rb.AddForce(knockbackVector, ForceMode.Impulse);
            StartCoroutine(knockbackCooldown());
        }
    }

    IEnumerator damageCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canBeDamaged = true;
    }

    IEnumerator knockbackCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canBeKnockedback = true;
    }
}