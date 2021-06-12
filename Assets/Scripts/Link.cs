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
    public Material spriteMaterial;

    private bool overHeating;
    //private BoxCollider2D selfCollider;
    GameObject player;


    LineRenderer line;

    public GameObject bulletPrefab;
    public float speed = 2f;
    public float multiShotStep = 15;

    bool dead = false;

    Color baseColor = new Color(0, 216, 255);

    [System.Serializable]
    public struct power
    {
        [SerializeField]
        public Bullet.modifier bMod;
        public int multiShot;
    }

    [SerializeField]
    public power p = new power();

    Gradient g = new Gradient();
    GradientColorKey[] ck = new GradientColorKey[2];
    GradientAlphaKey[] ak = new GradientAlphaKey[2];

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // selfCollider = gameObject.GetComponent<BoxCollider2D>();
        // line = GetComponent<LineRenderer>();
    }

    Bullet.modifier SetMods(Bullet.modifier tmod)
    {
        tmod.laser = (tmod.laser) ? tmod.laser : p.bMod.laser;
        tmod.fire = (tmod.fire) ? tmod.fire : p.bMod.fire;
        tmod.ice = (tmod.ice) ? tmod.ice : p.bMod.ice;
        tmod.zigzag = (tmod.zigzag) ? tmod.zigzag : p.bMod.zigzag;
        tmod.explosion = (tmod.explosion) ? tmod.explosion : p.bMod.explosion;
        tmod.bounce = (tmod.bounce) ? tmod.bounce : p.bMod.bounce;

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
            Bullet b = g.GetComponent<Bullet>();
            b.rb = g.GetComponent<Rigidbody2D>();
            b.noMulti = true;
            b.resetValue(tmp, SetMods(tmod));
            //Debug.DrawRay(c.point, tmp, Color.blue, 1f);
        }
    }

    void Update()
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
        ModifyGradient(Color.Lerp(Color.cyan, Color.red, curHeat / 100));
        tmp.color = g;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (dead)
            return;

        if (col.gameObject.tag == "EnemyBullet")
        {
            if (!linkActive)
            {
                Die();
            }
            col.gameObject.tag = "PlayerBullet";
            var r = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 velocity = -col.contacts[0].normal * speed;
            Bullet tb = col.gameObject.GetComponent<Bullet>();
            Bullet.modifier tmod = tb.mod;

            if (tb.noMulti == false && linkActive)
            {
                if (p.multiShot > 0)
                {
                    MoultiShotage(velocity, col.contacts[0], tmod);
                    Destroy(col.gameObject);
                }
                else
                {
                    col.gameObject.GetComponent<Bullet>().resetValue(velocity, SetMods(tmod));
                }
            }
        }
    }

    public void Die()
    {
        StartCoroutine(Death());
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
            spriteMaterial.SetColor("_Color", new Color(8f, 8f, 8f));
        }
        else
        {
            spriteMaterial.SetColor("_Color", new Color(2, 2, 2));
        }

        linkActive = value;
        //selfCollider.enabled = value;
    }
}
