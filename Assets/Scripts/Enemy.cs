using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 1;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            var g = GameObject.Instantiate(bullet, transform.position + -(transform.position - player.transform.position).normalized , Quaternion.identity);
            var r = g.GetComponent<Rigidbody2D>();
            r.AddForce(-(transform.position - player.transform.position).normalized * 0.01f * bulletSpeed, ForceMode2D.Impulse);
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
