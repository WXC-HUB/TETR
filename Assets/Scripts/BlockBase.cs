using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        this.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
    }

    public void goingDown(float speed_scale)
    {
        _rigidbody.MovePosition(new Vector2( transform.position.x, transform.position.y - 0.03f * speed_scale ));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
