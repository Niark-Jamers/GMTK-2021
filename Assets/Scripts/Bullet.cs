using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody2D rb;
    SpriteRenderer sr;

    public GameObject fire;
    public GameObject ice;
    public GameObject boom;
    ParticleSystem boomps;

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

    [Header("ZIGZAG")]
    public bool noMulti;
    public float multiTimer = 1f;
    float multiAltTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Vector2 tmp = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 500;
        // rb.AddForce(tmp);
        // direction = tmp.normalized;
        BaseScale = transform.localScale;
        bounceAltTimer = bounceTimer / 2;
        zzAltTimer = zzTimer / 2;
        zzDir = Vector3.Cross(direction, (zzGoRight) ? Vector3.forward : Vector3.back).normalized;
        ActivateModifier();
    }

    void ActivateFire(bool t)
    {
        fire.SetActive(t);
    }

    void ActivateIce(bool t)
    {
        ice.SetActive(t);
    }

    void ActivateBoom(bool t)
    {
        boom.SetActive(t);
        boomps = boom.GetComponent<ParticleSystem>();

    }

    void playBoom()
    {
        boomps.Play();
    }

    void ZigZag()
    {
        zzAltTimer += Time.deltaTime;
        if (zzAltTimer > zzTimer)
        {
            zzGoRight = !zzGoRight;
            zzAltTimer = 0f;
            rb.velocity = direction;
            zzDir = Vector3.Cross(direction, (zzGoRight) ? Vector3.forward : Vector3.back).normalized;
        }
        rb.velocity = (direction + (Vector2)zzDir * zzStr);
    }

    public void resetOnhit(Vector3 dir)
    {
        rb.velocity = dir;
        direction = dir;
        zzDir = Vector3.Cross(direction, (zzGoRight) ? Vector3.forward : Vector3.back).normalized;
        zzAltTimer = zzTimer / 2;
    }

    void Bounce()
    {
        bounceAltTimer += Time.deltaTime;
        if (bounceAltTimer > bounceTimer)
        {
            isGoBig = !isGoBig;
            bounceAltTimer = 0f;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, BaseScale * ((isGoBig) ? 1 + sizeMult : 1 - sizeMult), Time.deltaTime);
    }

    public void ActivateModifier()
    {
        ActivateFire(mod.fire);
        ActivateIce(mod.ice);
        ActivateBoom(mod.explosion);
    }

    private void Update()
    {
        if (mod.bounce)
            Bounce();
        // Debug.DrawRay(transform.position, direction, Color.red, Time.deltaTime);
        // Debug.DrawRay(transform.position, zzDir, Color.blue, Time.deltaTime);

        if (noMulti)
        {
            multiAltTimer += Time.deltaTime;
            if (multiAltTimer > multiTimer)
            {
                multiAltTimer = 0;
                noMulti = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (mod.zigzag)
            ZigZag();
    }

    private void OnValidate()
    {
        ActivateModifier();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (mod.explosion)
        {
            playBoom();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (mod.explosion)
        {
            playBoom();
        }
    }
}
