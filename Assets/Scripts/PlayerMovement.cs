using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 50f;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector3 moveDir;
    private Vector3 lastMoveDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isIdle", true);
        animator.SetFloat("MovX", 0);
        animator.SetFloat("MovY", 0);
        animator.SetFloat("LastDirection", 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float moveX = 0f;
        float moveY = 0f;
        animator.SetFloat("MovX", 0);
        animator.SetFloat("MovY", 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = +1f;
            animator.SetFloat("MovY", 1);
            animator.SetFloat("LastDirection", 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
            animator.SetFloat("MovY", -1);
            animator.SetFloat("LastDirection", 1);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
            animator.SetFloat("MovX", -1);
            animator.SetFloat("LastDirection", 2);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = +1f;
            animator.SetFloat("MovX", 1);
            animator.SetFloat("LastDirection", 3);
        }

        moveDir = new Vector3(moveX, moveY).normalized;

        if(moveX == 0f && moveY == 0f)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }

        //Debug.Log("moveX" + moveX);
        //Debug.Log("moveY" + moveY);

        //bool isIdle = moveX == 0 && moveY == 0;
        //if (isIdle)
        //{
        //    playerMain.PlayerSwapAimNormal.PlayIdleAnim();
        //}
        //else
        //{
        //    lastMoveDir = moveDir;
        //    playerMain.PlayerSwapAimNormal.PlayMoveAnim(moveDir);
        //}
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * speed;
    }
}
