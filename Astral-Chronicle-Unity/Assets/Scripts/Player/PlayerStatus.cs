using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    public ConstellationData selectedConstellation { get; private set; }
    public VocationData currentVocation { get; private set; }

    // ��{�\�͒l
    public int strength;
    public int dexterity;
    public int intelligence;
    public int vitality;

    [Header("�s���p�����[�^")]
    public int combatExperience = 0;
    public int gatheringExperience = 0;
    public int craftingExperience = 0;
    public int explorationExperience = 0;
    public int socialExperience = 0;

    // ���x���ƌo���l (�����I�ɒǉ�)
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    // �h���X�e�[�^�X (�v�Z�ŋ��܂�)
    public int attackPower;
    public int defense;
    public int magicPower;

    public PlayerHealth playerHealth;
    private StatusUIController statusUIController;
    void Awake()
    {
        strength = 10;
        dexterity = 10;
        intelligence = 10;
        vitality = 10;

        playerHealth = GetComponent<PlayerHealth>();
        CalculateDerivedStats();
    }

    void CalculateDerivedStats()
    {
        attackPower = strength * 2;
        defense = vitality * 1;
        magicPower = intelligence * 2;

        if (playerHealth != null)
        {
            playerHealth.maxHealth = vitality * 10;
            playerHealth.currentHealth = playerHealth.maxHealth;
            playerHealth.UpdateHealthUI();
        }
    }

    // --- �������炪�������ꂽ���� ---

    // �v���C���[�ɐ����f�[�^���Z�b�g���郁�\�b�h
    public void SetConstellation(ConstellationData newConstellation)
    {
        selectedConstellation = newConstellation;
        Debug.Log(selectedConstellation.constellationName + "���I������܂����I");

        // �����{�[�i�X��K�p
        ApplyStatusBonus(
            selectedConstellation.initialStrengthBonus,
            selectedConstellation.initialDexterityBonus,
            selectedConstellation.initialIntelligenceBonus,
            selectedConstellation.initialVitalityBonus
        );
    }

    // �v���C���[�ɐE�ƃf�[�^���Z�b�g���郁�\�b�h
    public void SetVocation(VocationData newVocation)
    {
        currentVocation = newVocation;
        Debug.Log(currentVocation.vocationName + "���I������܂����I");

        // �E�ƃ{�[�i�X��K�p
        ApplyStatusBonus(
            currentVocation.strengthBonus,
            currentVocation.dexterityBonus,
            currentVocation.intelligenceBonus,
            currentVocation.vitalityBonus
        );

        // ���������{�[�i�X��K�p
        if (newVocation.constellationSynergyBonus != null)
        {
            foreach (var synergy in newVocation.constellationSynergyBonus)
            {
                if (synergy.constellation == selectedConstellation)
                {
                    ApplyStatusBonus(
                        synergy.bonusStrength,
                        synergy.bonusDexterity,
                        synergy.bonusIntelligence,
                        synergy.bonusVitality
                    );
                    Debug.Log($"���������{�[�i�X�K�p�I{synergy.constellation.constellationName}�Ƃ̑����{�[�i�X���l�����܂����B");
                    break;
                }
            }
        }
    }

    // �ėp�I�ȃX�e�[�^�X���Z���\�b�h
    private void ApplyStatusBonus(int strengthBonus, int dexterityBonus, int intelligenceBonus, int vitalityBonus)
    {
        strength += strengthBonus;
        dexterity += dexterityBonus;
        intelligence += intelligenceBonus;
        vitality += vitalityBonus;

        // �{�[�i�X�K�p��ɔh���X�e�[�^�X�ƍő�̗͂��Čv�Z
        CalculateDerivedStats();
    }
}