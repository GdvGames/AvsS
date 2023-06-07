using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{

    public Rigidbody2D mainRigidbody;

    public CircleCollider2D circleCollider;

    private float dirForce;

    private int tempDir;

    [SerializeField]
    private GameObject originalBall;

    private GameObject ball1, ball2;
    private SlimeScript ball1Script, ball2Script;

    [SerializeField]
    private AudioClip[] popSounds;

    private void Awake()
    {
        SetSlimeForce();
    }

    private void Start()
    {
       RandomizeStartDirection();
    }

    void InstantiateSlimes()
    {
        ball1 = Instantiate(originalBall);
        ball2 = Instantiate(originalBall);

        ball1Script = ball1.GetComponent<SlimeScript>();
        ball2Script = ball2.GetComponent<SlimeScript>();
    }

    void SetSlimeForce()
    {
        dirForce = 15f;

        switch (this.gameObject.tag)
        {
            case "Largest Ball":
                dirForce = 57f;
                break;

            case "Large Ball":
                dirForce = 46f;
                break;

            case "Medium Ball":
                dirForce = 35f;
                break;

            case "Small Ball":
                dirForce = 24f;
                break;

            case "Smallest Ball":
                dirForce = 16f;
                break;
        }
    }

    public void InitializeSlimes()
    {
        if (this.gameObject.tag != "Smallest Ball")
        {
            InstantiateSlimes();

            Vector3 temp = transform.position;

            ball1.transform.position = temp;
            ball1Script.mainRigidbody.AddForce(Vector2.left * dirForce, ForceMode2D.Impulse);

            ball2.transform.position = temp;
            ball2Script.mainRigidbody.AddForce(Vector2.right * dirForce, ForceMode2D.Impulse);

            PopDaSlime();
        }
        else
        {
            PopDaSlime();
        }

        
    }

    public void RandomizeStartDirection()
    {
        if(this.gameObject.tag == "Largest Ball")
        {
            tempDir = Random.Range(0, 2);


            if (tempDir == 0)
            {
                mainRigidbody.AddForce(Vector2.left * dirForce, ForceMode2D.Impulse);
            }
            else if (tempDir == 1)
            {
                mainRigidbody.AddForce(Vector2.right * dirForce, ForceMode2D.Impulse);
            }

            Debug.Log("La direzione è: " + tempDir);
        }
    }

    public void PopDaSlime()
    {
        AudioSource.PlayClipAtPoint(popSounds[Random.Range(0, popSounds.Length)], transform.position);
        gameObject.SetActive(false);
    }
}
