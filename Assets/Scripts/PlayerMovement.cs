using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Movement_variables
    [SerializeField] private PlayerController controller;
    [SerializeField] private Animator animator;
    private float horizontalMove = 0f;
    [SerializeField] private float speed = 40f;
    private bool jump = false;
    private bool crouch = false;
    #endregion

    #region Attack_variables
    public float Damage;
    public float attackspeed = 1;
    float attackTimer;
    public float hitboxtiming;
    public float endanimationtiming;
    bool isAttacking;
    Rigidbody2D PlayerRB;
    #endregion

    #region Health_variables
    float currHealth;
    public float maxHealth;
    public Slider HPSlider;
    #endregion

    #region Unity_functions
    private void Awake()
    {
        currHealth = maxHealth;

        attackTimer = 0;

        PlayerRB = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump")) 
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0)
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.L) && attackTimer <=0)
        {
            Strike();
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
    #endregion

    #region Controller_functions
    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }
    #endregion

    #region Attack_functions
    private void Attack()
    {
        Debug.Log("attacking now");
        attackTimer = attackspeed;
        //handles animations and hitboxes
        StartCoroutine(AttackRoutine());
    }

    private void Strike()
    {
        attackTimer = attackspeed;
        //handles animations and hitboxes
        StartCoroutine(StrikeRoutine());
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;

        animator.SetTrigger("Attacktrig");

        yield return new WaitForSeconds(hitboxtiming);
        Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.transform.GetComponent<Enemy>() != null)
                {
                    hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
                }
                else if (hit.transform.GetComponent<Dragon>() != null)
                {
                    hit.transform.GetComponent<Dragon>().TakeDamage(Damage);
                }
            }
        }
        yield return new WaitForSeconds(hitboxtiming);
        isAttacking = false;

        yield return null;
    }

    IEnumerator StrikeRoutine()
    {
        isAttacking = true;
        PlayerRB.velocity = Vector2.zero;

        animator.SetTrigger("Striketrig");

        yield return new WaitForSeconds(hitboxtiming);
        Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerRB.position, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.transform.GetComponent<Enemy>() != null)
                {
                    hit.transform.GetComponent<Enemy>().TakeDamage(Damage);
                }
                else if (hit.transform.GetComponent<Dragon>() != null)
                {
                    hit.transform.GetComponent<Dragon>().TakeDamage(Damage);
                }
            }
        }
        yield return new WaitForSeconds(hitboxtiming);
        isAttacking = false;

        yield return null;
    }
    #endregion

    #region Health_functions
    public void TakeDamage(float value)
    {

        //Decrement health
        currHealth -= value;
        Debug.Log("Health is now " + currHealth.ToString());

        HPSlider.value = currHealth / maxHealth;

        if (currHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        //Destroys enemy object
        Destroy(this.gameObject, 1f);
    }
    #endregion
}
