using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAppearanceDatabase", menuName = "Game Data/Player Appearance Database")]
public class PlayerAppearanceDatabase : ScriptableObject
{
    [Header("プレイヤーの外見パーツ")]
    public List<PlayerAppearanceData> hairStyles;   // 髪型
    public List<PlayerAppearanceData> faces;        // 顔
    public List<PlayerAppearanceData> bodies;       // 体
    public List<PlayerAppearanceData> accessories;  // アクセサリー
    // 他のパーツもここに追加
}