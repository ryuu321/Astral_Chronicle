using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{
    // �X�e�[�^�X�E�B���h�E�̃��[�g�ƂȂ�p�l���ւ̎Q��
    public GameObject statusWindowPanel;

    // UI�v�f�ւ̎Q��
    [Header("UI Element References")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI vocationText;
    public TextMeshProUGUI constellationText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI dexterityText;
    public TextMeshProUGUI intelligenceText;
    public TextMeshProUGUI vitalityText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI defensePowerText;
    public TextMeshProUGUI magicPowerText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI coExpText;
    public TextMeshProUGUI gaExpText;
    public TextMeshProUGUI crExpText;
    public TextMeshProUGUI exExpText;
    public TextMeshProUGUI soExpText;
    // �X�L���A�o�t�A�����ȂǁA����ǉ�����UI�v�f�������ɒǉ�

    // PlayerStatus�R���|�[�l���g�ւ̎Q��
    private PlayerStatus playerStatus;

    void Awake()
    {
        // PlayerStatus�R���|�[�l���g�������Ŏ擾
        playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogError("StatusUIController: PlayerStatus component not found in the scene.");
            enabled = false;
        }

        // ������ԂƂ��āA�X�e�[�^�X�E�B���h�E���\���ɂ���
        if (statusWindowPanel != null)
        {
            statusWindowPanel.SetActive(false);
        }
    }

    // �O���i��: PlayerManager�j����Ăяo����UI���X�V����
    public void UpdateStatusUI()
    {
        if (playerStatus != null)
        {
            // ���O�A�E�ƁA�����̍X�V
            if (playerNameText != null) playerNameText.text = "���O: �v���C���["; // �v���C���[���������I��PlayerStatus�ŊǗ�
            if (levelText != null) levelText.text = "���x��: " + playerStatus.level.ToString();
            if (vocationText != null && playerStatus.currentVocation != null)
            {
                vocationText.text = "�E��: " + playerStatus.currentVocation.vocationName;
            }
            if (constellationText != null && playerStatus.selectedConstellation != null)
            {
                constellationText.text = "����: " + playerStatus.selectedConstellation.constellationName;
            }

            // ��{�\�͒l�̍X�V
            if (strengthText != null) strengthText.text = "�ؗ�: " + playerStatus.strength.ToString();
            if (dexterityText != null) dexterityText.text = "��p��: " + playerStatus.dexterity.ToString();
            if (intelligenceText != null) intelligenceText.text = "�m��: " + playerStatus.intelligence.ToString();
            if (vitalityText != null) vitalityText.text = "������: " + playerStatus.vitality.ToString();

            // �h���X�e�[�^�X�̍X�V
            if (healthText != null && playerStatus.playerHealth != null)
            {
                healthText.text = $"�̗�: {playerStatus.playerHealth.currentHealth}/{playerStatus.playerHealth.maxHealth}";
            }
            if (attackPowerText != null) attackPowerText.text = "�U����: " + playerStatus.attackPower.ToString();
            if (defensePowerText != null) defensePowerText.text = "�h���: " + playerStatus.defense.ToString();
            if (magicPowerText != null) magicPowerText.text = "����: " + playerStatus.magicPower.ToString();

            //EXP�̍X�V
            if(expText != null) expText.text = "���o���l:" + playerStatus.currentExp.ToString();
            if(coExpText != null) coExpText.text = "�U���o���l:" + playerStatus.combatExperience.ToString();
            if(gaExpText != null) gaExpText.text = "���W�o���l:" + playerStatus.gatheringExperience.ToString();
            if(crExpText != null) crExpText.text = "����o���l:" + playerStatus.craftingExperience.ToString();
            if(exExpText != null) exExpText.text = "�T���o���l:" + playerStatus.explorationExperience.ToString();
            if(soExpText != null) soExpText.text = "�Ќ��o���l:" + playerStatus.socialExperience.ToString();
        }
    }

    // �X�e�[�^�X�E�B���h�E�̕\��/��\����؂�ւ��郁�\�b�h
    public void HandletatusWindow()
    {
        if (statusWindowPanel != null)
        {
            // ���݂̏�Ԃ̋t��ݒ�
            bool isActive = statusWindowPanel.activeSelf;
            statusWindowPanel.SetActive(!isActive);

            // �E�B���h�E���\�����ꂽ��UI���X�V����
            if (!isActive)
            {
                UpdateStatusUI();
            }
        }
    }
}