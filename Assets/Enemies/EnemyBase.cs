using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    private float currentHealth;
    [SerializeField] private float maxHealth;
    private bool canBeDamaged = true;
    private bool isAlive = true;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0){
            if(isAlive)
            {
                isAlive = false;
                StartCoroutine(die());
            }
        }
    }

    public void enemyDamage(float damage){
        if(canBeDamaged)
        {
        currentHealth -= damage;
        }
    }

        IEnumerator die()
    {
        print("OUCH!");
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
