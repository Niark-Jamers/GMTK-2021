using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 1;

    private Vector3 target;
    private Vector3 target2;

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
    public float minPlayerDistance = 3f;

    [Header("Bullets")]
    public Transform bulletFirePosition = null;
    public float fireDelay = 1;

    [Header("Audio")]
    public AudioClip shootClip;
    public AudioClip deathClip;

    Animator        animator;
    new SpriteRenderer  renderer;
    bool            dead;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        powerBall = GameObject.FindGameObjectWithTag("PowerBall");

        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    int currentPoint = 0;
    void Update()
    {
        if (dead)
            return;

        Vector2 movement = Vector3.zero;

        if (patrol && !isAggro)
        {
            var target = controlsPoints[currentPoint].transform.position;
            movement = Vector2.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            if (Vector2.Distance((Vector2)transform.position, target) < 0.1f)
                currentPoint = (currentPoint + 1) % controlsPoints.Length;
        }

        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (aggro)
        {
            if (distance < aggroDistance && !isAggro)
            {
                StartCoroutine(Shoot());
                isAggro = true;
            }
        }

        if (isAggro && distance > minPlayerDistance)
        {
            movement = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed);
        }

        if (movement.magnitude > 0.0f)
            movement -= (Vector2)transform.position;
        else
            movement = Vector2.zero;

        transform.position += (Vector3)movement;

        if (animator != null)
        {
            if (Mathf.Abs(movement.x) > 0.0f)
                renderer.flipX = movement.x < 0;
                // Debug.Log(Mathf.Abs(movement.y * 5));
            animator.SetFloat("MoveX", Mathf.Abs(movement.x));
            animator.SetFloat("MoveY", Mathf.Abs(movement.y) > Mathf.Abs(movement.x) ? 1 : 0);
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if (dead)
                yield break;

            if (animator != null)
                animator.SetTrigger("Attack");

            //if (Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, powerBall.transform.position))
           // {
                target = player.transform.position;
                target2 = powerBall.transform.position;
           // } else
           // {
            //    target = powerBall.transform.position;
            //}
            Vector3 targetDiff = target2 - target;
            float distFromT1 = Random.Range(-0.2f, 1.2f);
            Vector3 trueTarget = target + targetDiff * distFromT1;

            var t = bulletFirePosition != null ? bulletFirePosition.transform.position : transform.position;
            var g = GameObject.Instantiate(bullet, t + -(t - target).normalized, Quaternion.identity);
            var r = g.GetComponent<Rigidbody2D>();
            r.AddForce(-(t - trueTarget).normalized * bulletSpeed, ForceMode2D.Force);

            if (shootClip)
                AudioManager.PlayOnShot(shootClip, 0.5f);

            yield return new WaitForSeconds(fireDelay);
        }
    }

    void OnDrawGizmos()
    {
        if (aggro)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroDistance);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "PlayerBullet" && !dead)
        {
            if (animator != null)
                animator.SetTrigger("Death");
            if (deathClip != null)
                AudioManager.PlayOnShot(deathClip);
            dead = true;
        }
    }
}
