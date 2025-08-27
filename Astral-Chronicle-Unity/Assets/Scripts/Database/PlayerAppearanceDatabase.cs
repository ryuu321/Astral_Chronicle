using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAppearanceDatabase", menuName = "Game Data/Player Appearance Database")]
public class PlayerAppearanceDatabase : ScriptableObject
{
    [Header("�v���C���[�̊O���p�[�c")]
    public List<PlayerAppearanceData> hairStyles;   // ���^
    public List<PlayerAppearanceData> faces;        // ��
    public List<PlayerAppearanceData> bodies;       // ��
    public List<PlayerAppearanceData> accessories;  // �A�N�Z�T���[
    // ���̃p�[�c�������ɒǉ�
}