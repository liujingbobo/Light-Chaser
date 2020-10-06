using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movements : MonoBehaviour
{
    public float jumpForce = 300f;
    public float speed = 120f;
    Animator animator;
    Rigidbody2D rigid;
    int jumping = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    void move()
    {
        float horizonMove = Input.GetAxis("Horizontal");
        float moveSpeed = horizonMove * speed;
        if (horizonMove < 0)
        {
            animator.SetBool("left", true);
            animator.SetBool("right", false);
        }
        else if (horizonMove > 0)
        {
            animator.SetBool("left", false);
            animator.SetBool("right", true);
        }
        else
        {
            animator.SetBool("left", false);
            animator.SetBool("right", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumping == 0)
            {
                jumping = 1;
                rigid.AddForce(new Vector2(0, jumpForce));   //给刚体一个向上的力
            }
        }
         rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumping == 0) jumping = 1;
        else jumping = 0;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        jumping = 0;
    }
}
