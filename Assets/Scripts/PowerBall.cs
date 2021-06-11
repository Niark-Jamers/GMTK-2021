using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PowerBall : MonoBehaviour
{
    GameObject player;
    public float length;
    public float angle = 0;
    public float angleSpeed = 1;
    public float radius = 1;

    public float speed = 1.0f;

    public float min = 1f;
    public float max = 3f;

    PixelPerfectCamera  cam;

    new Camera camera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        length = Vector3.Distance(transform.position, player.transform.position);
        camera = Camera.main;
        cam = FindObjectOfType<PixelPerfectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.RightArrow))
        //     angle += angleSpeed;
        // if (Input.GetKey(KeyCode.LeftArrow))
        //     angle -= angleSpeed;

        // Trackball:
        // transform.position = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

        // Cancer control:
        // transform.position += new Vector3(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"), 0) * speed * Time.deltaTime;

        var m = (Vector2)Input.mousePosition + new Vector2(-10, 0);
        // var p = camera.ScreenToWorldPoint(new Vector3(m.x, m.y, camera.nearClipPlane));
        var c = m / new Vector2(camera.pixelWidth, camera.pixelHeight);
        c = new Vector2(Mathf.Clamp01(c.x), Mathf.Clamp01(c.y));
        // c *= camera.orthographicSize;
        c = c * 2 - 1 * Vector2.one;
        c = camera.projectionMatrix.inverse.MultiplyPoint(c);
        Debug.Log(c);
        // c = camera.ViewportToWorldPoint(c);
        // c = c + 0.5f * Vector2.one;
        // c *= camera.orthographicSize;
        // c += new Vector2(cam.transform.position.x, cam.transform.position.y);
        var p = c;

        float distance = Vector2.Distance(player.transform.position, p);
        // Debug.Log(((Vector2)p - (Vector2)player.transform.position).normalized);
        transform.position = player.transform.position + (Vector3)(Mathf.Clamp(distance, min, max) * ((Vector2)p - (Vector2)player.transform.position).normalized);

        // transform.position = new Vector3(p.x, p.y, transform.position.z);
    }
}
