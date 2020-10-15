using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTilesAnim : MonoBehaviour
{
    Animator animator;
    int change = 0;
    int once = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player"&&once==0)
        {
            once = 1;
            change++;
            animator.SetInteger("change", change);
        }
    }
}
