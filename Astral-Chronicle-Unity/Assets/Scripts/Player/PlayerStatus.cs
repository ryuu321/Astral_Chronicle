using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // ��{�\�͒l
    public int strength;    // �ؗ�
    public int dexterity;   // ��p��
    public int intelligence; // �m��
    public int vitality;    // ������

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
