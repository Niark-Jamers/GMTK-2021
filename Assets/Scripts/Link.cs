using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Link : MonoBehaviour
{
    public GameObject target;
    public Slider heatBar;
    public float multiplier = 10;
    public float curHeat = 1f;
    public bool linkActive;
    public float heatSpeed = 2f;
    public float overheatSpeed = 1f;

    public ParticleSystem ps;
    public ParticleSystem psSub;
    public ParticleSystem damagePS;
    public Material spriteMaterial;
    public Material outlineMaterial;

    private bool overHeating;
    //private BoxCollider2D selfCollider;
    GameObject player;

    AudioSource audioSource;
    public AudioClip vwoupvwoupClip;
    public AudioClip overheatClip;
    public AudioClip hitClip;

    LineRenderer line;

    public GameObject bulletPrefab;
    public float speed = 20f;
    public float multiShotStep = 15;

    bool dead = false;

    [System.Serializable]
    public struct power
    {
        [SerializeField]
        public Bullet.modifier bMod;
        public int multiShot;
    }

    [SerializeField]
    public power p = new power();

    public Gradient gg;

    Gradient g = new Gradient();
    GradientColorKey[] ck = new GradientColorKey[2];
    GradientAlphaKey[] ak = new GradientAlphaKey[2];

    float subSpeedDrag;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // selfCollider = gameObject.GetComponent<BoxCollider2D>();
        subSpeedDrag = psSub.limitVelocityOverLifetime.drag.constant;
        // Debug.Log(subSpeedDrag);
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = vwoupvwoupClip;
        audioSource.loop = true;
        audioSource.volume = 0;
        audioSource.Play();

    }

    Bullet.modifier SetMods(Bullet.modifier tmod)
    {
        tmod.laser = (tmod.laser) ? tmod.laser : p.bMod.laser;
        tmod.fire = (tmod.fire) ? tmod.fire : p.bMod.fire;
        tmod.ice = (tmod.ice) ? tmod.ice : p.bMod.ice;
        tmod.zigzag = (tmod.zigzag) ? tmod.zigzag : p.bMod.zigzag;
        tmod.explosion = (tmod.explosion) ? tmod.explosion : p.bMod.explosion;
        tmod.bounce = p.bMod.bounce;

        return tmod;
    }

    void MoultiShotage(Vector2 dir, ContactPoint2D c, Bullet.modifier tmod)
    {
        Vector2 baseDir = Quaternion.Euler(0, 0, (-multiShotStep * p.multiShot) / 2) * dir.normalized;
        // Debug.DrawRay(c.point, baseDir, Color.blue, 1f);
        for (int i = 0; i <= p.multiShot; i++)
        {
            Vector2 tmp = Quaternion.Euler(0, 0, multiShotStep * i) * baseDir;
            GameObject g = Instantiate(bulletPrefab, c.point, Quaternion.Euler(0, 0, 0));
            g.tag = "PlayerBullet";
            Bullet b = g.GetComponent<Bullet>();
            b.rb = g.GetComponent<Rigidbody2D>();
            b.sr = GetComponent<SpriteRenderer>();
            b.noMulti = true;
            b.protecDown = false;
            b.resetValue(tmp * speed, SetMods(tmod));
            Debug.DrawRay(c.point, tmp, Color.blue, 1f);
        }
    }

    void FixedUpdate()
    {
        UpdateHeat();
        heatBar.value = curHeat;

        Vector2 diff = target.transform.position - player.transform.position;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, rot_z);
        transform.localScale = new Vector3(diff.magnitude, 0.2f, 0.2f);

        transform.localPosition = (target.transform.localPosition + player.transform.localPosition) / 2;
    }

    void ModifyGradient(Color c)
    {
        ck[0].color = c;
        ck[0].time = 0.0f;
        ck[1].color = c;
        ck[1].time = 1.0f;
        ak[0].alpha = 1.0f;
        ak[0].time = 0.0f;
        ak[1].alpha = 1.0f;
        ak[1].time = 1.0f;
        g.SetKeys(ck, ak);
    }

    Color Slurp(Color c1, Color c2, float t)
    {
        return new Color(c1.r * (1 - t) + c2.r * t,
        c1.g * (1 - t) + c2.g * t,
        c1.b * (1 - t) + c2.b * t);
    }

    void UpdateHeat()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !overHeating)
        {
            EnableLink(true);
            if (curHeat < 100f)
            {
                curHeat += Mathf.Clamp(Time.deltaTime * heatSpeed, 0, 100);
                if (curHeat >= 100)
                {
                    overHeating = true;
                    AudioManager.PlayOnShot(overheatClip);
                    EnableLink(false);
                }
            }
            else
            {
                Debug.Log("TROP CHAUD CA MERE");
            }
        }
        else
        {
            EnableLink(false);
            if (curHeat > 0f)
            {
                if (overHeating)
                {
                    curHeat -= Mathf.Clamp(Time.deltaTime * overheatSpeed, 0, 100);
                    if (curHeat <= 0)
                    {
                        overHeating = false;
                    }
                }
                else
                {
                    curHeat -= Mathf.Clamp(Time.deltaTime * heatSpeed, 0, 100);
                }
            }
        }
        var tmp = ps.colorOverLifetime;
        ModifyGradient(gg.Evaluate(curHeat / 100));
        tmp.color = g;

        if (curHeat > 0 && !overHeating)
            audioSource.volume = curHeat / 100.0f;
        else
            audioSource.volume = 0;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (dead)
            return;
        if (col.gameObject.tag == "PlayerBullet")
        {
            var r = col.gameObject.GetComponent<Rigidbody2D>();
            Bullet tb = col.gameObject.GetComponent<Bullet>();
            if (tb.protecDown)
            {
                Vector2 velocity = -col.contacts[0].normal * speed;
                if (tb.mod.bounce > 0)
                {

                    Bullet.modifier tmod = tb.mod;
                    tb.mod.bounce--;
                    tb.resetValue(Vector3.Reflect(tb.direction, -col.contacts[0].normal));
                }
                else
                {
                    Destroy(col.gameObject);
                }
            }
        }
        if (col.gameObject.tag == "EnemyBullet")
        {
            if (!linkActive)
            {
                TakeHit(col.contacts[0].point);
                Destroy(col.gameObject);
                return;
            }
            else
            {
                col.gameObject.tag = "PlayerBullet";
                var r = col.gameObject.GetComponent<Rigidbody2D>();
                Bullet tb = col.gameObject.GetComponent<Bullet>();
                Vector2 velocity = -col.contacts[0].normal * speed;
                Bullet.modifier tmod = tb.mod;
                if (tb.noMulti == false)
                {
                    if (p.multiShot > 0)
                    {
                        MoultiShotage(velocity, col.contacts[0], tmod);
                        Destroy(col.gameObject);
                    }
                    else
                    {
                        Debug.Log("renvoi");
                        col.gameObject.GetComponent<Bullet>().resetValue(velocity, SetMods(tmod));
                    }
                }
            }

        }
    }

    public void TakeHit(Vector2 pos)
    {
        damagePS.gameObject.transform.position = pos;
        damagePS.Play();
        var p = player.GetComponent<Player>();
        p.lifePoints -= 1;
        GUIManager.Instance.UpdateLife(p.lifePoints);
        //        Debug.Log(p.lifePoints);

        if (p.lifePoints == 0)
        {
            CameraManager.Shake(10, 0.6f);
            StartCoroutine(Death());
        }
        else
        {
            CameraManager.Shake(4, 0.15f);
            AudioManager.PlayOnShot(hitClip, 0.3f);
        }
    }

    IEnumerator Death()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        dead = true;

        player.GetComponent<Player>().Die();

        yield return new WaitForSeconds(2f);

        GameManager.Instance.ReloadLevel();
    }

    void EnableLink(bool value)
    {
        if (value)
        {
            spriteMaterial.SetColor("_Color", gg.Evaluate(curHeat / 100) * 8);
            outlineMaterial.SetColor("_OutlineColor", gg.Evaluate(curHeat / 100) * 8);
            var tmp = psSub.limitVelocityOverLifetime;
            var stmp = tmp.drag;
            stmp.constant = subSpeedDrag / 5;
            tmp.drag = stmp;
            // Debug.Log(psSub.limitVelocityOverLifetime.drag.constant);
            // var tmp = ps.velocityOverLifetime;
            // var mmtmp = tmp.x;
            // mmtmp.constantMin = subSpeedMinX * 10;
            // mmtmp.constantMax = subSpeedMaxX * 10;
            // mmtmp = tmp.y;
            // mmtmp.constantMin = subSpeedMinY * 10;
            // mmtmp.constantMax = subSpeedMaxY * 10;
        }
        else
        {
            spriteMaterial.SetColor("_Color", gg.Evaluate(curHeat / 100) * 2f);
            outlineMaterial.SetColor("_OutlineColor", gg.Evaluate(curHeat / 100) * 2f);
            var tmp = psSub.limitVelocityOverLifetime;
            var stmp = tmp.drag;
            stmp.constant = subSpeedDrag;
            tmp.drag = stmp;
            // var tmp = ps.velocityOverLifetime;
            // var mmtmp = tmp.x;
            // mmtmp.constantMin = subSpeedMinX;
            // mmtmp.constantMax = subSpeedMaxX;
            // mmtmp = tmp.y;
            // mmtmp.constantMin = subSpeedMinY;
            // mmtmp.constantMax = subSpeedMaxY;
        }

        linkActive = value;
        //selfCollider.enabled = value;
    }
}
