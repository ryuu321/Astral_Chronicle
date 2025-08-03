using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("���݂̐E��")]
    public VocationData currentVocation; // ���݂̐E�Ƃ�ێ� (Inspector�Ŋm�F�p)

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

    // �����ɂ�鏉���{�[�i�X��K�p���郁�\�b�h
    public void ApplyInitialStatBonus(int strengthBonus, int dexterityBonus, int intelligenceBonus, int vitalityBonus)
    {
        strength += strengthBonus;
        dexterity += dexterityBonus;
        intelligence += intelligenceBonus;
        vitality += vitalityBonus;

        Debug.Log("�����X�e�[�^�X�{�[�i�X�K�p: �ؗ�+" + strengthBonus + ", ��p��+" + dexterityBonus + ", �m��+" + intelligenceBonus + ", ������+" + vitalityBonus);

        // �{�[�i�X�K�p��ɔh���X�e�[�^�X�ƍő�̗͂��Čv�Z
        CalculateDerivedStats();
    }

    // �E�Ɛ����ɂ�鏉���{�[�i�X��K�p���郁�\�b�h
    public void ApplyVocationBonus(int strengthBonus, int dexterityBonus, int intelligenceBonus, int vitalityBonus)
    {
        this.strength += strengthBonus;
        this.dexterity += dexterityBonus;
        this.intelligence += intelligenceBonus;
        this.vitality += vitalityBonus;

        // �ő�̗͂ɂ������̓{�[�i�X�𔽉f
        // GameManager.instance.currentPlayerHealth �𒼐ڎQ�Ƃ���̂���߂�
        if (playerHealth != null) // PlayerStatus���g��playerHealth�Q�Ƃ��g�p
        {
            playerHealth.maxHealth += vitalityBonus * 5; // ��: ������1�ɂ��ő�̗�5����
            playerHealth.Heal(vitalityBonus * 5); // ���������񕜁iHeal�ōő�l�𒴂��Ȃ��悤�ɐ��䂳���j
        }
        else
        {
            Debug.LogWarning("PlayerHealth component not found for PlayerStatus! Cannot update max health from vocation bonus.");
        }

        Debug.Log("�E�ƃ{�[�i�X�K�p��̃X�e�[�^�X: �ؗ�:" + this.strength + ", ��p��:" + this.dexterity +
                  ", �m��:" + this.intelligence + ", ������:" + this.vitality);
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
