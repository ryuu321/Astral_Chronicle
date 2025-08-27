using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Character/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [Header("NPC�f�[�^")]
    public List<CharacterData> allNPCs;

    [Header("�����X�^�[�f�[�^")]
    public List<CharacterData> allMonsters;

    // ���̃L�����N�^�[�^�C�v�������ɒǉ��\
}