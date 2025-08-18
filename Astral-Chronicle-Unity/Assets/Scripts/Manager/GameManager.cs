using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Data References")]
    public ConstellationDatabase constellationDatabase;
    public VocationDatabase vocationDatabase;

    public ConstellationData selectedConstellation { get; private set; }
    public VocationData selectedVocation { get; private set; }

    private PlayerHealth currentPlayerHealth;
    public PlayerStatus currentPlayerStatus { get; private set; }

    private bool isGameStarted = false;

    [Header("�Q�[�����ԊǗ�")]
    public float secondsPerGameDay = 1f;
    public int daysPerGameMonth = 30;
    public int monthsPerGameYear = 12;

    private float gameTimeTimer = 0f;
    private int currentDay = 1;
    private int currentMonth = 1;
    private int currentYear = 0;

    [Header("�c�����ݒ�")]
    public int childhoodYears = 5;
    public int pathAnnouncementAge = 6;
    public int pathFinalizationAge = 7;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        InitializeGameStart();
    }

    void InitializeGameStart()
    {
        Time.timeScale = 1f;

        if (UIManager.instance != null)
        {
            UIManager.instance.HideGameOverUI();
            UIManager.instance.HideConstellationSelectionUI();
            UIManager.instance.HideVocationSelectionUI();
        }
        else
        {
            Debug.LogError("UIManager instance not found!");
            enabled = false;
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            currentPlayerHealth = playerObject.GetComponent<PlayerHealth>();
            currentPlayerStatus = playerObject.GetComponent<PlayerStatus>();
            if (currentPlayerHealth != null && UIManager.instance != null)
            {
                currentPlayerHealth.SetUIManager(UIManager.instance);
            }
        }
        else
        {
            Debug.LogWarning("Player object with 'Player' tag not found!");
        }

        StartConstellationSelectionProcess();
    }

    void Update()
    {
        if (Time.timeScale <= 0f || gameTimeTimer == float.MaxValue || !isGameStarted)
        {
            return;
        }

        gameTimeTimer += Time.deltaTime;
        if (gameTimeTimer >= secondsPerGameDay)
        {
            gameTimeTimer -= secondsPerGameDay;
            currentDay++;

            if (currentDay > daysPerGameMonth)
            {
                currentDay = 1;
                currentMonth++;
                if (currentMonth > monthsPerGameYear)
                {
                    currentMonth = 1;
                    currentYear++;
                }
            }

            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateGameTimeDisplay(currentYear, currentMonth, currentDay);
            }

            if (currentYear == childhoodYears && currentMonth == 1 && currentDay == 1)
            {
                Time.timeScale = 0f;
                gameTimeTimer = float.MaxValue;
                StartVocationSelectionProcess();
                return;
            }

            if (currentYear == pathAnnouncementAge && currentMonth == 1 && currentDay == 1)
            {
                Time.timeScale = 0f;
                gameTimeTimer = float.MaxValue;
                if (DialogueManager.instance != null)
                {
                    DialogueManager.instance.StartPathAnnouncementDialogue(OnPathAnnouncementDialogueEnd);
                }
                return;
            }

            if (currentYear == pathFinalizationAge && currentMonth == 1 && currentDay == 1)
            {
                Time.timeScale = 0f;
                gameTimeTimer = float.MaxValue;
                if (DialogueManager.instance != null)
                {
                    DialogueManager.instance.StartPathFinalizationDialogue(OnPathFinalizationDialogueEnd);
                }
                return;
            }
        }
    }

    void OnPathAnnouncementDialogueEnd()
    {
        Debug.Log("�i�H���m�̉�b���I�����܂����B7�΂܂ł̏������Ԃ��n�܂�܂��B");
        Time.timeScale = 1f;
        gameTimeTimer = 0f;
    }

    void OnPathFinalizationDialogueEnd()
    {
        Debug.Log("�i�H����̉�b���I�����܂����B");
        Time.timeScale = 1f;
    }

    public void HandlePathChoice(DialogueData.DialogueOption selectedOption)
    {
        Debug.Log("GameManager: �i�H�I�����󂯕t���܂����B");

        if (selectedOption.optionText == "���s�̊w���֍s��")
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
    }

    public void StartConstellationSelectionProcess()
    {
        if (constellationDatabase == null)
        {
            Debug.LogError("Constellation Database is not assigned in GameManager!");
            enabled = false;
            return;
        }

        List<ConstellationData> availableConstellations = new List<ConstellationData>();
        if (constellationDatabase.zodiacConstellations != null)
        {
            availableConstellations.AddRange(constellationDatabase.zodiacConstellations);
        }

        if (ShouldUnlockHiddenConstellations())
        {
            if (constellationDatabase.hiddenConstellations != null)
            {
                availableConstellations.AddRange(constellationDatabase.hiddenConstellations);
            }
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowConstellationSelectionUI(availableConstellations, SelectConstellation);
        }
    }

    bool ShouldUnlockHiddenConstellations()
    {
        if (currentPlayerStatus != null && currentPlayerStatus.gatheringExperience >= 100)
        {
            return true;
        }
        return false;
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

    public void StartVocationSelectionProcess()
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

        if (availableVocations.Count == 0 && vocationDatabase.allVocations.Count > 0)
        {
            availableVocations.AddRange(vocationDatabase.allVocations);
        }
        else if (availableVocations.Count == 0)
        {
            Debug.LogError("No vocations defined in VocationDatabase! Cannot present vocation choices.");
            enabled = false;
            return;
        }

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowVocationSelectionUI(availableVocations, SelectVocation);
        }
    }

    void SelectVocation(VocationData selectedVocationData)
    {
        if (currentPlayerStatus != null)
        {
            currentPlayerStatus.SetVocation(selectedVocationData);
        }
        Time.timeScale = 1f;
        Debug.Log("�M���̋V���������܂����B�V���Ȑl�����n�܂�܂��I");
    }

    public void HandleGameOver()
    {
        Debug.Log("GameManager: �Q�[���I�[�o�[���������܂��B");
        Time.timeScale = 0f;
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowGameOverUI();
        }
    }

    public void LoadCurrentScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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