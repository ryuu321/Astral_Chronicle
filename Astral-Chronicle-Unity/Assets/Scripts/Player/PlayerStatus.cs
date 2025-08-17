using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    
    public ConstellationData selectedConstellation { get; private set; }


    public VocationData currentVocation { get; private set; } // ���݂̐E�Ƃ�ێ� (Inspector�Ŋm�F�p)

    // ��{�\�͒l
    public int strength;    // �ؗ�
    public int dexterity;   // ��p��
    public int intelligence; // �m��
    public int vitality;    // ������

    [Header("�s���p�����[�^")]
    public int combatExperience = 0;    // �퓬�o���i�G��|���A�퓬�C�x���g�ɎQ���Ȃǁj
    public int gatheringExperience = 0; // �̏W�o���i�f�ނ��̏W�Ȃǁj
    public int craftingExperience = 0;  // ���Y�o���i�A�C�e�����N���t�g�Ȃǁj
    public int explorationExperience = 0; // �T���o���i�B���G���A�����ANPC�Ƃ̉�b�Ȃǁj
    public int socialExperience = 0;    // �Љ�o��/�𗬓x�iNPC�Ƃ̉�b�A�N�G�X�g�����Ȃǁj

    // ���x���ƌo���l (�����I�ɒǉ�)
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    // �h���X�e�[�^�X (�v�Z�ŋ��܂�)
    public int attackPower;
    public int defense;
    public int magicPower;

    // PlayerHealth�ւ̎Q�� (�̗͂̍ő�l���X�V���邽��)
    private PlayerHealth playerHealth;

    void Awake()
    {
        // �����X�e�[�^�X��ݒ� (�e�X�g�p)
        strength = 10;
        dexterity = 10;
        intelligence = 10;
        vitality = 10;

        // PlayerHealth�R���|�[�l���g���擾
        playerHealth = GetComponent<PlayerHealth>();

        // �h���X�e�[�^�X���v�Z
        CalculateDerivedStats();
    }

    // ��{�\�͒l����h���X�e�[�^�X���v�Z���郁�\�b�h
    void CalculateDerivedStats()
    {
        attackPower = strength * 2; // ��: �ؗ͂�2�{���U����
        defense = vitality * 1;     // ��: �����͂Ɠ����l���h���
        magicPower = intelligence * 2; // ��: �m�͂�2�{�����@�U����

        // PlayerHealth�̍ő�̗͂��X�V����
        if (playerHealth != null)
        {
            // �����͂��ő�̗͂ɉe������ꍇ
            playerHealth.maxHealth = vitality * 10; // ��: �����͂�10�{���ő�̗�
            playerHealth.currentHealth = playerHealth.maxHealth; // �̗͂��ő�l�Ƀ��Z�b�g
            playerHealth.UpdateHealthUI(); // UI���X�V
        }
    }

    // PlayerStatus.cs �Ɉȉ��̃��\�b�h��ǉ�

    public void SetConstellation(ConstellationData newConstellation)
    {
        selectedConstellation = newConstellation;
        Debug.Log(selectedConstellation.constellationName + "���I������܂����I");
        ApplyConstellationBuff(selectedConstellation); // �������C��
    }

    // �I�����ꂽ�����̃o�t���v���C���[�ɓK�p���郁�\�b�h
    void ApplyConstellationBuff(ConstellationData constellation)
    {
                    Debug.Log("�����{�[�i�X�K�p: " + constellation.constellationName +
                      " �ؗ�+" + constellation.initialStrengthBonus + ", ��p��+" + constellation.initialDexterityBonus +
                      ", �m��+" + constellation.initialIntelligenceBonus + ", ������+" + constellation.initialVitalityBonus);

            ApplyStatusBonus(
                constellation.initialStrengthBonus,
                constellation.initialDexterityBonus,
                constellation.initialIntelligenceBonus,
                constellation.initialVitalityBonus
            );
    }

    // �����ɂ�鏉���{�[�i�X��K�p���郁�\�b�h
    public void ApplyStatusBonus(int strengthBonus, int dexterityBonus, int intelligenceBonus, int vitalityBonus)
    {
        strength += strengthBonus;
        dexterity += dexterityBonus;
        intelligence += intelligenceBonus;
        vitality += vitalityBonus;

        // �{�[�i�X�K�p��ɔh���X�e�[�^�X�ƍő�̗͂��Čv�Z
        CalculateDerivedStats();
    }

    public void SetVocation(VocationData newVocation)
    {
        currentVocation = newVocation;
        Debug.Log(currentVocation.vocationName + "���I������܂����I");
        ApplyVocationBuff(currentVocation);
    }

    // �I�����ꂽ�E�Ƃ̃{�[�i�X���v���C���[�ɓK�p���郁�\�b�h
    void ApplyVocationBuff(VocationData vocation)
    {
        ApplyStatusBonus(
            vocation.strengthBonus,
            vocation.dexterityBonus,
            vocation.intelligenceBonus,
            vocation.vitalityBonus
        );

        Debug.Log("�E�ƃ{�[�i�X�K�p: " + vocation.vocationName +
                  " �ؗ�+" + vocation.strengthBonus + ", ��p��+" + vocation.dexterityBonus +
                  ", �m��+" + vocation.intelligenceBonus + ", ������+" + vocation.vitalityBonus);

        foreach (var synergy in vocation.constellationSynergyBonus)
        {
            // �v���C���[�̐������A�����{�[�i�X�̑Ώې����ƈ�v���邩�`�F�b�N
            if (synergy.constellation == selectedConstellation)
            {
                ApplyStatusBonus(
                    synergy.bonusStrength,
                    synergy.bonusDexterity,
                    synergy.bonusIntelligence,
                    synergy.bonusVitality
                );

                Debug.Log($"���������{�[�i�X�K�p�I{synergy.constellation.constellationName}�Ƃ̑����{�[�i�X���l�����܂����B");
                // �{�[�i�X�͈�����K�p�����z��
                Debug.Log("�����{�[�i�X�K�p: " + vocation.vocationName +
                 " �ؗ�+" + synergy.bonusStrength + ", ��p��+" + synergy.bonusDexterity +
                 ", �m��+" + synergy.bonusIntelligence + ", ������+" + synergy.bonusVitality);

                break;
            }
        }
    }

    // ���x���A�b�v���� (�����I�ɒǉ�)
    public void LevelUp()
    {
        level++;
        // �X�e�[�^�X���㏸�����郍�W�b�N
        // ExpToNextLevel���X�V���郍�W�b�N
        CalculateDerivedStats(); // �Čv�Z
        Debug.Log("���x���A�b�v�I���݂̃��x��: " + level);
    }
}
