using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLink : MonoBehaviour
{

    public float speed = 0.01f;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            var r = col.gameObject.GetComponent<Rigidbody2D>();
            Debug.DrawLine(col.contacts[0].point, col.contacts[0].point -col.contacts[0].normal, Color.red, 1f);
            r.velocity = -col.contacts[0].normal;
        }
    }
}
