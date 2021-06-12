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

    public SpriteRenderer selfSprite;
    public Material spriteMaterial;

    private bool overHeating;
    //private BoxCollider2D selfCollider;
    GameObject player;

    LineRenderer line;

    public GameObject bulletPrefab;
    public float speed = 2f;
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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // selfCollider = gameObject.GetComponent<BoxCollider2D>();
        // line = GetComponent<LineRenderer>();
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
            b.resetOnhit(tmp);
            b.noMulti = true;
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
                } else
                {
                    curHeat -= Mathf.Clamp(Time.deltaTime * heatSpeed, 0, 100);
                }
            }
        }
        selfSprite.color = Color.Lerp(Color.blue, Color.red, curHeat / 100);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            if (!linkActive)
            {
                GameManager.Instance.ReloadLevel();
            }
            var r = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 velocity = -col.contacts[0].normal * speed;
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

    void EnableLink(bool value)
    {
        if (value)
        {
            spriteMaterial.SetColor("_Color", new Color(15f, 15f, 15f));
        } else
        {
            spriteMaterial.SetColor("_Color", new Color(5, 5, 5));
        }

        linkActive = value;
        //selfCollider.enabled = value;
    }
}
