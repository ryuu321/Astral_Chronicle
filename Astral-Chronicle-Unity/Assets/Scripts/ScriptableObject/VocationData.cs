using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVocationData", menuName = "Game Data/Vocation Data")]
public class VocationData : ScriptableObject
{
    public string vocationName; // �E�Ɩ� (��: ��m�A���@�g���A���l)
    public Sprite icon;
    [TextArea(3, 5)]
    public string description; // �E�Ƃ̐���

    [Header("�����X�e�[�^�X�{�[�i�X")]
    public int strengthBonus = 0;
    public int dexterityBonus = 0;
    public int intelligenceBonus = 0;
    public int vitalityBonus = 0;

    [Header("�K�v�o���l (�M���̋V�Œ񎦂�������)")]
    public int requiredCombatExperience = 0;
    public int requiredGatheringExperience = 0;
    public int requiredCraftingExperience = 0;
    public int requiredExplorationExperience = 0;
    public int requiredSocialExperience = 0;

    // �E�ƃA�C�R���ȂǁA�ǉ��������f�[�^������΂����ɒǉ�
    [Header("Constellation Synergy Bonus")]
    public List<VocationSynergyBonus> constellationSynergyBonus;

    [System.Serializable]
    public struct VocationSynergyBonus
    {
        public ConstellationData constellation;// �{�[�i�X�Ώۂ̐���
        public int bonusStrength;
        public int bonusDexterity;  // ��p���{�[�i�X
        public int bonusIntelligence; // �m�̓{�[�i�X
        public int bonusVitality;   // �����̓{�[�i�X
    }
}
