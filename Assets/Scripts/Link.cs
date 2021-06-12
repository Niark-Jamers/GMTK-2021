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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // selfCollider = gameObject.GetComponent<BoxCollider2D>();
        // line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateHeat();
        heatBar.value = curHeat;
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

    private void FixedUpdate()
    {

        transform.position = (target.transform.position + player.transform.position) / 2;
        Vector2 diff = target.transform.position - player.transform.position;

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        transform.localScale = new Vector3(diff.magnitude, 0.2f, 0.2f);

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
