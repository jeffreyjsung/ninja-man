using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{
    private Dragon enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<Dragon>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(enemyParent.attackDamage);
        }
    }
}
