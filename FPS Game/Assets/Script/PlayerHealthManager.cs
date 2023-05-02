using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Text healthText;
    public Animator animator;
    public float invulnerabilityDuration = 2f;

    private bool isDamaged;
    private bool isInvulnerable;
    private GameManager gameManager;
    private MonsterBehavior monsterBehavior;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamaged)
        {
            animator.SetBool("Damaged", true);
        }
        else
        {
            animator.SetBool("Damaged", false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            return;
        }

        isDamaged = true;
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthUI();

        // Reset the damaged state and start the invulnerability period
        StartCoroutine(ResetDamagedState());
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth.ToString();
    }

    private IEnumerator ResetDamagedState()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isDamaged = false;
        isInvulnerable = false;
    }
}
