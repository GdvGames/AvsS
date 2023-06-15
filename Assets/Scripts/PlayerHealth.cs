using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;

    public Image frontHealthBar;
    public Image backHealthBar;

    public TextMeshProUGUI healthText;

    public PlayerScript player;

    void Start()
    {
        health = maxHealth;
    }

    
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(Random.Range(5,10));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if(fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB,hFraction,percentComplete);
        }

        if(fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF,backHealthBar.fillAmount,percentComplete);
        }
        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (player.canTakeDamage)
        {
            health -= damage;
            lerpTimer = 0;
            //player.GotDamaged();
        }
        
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0;
    }

    public void IncreaseHealth(int level)
    {
        maxHealth += (health * 0.01f) * ((100 - level) * 0.1f);
        health = maxHealth;
    }
}
