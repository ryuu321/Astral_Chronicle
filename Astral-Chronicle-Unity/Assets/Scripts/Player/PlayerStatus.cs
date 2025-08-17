using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    
    public ConstellationData selectedConstellation { get; private set; }


    public VocationData currentVocation { get; private set; } // 現在の職業を保持 (Inspectorで確認用)

    // 基本能力値
    public int strength;    // 筋力
    public int dexterity;   // 器用さ
    public int intelligence; // 知力
    public int vitality;    // 生命力

    [Header("行動パラメータ")]
    public int combatExperience = 0;    // 戦闘経験（敵を倒す、戦闘イベントに参加など）
    public int gatheringExperience = 0; // 採集経験（素材を採集など）
    public int craftingExperience = 0;  // 生産経験（アイテムをクラフトなど）
    public int explorationExperience = 0; // 探索経験（隠しエリア発見、NPCとの会話など）
    public int socialExperience = 0;    // 社会経験/交流度（NPCとの会話、クエスト完了など）

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

    // PlayerStatus.cs に以下のメソッドを追加

    public void SetConstellation(ConstellationData newConstellation)
    {
        selectedConstellation = newConstellation;
        Debug.Log(selectedConstellation.constellationName + "が選択されました！");
        ApplyConstellationBuff(selectedConstellation); // ここを修正
    }

    // 選択された星座のバフをプレイヤーに適用するメソッド
    void ApplyConstellationBuff(ConstellationData constellation)
    {
                    Debug.Log("星座ボーナス適用: " + constellation.constellationName +
                      " 筋力+" + constellation.initialStrengthBonus + ", 器用さ+" + constellation.initialDexterityBonus +
                      ", 知力+" + constellation.initialIntelligenceBonus + ", 生命力+" + constellation.initialVitalityBonus);

            ApplyStatusBonus(
                constellation.initialStrengthBonus,
                constellation.initialDexterityBonus,
                constellation.initialIntelligenceBonus,
                constellation.initialVitalityBonus
            );
    }

    // 星座による初期ボーナスを適用するメソッド
    public void ApplyStatusBonus(int strengthBonus, int dexterityBonus, int intelligenceBonus, int vitalityBonus)
    {
        strength += strengthBonus;
        dexterity += dexterityBonus;
        intelligence += intelligenceBonus;
        vitality += vitalityBonus;

        // ボーナス適用後に派生ステータスと最大体力を再計算
        CalculateDerivedStats();
    }

    public void SetVocation(VocationData newVocation)
    {
        currentVocation = newVocation;
        Debug.Log(currentVocation.vocationName + "が選択されました！");
        ApplyVocationBuff(currentVocation);
    }

    // 選択された職業のボーナスをプレイヤーに適用するメソッド
    void ApplyVocationBuff(VocationData vocation)
    {
        ApplyStatusBonus(
            vocation.strengthBonus,
            vocation.dexterityBonus,
            vocation.intelligenceBonus,
            vocation.vitalityBonus
        );

        Debug.Log("職業ボーナス適用: " + vocation.vocationName +
                  " 筋力+" + vocation.strengthBonus + ", 器用さ+" + vocation.dexterityBonus +
                  ", 知力+" + vocation.intelligenceBonus + ", 生命力+" + vocation.vitalityBonus);

        foreach (var synergy in vocation.constellationSynergyBonus)
        {
            // プレイヤーの星座が、相性ボーナスの対象星座と一致するかチェック
            if (synergy.constellation == selectedConstellation)
            {
                ApplyStatusBonus(
                    synergy.bonusStrength,
                    synergy.bonusDexterity,
                    synergy.bonusIntelligence,
                    synergy.bonusVitality
                );

                Debug.Log($"星座相性ボーナス適用！{synergy.constellation.constellationName}との相性ボーナスを獲得しました。");
                // ボーナスは一つだけ適用される想定
                Debug.Log("相性ボーナス適用: " + vocation.vocationName +
                 " 筋力+" + synergy.bonusStrength + ", 器用さ+" + synergy.bonusDexterity +
                 ", 知力+" + synergy.bonusIntelligence + ", 生命力+" + synergy.bonusVitality);

                break;
            }
        }
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
