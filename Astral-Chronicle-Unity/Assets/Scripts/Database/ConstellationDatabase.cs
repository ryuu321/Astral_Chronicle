using UnityEngine;
using System.Collections.Generic;

// 全てのConstellationDataをまとめるデータベース
[CreateAssetMenu(fileName = "ConstellationDatabase", menuName = "Game Data/Constellation Database")]
public class ConstellationDatabase : ScriptableObject
{
    [Header("黄道十二星座")]
    public List<ConstellationData> zodiacConstellations; // 黄道十二星座のリスト

    [Header("隠し星座")]
    public List<ConstellationData> hiddenConstellations; // 隠し星座のリスト

    // 全ての星座データを統合して返すプロパティ (UI生成などに便利)
    public List<ConstellationData> GetAllConstellations()
    {
        List<ConstellationData> all = new List<ConstellationData>();
        if (zodiacConstellations != null) all.AddRange(zodiacConstellations);
        if (hiddenConstellations != null) all.AddRange(hiddenConstellations);
        return all;
    }
}