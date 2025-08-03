using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VocationDatabase", menuName = "Game Data/Vocation Database")]
public class VocationDatabase : ScriptableObject
{
    public List<VocationData> allVocations; // 全ての職業データのリスト
}
