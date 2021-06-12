using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    Rigidbody2D rb;
    SpriteRenderer sr;

    public GameObject firePS;
    public GameObject icePS;
    public GameObject boomPS;

    List<Color> colorList = new List<Color>();

    [System.Serializable]
    public struct modifier
    {
        public bool laser;
        public bool fire;
        public bool ice;
        public bool zigzag;
        public bool explosion;
        public bool bounce;
    }

    [SerializeField]
    public modifier mod = new modifier();

    [Header("BOUNCE")]
    public float sizeMult;
    Vector3 BaseScale;
    bool isGoBig;
    public float bounceTimer = 0.5f;
    float bounceAltTimer = 0;

    [Header("ZIGZAG")]
    public Vector2 direction;
    Vector3 zzDir;
    public float zzStr = 1;
    public bool zzGoRight;
    public float zzTimer = 1f;
    float zzAltTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 tmp = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 500;
        rb.AddForce(tmp);
        direction = tmp.normalized;
        BaseScale = transform.localScale;
        bounceAltTimer = bounceTimer / 2;
        zzAltTimer = zzTimer / 2;
        zzDir = Vector3.Cross(direction, (zzGoRight)? Vector3.forward : Vector3.back).normalized;
    }

    void ActivateFire()
    {
        firePS.SetActive(true);
    }

    void ActivateIce()
    {
        icePS.SetActive(true);
    }

    void ActivateBoom()
    {
        boomPS.SetActive(true);
    }

    void ZigZag()
    {
         zzAltTimer += Time.deltaTime;
        if (zzAltTimer > zzTimer)
        {
            zzGoRight = !zzGoRight;
            zzAltTimer = 0f;
            rb.velocity = direction;
            zzDir = Vector3.Cross(direction, (zzGoRight)? Vector3.forward : Vector3.back).normalized;
        }
        rb.AddForce(zzDir * zzStr);
    }

    void Bounce()
    {
        bounceAltTimer += Time.deltaTime;
        if (bounceAltTimer > bounceTimer)
        {
            isGoBig = !isGoBig;
            bounceAltTimer = 0f;
        }
       transform.localScale = Vector3.Lerp(transform.localScale, BaseScale * ((isGoBig)?1 + sizeMult: 1 - sizeMult), Time.deltaTime);
    }

    public void ActivateModifier()
    {
        if (mod.fire)
            ActivateFire();
        if (mod.ice)
            ActivateIce();
    }

    private void Update()
    {
        if (mod.bounce)
            Bounce();
    }

    private void FixedUpdate() {
        if (mod.zigzag)
            ZigZag();
    }

    private void OnValidate() {
        ActivateModifier();
    }
}
