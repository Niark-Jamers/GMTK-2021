using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLink : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed = 0.01f;
    public float multiShotStep = 15;

    [System.Serializable]
    public struct power
    {
        [SerializeField]
        Bullet.modifier bMod;
        public int multiShot;
    }

    [SerializeField]
    public power p = new power();

    void MoultiShotage(Vector2 dir, ContactPoint2D c, Bullet.modifier tmod)
    {
        Vector2 baseDir = Quaternion.Euler(0, 0, (-multiShotStep * p.multiShot) / 2) * dir.normalized;
        // Debug.DrawRay(c.point, baseDir, Color.blue, 1f);
        for (int i = 0; i <= p.multiShot; i++)
        {
            Vector2 tmp = Quaternion.Euler(0, 0, multiShotStep * i) * baseDir;
            GameObject g = Instantiate(bulletPrefab, c.point, Quaternion.Euler(0, 0, 0));
            Bullet b = g.GetComponent<Bullet>();
            b.rb = g.GetComponent<Rigidbody2D>();
            b.resetOnhit(tmp);
            b.noMulti = true;
            Debug.DrawRay(c.point, tmp, Color.blue, 1f);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            //Debug.DrawLine(col.contacts[0].point, col.contacts[0].point - col.contacts[0].normal, Color.red, 1f);
            Vector2 velocity = -col.contacts[0].normal;
            Bullet tb = col.gameObject.GetComponent<Bullet>();
            
            if (tb.noMulti == false)
            {
                if (p.multiShot > 0)
                {
                    Bullet.modifier tmod = tb.mod;
                    MoultiShotage(velocity, col.contacts[0], tmod);
                    Destroy(col.gameObject);
                }
                else
                    col.gameObject.GetComponent<Bullet>().resetOnhit(velocity);
            }
        }
    }
}
