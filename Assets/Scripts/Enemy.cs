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

    [Header("Turret")]
    public bool turret = false;
    // public Vector2 direction;

    [Header("Bullets")]
    public Transform bulletFirePosition = null;
    public float fireDelay = 1;


    [Header("Audio")]
    public AudioClip shootClip;
    public AudioClip deathClip;

    Animator        animator;
    new SpriteRenderer  renderer;
    new Rigidbody2D     rigidbody2D;
    bool            dead;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        powerBall = GameObject.FindGameObjectWithTag("PowerBall");

        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (turret)
        {
            StartCoroutine(Shoot());
        }
    }

    int currentPoint = 0;
    Vector2 movement = Vector3.zero;
    void Update()
    {
        if (dead)
            return;

        if (patrol && !isAggro)
        {
            var target = controlsPoints[currentPoint].transform.position;
            movement = (target - transform.position).normalized * Time.deltaTime * moveSpeed;
            if (Vector2.Distance((Vector2)transform.position, target) < 0.1f)
                currentPoint = (currentPoint + 1) % controlsPoints.Length;
        }

        float distance = Vector2.Distance(player.transform.position, transform.position);
        // Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        if (aggro)
        {
            if (distance < aggroDistance * GameManager.Instance.aggroZoneMultiplier && !isAggro)
            {
                RaycastHit2D[] results = new RaycastHit2D[2];
                var hit = Physics2D.RaycastNonAlloc(transform.position, player.transform.position - transform.position, results, aggroDistance, 1);
                // results[0] is always the enemy
                if (results[1].collider != null && results[1].collider.tag == "Player")
                {
                    StartCoroutine(Shoot());
                    isAggro = true;
                }
            }
        }

        if (isAggro && distance > minPlayerDistance)
            movement = (player.transform.position - transform.position).normalized * Time.deltaTime * moveSpeed;

        rigidbody2D.MovePosition((Vector2)rigidbody2D.position + movement);

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
            float distFromT1 = Random.Range(0f, 0.3f);
            Vector3 trueTarget = target + targetDiff * distFromT1;

            var t = bulletFirePosition != null ? bulletFirePosition.transform.position : transform.position;
            var g = GameObject.Instantiate(bullet, t + -(t - target).normalized, Quaternion.identity);
            var r = g.GetComponent<Bullet>();
            r.direction = (-(t - trueTarget).normalized * bulletSpeed * GameManager.Instance.bulletSpeedMultiplier);

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
                AudioManager.PlayOnShot(deathClip, 0.5f);
                GetComponent<Collider2D>().enabled = false;
            
            dead = true;
        }
    }
}
