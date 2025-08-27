using UnityEngine;

public class Character : MonoBehaviour
{
    // ���̃L�����N�^�[�̃f�[�^��ێ�����ScriptableObject
    public CharacterData characterData;

    // �L�����N�^�[�̊O���𐧌䂷��R���|�[�l���g
    protected PlayerAppearanceController appearanceController;
    // �L�����N�^�[�̃X�e�[�^�X�𐧌䂷��R���|�[�l���g
    protected PlayerStatus statusController;
    // �L�����N�^�[�̗̑͂𐧌䂷��R���|�[�l���g
    protected PlayerHealth healthController;

    protected virtual void Awake()
    {
        // �K�v�ȃR���|�[�l���g���擾
        appearanceController = GetComponent<PlayerAppearanceController>();
        statusController = GetComponent<PlayerStatus>();
        healthController = GetComponent<PlayerHealth>();

        // �L�����N�^�[�f�[�^��������
        if (characterData != null)
        {
            InitializeCharacter();
        }
    }

    // CharacterData�Ɋ�Â��ăL�����N�^�[�����������郁�\�b�h
    public void InitializeCharacter()
    {
        if (characterData == null) return;
        GetComponent<SpriteRenderer>().sprite = characterData.characterSprite;

        // �����ڂ̏�����
        if (appearanceController != null)
        {
            appearanceController.UpdateAppearance(
                characterData.hairStyle,
                characterData.face,
                characterData.body
            );
        }

        // �X�e�[�^�X�̏�����
        if (statusController != null)
        {
            statusController.InitializeStatus(
                characterData.baseStrength,
                characterData.baseDexterity,
                characterData.baseIntelligence,
                characterData.baseVitality
            );
            // �E�ƂƐ����������ŏ������ł���
        }

        // �̗͂̏�����
        if (healthController != null)
        {
            healthController.InitializeHealth(characterData.hp) ;
            Debug.Log("hp");
        }

        // ���O���Q�[���I�u�W�F�N�g���ɐݒ�
        gameObject.name = characterData.characterName;
    }
}