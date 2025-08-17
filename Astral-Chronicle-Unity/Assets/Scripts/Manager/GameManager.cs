
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

    private bool isGameStarted = false; // �ǉ�: �Q�[�����J�n���ꂽ���ǂ����̃t���O

    // �Q�[�����ԊǗ� (�ȑO�̒ǉ���)
    [Header("�Q�[�����ԊǗ�")]
    public float secondsPerGameDay = 1; // 1 real-hour = 1 game-day
    public int daysPerGameMonth = 30;       // 1�Q�[�����̓���
    public int monthsPerGameYear = 12;      // 1�Q�[���N�̌���

    public float gameTimeTimer = 0f;
    public int currentDay = 1;     // �ǉ�: �����̕ϐ��錾
    public int currentMonth = 1;   // ����1����J�n
    public int currentYear = 0;    // �N��0����J�n

    [Header("�c�����ݒ�")]
    public int childhoodYears = 5; // �c�����̊��� (�Q�[�����N��)
    public int pathAnnouncementAge = 6; // 6�΂Ői�H���m
    public int pathFinalizationAge = 7;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);// ����̓V�[�������[�h��GameManager���Đ��������O��Ƃ���i�n�C�X�R�A�@�\���Ȃ����߁j
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
    }

    void Update()
    {
        // �Q�[������~���łȂ��A���c�������I�����Ă��Ȃ��ꍇ�̂ݎ��Ԃ�i�߂�
        // gameTimeTimer = float.MaxValue; �Ŏ��Ԃ��~�߂Ă���̂ŁA���̃`�F�b�N���ǉ�
        if (Time.timeScale <= 0f || gameTimeTimer == float.MaxValue || !isGameStarted) // <=0f �͒�~���Afloat.MaxValue�͗c�����I��
        {
            return; // ���Ԃ�i�߂�K�v���Ȃ���΂����ŏ������I��
        }

        gameTimeTimer += Time.deltaTime; // �����̎��Ԃ����Z

        // ���̐i�s�`�F�b�N
        if (gameTimeTimer >= secondsPerGameDay)
        {
            gameTimeTimer -= secondsPerGameDay; // �]����c���ă^�C�}�[�����Z�b�g
            currentDay++; // ����i�߂�

            // ���̐i�s�`�F�b�N
            if (currentDay > daysPerGameMonth)
            {
                currentDay = 1; // ����1�Ƀ��Z�b�g
                currentMonth++; // ����i�߂�

                // �N�̐i�s�`�F�b�N
                if (currentMonth > monthsPerGameYear)
                {
                    currentMonth = 1; // ����1�Ƀ��Z�b�g
                    currentYear++; // �N��i�߂�
                }
            }

            // UI�̍X�V (���A���A�N���ύX���ꂽ��)
            // UpdateGameTimeDisplay�́A���A���A�N�̂ǂꂩ���ς������Ă�
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateGameTimeDisplay(currentYear, currentMonth, currentDay);
            }

            // �c�����I���`�F�b�N (�N���i�񂾌�ɂ̂݃`�F�b�N)
            // currentYear�� childhoodYears �ɓ��B�����u�Ԃɏ���
            if (currentYear == childhoodYears && currentMonth == 1 && currentDay == 1)
            {
                Debug.Log("5�΂ɂȂ�܂����I�M���̋V�̏��������܂��B");
                Time.timeScale = 0f; // �Q�[�����Ԃ��~
                StartVocationSelectionProcess(); // �E�ƑI���v���Z�X���J�n
                return;
            }
            // 6�΃`�F�b�N (�i�H���m�C�x���g)
            if (currentYear == pathAnnouncementAge && currentMonth == 1 && currentDay == 1)
            {
                Debug.Log("6�΂ɂȂ�܂����B�����^�[���b�������Ă��܂��B");
                Time.timeScale = 0f;
                DialogueManager.instance.StartPathAnnouncementDialogue(OnPathAnnouncementDialogueEnd);
                return;
            }
            // 7�΃`�F�b�N (�i�H����C�x���g)
            if (currentYear == pathFinalizationAge && currentMonth == 1 && currentDay == 1)
            {
                Debug.Log("7�΂ɂȂ�܂����B�i�H���ŏI���肵�܂��B");
                Time.timeScale = 0f;
                DialogueManager.instance.StartPathFinalizationDialogue(OnPathFinalizationDialogueEnd);
                return;
            }
        }
    }

    bool ShouldUnlockHiddenConstellations()
    {
        // �Ⴆ�΁A�v���C���[�̍̏W�o���l��100�ȏ�̏ꍇ�ɊJ��
        if (currentPlayerStatus != null && currentPlayerStatus.gatheringExperience >= 100)
        {
            return true;
        }

        return false; // �ŏ��͏��false�ŉB�������͕\�����Ȃ�
    }

    void SelectConstellation(ConstellationData selectedConstellationData)
    {
        if (currentPlayerStatus != null)
        {
            currentPlayerStatus.SetConstellation(selectedConstellationData);
        }
        isGameStarted = true;
        Debug.Log("�Q�[�����J�n����܂��i5�N�Ԃ̎��R���ԊJ�n�j");
    }

    // �����I���v���Z�X���J�n���郁�\�b�h (�`���[�g���A������Ăяo�����z��)
    public void StartConstellationSelectionProcess()
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

            isGameStarted = true;
            // �ȍ~�̃Q�[���J�n���W�b�N (��: 5�N�Ԃ̎��R�Ȋ��Ԃ̊J�n�A�v���C���[����J�n�Ȃ�)
            Debug.Log("�Q�[�����J�n����܂��i5�N�Ԃ̎��R���ԊJ�n�j");
            // ���̎��_��5�N�Ԃ̃Q�[�������Ԃ������o��
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
        currentPlayerStatus.SetVocation(selectedVocationData); // �I�����ꂽ�E�ƃf�[�^��ێ�
        // �v���C���[�ɐE�ƃ{�[�i�X��K�p����


        Time.timeScale = 1f; // �Q�[�����Ԃ��ĊJ
        Debug.Log("�M���̋V���������܂����B�V���Ȑl�����n�܂�܂��I");
        // ��������Q�[���̎��̒i�K�i�`���J�n�A���s�ւ̈ړ��Ȃǁj�ɐi��
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

    void OnPathAnnouncementDialogueEnd()
    {
        Debug.Log("�i�H���m�̉�b���I�����܂����B7�΂܂ł̏������Ԃ��n�܂�܂��B");
        Time.timeScale = 1f; // �Q�[�����ĊJ
        gameTimeTimer = 0f; // �^�C�}�[�����Z�b�g���A�Ăю��Ԃ�i�߂�
    }

    // 7�Ύ��̐i�H����_�C�A���O�I������DialogueManager����Ă΂��
    void OnPathFinalizationDialogueEnd()
    {
        Debug.Log("�i�H����̉�b���I�����܂����B");
        Time.timeScale = 1f;
        // TODO: ������7�Ύ��̐i�H�����̃��W�b�N������
    }


    // DialogueManager����Ăяo����A�I�����ɉ����ď����𕪊�
    public void HandlePathChoice(DialogueData.DialogueOption selectedOption)
    {
        Debug.Log("GameManager: �i�H�I�����󂯕t���܂����B");

        if (selectedOption.optionText == "�w���֍s��")
        {
            Debug.Log("�I��: �w���֍s��");
        }
        else if (selectedOption.optionText == "�Ӌ��ŕ�炵������")
        {
            Debug.Log("�I��: �Ӌ��Ɏc��");
        }
        else if (selectedOption.optionText == "�`���҂ɂȂ�")
        {
            Debug.Log("�I��: �`���҂ɂȂ�");
        }
        else
        {
            Debug.LogWarning("�s���Ȑi�H�I���ł�: " + selectedOption.optionText);
        }
        // TODO: �����Ŏ��ۂ̃V�[���J�ڂ�v���C���[�ւ̉e��������
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

    void OnDialogueEnd()
    {
        // ��b���I��������A�Q�[�����ĊJ
        Time.timeScale = 1f;
        // TODO: �����Ői�H�����̃��W�b�N������
    }
}