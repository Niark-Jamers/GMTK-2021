using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Link : MonoBehaviour
{
    public GameObject target;
    public float multiplier = 10;
    GameObject player;

    LineRenderer line;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        transform.position = (target.transform.position + player.transform.position) / 2;
        Vector2 diff = target.transform.position - player.transform.position;
         
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        // if (target != null)
        //     line.SetPositions(new Vector3[] { transform.position, target.transform.position });
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            var r = col.gameObject.GetComponent<Rigidbody2D>();
            r.velocity = -col.contacts[0].normal * r.velocity.magnitude * multiplier;
        }
    }
}
