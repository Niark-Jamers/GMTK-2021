using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.0f;

    Animator        animator;
    SpriteRenderer  spriteRenderer;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
            return;
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.position += movement * speed * Time.deltaTime;

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        if (movement.magnitude > 0)
            spriteRenderer.flipX = movement.x > 0;
    }

    public void Die()
    {
        dead = true;
        animator.SetTrigger("Death");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (dead)
            return;

        if (col.gameObject.tag == "EnemyBullet")
        {
            FindObjectOfType<Link>().Die();
        }
    }

}
