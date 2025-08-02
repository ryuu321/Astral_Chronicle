using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic; // List<T> ���g������

public class GameManager : MonoBehaviour
{
    // UIManager���Ǘ�����UI�Q�Ƃ�GameManager����폜�ς݂ł��邱�Ƃ��m�F

    [Header("Game Data")]
    // �C��: List<ConstellationData> �̑���� ConstellationDatabase ���Q��
    public ConstellationDatabase constellationDatabase;

    public static GameManager instance;

    public ConstellationData selectedConstellation { get; private set; }

    private PlayerHealth currentPlayerHealth;
    private PlayerStatus currentPlayerStatus; // �ǉ�: PlayerStatus�ւ̎Q��


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad�́AGameManager���V�[���J�ڌ���f�[�^��ێ�����K�v������ꍇ�Ɍ���
            // ����̓V�[�������[�h��GameManager���Đ��������O��Ƃ���i�n�C�X�R�A�@�\���Ȃ����߁j
        }
        else
        {
            // ���ɃC���X�^���X�����݂��A�����ꂪ�������g�łȂ���Δj��
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    void Start()
    {
        InitializeGameStart();
    }

    // �Q�[���J�n���̏���������
    void InitializeGameStart()
    {
        Time.timeScale = 1f;

        // UIManager�����݂��邩�m�F���AUI�̏��������Ϗ�
        if (UIManager.instance != null)
        {
            UIManager.instance.HideGameOverUI(); // UIManager�ɉB���Ă��炤
            UIManager.instance.HideConstellationSelectionUI(); // UIManager�ɉB���Ă��炤 (������ԂƂ���)
        }
        else
        {
            Debug.LogError("UIManager instance not found! Please ensure UIManager GameObject exists and has UIManager.cs attached.");
            enabled = false; // UIManager�Ȃ��ł͑��s�ł��Ȃ����߃X�N���v�g�𖳌���
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            currentPlayerHealth = playerObject.GetComponent<PlayerHealth>();
            currentPlayerStatus = playerObject.GetComponent<PlayerStatus>(); // �ǉ�: PlayerStatus���擾

            // PlayerHealth��UIManager�Q�Ƃ�n��
            if (currentPlayerHealth != null && UIManager.instance != null)
            {
                currentPlayerHealth.SetUIManager(UIManager.instance);
            }

            // PlayerStatus��������Ȃ��ꍇ�̃G���[�`�F�b�N
            if (currentPlayerStatus == null)
            {
                Debug.LogWarning("PlayerStatus component not found on Player object! Player stats will not be affected by constellation.");
            }
        }
        else
        {
            Debug.LogWarning("Player object with 'Player' tag not found in scene!");
        }

        // �����I��UI�̕\���v���Z�X���J�n
        StartConstellationSelectionProcess();
    }

    // �����I���v���Z�X���J�n���郁�\�b�h (�`���[�g���A������Ăяo�����z��)
    void StartConstellationSelectionProcess()
    {
        // �C��: ConstellationDatabase�����蓖�Ă��Ă��邩�m�F
        if (constellationDatabase == null)
        {
            Debug.LogError("Constellation Database is not assigned in GameManager! Please assign MasterConstellationDatabase asset.");
            enabled = false;
            return;
        }

        List<ConstellationData> availableConstellations = new List<ConstellationData>();

        // �����\�񐯍���ǉ� (�Z�I���[�̑I����)
        if (constellationDatabase.zodiacConstellations != null)
        {
            availableConstellations.AddRange(constellationDatabase.zodiacConstellations);
        }

        // TODO: �����ŉB�������̊J���������`�F�b�N���A�J������Ă���Βǉ�
        // ��揑: �u����̏��������i��: �`���[�g���A�����Ԓ��̓���̍s���A�B��NPC�Ƃ̌𗬁A����̃A�C�e�������Ȃǁj�𖞂����Ă����ꍇ�v
        // ����̓e�X�g�p�ɉ��̊J��������ݒ�
        if (ShouldUnlockHiddenConstellations()) // ���̊J�������`�F�b�N���\�b�h
        {
            if (constellationDatabase.hiddenConstellations != null)
            {
                availableConstellations.AddRange(constellationDatabase.hiddenConstellations);
                Debug.Log("�B���������J������܂����I");
            }
        }

        // UIManager�ɕ\���\�Ȑ������X�g��n���A�I�����ꂽ�ۂ̃R�[���o�b�N��o�^
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowConstellationSelectionUI(availableConstellations, SelectConstellation);
        }
    }

    // ���̉B�������J������ (�e�X�g�p)
    // ���ۂ̃Q�[���ł́A�v���C���[�̃p�����[�^��t���O�Ɋ�Â��ĕ��G�ȃ��W�b�N���L�q
    bool ShouldUnlockHiddenConstellations()
    {
        // ��: (�f�o�b�O�ړI�ŏ��true�ɂ��邩�A����̃{�^�������������Ȃ�)
        return false; // �ŏ��͏��false�ŉB�������͕\�����Ȃ�
    }


    // �������I�����ꂽ���̏��� (UIManager����̃R�[���o�b�N)
    void SelectConstellation(ConstellationData selectedConstellationData)
    {
        this.selectedConstellation = selectedConstellationData; // �I�����ꂽ�����f�[�^��ێ�
        Debug.Log(selectedConstellationData.constellationName + "���I������܂����I");

        // �v���C���[�Ƀo�t��K�p����
        ApplyConstellationBuff(selectedConstellationData);

        // �ȍ~�̃Q�[���J�n���W�b�N (��: 5�N�Ԃ̎��R�Ȋ��Ԃ̊J�n�A�v���C���[����J�n�Ȃ�)
        Debug.Log("�Q�[�����J�n����܂��i5�N�Ԃ̎��R���ԊJ�n�j");
        // ��������`���[�g���A����̃Q�[���i�s���n�܂�
    }

    // �I�����ꂽ�����̃o�t���v���C���[�ɓK�p���郁�\�b�h
    void ApplyConstellationBuff(ConstellationData constellation)
    {
        if (currentPlayerStatus != null) // PlayerStatus�����݂���ΓK�p
        {
            Debug.Log("�����o�t�K�p: " + constellation.constellationName +
                      " �ؗ�+" + constellation.initialStrengthBonus + ", ��p��+" + constellation.initialDexterityBonus +
                      ", �m��+" + constellation.initialIntelligenceBonus + ", ������+" + constellation.initialVitalityBonus);

            currentPlayerStatus.ApplyInitialStatBonus(
                constellation.initialStrengthBonus,
                constellation.initialDexterityBonus,
                constellation.initialIntelligenceBonus,
                constellation.initialVitalityBonus
            );
        }
        else
        {
            Debug.LogWarning("PlayerStatus component not found on Player object! Cannot apply constellation buffs.");
        }
    }

    // �Q�[���I�[�o�[���� (PlayerHealth����Ă΂��)
    public void HandleGameOver()
    {
        Debug.Log("GameManager: �Q�[���I�[�o�[���������܂��B");
        Time.timeScale = 0f; // �Q�[�����ꎞ��~
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowGameOverUI();
        }
    }

