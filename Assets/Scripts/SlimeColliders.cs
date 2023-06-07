using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeColliders : MonoBehaviour
{
    public SlimeScript slimeScript;

    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Debug.Log("FRECCIA-HIT");

            slimeScript.InitializeSlimes();
        }
    }
}
