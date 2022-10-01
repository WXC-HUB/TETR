using System.Collections;
using UnityEngine;


public class BulletBase : MonoBehaviour
{
    public float m_speed = 2f;
    public void InitBullet(Vector3 dir)
    {
        dir = (new Vector3(dir.x, dir.y, 0)).normalized;
        var z = (dir.y > 0 ? 1 : -1) * Vector3.Angle(new Vector3(1, 0, 0), new Vector3(dir.x, dir.y, 0));
        transform.localRotation = Quaternion.Euler(0, 0, z);

        GetComponent<Rigidbody2D>().velocity = dir * m_speed;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            DestroyBullet();
        }else if(collision.gameObject.layer == LayerMask.NameToLayer("BlockFixed") || collision.gameObject.layer == LayerMask.NameToLayer("BlockGoingDown"))
        {
            collision.gameObject.GetComponent<BlockBase>().OnTakeDamage(1);
            DestroyBullet();
        }else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<GameCharacterBase>().OnTakeDamage(1);
            DestroyBullet();
        }

    }
}
