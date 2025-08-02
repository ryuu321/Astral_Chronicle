using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 基本能力値
    public int strength;    // 筋力
    public int dexterity;   // 器用さ
    public int intelligence; // 知力
    public int vitality;    // 生命力

    // レベルと経験値 (将来的に追加)
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    // 派生ステータス (計算で求まる)
    public int attackPower;
    public int defense;
    public int magicPower;

    // PlayerHealthへの参照 (体力の最大値を更新するため)
    private PlayerHealth playerHealth;

    void Awake()
    {
        // 初期ステータスを設定 (テスト用)
        strength = 10;
        dexterity = 10;
        intelligence = 10;
        vitality = 10;

        // PlayerHealthコンポーネントを取得
        playerHealth = GetComponent<PlayerHealth>();

        // 派生ステータスを計算
        CalculateDerivedStats();
    }

    // 基本能力値から派生ステータスを計算するメソッド
    void CalculateDerivedStats()
    {
        attackPower = strength * 2; // 例: 筋力の2倍が攻撃力
        defense = vitality * 1;     // 例: 生命力と同じ値が防御力
        magicPower = intelligence * 2; // 例: 知力の2倍が魔法攻撃力

        // PlayerHealthの最大体力を更新する
        if (playerHealth != null)
        {
            // 生命力が最大体力に影響する場合
            playerHealth.maxHealth = vitality * 10; // 例: 生命力の10倍が最大体力
            playerHealth.currentHealth = playerHealth.maxHealth; // 体力を最大値にリセット
            playerHealth.UpdateHealthUI(); // UIも更新
        }
    }

    // 星座による初期ボーナスを適用するメソッド
    public void ApplyInitialStatBonus(int strengthBonus, int dexterityBonus, int intelligenceBonus, int vitalityBonus)
    {
        strength += strengthBonus;
        dexterity += dexterityBonus;
        intelligence += intelligenceBonus;
        vitality += vitalityBonus;

        Debug.Log("初期ステータスボーナス適用: 筋力+" + strengthBonus + ", 器用さ+" + dexterityBonus + ", 知力+" + intelligenceBonus + ", 生命力+" + vitalityBonus);

        // ボーナス適用後に派生ステータスと最大体力を再計算
        CalculateDerivedStats();
    }

    // レベルアップ処理 (将来的に追加)
    public void LevelUp()
    {
        level++;
        // ステータスを上昇させるロジック
        // ExpToNextLevelを更新するロジック
        CalculateDerivedStats(); // 再計算
        Debug.Log("レベルアップ！現在のレベル: " + level);
    }
}
