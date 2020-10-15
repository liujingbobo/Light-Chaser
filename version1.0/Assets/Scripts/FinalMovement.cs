using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    
    public float speed,jumpForce;
    private float horizontalMove;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround,isJump,isDashing;
    bool jumpPressed;
    int jumpCount;
    [Header("Dash参数")]
    public float dashTime;  //冲锋时长
    private float dashTimeLeft; //冲锋剩余时间
    private float lastDash   = -10;
    public float dashCoolDown;
    public float dashSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Jump") && jumpCount >0)
        {
            jumpPressed =true;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(Time.time>=(lastDash + dashCoolDown))
            {
                //可执行dash
                ReadyToDash();
            }
        }
    }
    private void FixedUpdate() 
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position,0.2f,ground);

       
        Dash();
        if(isDashing)
            return;

        GroundMovement();
        Jump();
        SwitchAnim();
    }

    void GroundMovement()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        //Debug.Log(horizontalMove);
        rb.velocity = new Vector2(horizontalMove * speed , rb.velocity.y);

        if(horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove,1,1);
        }

    }    

    void Jump()
    {
        if(isGround)
        {
            jumpCount = 2;
            isJump =false;
        }
        if(jumpPressed && isGround)
        {
            isJump =true;
            rb.velocity =new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount>0 && isJump)
        {
            rb.velocity =new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed =false;
        }
    }
    void SwitchAnim()
    {
        anim.SetFloat("running",Mathf.Abs(rb.velocity.x));

        if(isGround)
        {
            anim.SetBool("falling",false);
        }
        else if(!isGround && rb.velocity.y >0)
        {
            anim.SetBool("jumping",true);
        }
        else if(rb.velocity.y<0)
        {
            anim.SetBool("jumping",false);
            anim.SetBool("falling",true);
        }
    }

    void ReadyToDash()
    {
        isDashing =true;
        dashTimeLeft = dashTime;
        lastDash =Time.time;
    }
    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                
                if (rb.velocity.y > 0 && !isGround)
                {
                    
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);//在空中Dash向上
                }
                rb.velocity = new Vector2(dashSpeed * horizontalMove, rb.velocity.y);//地面Dash
                //Debug.Log(horizontalMove); 
                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFormPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                if (!isGround)
                {
                    //目的为了在空中结束 Dash 的时候可以接一个小跳跃。根据自己需要随意删减调整
                    rb.velocity = new Vector2(dashSpeed * horizontalMove, jumpForce);
                }
            }
        }

    }
}

