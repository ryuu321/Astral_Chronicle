using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAppearancePart", menuName = "Game Data/Player Appearance Part")]
public class PlayerAppearanceData : ScriptableObject
{
    public string partName; // �p�[�c���i��: �|�j�[�e�[���A�V���[�g�J�b�g�j
    public Sprite partSprite; // ���̃p�[�c�̃X�v���C�g
    public Color partColor = Color.white; // �p�[�c�̐F�i��ŕύX�\�j
}