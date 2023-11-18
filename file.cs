using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; //only this script can access
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    // [SerializeField] is for adjusting ingame

    private enum MovementState { idle, running, jumping, falling }
    // idle = 0, running = 1 ...
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {

        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        // pass rb.velocity.y instead of 0 so it wont reset movement in y


        //if (Input.GetKeyDown("space")) 
        // only execute when press not hold if u want hold just use GetKey

        if (Input.GetButtonDown("Jump") && IsGrounded()) 
        {
           rb.velocity = new Vector2(rb.velocity.x, jumpForce); // get the access
        }

        UpdateAnimationState();
    }

    // void is just an execution doesnt return any
    private void UpdateAnimationState()
    {

        MovementState state;

        if (dirX > 0f)
        {

            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }


        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded() // return true false for jumping
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    
}
