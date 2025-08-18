using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private UIManager uiManager;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SetUIManager(UIManager manager)
    {
        uiManager = manager;
        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        Debug.Log("�v���C���[�� " + damageAmount + " �_���[�W���󂯂܂����B���݂̗̑�: " + currentHealth);
        UpdateHealthUI();
        if (currentHealth <= 0) Die();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        Debug.Log("�v���C���[�� " + healAmount + " �񕜂��܂����B���݂̗̑�: " + currentHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        Debug.Log("�v���C���[�͎��S���܂����I�Q�[���I�[�o�[�I");
        if (GameManager.instance != null)
        {
            GameManager.instance.HandleGameOver();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        gameObject.SetActive(false);
    }
}