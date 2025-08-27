using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAppearancePart", menuName = "Game Data/Player Appearance Part")]
public class PlayerAppearanceData : ScriptableObject
{
    public string partName; // パーツ名（例: ポニーテール、ショートカット）
    public Sprite partSprite; // このパーツのスプライト
    public Color partColor = Color.white; // パーツの色（後で変更可能）
}