using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
	public float maxHealth = 100f; 
	public float currHealth;
	public HealthBar healthBar;
	public bool isGrounded;
	public LayerMask ground;
    public bool jumpNum;
    public float jumpVel = 100f;
    public float damage = 5; 

    //GameObject X, Zero;
    //int CharacterSelect;


    [SerializeField]
    private float runSpeed = 2f;

    [SerializeField]
    private float jumpHeight = 4f;

    [SerializeField]
    Transform groundChecker;

    [SerializeField]
    GameObject hitBox;

    bool isAttacking = false;



	Animator animator;
	Rigidbody2D rb2d;
	SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
		currHealth = maxHealth; 
		//healthBar.SetMaxHealth((int)maxHealth);

        //CharacterSelect = 1;

       // X = GameObject.Find("X");
       // Zero = GameObject.Find("Zero");

        //X.SetActive(true);
        //Zero.SetActive(false);

    	animator = GetComponent<Animator>();
    	rb2d = GetComponent<Rigidbody2D>();
    	spriteRenderer = GetComponent<SpriteRenderer>();
        hitBox.SetActive(false);

        // Multi Mask so that player can jump on enemies too
        ground = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Ground");
        
    }

    // Update is called once per frame
    void Update()
    {

        // Update healthbar 
        //healthBar.SetHealth((int)currHealth);

        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), 
            new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), ground);
       /* if(Physics2D.Linecast(transform.position, groundChecker.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        */
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
            //animator.Play("Walking");
            //spriteRenderer.flipX = false;
            transform.localScale = new Vector3(2,2,1);
        }
        else if(Input.GetKey("a") || Input.GetKey("left"))
        {
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);
            if(isGrounded && !isAttacking)
            {
                animator.Play("Walking");
            }
            //animator.Play("Walking");
            //spriteRenderer.flipX = true;
            transform.localScale = new Vector3(-2,2,1);
        }
        else if(Input.GetKey("p") && !isAttacking)
        {
            isAttacking = true;

            animator.Play("attack");

            //Invoke("AttackReset", .5f);
            StartCoroutine(Attacker());
        }
        else
        {
            if(isGrounded && !isAttacking)
            {
                animator.Play("Idle");
            }
            //animator.Play("Idle");
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if(Input.GetKey("space"))
        {
            //float jumpVel = 100f;
            if(isGrounded)
            {
                animator.Play("Jumping");
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                //rb2d.velocity = Vector2.up * jumpVel;
            }
            else
            {
                if(jumpNum)
                {
                    animator.Play("Jumping");
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                    //rb2d.velocity = Vector2.up * jumpVel;
                    jumpNum = false;
                }
            }
            //rb2d.velocity = new Vector2(rb2d.velocity.x, 3);
            //animator.Play("Jumping");
            //rb2d.velocity = new Vector2(rb2d.velocity.x, 3);

        }
        
    }

    void AttackReset()
    {
        isAttacking = false;
    }

    IEnumerator Attacker()
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(.5f);
        hitBox.SetActive(false);
        isAttacking = false;

    }


	public void TakeDamage(float damage){
		currHealth = Mathf.Max(currHealth - damage, 0f); 
	}

}
