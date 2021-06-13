using UnityEngine;

public class DestroyOnBulletType : MonoBehaviour
{
    public bool destroyOnFire;
    public bool destroyOnIce;

    new Collider2D collider2D;

    void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "PlayerBullet")
        {
            var b = c.gameObject.GetComponent<Bullet>();
            if (b.mod.fire && destroyOnFire)
            {
                Destroy(gameObject);
                return;
            }
            if (b.mod.ice && destroyOnIce)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
}
