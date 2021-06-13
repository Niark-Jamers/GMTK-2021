using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{

    [HideInInspector]
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    public GameObject fire;
    public GameObject ice;
    public GameObject boom;
    public GameObject laser;
    ParticleSystem boomps;
    ParticleSystem laserps;

    List<Color> colorList = new List<Color>();

    [System.Serializable]
    public struct modifier
    {
        public bool laser;
        public bool fire;
        public bool ice;
        public bool zigzag;
        public bool explosion;
        public int bounce;
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

    [Header("MULTI")]
    public bool noMulti;
    public float multiTimer = 1f;
    float multiAltTimer = 0;

    [Header("LASER")]
    public float laserSpeed = 20;
    bool toBeKilled = false;

    float protec = 0.4f;
    float protecTimer = 0;
    public bool protecDown = false;

    int bounceCount = 0;

    [Header("MATERIAL")]
    public Material playerBulletMat;
    public Material ennemyBulletMat;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        // Vector2 tmp = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 500;
        // rb.AddForce(tmp);
        // direction = tmp.normalized;
        //direction = rb.velocity;
        BaseScale = transform.localScale;
        bounceAltTimer = bounceTimer / 2;
        zzAltTimer = zzTimer / 2;
        zzDir = Vector3.Cross(direction, (zzGoRight) ? Vector3.forward : Vector3.back).normalized;
        CheckMat();
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

    void ActivateLaser(bool t)
    {
        if (rb == null || t == false)
            return;
        laser.SetActive(false);
        laser.SetActive(t);
        laserps = laser.GetComponent<ParticleSystem>();
        direction = direction.normalized * laserSpeed;
        rb.velocity = direction;
        ParticleSystem.VelocityOverLifetimeModule tmp = laserps.velocityOverLifetime;
        tmp.x = direction.x;
        tmp.y = direction.y;
        laserps.Play();
    }


    void PauseLaser()
    {
        laserps.Stop();
        ParticleSystem.VelocityOverLifetimeModule tmp = laserps.velocityOverLifetime;
        tmp.x = 0;
        tmp.y = 0;
    }

    void ActivateBoom(bool t)
    {
        // boom.SetActive(t);
        // boomps = boom.GetComponent<ParticleSystem>();

    }

    void PlayBoom()
    {
        Debug.Log("BOOM");
        var tmp = Instantiate(boom, transform.position, Quaternion.Euler(0, 0, 0));
        tmp.SetActive(true);
        tmp.GetComponent<ParticleSystem>().Play();
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

    public void resetValue(Vector3 dir)
    {
        rb.velocity = dir;
        direction = dir;
        zzDir = Vector3.Cross(direction, (zzGoRight) ? Vector3.forward : Vector3.back).normalized;
        zzAltTimer = zzTimer / 2;
        CheckMat();
        ActivateModifier();
    }

    public void resetValue(Vector3 dir, modifier tmod)
    {
        rb.velocity = dir;
        //Debug.Log(rb.velocity);
        direction = dir;
        zzDir = Vector3.Cross(direction, (zzGoRight) ? Vector3.forward : Vector3.back).normalized;
        zzAltTimer = zzTimer / 2;
        mod = tmod;
        CheckMat();
        ActivateModifier();
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

    void CheckMat()
    {
        if (tag == "EnemyBullet")
            sr.material = ennemyBulletMat;
        if (tag == "PlayerBullet")
            sr.material = playerBulletMat;
    }

    public void ActivateModifier()
    {
        ActivateFire(mod.fire);
        ActivateIce(mod.ice);
        ActivateBoom(mod.explosion);
        ActivateLaser(mod.laser);
    }

    void DoGoodRota()
    {
        //Vector2 diff = direction - (Vector2)this.transform.position;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    private void Update()
    {
        DoGoodRota();
        if (protecDown == false)
            protecTimer += Time.deltaTime;
        if (protecTimer > protec)
            protecDown = true;
        if (mod.bounce > 0)
            Bounce();

        Debug.DrawRay(transform.position, direction, Color.red, Time.deltaTime);
        Debug.DrawRay(transform.position, zzDir, Color.blue, Time.deltaTime);

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
        {
            ZigZag();
        }
        else
            rb.velocity = direction;
    }

    private void OnValidate()
    {
        ActivateModifier();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (mod.explosion)
            PlayBoom();
        if (other.gameObject.tag == "Link")
            return;
        if (mod.bounce > 0)
        {
            mod.bounce--;
            resetValue(Vector3.Reflect(direction, -other.contacts[0].normal));
        }
        else if (this.tag == "EnemyBullet" && other.gameObject.tag == "Enemy")
            return;
        else
        {
            //Debug.Log("suside");
            Destroy(this.gameObject);
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (mod.laser)
        // {
        //     PauseLaser();
        // }
        if (mod.explosion)
        {
            PlayBoom();
        }
    }
}
