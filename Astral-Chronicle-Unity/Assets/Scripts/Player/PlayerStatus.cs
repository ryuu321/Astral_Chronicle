using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    public ConstellationData selectedConstellation { get; private set; }
    public VocationData currentVocation { get; private set; }

    // 基本能力値
    public int strength;
    public int dexterity;
    public int intelligence;
    public int vitality;

    [Header("行動パラメータ")]
    public int combatExperience = 0;
    public int gatheringExperience = 0;
    public int craftingExperience = 0;
    public int explorationExperience = 0;
    public int socialExperience = 0;

    // レベルと経験値 (将来的に追加)
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    // 派生ステータス (計算で求まる)
    public int attackPower;
    public int defense;
    public int magicPower;

    public PlayerHealth playerHealth;
    private StatusUIController statusUIController;
    void Awake()
    {
        strength = 10;
        dexterity = 10;
        intelligence = 10;
        vitality = 10;

        playerHealth = GetComponent<PlayerHealth>();
        CalculateDerivedStats();
    }

    void CalculateDerivedStats()
    {
        attackPower = strength * 2;
        defense = vitality * 1;
        magicPower = intelligence * 2;

        if (playerHealth != null)
        {
            playerHealth.maxHealth = vitality * 10;
            playerHealth.currentHealth = playerHealth.maxHealth;
            playerHealth.UpdateHealthUI();
        }
    }

    // --- ここからが整理された部分 ---

    // プレイヤーに星座データをセットするメソッド
    public void SetConstellation(ConstellationData newConstellation)
    {
        selectedConstellation = newConstellation;
        Debug.Log(selectedConstellation.constellationName + "が選択されました！");

        // 星座ボーナスを適用
        ApplyStatusBonus(
            selectedConstellation.initialStrengthBonus,
            selectedConstellation.initialDexterityBonus,
            selectedConstellation.initialIntelligenceBonus,
            selectedConstellation.initialVitalityBonus
        );
    }

    // プレイヤーに職業データをセットするメソッド
    public void SetVocation(VocationData newVocation)
    {
        currentVocation = newVocation;
        Debug.Log(currentVocation.vocationName + "が選択されました！");

        // 職業ボーナスを適用
        ApplyStatusBonus(
            currentVocation.strengthBonus,
            currentVocation.dexterityBonus,
            currentVocation.intelligenceBonus,
            currentVocation.vitalityBonus
        );

        // 星座相性ボーナスを適用
        if (newVocation.constellationSynergyBonus != null)
        {
            foreach (var synergy in newVocation.constellationSynergyBonus)
            {
                if (synergy.constellation == selectedConstellation)
                {
                    ApplyStatusBonus(
                        synergy.bonusStrength,
                        synergy.bonusDexterity,
                        synergy.bonusIntelligence,
                        synergy.bonusVitality
                    );
                    Debug.Log($"星座相性ボーナス適用！{synergy.constellation.constellationName}との相性ボーナスを獲得しました。");
                    break;
                }
            }
        }
    }

    // 汎用的なステータス加算メソッド
    private void ApplyStatusBonus(int strengthBonus, int dexterityBonus, int intelligenceBonus, int vitalityBonus)
    {
        strength += strengthBonus;
        dexterity += dexterityBonus;
        intelligence += intelligenceBonus;
        vitality += vitalityBonus;

        // ボーナス適用後に派生ステータスと最大体力を再計算
        CalculateDerivedStats();
    }
}