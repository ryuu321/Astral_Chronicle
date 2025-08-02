using UnityEngine;
using System.Collections.Generic; // List<T> ���g������

[CreateAssetMenu(fileName = "NewConstellation", menuName = "Game Data/Constellation Data")]
public class ConstellationData : ScriptableObject
{
    public string constellationName;      // �����̖��O (��: ���r��)
    public string description;            // �����̐�����
    public Sprite icon;                   // �����̃A�C�R���摜

    [Header("�����\�͒l�o�t")]
    public int initialStrengthBonus = 0;   // �����ؗ̓{�[�i�X
    public int initialDexterityBonus = 0;  // ������p���{�[�i�X
    public int initialIntelligenceBonus = 0; // �����m�̓{�[�i�X
    public int initialVitalityBonus = 0;   // ���������̓{�[�i�X

    [Header("�J������ (�B�������p)")]
    public bool isHiddenConstellation = false; // �B���������ǂ���
    // TODO: �����I�ɁA��̓I�ȊJ�������i��: List<ConditionData> unlockConditions�j��ǉ�
    // ����̓t���O�݂̂ŁA���W�b�N��GameManager���ŊȈՓI�ɔ���

    [Header("���� (�I�v�V����: �����I�Ȋg��)")]
    public List<ConstellationData> friendlyConstellations; // �F�D�I�Ȑ���
    public List<ConstellationData> hostileConstellations;  // �G�ΓI�Ȑ���
    // �����ɉ������o�t/�f�o�t�̔{���Ȃǂ���`�\
}