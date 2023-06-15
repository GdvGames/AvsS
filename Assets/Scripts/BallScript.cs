using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private float forceX, forceY;

    private Rigidbody2D myBody;

    [SerializeField]
    private bool moveLeft, moveRight;

    [SerializeField]
    private GameObject originalBall;

    private GameObject ball1, ball2;
    private BallScript ball1Script, ball2Script;

    [SerializeField]
    private AudioClip[] popSounds;

    [SerializeField]
    private float maxSpeed = 5f;

    [SerializeField]
    private float speed;
    private int damage;
    private int exp;

    public PlayerScript playerScript;

    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        SetBallSpeed();
    }

    private void Start()
    {
        MoveBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        speed = myBody.velocity.magnitude;
    }

    void InstantiateBalls()
    {
        ball1 = Instantiate(originalBall);
        ball2 = Instantiate(originalBall);

        ball1.name = originalBall.name;
        ball2.name = originalBall.name;

        ball1Script = ball1.GetComponent<BallScript>();
        ball2Script = ball2.GetComponent<BallScript>();
    }

    void InitializeBallsAndTurnOffCurrentBall()
    {
        InstantiateBalls();

        Vector3 temp = transform.position;

        ball1.transform.position = temp;
        ball1Script.SetMoveLeft(true);

        ball2.transform.position = temp;
        ball2Script.SetMoveRight(true);

        ball1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1.5f);
        ball2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1.5f);

        AudioSource.PlayClipAtPoint(popSounds[Random.Range(0, popSounds.Length)], transform.position);
        gameObject.SetActive(false);
    }

    public void SetMoveLeft(bool canMoveLeft)
    {
        this.moveLeft = canMoveLeft;
        this.moveRight = !canMoveLeft;
    }

    public void SetMoveRight(bool canMoveRight)
    {
        this.moveRight= canMoveRight;
        this.moveLeft = !canMoveRight;
    }

    void MoveBall()
    {
        if (moveLeft)
        {
            myBody.velocity = new Vector2(-forceX, forceY);
        }

        if (moveRight)
        {
            myBody.velocity = new Vector2(forceX, forceY);
        }
    }

    void SetBallSpeed()
    {
        forceX = 2.5f;

        switch (this.gameObject.tag)
        {
            case "Largest Ball":
                forceY = 5f;
                damage = 20;
                exp = 5;
                break;

            case "Large Ball":
                forceY = 4.5f;
                damage = 15;
                exp = 10;
                break;

            case "Medium Ball":
                forceY = 4f;
                damage = 10;
                exp = 15;
                break;

            case "Small Ball":
                forceY = 3f;
                damage = 5;
                exp = 18;
                break;

            case "Smallest Ball":
                forceY = 2.5f;
                damage = 3;
                exp = 20;
                break;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (speed < maxSpeed)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                Vector2 fNormal = collision.contacts[0].normal;
                Vector2 new_dir = Vector2.Reflect(myBody.velocity, fNormal).normalized;

                myBody.velocity = new_dir * speed;
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 fNormal = collision.contacts[0].normal;
                Vector2 new_dir = Vector2.Reflect(myBody.velocity, fNormal).normalized;

                myBody.velocity = new_dir * speed;

                playerScript.playerHitByBall = true;
                playerScript.amountOfDamage = damage;
            }

            if (collision.gameObject.CompareTag("Arrow"))
            {
                if (!gameObject.CompareTag("Smallest Ball"))
                {
                    InitializeBallsAndTurnOffCurrentBall();
                }
                else
                {
                    AudioSource.PlayClipAtPoint(popSounds[Random.Range(0, popSounds.Length)], transform.position);
                    gameObject.SetActive(false);
                }
                playerScript.arrowHitsBall = true;
                playerScript.amountOfExp = exp;
            }
        }
    }
}
