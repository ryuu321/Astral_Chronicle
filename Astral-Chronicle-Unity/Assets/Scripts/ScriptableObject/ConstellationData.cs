using UnityEngine;
using System.Collections.Generic; // List<T> を使うため

[CreateAssetMenu(fileName = "NewConstellation", menuName = "Game Data/Constellation Data")]
public class ConstellationData : ScriptableObject
{
    public string constellationName;      // 星座の名前 (例: 牡羊座)
    public string description;            // 星座の説明文
    public Sprite icon;                   // 星座のアイコン画像

    [Header("初期能力値バフ")]
    public int initialStrengthBonus = 0;   // 初期筋力ボーナス
    public int initialDexterityBonus = 0;  // 初期器用さボーナス
    public int initialIntelligenceBonus = 0; // 初期知力ボーナス
    public int initialVitalityBonus = 0;   // 初期生命力ボーナス

    [Header("開放条件 (隠し星座用)")]
    public bool isHiddenConstellation = false; // 隠し星座かどうか
    // TODO: 将来的に、具体的な開放条件（例: List<ConditionData> unlockConditions）を追加
    // 現状はフラグのみで、ロジックはGameManager側で簡易的に判定

    [Header("相性 (オプション: 将来的な拡張)")]
    public List<ConstellationData> friendlyConstellations; // 友好的な星座
    public List<ConstellationData> hostileConstellations;  // 敵対的な星座
    // 相性に応じたバフ/デバフの倍率なども定義可能
}