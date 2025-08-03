using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30; // 敵の最大体力 (例: スライムなら30)
    public int currentHealth;   // 敵の現在の体力
    // スコア機能は現在不要なので、scoreOnDefeat は削除済み

    void Awake()
    {
        currentHealth = maxHealth; // ゲーム開始時に体力を最大に設定
    }

    // ダメージを受ける処理
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // 体力を減らす
        currentHealth = Mathf.Max(0, currentHealth); // 体力が0未満にならないようにする

        Debug.Log(gameObject.name + "が " + damageAmount + " ダメージを受けました。現在の体力: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // 体力が0以下になったら死亡処理
        }
    }

    // 敵が死亡したときの処理
    void Die()
    {
        Debug.Log(gameObject.name + "は倒されました！");

        // 戦闘経験の加算（PlayerStatusに反映）
        // GameManagerが存在し、かつプレイヤーのPlayerStatusが取得できていれば、経験値を加算
        if (GameManager.instance != null && GameManager.instance.currentPlayerStatus != null)
        {
            GameManager.instance.currentPlayerStatus.combatExperience += 5; // 例として5の戦闘経験
            Debug.Log("戦闘経験を5獲得しました。現在の戦闘経験: " + GameManager.instance.currentPlayerStatus.combatExperience);
            // 必要に応じて、UI更新などもGameManager経由で指示
        }

        // 敵オブジェクトをシーンから削除する
        Destroy(gameObject);
    }
}