    // ���݂̃V�[���������[�h����֐� (�{�^������Ă΂��)
    public void LoadCurrentScene()
    {
        Time.timeScale = 1f; // �Q�[�����ԃX�P�[�������ɖ߂�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // GameManager��DontDestroyOnLoad�łȂ��ꍇ�A�V�[�����[�h��Start���ēx�Ă΂�A
        // InitializeGameStart()��UI������������邽�߁A���ɂ����ł̒ǉ������͕s�v�B
    }

    // CollectItem ���\�b�h (�A�C�e�����W���W�b�N�͂����Ɉێ�)
    public void CollectItem(ItemData item)
    {
        if (item == null) return;
        switch (item.itemType)
        {
            case ItemType.Coin: Debug.Log("�R�C�����Q�b�g�I"); break;
            case ItemType.HealthPotion:
                if (currentPlayerHealth != null) currentPlayerHealth.Heal(item.value);
                Debug.Log("�񕜃|�[�V�������Q�b�g�I�̗� " + item.value + " �񕜁I");
                break;
            case ItemType.Key: Debug.Log("�����Q�b�g�I"); break;
            case ItemType.Gem: Debug.Log("��΂��Q�b�g�I"); break;
            default: Debug.Log("���m�̃A�C�e�����Q�b�g�I: " + item.itemType.ToString()); break;
        }
        if (!string.IsNullOrEmpty(item.collectMessage)) Debug.Log("���b�Z�[�W: " + item.collectMessage);
    }
}