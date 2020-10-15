using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
     
    /*[SerializeField]*/
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll2D;
    public Collider2D disColl;
    public Transform cellingCheck,groundCheck;
    public float speed;
    public float jumpForce;
    public LayerMask ground;    //地面层级
    private bool isGround;
    private int extraJump;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        Movement();
        SwitchAnim();
        isGround = Physics2D.OverlapCircle(groundCheck.position,0.2f,ground);
    }
    void Update() {
        //Jump();
        Down();
        finalJump();
    }


    void Movement() //控制人物移动函数
    {

        //角色移动
        float horizontalMove = Input.GetAxis("Horizontal");
        float facedirection = Input.GetAxisRaw("Horizontal");      //获得1，-1，0    
        //if(HorizontalMove != 0)                                  
        rb.velocity = new Vector2( horizontalMove*speed*Time.fixedDeltaTime , rb.velocity.y);
        anim.SetFloat("running",Mathf.Abs(facedirection));

        if(facedirection != 0)          //改变scale为了改变图片翻转
        {
            transform.localScale = new Vector3(facedirection,1,1);   
        }

    }

    void SwitchAnim()
    {
        //转换跳跃上升下降的动画
        if(anim.GetBool("jumping"))
        {
            if(rb.velocity.y < 0)       //如果跳跃给的jumpforce为0时候
            {
                anim.SetBool("jumping",false);
                anim.SetBool("falling",true);
            }
        }
        else if(coll2D.IsTouchingLayers(ground))        //判断层级碰撞
        {
            anim.SetBool("falling",false);
        }
    }

    void Down()
    {
       // Debug.Log(Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground));
        
        {
            if(Input.GetButtonDown("Down"))
            {
                anim.SetBool("down",true);
                disColl.enabled = false;
            }
            else if(Input.GetButtonUp("Down"))
            {
                if(!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))      //检验头顶是否有物体
                anim.SetBool("down",false);
                disColl.enabled = true;
            }
        }
    }

    // void Jump()
    // {
    //     //角色跳跃
    //     if(Input.GetButtonDown("Jump") && coll2D.IsTouchingLayers(ground)) 
    //     {
    //         rb.velocity= new Vector2(rb.velocity.x , jumpForce * Time.fixedDeltaTime);
    //         anim.SetBool("jumping",true);
    //     }
    // }

    void finalJump()
    {
        if(isGround)
        {
            extraJump =1;
        }
        if(Input.GetButtonDown("Jump") && extraJump >0)
        {
            rb.velocity = Vector2.up * jumpForce;
            anim.SetBool("jumping",true);
            extraJump--;
        }
        if(Input.GetButtonDown("Jump") && extraJump == 0 && isGround)
        {
            rb.velocity = Vector2.up * jumpForce;
            anim.SetBool("jumping",true);
        }
    }
}
