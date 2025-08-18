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

    [Header("ゲーム時間管理")]
    public float secondsPerGameDay = 1f;
    public int daysPerGameMonth = 30;
    public int monthsPerGameYear = 12;

    private float gameTimeTimer = 0f;
    private int currentDay = 1;
    private int currentMonth = 1;
    private int currentYear = 0;

    [Header("幼少期設定")]
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
        Debug.Log("進路告知の会話が終了しました。7歳までの準備期間が始まります。");
        Time.timeScale = 1f;
        gameTimeTimer = 0f;
    }

    void OnPathFinalizationDialogueEnd()
    {
        Debug.Log("進路決定の会話が終了しました。");
        Time.timeScale = 1f;
    }

    public void HandlePathChoice(DialogueData.DialogueOption selectedOption)
    {
        Debug.Log("GameManager: 進路選択を受け付けました。");

        if (selectedOption.optionText == "王都の学園へ行く")
        {
            Debug.Log("選択: 学園へ行く");
        }
        else if (selectedOption.optionText == "辺境で暮らし続ける")
        {
            Debug.Log("選択: 辺境に残る");
        }
        else if (selectedOption.optionText == "冒険者になる")
        {
            Debug.Log("選択: 冒険者になる");
        }
        else
        {
            Debug.LogWarning("不明な進路選択です: " + selectedOption.optionText);
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
        Debug.Log("ゲームが開始されます（5年間の自由期間開始）");
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
        Debug.Log("信託の儀が完了しました。新たな人生が始まります！");
    }

    public void HandleGameOver()
    {
        Debug.Log("GameManager: ゲームオーバーを処理します。");
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
            case ItemType.Coin: Debug.Log("コインをゲット！"); break;
            case ItemType.HealthPotion:
                if (currentPlayerHealth != null) currentPlayerHealth.Heal(item.value);
                Debug.Log("回復ポーションをゲット！体力 " + item.value + " 回復！");
                break;
            case ItemType.Key: Debug.Log("鍵をゲット！"); break;
            case ItemType.Gem: Debug.Log("宝石をゲット！"); break;
            default: Debug.Log("未知のアイテムをゲット！: " + item.itemType.ToString()); break;
        }
        if (!string.IsNullOrEmpty(item.collectMessage)) Debug.Log("メッセージ: " + item.collectMessage);
    }

    void OnDialogueEnd()
    {
        // 会話が終了したら、ゲームを再開
        Time.timeScale = 1f;
        // TODO: ここで進路決定後のロジックを実装
    }
}