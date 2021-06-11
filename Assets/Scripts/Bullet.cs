using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;

    [System.Serializable]
    public struct modifier
    {
        public bool fire;
        public bool ice;
        public bool multishot;
        public bool laser;
        public bool zigzag;
        public bool explosion;
        public bool bounce;
    }

    [SerializeField]
    public modifier mod = new modifier();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f)).normalized * 500);
    }

    void ActivateFire()
    {

    }
}
