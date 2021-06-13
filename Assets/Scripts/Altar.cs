using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    Animator animator;

    bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.Instance.newPowerAdded -= NewPowerCallback;
        GameManager.Instance.newPowerAdded += NewPowerCallback;
    }

    void OnDisable()
    {
        GameManager.Instance.newPowerAdded -= NewPowerCallback;
    }

    void NewPowerCallback()
    {
        if (activated && isActiveAndEnabled)
        {
            animator.enabled = true;
            enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player" && !activated)
        {
            GameManager.Instance.PowerRoulette();
            activated = true;
        }
    }
}
