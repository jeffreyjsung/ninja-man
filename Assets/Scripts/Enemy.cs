using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float speed;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float damage;
    public float passiveTimer;
    private float originalTime;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    public Slider HPSlider;
    #endregion

    #region Unity_functions
    //runs once on creation
    private void Awake()
    {
        EnemyRB = GetComponent<Rigidbody2D>();

        currHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();

        originalTime = passiveTimer;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    //runs once every frame
    private void Update()
    {
        Move();
        Passive();
    }
    #endregion

    #region Movement_functions

    //move directly at player
    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        if (transform.position.x < player.position.x)
        {
            spriteRenderer.flipX = false;
        } else
        {
            spriteRenderer.flipX = true;
        }
    }
    #endregion

    #region Attack_functions

    private void Passive()
    {
        if (passiveTimer <= 0)
        {
            Attack();
            passiveTimer = originalTime;
        }
        else
        {
            passiveTimer -= Time.deltaTime;
            animator.SetBool("jump", false);
        }
    }

    private void Attack()
    {
        EnemyRB.AddForce(new Vector2(0f, 800));
        animator.SetBool("jump", true);
    }
    #endregion

    #region Health_functions

    //enemy takes damage based on value param
    public void TakeDamage(float value)
    {
        //decrement health
        currHealth -= value;

        Debug.Log("Health is now " + currHealth.ToString());

        HPSlider.value = currHealth / maxHealth;

        //check for death
        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Destroys enemy object
        Destroy(this.gameObject);
    }
    #endregion
}
