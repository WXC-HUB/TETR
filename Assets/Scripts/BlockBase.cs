using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    public bool isSolid = false;

    public int HP = 10;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsDead();
    }

    public void CheckIsDead()
    {
        if (HP <= 0)
        {
            OnDead();
        }
    }

    public virtual void OnDead()
    {
        Destroy(gameObject);
    }

    public virtual void OnBeAttack()
    {

    }

    public virtual void OnGetSolid()
    {
        this.GetComponent<SpriteRenderer>().color = Color.gray;
        isSolid = true;
    }
}
