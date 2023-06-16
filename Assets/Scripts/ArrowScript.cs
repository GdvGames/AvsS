using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private Rigidbody2D myBody;

    private float speed = 5f;
    
    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myBody.velocity = new Vector2(0, speed);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Top" || collision.tag == "Platform")
        {
            Destroy(gameObject);
        }

        string[] name = collision.name.Split();

        if (name.Length > 1)
        {
            if (name[1] == "Ball")
            {
                Destroy(gameObject);
            }
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Top") || collision.gameObject.CompareTag("Platform"))
        {
            Destroy(gameObject);
        }

        string[] name = collision.gameObject.name.Split();

        if (name.Length > 1)
        {
            if (name[1] == "Slime")
            {
                Destroy(gameObject);
            }
        }
    }
}
