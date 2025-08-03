using UnityEngine;
using System.Collections.Generic;

// �S�Ă�ConstellationData���܂Ƃ߂�f�[�^�x�[�X
[CreateAssetMenu(fileName = "ConstellationDatabase", menuName = "Game Data/Constellation Database")]
public class ConstellationDatabase : ScriptableObject
{
    [Header("�����\�񐯍�")]
    public List<ConstellationData> zodiacConstellations; // �����\�񐯍��̃��X�g

    [Header("�B������")]
    public List<ConstellationData> hiddenConstellations; // �B�������̃��X�g

    // �S�Ă̐����f�[�^�𓝍����ĕԂ��v���p�e�B (UI�����Ȃǂɕ֗�)
    public List<ConstellationData> GetAllConstellations()
    {
        List<ConstellationData> all = new List<ConstellationData>();
        if (zodiacConstellations != null) all.AddRange(zodiacConstellations);
        if (hiddenConstellations != null) all.AddRange(hiddenConstellations);
        return all;
    }
}