using UnityEngine;

// ScriptableObject�Ƃ��ăA�Z�b�g���쐬�ł���悤�ɂ���
[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    // ��{���
    public string characterName;
    public Sprite characterSprite;
    public int age;
    public int hp;
    

    // �X�e�[�^�X�iPlayerStatus���番���j
    public int baseStrength;
    public int baseDexterity;
    public int baseIntelligence;
    public int baseVitality;

    // �����ڏ��iPlayerAppearanceData���番���j
    public PlayerAppearanceData hairStyle;
    public PlayerAppearanceData face;
    public PlayerAppearanceData body;
    // ���̃p�[�c�������ɒǉ�

    // �E�Ɓi�����ł�VocationData�ւ̎Q�Ƃ���������j
    public VocationData initialVocation;
    public ConstellationData initialConstellation; // �I������������
}