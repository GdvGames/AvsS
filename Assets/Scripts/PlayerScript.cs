using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject arrow;

    [SerializeField]
    private AudioClip shootSound;

    [SerializeField]
    private float speed = 150f;
    
    private float maxVelocity = 4f;

    private Rigidbody2D myBody;
    private Animator anim;
    private SpriteRenderer sprite;

    private bool canShoot;
    private bool canWalk;
    public bool canTakeDamage = true;

    private int flickerAmount = 4;
    private float flickerDuration = 0.1f;

    public bool arrowHitsBall = false;
    public bool playerHitByBall = false;

    public PlayerHealth playerHealth;
    public LevelSystem levelSystem;

    public int amountOfDamage;
    public int amountOfExp;
    
    // Start is called before the first frame update
    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        InitializeVariables();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();

        if (arrowHitsBall)
        {
            ArrowHitBall();
            arrowHitsBall = false;
        }

        if (playerHitByBall)
        {
            PlayerGotHit();
            playerHitByBall = false;
        }
    }

    private void FixedUpdate()
    {
        PlayerWalk();
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canShoot)
            {
                canShoot = false;
                StartCoroutine(ShootTheArrow());
            }
        }
    }

    IEnumerator ShootTheArrow()
    {
        canWalk = false;
        anim.Play("Shoot");

        Vector3 temp = transform.position;
        temp.y += 1.0f;

        Instantiate(arrow, temp, Quaternion.identity);

        AudioSource.PlayClipAtPoint(shootSound, transform.position);

        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Shoot", false);
        canWalk = true;

        yield return new WaitForSeconds(0.3f);
        canShoot = true;
    }

    void InitializeVariables()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canShoot = true;
        canWalk = true;
    }

    void PlayerWalk()
    {
        var force = 0f;
        var velocity = Mathf.Abs(myBody.velocity.x);

        float h = Input.GetAxis("Horizontal");

        if (canWalk)
        {
            if (h > 0)
            {
                //move to right
                if (velocity < maxVelocity)
                    force = speed;

                Vector3 scale = transform.localScale;
                scale.x = -8; // 10 per via dello scale del player
                transform.localScale = scale;

                anim.SetBool("Walk", true);
            }
            else if (h < 0)
            {
                //move to the left
                if (velocity < maxVelocity)
                    force = -speed;

                Vector3 scale = transform.localScale;
                scale.x = 8;
                transform.localScale = scale;

                anim.SetBool("Walk", true);
            }
            else if (h == 0)
            {
                anim.SetBool("Walk", false);
            }
        }

        myBody.AddForce(new Vector2(force, 0));
    }

    IEnumerator KillThePlayer() // and restart the game
    {
        transform.position = new Vector3(200, 200, 0);

        yield return new WaitForSeconds(1.5f);

        //Application.LoadLevel(Application.loadedLevelName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string[] name = collision.name.Split();

        if(name.Length > 1)
        {
            if(name[1] == "Slime")
            {
                //StartCoroutine(KillThePlayer());
            }
        }
    }

    public void GotDamaged()
    {
        StartCoroutine(DamageFlicker());
    }

    IEnumerator DamageFlicker()
    {
        canTakeDamage = false;
        for (int i = 0; i < flickerAmount; i++)
        {
            sprite.color = new Color(1f,1f, 1f, 0.5f);
            yield return new WaitForSeconds(flickerDuration);
            sprite.color = Color.white;
            yield return new WaitForSeconds(flickerDuration);
            canTakeDamage = true;
        }
    }

    private void ArrowHitBall()
    {
        levelSystem.GainExperienceScalable(amountOfExp, levelSystem.level);
    }

    private void PlayerGotHit()
    {
        playerHealth.TakeDamage(amountOfDamage);
        GotDamaged();
    }
}
