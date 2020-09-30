using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    #region Movement_variables
    public float moveSpeed;
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    #endregion

    #region Physics_components
    Rigidbody2D DragonRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    [HideInInspector] public Transform target;
    public Transform leftLimit;
    public Transform rightLimit;
    public GameObject hotZone;
    public GameObject triggerArea;
    #endregion

    #region Attack_variables
    public float attackDamage;
    public float attackDistance;
    public float timer;
    private float distance;
    private bool attackMode;
    [HideInInspector] public bool inRange;
    private bool cooling;
    private float initTimer;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    public Slider HPSlider;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        SelectTarget();
        initTimer = timer;

        DragonRB = GetComponent<Rigidbody2D>();

        currHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (!attackMode)
        {
            Move();
        }

        if (!InsideofLimits() && !inRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dragon_strike"))
        {
            SelectTarget();
        }

        if (inRange)
        {
            EnemyLogic();
        }
    }

    #endregion

    #region Movement_functions
    private void Move()
    {
        animator.SetBool("canWalk", true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dragon_strike"))
        {
            Vector2 targetPosition = new Vector2(target.position.x,
                                                 target.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {
            target = leftLimit;
        } 
        else
        {
            target = rightLimit;
        }

        Flip();
    }

    public void Flip()
    {
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x > target.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }

        transform.eulerAngles = rotation;
    }
    #endregion

    #region Attack_functions
 

    void Attack()
    {
        timer = initTimer;
        attackMode = true;

        animator.SetBool("canWalk", false);
        animator.SetBool("Attack", true);
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        animator.SetBool("Attack", false);
    }
    void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }

        if (cooling)
        {
            Cooldown();
            Debug.Log("?????");
            animator.SetBool("Attack", false);
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
        Debug.Log(cooling);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = initTimer;
        }
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value)
    {
        currHealth -= value;

        HPSlider.value = currHealth / maxHealth;

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        Destroy(this.gameObject, 1f);
    }

    #endregion
}
