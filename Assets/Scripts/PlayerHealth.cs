
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public TextMeshProUGUI healthText;

    private float currentHealth;
    private float previousHealth;

    void Start()
    {
        currentHealth = maxHealth;
        previousHealth = currentHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0f)
            currentHealth = 0f;

        if (currentHealth != previousHealth)
            UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }

        previousHealth = currentHealth;
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + Mathf.CeilToInt(currentHealth);
    }

    void Die()
    {
        Debug.Log("Player Died!");
        GameManager.Instance.GameOver(); // Call Game Over screen
    }
}
