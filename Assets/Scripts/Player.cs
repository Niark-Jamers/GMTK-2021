using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;

    public int lifePoints = 3;

    public AudioClip deathClip;

    Animator        animator;
    SpriteRenderer  spriteRenderer;
    new Rigidbody2D     rigidbody2D;
    PowerBall       powerBall;
    public bool dead = false;

    public bool freeMovements = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        powerBall = FindObjectOfType<PowerBall>();
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (dead || freeMovements)
            return;

        var movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // // Glitched by physic:
        // float d1 = Vector2.Distance((Vector2)powerBall.transform.position, (Vector2)transform.position);
        // if (d1 > powerBall.max + 0.5f)
        // {
        //     if (d1 < Vector2.Distance((Vector2)powerBall.transform.position, (Vector2)transform.position + movement))
        //         movement = Vector2.zero;
        // }

        rigidbody2D.MovePosition(rigidbody2D.position + movement * speed * Time.fixedDeltaTime);

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        if (movement.magnitude > 0)
            spriteRenderer.flipX = movement.x > 0;
    }

    public void Die()
    {
        rigidbody2D.isKinematic = true;
        dead = true;
        animator.SetTrigger("Death");
        AudioManager.PlayOnShot(deathClip);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (dead)
            return;

        if (col.gameObject.tag == "EnemyBullet")
        {
            FindObjectOfType<Link>().TakeHit(col.contacts[0].point);
        }
    }

}
