using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    private float forceImpulse = 50f;
    private Rigidbody2D colliderBody;

    public Rigidbody2D testBody;
    
    private Vector2 direction;

    private bool canBounce=false;

    private bool top = false;
    private bool bottom = false;
    private bool left = false;
    private bool right = false;

    private void Start()
    {
        direction = Vector2.one.normalized;
    }

    private void FixedUpdate()
    {
        //Debug.Log("velocit�: " + testBody.velocity.magnitude.ToString());

        if (canBounce)
        {
            if (top)
            {
                ImpulseTop();
            }
            else if (bottom)
            {
                ImpulseBottom();
            }
            else if (left)
            {
                ImpulseLeft();
            }
            else if (right)
            {

            }


        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliderBody = this.transform.parent.GetChild(6).GetComponent<Rigidbody2D>();

        direction = (colliderBody.transform.position - this.transform.position).normalized;
        //Debug.Log("Direzione: " + direction.ToString());

        if (!canBounce)
        {
            if (collision.transform.gameObject.tag == "Ground")
            {
                canBounce = true;
                bottom = true;
            }
            else if (collision.transform.gameObject.tag == "Top")
            {
                canBounce = true;
                top = true;
            }
            else if (collision.transform.gameObject.tag == "Right Wall")
            {
                canBounce = true;
                right = true;
            }
            else if (collision.transform.gameObject.tag == "Left Wall")
            {
                canBounce = true;
                left = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (colliderBody.velocity.magnitude <= 0.5f)
        {
            if (collision.transform.gameObject.tag == "Ground")
            {
                Debug.Log("Applico STAY");
                canBounce = true;
                bottom = true;
            }
        }
    }

    void ImpulseBottom()
    {
        colliderBody.AddForce(Vector2.up * direction * forceImpulse, ForceMode2D.Impulse);

        BounceDelay();

        Debug.Log("Spingo in SU!");
        bottom = false;
        canBounce=false;
    }

    void ImpulseTop()
    {
        colliderBody.AddForce(Vector2.down * direction * forceImpulse, ForceMode2D.Impulse);

        BounceDelay();

        Debug.Log("Spingo in GIU!");
        top = false;
        canBounce = false;
    }

    void ImpulseLeft()
    {
        colliderBody.AddForce(Vector2.right * direction * forceImpulse, ForceMode2D.Impulse);

        BounceDelay();

        Debug.Log("Spingo a DESTRA!");
        left = false;
        canBounce = false;
    }

    void ImpulseRight()
    {
        colliderBody.AddForce(Vector2.left * direction * forceImpulse, ForceMode2D.Impulse);

        BounceDelay();

        Debug.Log("Spingo a SINISTRA!");
        right = false;
        canBounce = false;
    }

    IEnumerator BounceDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }

}
