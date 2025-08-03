using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVocationData", menuName = "Game Data/Vocation Data")]
public class VocationData : ScriptableObject
{
    public string vocationName; // 職業名 (例: 戦士、魔法使い、商人)
    [TextArea(3, 5)]
    public string description; // 職業の説明

    [Header("初期ステータスボーナス")]
    public int strengthBonus = 0;
    public int dexterityBonus = 0;
    public int intelligenceBonus = 0;
    public int vitalityBonus = 0;

    [Header("必要経験値 (信託の儀で提示される条件)")]
    public int requiredCombatExperience = 0;
    public int requiredGatheringExperience = 0;
    public int requiredCraftingExperience = 0;
    public int requiredExplorationExperience = 0;
    public int requiredSocialExperience = 0;

    // 職業アイコンなど、追加したいデータがあればここに追加
    public Sprite icon;
}
