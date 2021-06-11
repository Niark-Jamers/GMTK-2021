using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBall : MonoBehaviour
{
    GameObject player;
    public float length;
    public float angle = 0;
    public float angleSpeed = 1;
    public float radius = 1;

    public float speed = 1.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        length = Vector3.Distance(transform.position, player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.RightArrow))
        //     angle += angleSpeed;
        // if (Input.GetKey(KeyCode.LeftArrow))
        //     angle -= angleSpeed;

        // transform.position = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

        transform.position += new Vector3(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"), 0) * speed * Time.deltaTime;
    }
}
