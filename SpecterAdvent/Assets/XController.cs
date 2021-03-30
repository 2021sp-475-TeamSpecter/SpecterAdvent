using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XController : MonoBehaviour
{
	public float maxHealth = 100f; 
	public float currHealth;
	public HealthBar healthBar;
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
		//ealthBar.SetMaxHealth((int)maxHealth);

    	animator = GetComponent<Animator>();
    	rb2d = GetComponent<Rigidbody2D>();
    	spriteRenderer = GetComponent<SpriteRenderer>();
    	blastLock = Resources.Load("Blast");
        
    }

    // Update is called once per frame
    void Update()
    {

        // Update healthbar 
        //healthBar.SetHealth((int)currHealth);

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
            //animator.Play("Idle");
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if(Input.GetKey("space"))
        {
            //float jumpVel = 100f;
            if(isGrounded)
            {
                animator.Play("XJump");
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpHeight);
                //rb2d.velocity = Vector2.up * jumpVel;
            }
            else
            {
                if(jumpNum)
                {
                    animator.Play("XJump");
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

    /*IEnumerator Attacker()
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(.5f);
        hitBox.SetActive(false);
        isAttacking = false;

    }*/


	public void TakeDamage(float damage){
		currHealth = Mathf.Max(currHealth - damage, 0f); 
	}

}
