using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 必要であれば残す

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // 体力バーのフィルImageコンポーネントへの参照はUIManagerが持つべきなので削除
    // public Image healthBarFillImage; // これを削除

    // UIManagerへの参照を追加
    private UIManager uiManager;

    void Awake()
    {
        currentHealth = maxHealth;
        // AwakeではまだUIManagerが初期化されていない可能性があるので、UpdateHealthUIは呼ばない
        // SetUIManagerで呼ばれる
    }

    // UIManagerからの参照を設定するメソッド
    public void SetUIManager(UIManager manager)
    {
        uiManager = manager;
        UpdateHealthUI(); // UIManagerが設定されたら初回更新
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
        // UIの更新をUIManagerに委譲
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

    public void ApplyInitialStatBonus(int strength, int dexterity, int intelligence, int vitality)
    {
        Debug.Log("初期ステータスボーナス適用: 筋力+" + strength + ", 器用さ+" + dexterity + ", 知力+" + intelligence + ", 生命力+" + vitality);
        maxHealth += vitality;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
}