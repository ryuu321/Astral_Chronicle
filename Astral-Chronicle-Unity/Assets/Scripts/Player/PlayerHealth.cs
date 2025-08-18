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
        Debug.Log("プレイヤーが " + damageAmount + " ダメージを受けました。現在の体力: " + currentHealth);
        UpdateHealthUI();
        if (currentHealth <= 0) Die();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        Debug.Log("プレイヤーが " + healAmount + " 回復しました。現在の体力: " + currentHealth);
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
        Debug.Log("プレイヤーは死亡しました！ゲームオーバー！");
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