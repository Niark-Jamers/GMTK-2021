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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        powerBall = GameObject.FindGameObjectWithTag("PowerBall");
        StartCoroutine(Shoot());
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
