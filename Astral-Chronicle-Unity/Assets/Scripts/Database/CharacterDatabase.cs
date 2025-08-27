using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Character/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [Header("NPCデータ")]
    public List<CharacterData> allNPCs;

    [Header("モンスターデータ")]
    public List<CharacterData> allMonsters;

    // 他のキャラクタータイプもここに追加可能
}