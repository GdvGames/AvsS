using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlimeScript : MonoBehaviour
{
    public Minion minion;

    private Rigidbody2D rigidBody;
    private float speed = 400f;
    Vector3 lastVelocity;

    private float forceX, forceY;
    private PlayerScript playerScript;

    private string slimeName;
    private int damage;
    private int exp;
    private int points;
    private int slimeHealth;

    private Color slimeColor;

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
        
    }

    private void FixedUpdate()
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
        slime1Script.minion = minion;
        slime1Script.SetSlimes();

        slime2Script = slime2.GetComponent<SlimeScript>();
        slime2Script.minion = minion;
        slime2Script.SetSlimes();
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
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rigidBody.velocity = direction * Mathf.Max(speed, 0.2f);
        
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
                //gameObject.SetActive(false);
                Destroy(gameObject);
            }
            playerScript.arrowHitsBall = true;
            playerScript.amountOfExp = exp;
        }
    }

    void SetSlimes()
    {
        forceX = 20f;
        popDistance = new Vector3(0f, 0f);

        switch (this.gameObject.tag)
        {
            case "Largest Ball":
                forceY = 20f;
                if(minion != null)
                {

                }
                slimeName = "Largest " + minion.minionName;
                slimeHealth = minion.health;
                damage = minion.minionDamage;
                exp = minion.givenExp;
                points = minion.givenPoints;

                slimeColor = minion.minionColor;
                this.GetComponent<SpriteRenderer>().color = slimeColor;
                this.GetComponent<SpriteRenderer>().sprite = minion.demonSprite;

                //popDistance = new Vector3(.6f, 0);
                break;

            case "Large Ball":
                forceY = 15f;
                if (minion != null)
                {
                    slimeName = "Large " + minion.minionName;
                    slimeHealth = Mathf.RoundToInt(minion.health - ((minion.health * 10) / 100));
                    damage = Mathf.RoundToInt(minion.minionDamage - ((minion.minionDamage * 10) / 100));
                    exp = Mathf.RoundToInt(minion.givenExp + ((minion.givenExp * 10) / 100)); ;
                    points = Mathf.RoundToInt(minion.givenPoints + ((minion.givenPoints * 10) / 100)); ;

                    slimeColor = minion.minionColor;
                    this.GetComponent<SpriteRenderer>().color = slimeColor;
                    this.GetComponent<SpriteRenderer>().sprite = minion.demonSprite;
                }
                //popDistance = new Vector3(.45f, 0);
                break;

            case "Medium Ball":
                forceY = 10f;
                if (minion != null)
                {
                    slimeName = "Medium " + minion.minionName;
                    slimeHealth = Mathf.RoundToInt(minion.health - ((minion.health * 20) / 100));
                    damage = Mathf.RoundToInt(minion.minionDamage - ((minion.minionDamage * 20) / 100));
                    exp = Mathf.RoundToInt(minion.givenExp + ((minion.givenExp * 20) / 100)); ;
                    points = Mathf.RoundToInt(minion.givenPoints + ((minion.givenPoints * 20) / 100)); ;

                    slimeColor = minion.minionColor;
                    this.GetComponent<SpriteRenderer>().color = slimeColor;
                    this.GetComponent<SpriteRenderer>().sprite = minion.demonSprite;
                }
                //popDistance = new Vector3(.3f, 0);
                break;

            case "Small Ball":
                forceY = 5f;
                if (minion != null)
                {
                    slimeName = "Small " + minion.minionName;
                    slimeHealth = Mathf.RoundToInt(minion.health - ((minion.health * 30) / 100));
                    damage = Mathf.RoundToInt(minion.minionDamage - ((minion.minionDamage * 30) / 100));
                    exp = Mathf.RoundToInt(minion.givenExp + ((minion.givenExp * 30) / 100)); ;
                    points = Mathf.RoundToInt(minion.givenPoints + ((minion.givenPoints * 30) / 100)); ;

                    slimeColor = minion.minionColor;
                    this.GetComponent<SpriteRenderer>().color = slimeColor;
                    this.GetComponent<SpriteRenderer>().sprite = minion.demonSprite;
                }
                //popDistance = new Vector3(.2f, 0);
                break;

            case "Smallest Ball":
                forceY = 3f;
                if (minion != null)
                {
                    slimeName = "Smallest " + minion.minionName;
                    slimeHealth = Mathf.RoundToInt(minion.health - ((minion.health * 40) / 100));
                    damage = Mathf.RoundToInt(minion.minionDamage - ((minion.minionDamage * 40) / 100));
                    exp = Mathf.RoundToInt(minion.givenExp + ((minion.givenExp * 40) / 100)); ;
                    points = Mathf.RoundToInt(minion.givenPoints + ((minion.givenPoints * 40) / 100)); ;

                    slimeColor = minion.minionColor;
                    this.GetComponent<SpriteRenderer>().color = slimeColor;
                    this.GetComponent<SpriteRenderer>().sprite = minion.demonSprite;
                }
                //popDistance = new Vector3(.1f, 0);
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
