using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Health")]
	public float maxHealth = 100f; 
	public float currHealth;
	public HealthBar healthBar;

    [Header("Resistance")]
    public DamageType[] resistances; 
    public float[] percentBlocked; 

    [Header("Movement")]
	public bool isGrounded;
	public LayerMask ground;
    public bool jumpNum;
    public float jumpVel = 100f;
    public float damage = 5; 


    public float attackSpeed = 1f; 
    float lastAttack = -10; 

    [SerializeField]
    private float runSpeed = 2f;

    [SerializeField]
    private float jumpHeight = 4f;

    [SerializeField]
    Transform groundChecker;

    [SerializeField]
    public GameObject hitBox;

    public bool isAttacking = false;



	public Animator animator;
	public Rigidbody2D rb2d;
	public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
		currHealth = maxHealth; 
		healthBar.SetMaxHealth((int)maxHealth);

    	animator = GetComponent<Animator>();
    	rb2d = GetComponent<Rigidbody2D>();
    	spriteRenderer = GetComponent<SpriteRenderer>();
        hitBox.SetActive(false);

        // Multi Mask so that player can jump on enemies too
        ground = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ground");
        
    }
    
    public void Respawn()
    {
        currHealth = maxHealth; 
        rb2d = GetComponent<Rigidbody2D>();
        hitBox.SetActive(false);
        rb2d.velocity = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {

        isAttacking = Time.time < attackSpeed + lastAttack;

        // Update healthbar 
        healthBar.SetHealth((int)currHealth);
        
        if (IsDead()) return;

        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), 
            new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), ground);

        if(isGrounded){
            jumpNum = true;
        }
        
        if(Input.GetKey("d") || Input.GetKey("right"))
        {
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);
            if(isGrounded && !isAttacking)
            {
                animator.Play("Walking");
            }
            transform.localScale = new Vector3(2,2,1);
        }
        else if(Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);
            if(isGrounded && !isAttacking)
            {
                animator.Play("Walking");
            }
            transform.localScale = new Vector3(-2,2,1);
        }
        else if(Input.GetKey("p") && !isAttacking)
        {
            lastAttack = Time.time; 

            animator.Play("attack");
            SoundManager.PlaySound ("SwordSlash");

            //Invoke("AttackReset", .5f);
            StartCoroutine(Attacker());
        }
        else
        {
            if(isGrounded && !isAttacking)
            {
                animator.Play("Idle");
            }
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if(Input.GetKey("space"))
        {
            if(isGrounded)
            {
                animator.Play("Jumping");
                SoundManager.PlaySound ("JumpSound");
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
            }
            else
            {
                if(jumpNum)
                {
                    animator.Play("Jumping");
                    SoundManager.PlaySound ("JumpSound");
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                    jumpNum = false;
                }
            }

        }
        
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }

    IEnumerator Attacker()
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(.5f);
        hitBox.SetActive(false);

    }

	// subtracts a given amount of health from player scaling the damage
    // based on resistances to types of attacks 
    public void TakeDamage(float damage, DamageType damageType)
    {
        float adjustedDamage = damage;
        // adjust damage based on player's resistances  
        for (int i = 0; i < resistances.Length; ++i)
        { 
            if (resistances[i] == damageType)
                adjustedDamage *= 1 - percentBlocked[i];
        }
        // take off health
        // ensure health does not go below 0 
        currHealth = Mathf.Max(currHealth - adjustedDamage, 0f);
    }
    
    // adds health to player
    public void AddHealth(float amountOfHealth)
    {
        currHealth = Mathf.Min(currHealth + amountOfHealth, maxHealth);
    }
    
    public bool IsDead()
    {
        return currHealth <= 0; 
    }
}
