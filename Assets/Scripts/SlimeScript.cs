using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{

    private Rigidbody2D rigidBody;
    private float speed = 400f;
    Vector3 lastVelocity;

    private float forceX, forceY;
    private PlayerScript playerScript;

    private int damage;
    private int exp;
    private Vector3 popDistance;

    [SerializeField]
    private bool moveLeft, moveRight;

    [SerializeField]
    private GameObject originalSlime;

    private GameObject slime1, slime2;
    private SlimeScript slime1Script, slime2Script;

    [SerializeField]
    private AudioClip[] popSounds;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        SetSlimes();
    }

    private void Start()
    {
        MoveSlimes();
    }

    private void Update()
    {
        lastVelocity = rigidBody.velocity;
    }

    void InstantiateSlimes()
    {
        slime1 = Instantiate(originalSlime);
        slime2 = Instantiate(originalSlime);

        slime1.name = originalSlime.name;
        slime2.name = originalSlime.name;

        slime1Script = slime1.GetComponent<SlimeScript>();
        slime2Script = slime2.GetComponent<SlimeScript>();
    }

    void InitializeSlimeAndTurnOffCurrent()
    {
        InstantiateSlimes();

        Vector3 temp = transform.position;

        slime1.transform.position = temp - popDistance;
        slime1Script.SetMoveLeft(true);

        slime2.transform.position = temp + popDistance;
        slime2Script.SetMoveRight(true);

        AudioSource.PlayClipAtPoint(popSounds[Random.Range(0, popSounds.Length)], transform.position);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rigidBody.velocity = direction * Mathf.Max(speed, 0f);

        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.playerHitByBall = true;
            playerScript.amountOfDamage = damage;
        }

        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (!gameObject.CompareTag("Smallest Ball"))
            {
                InitializeSlimeAndTurnOffCurrent();
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

    void SetSlimes()
    {
        forceX = 20f;

        switch (this.gameObject.tag)
        {
            case "Largest Ball":
                forceY = 20f;
                damage = 20;
                exp = 5;
                popDistance = new Vector3(.6f, 0);
                break;

            case "Large Ball":
                forceY = 15f;
                damage = 15;
                exp = 10;
                popDistance = new Vector3(.45f, 0);
                break;

            case "Medium Ball":
                forceY = 10f;
                damage = 10;
                exp = 15;
                popDistance = new Vector3(.3f, 0);
                break;

            case "Small Ball":
                forceY = 5f;
                damage = 5;
                exp = 18;
                popDistance = new Vector3(.2f, 0);
                break;

            case "Smallest Ball":
                forceY = 3f;
                damage = 3;
                exp = 20;
                popDistance = new Vector3(.1f, 0);
                break;
        }
    }

    public void SetMoveLeft(bool canMoveLeft)
    {
        this.moveLeft = canMoveLeft;
        this.moveRight = !canMoveLeft;
    }

    public void SetMoveRight(bool canMoveRight)
    {
        this.moveRight = canMoveRight;
        this.moveLeft = !canMoveRight;
    }

    void MoveSlimes()
    {
        if (moveRight)
        {
            rigidBody.AddForce(new Vector2(forceX * Time.deltaTime * speed, forceY * Time.deltaTime * speed));
        }

        if (moveLeft)
        {
            rigidBody.AddForce(new Vector2(-forceX * Time.deltaTime * speed, forceY * Time.deltaTime * speed));
        }
    }
}
