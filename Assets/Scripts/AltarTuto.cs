using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarTuto : MonoBehaviour
{
    Animator animator;

    bool activated = false;
    GameObject powerBall;
    GameObject link;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        powerBall = FindObjectOfType<PowerBall>().gameObject;
        link = FindObjectOfType<Link>().gameObject;
        powerBall.SetActive(false);
        link.SetActive(false);
        player = FindObjectOfType<Player>();
        player.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("Vector1_3d3385984e454112a860d8a8bf51e293", 0);
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player" && !activated)
        {
            animator.enabled = true;
            StartCoroutine(A());
            activated = true;
        }
    }

    IEnumerator A()
    {
        yield return new WaitForSeconds(0.3f);
        powerBall.SetActive(true);
        link.SetActive(true);
        player.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("Vector1_3d3385984e454112a860d8a8bf51e293", 2.3f);
    }
}
