using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic; // List<T> ���g������

public class GameManager : MonoBehaviour
{
    // UI�Q�Ƃ͑S��UIManager�Ɉړ��ς�

    [Header("Game Data References")]
    // �C��: List<ConstellationData> �̑���� ConstellationDatabase ���Q��
    public ConstellationDatabase constellationDatabase; // ConstellationDatabase�A�Z�b�g�̎Q��
    public VocationDatabase vocationDatabase; // �E�ƃf�[�^�x�[�X�ւ̎Q�� (�ȑO�̒ǉ���)

    public static GameManager instance;

    public ConstellationData selectedConstellation { get; private set; } // �I�����ꂽ�����f�[�^
    public VocationData selectedVocation { get; private set; } // �I�����ꂽ�E�ƃf�[�^ (�ȑO�̒ǉ���)

    private PlayerHealth currentPlayerHealth;
    public PlayerStatus currentPlayerStatus { get; private set; }  // �ǉ�: PlayerStatus�ւ̎Q��


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
            UIManager.instance.HideVocationSelectionUI(); // �E�ƑI��UI�������͔�\��
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
                Debug.LogWarning("PlayerStatus component not found on Player object! Player stats will not be affected by constellation or vocation.");
            }
        }
        else
        {
            Debug.LogWarning("Player object with 'Player' tag not found in scene!");
        }

        // �����I��UI�̕\���v���Z�X���J�n
        //StartConstellationSelectionProcess();
        StartVocationSelectionProcess();
    }

    // �Q�[�����ԊǗ� (�ȑO�̒ǉ���)
    [Header("�Q�[�����ԊǗ�")]
    public float timePerGameMonth = 5f; // �������E��5�b�ŃQ�[����1�����o�� (�����\)
    private float gameTimeTimer = 0f;
    private int currentMonth = 0; // ���݂̃Q�[��������
    private int currentYear = 0;  // ���݂̃Q�[�����N��

    [Header("�c�����ݒ�")]
    public int childhoodYears = 5; // �c�����̊��� (�Q�[�����N��)

    // Update ���\�b�h (�Q�[�����Ԑi�s�ƐM���̋V�Ăяo��)
    void Update()
    {
        // �Q�[������~���łȂ��ꍇ�̂ݎ��Ԃ�i�߂�
        if (Time.timeScale > 0f)
        {
            gameTimeTimer += Time.deltaTime; // �����̎��Ԃ����Z

            if (gameTimeTimer >= timePerGameMonth)
            {
                gameTimeTimer -= timePerGameMonth; // �^�C�}�[�����Z�b�g
                currentMonth++; // ����i�߂�

                if (currentMonth >= 12) // 12������1�N
                {
                    currentMonth = 1;
                    currentYear++; // �N��i�߂�
                                   // Debug.Log("�Q�[��������: " + currentYear + "�� " + (currentMonth + 1) + "����"); // �f�o�b�O���O��UI�\���ɒu��������

                    // UI�̍X�V
                    if (UIManager.instance != null)
                    {
                        UIManager.instance.UpdateGameTimeDisplay(currentYear, currentMonth);
                    }

                    // 5�N�o�߃`�F�b�N (�c�����I��)
                    if (currentYear >= childhoodYears)
                    {
                        Debug.Log("5�΂ɂȂ�܂����I�M���̋V�̏��������܂��B");
                        Time.timeScale = 0f; // �Q�[�����Ԃ��~

                        // �c�����^�C�}�[���~�����邽�߁A���ɑ傫�Ȓl�ɂ��邩�Abool�t���O�Ő���
                        // ����ɂ��A�E�ƑI�𒆂Ɏ��Ԃ��i�ނ̂�h���A��x�����Ă΂�Ȃ��悤�ɂ���
                        gameTimeTimer = float.MaxValue; // ����ł���ȏ�Update�������𖞂����Ȃ��悤�ɂ���
                                                        // ���邢�� bool childhoodEnded = true; If (!childhoodEnded) { ... } �Ƃ���

                        StartVocationSelectionProcess(); // �E�ƑI���v���Z�X���J�n
                    }
                }
            }
        }
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
        // ���̎��_��5�N�Ԃ̃Q�[�������Ԃ������o��
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


    // �E�ƑI���v���Z�X���J�n���郁�\�b�h (5�N�o�ߌ�ɌĂ΂��)
    void StartVocationSelectionProcess()
    {
        if (vocationDatabase == null)
        {
            Debug.LogError("Vocation Database is not assigned in GameManager!");
            enabled = false;
            return;
        }
        if (currentPlayerStatus == null)
        {
            Debug.LogError("PlayerStatus is not assigned or found! Cannot determine vocations for selection.");
            return;
        }

        List<VocationData> availableVocations = new List<VocationData>();

        // �S�Ă̐E�Ƃ����[�v���A�v���C���[�̌o���l�������𖞂����Ă��邩�`�F�b�N
        foreach (VocationData vocation in vocationDatabase.allVocations)
        {
            bool canSelect = true;
            if (currentPlayerStatus.combatExperience < vocation.requiredCombatExperience) canSelect = false;
            if (currentPlayerStatus.gatheringExperience < vocation.requiredGatheringExperience) canSelect = false;
            if (currentPlayerStatus.craftingExperience < vocation.requiredCraftingExperience) canSelect = false;
            if (currentPlayerStatus.explorationExperience < vocation.requiredExplorationExperience) canSelect = false;
            if (currentPlayerStatus.socialExperience < vocation.requiredSocialExperience) canSelect = false;

            if (canSelect)
            {
                availableVocations.Add(vocation);
            }
        }

        // ���Ȃ��Ƃ�1�͑I����������悤�ɕۏ؂���i��: �S�Ă̐E�Ƃ������𖞂����Ȃ��ꍇ�j
        if (availableVocations.Count == 0 && vocationDatabase.allVocations.Count > 0)
        {
            Debug.LogWarning("No vocations meet the player's experience requirements. Presenting all available vocations as fallback.");
            availableVocations.AddRange(vocationDatabase.allVocations); // �S�Ă̐E�Ƃ�񎦂����
        }
        else if (availableVocations.Count == 0) // VocationDatabase���̂���̏ꍇ
        {
            Debug.LogError("No vocations defined in VocationDatabase! Cannot present vocation choices.");
            enabled = false;
            return;
        }


        // UIManager�ɕ\���\�ȐE�ƃ��X�g��n���A�I�����ꂽ�ۂ̃R�[���o�b�N��o�^
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowVocationSelectionUI(availableVocations, SelectVocation);
        }
    }

    // �E�Ƃ��I�����ꂽ���̏��� (UIManager����̃R�[���o�b�N)
    void SelectVocation(VocationData selectedVocationData)
    {
        this.selectedVocation = selectedVocationData; // �I�����ꂽ�E�ƃf�[�^��ێ�
        Debug.Log(selectedVocationData.vocationName + "���I������܂����I");

        // �v���C���[�ɐE�ƃ{�[�i�X��K�p����
        ApplyVocationBonus(selectedVocationData);

        Time.timeScale = 1f; // �Q�[�����Ԃ��ĊJ
        Debug.Log("�M���̋V���������܂����B�V���Ȑl�����n�܂�܂��I");
        // ��������Q�[���̎��̒i�K�i�`���J�n�A���s�ւ̈ړ��Ȃǁj�ɐi��
    }

    // �I�����ꂽ�E�Ƃ̃{�[�i�X���v���C���[�ɓK�p���郁�\�b�h
    void ApplyVocationBonus(VocationData vocation)
    {
        if (currentPlayerStatus != null)
        {
            Debug.Log("�E�ƃ{�[�i�X�K�p: " + vocation.vocationName +
                      " �ؗ�+" + vocation.strengthBonus + ", ��p��+" + vocation.dexterityBonus +
                      ", �m��+" + vocation.intelligenceBonus + ", ������+" + vocation.vitalityBonus);

            currentPlayerStatus.ApplyVocationBonus(
                vocation.strengthBonus,
                vocation.dexterityBonus,
                vocation.intelligenceBonus,
                vocation.vitalityBonus
            );
        }
        else
        {
            Debug.LogWarning("PlayerStatus component not found on Player object! Cannot apply vocation bonuses.");
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