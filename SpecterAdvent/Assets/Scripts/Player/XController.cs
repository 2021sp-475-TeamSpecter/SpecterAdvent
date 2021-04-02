using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XController : MonoBehaviour
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
    Object blastLock;


    [SerializeField]
    private float runSpeed = 2f;

    [SerializeField]
    private float jumpHeight = 4f;

    [SerializeField]
    Transform groundChecker;

    [SerializeField]
    Transform XBarrel;


    //[SerializeField]
    //GameObject hitBox;

    bool isAttacking = false;

    bool isTurned;



	Animator animator;
	Rigidbody2D rb2d;
	SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
		currHealth = maxHealth; 
		healthBar.SetMaxHealth((int)maxHealth);

    	animator = GetComponent<Animator>();
    	rb2d = GetComponent<Rigidbody2D>();
    	spriteRenderer = GetComponent<SpriteRenderer>();
    	blastLock = Resources.Load("Blast");
        
    }

    public void Respawn()
    {
        currHealth = maxHealth; 
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {

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
                animator.Play("Run");
            }

            transform.localScale = new Vector3(2,2,1);
            isTurned = false;
        }
        else if(Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);
            if(isGrounded && !isAttacking)
            {
                animator.Play("Run");
            }
            transform.localScale = new Vector3(-2,2,1);
            isTurned = true;
        }
        else if(Input.GetKey("p") && !isAttacking)
        {
            isAttacking = true;

            animator.Play("XFire");

            GameObject blast = (GameObject)Instantiate(blastLock);
            blast.GetComponent<BlastControl>().Shooter(isTurned);
            blast.GetComponent<BlastControl>().SetBulletDamage(damage);
            blast.transform.position = XBarrel.transform.position;
            Invoke("AttackReset", .2f);
        }
        else
        {
            if(isGrounded && !isAttacking)
            {
                animator.Play("XIdle");
            }
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if(Input.GetKey("space"))
        {
            if(isGrounded)
            {
                animator.Play("XJump");
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
            }
            else
            {
                if(jumpNum)
                {
                    animator.Play("XJump");
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                    jumpNum = false;
                }
            }
        }
    }

    void AttackReset()
    {
        isAttacking = false;
    }

	// subtracts a given amount of health from enemy scaling the damage
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
