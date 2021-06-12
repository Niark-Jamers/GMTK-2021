using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 1;

    private Vector3 target;
    GameObject player;
    GameObject powerBall;

    public float moveSpeed = 5;

    [Header("Patrol")]
    public bool patrol = true;
    public GameObject[] controlsPoints;
    bool isAggro = false;

    [Header("PlayerAggro")]
    public bool aggro = true;
    public float aggroDistance = 10f;

    Animator    animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        powerBall = GameObject.FindGameObjectWithTag("PowerBall");
        StartCoroutine(Shoot());

        animator = GetComponent<Animator>();
    }

    int currentPoint = 0;
    void Update()
    {
        Vector2 movement = Vector3.zero;

        if (patrol && !isAggro)
        {
            var target = controlsPoints[currentPoint].transform.position;
            movement = Vector2.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            if (Vector2.Distance((Vector2)transform.position + movement, target) < 0.1f)
                currentPoint = (currentPoint + 1) % controlsPoints.Length;
        }

        if (aggro)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < aggroDistance)
                isAggro = true;
        }

        if (isAggro)
        {
            movement = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
        }

        transform.position += (Vector3)movement;

        if (animator != null)
        {
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
            // animator.SetTrigger("Attack");
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, powerBall.transform.position))
            {
                target = player.transform.position;
            } else
            {
                target = powerBall.transform.position;
            }
            var g = GameObject.Instantiate(bullet, transform.position + -(transform.position - target).normalized , Quaternion.identity);
            var r = g.GetComponent<Rigidbody2D>();
            r.AddForce(-(transform.position - target).normalized * 0.01f * bulletSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Debug.Log("Enemy HIT!");
        }
    }
}
