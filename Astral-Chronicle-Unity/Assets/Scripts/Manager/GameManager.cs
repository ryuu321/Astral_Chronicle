
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic; // List<T> を使うため

public class GameManager : MonoBehaviour
{
    // UI参照は全てUIManagerに移動済み

    [Header("Game Data References")]
    // 修正: List<ConstellationData> の代わりに ConstellationDatabase を参照
    public ConstellationDatabase constellationDatabase; // ConstellationDatabaseアセットの参照
    public VocationDatabase vocationDatabase; // 職業データベースへの参照 (以前の追加分)

    public static GameManager instance;

    public ConstellationData selectedConstellation { get; private set; } // 選択された星座データ
    public VocationData selectedVocation { get; private set; } // 選択された職業データ (以前の追加分)

    private PlayerHealth currentPlayerHealth;
    public PlayerStatus currentPlayerStatus { get; private set; }  // 追加: PlayerStatusへの参照

    private bool isGameStarted = false; // 追加: ゲームが開始されたかどうかのフラグ

    // ゲーム時間管理 (以前の追加分)
    [Header("ゲーム時間管理")]
    public float secondsPerGameDay = 1; // 1 real-hour = 1 game-day
    public int daysPerGameMonth = 30;       // 1ゲーム月の日数
    public int monthsPerGameYear = 12;      // 1ゲーム年の月数

    public float gameTimeTimer = 0f;
    public int currentDay = 1;     // 追加: 日数の変数宣言
    public int currentMonth = 1;   // 月を1から開始
    public int currentYear = 0;    // 年を0から開始

    [Header("幼少期設定")]
    public int childhoodYears = 5; // 幼少期の期間 (ゲーム内年数)
    public int pathAnnouncementAge = 6; // 6歳で進路告知
    public int pathFinalizationAge = 7;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);// 今回はシーンリロードでGameManagerも再生成される前提とする（ハイスコア機能がないため）
        }
        else
        {
            // 既にインスタンスが存在し、かつそれが自分自身でなければ破棄
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

    // ゲーム開始時の初期化処理
    void InitializeGameStart()
    {
        Time.timeScale = 1f;

        // UIManagerが存在するか確認し、UIの初期化を委譲
        if (UIManager.instance != null)
        {
            UIManager.instance.HideGameOverUI(); // UIManagerに隠してもらう
            UIManager.instance.HideConstellationSelectionUI(); // UIManagerに隠してもらう (初期状態として)
            UIManager.instance.HideVocationSelectionUI(); // 職業選択UIも初期は非表示
        }
        else
        {
            Debug.LogError("UIManager instance not found! Please ensure UIManager GameObject exists and has UIManager.cs attached.");
            enabled = false; // UIManagerなしでは続行できないためスクリプトを無効化
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            currentPlayerHealth = playerObject.GetComponent<PlayerHealth>();
            currentPlayerStatus = playerObject.GetComponent<PlayerStatus>(); // 追加: PlayerStatusを取得

            // PlayerHealthにUIManager参照を渡す
            if (currentPlayerHealth != null && UIManager.instance != null)
            {
                currentPlayerHealth.SetUIManager(UIManager.instance);
            }

            // PlayerStatusが見つからない場合のエラーチェック
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
        // ゲームが停止中でない、かつ幼少期が終了していない場合のみ時間を進める
        // gameTimeTimer = float.MaxValue; で時間を止めているので、そのチェックも追加
        if (Time.timeScale <= 0f || gameTimeTimer == float.MaxValue || !isGameStarted) // <=0f は停止中、float.MaxValueは幼少期終了
        {
            return; // 時間を進める必要がなければここで処理を終了
        }

        gameTimeTimer += Time.deltaTime; // 現実の時間を加算

        // 日の進行チェック
        if (gameTimeTimer >= secondsPerGameDay)
        {
            gameTimeTimer -= secondsPerGameDay; // 余りを残してタイマーをリセット
            currentDay++; // 日を進める

            // 月の進行チェック
            if (currentDay > daysPerGameMonth)
            {
                currentDay = 1; // 日を1にリセット
                currentMonth++; // 月を進める

                // 年の進行チェック
                if (currentMonth > monthsPerGameYear)
                {
                    currentMonth = 1; // 月を1にリセット
                    currentYear++; // 年を進める
                }
            }

            // UIの更新 (日、月、年が変更されたら)
            // UpdateGameTimeDisplayは、日、月、年のどれかが変わったら呼ぶ
            if (UIManager.instance != null)
            {
                UIManager.instance.UpdateGameTimeDisplay(currentYear, currentMonth, currentDay);
            }

            // 幼少期終了チェック (年が進んだ後にのみチェック)
            // currentYearが childhoodYears に到達した瞬間に処理
            if (currentYear == childhoodYears && currentMonth == 1 && currentDay == 1)
            {
                Debug.Log("5歳になりました！信託の儀の準備をします。");
                Time.timeScale = 0f; // ゲーム時間を停止
                StartVocationSelectionProcess(); // 職業選択プロセスを開始
                return;
            }
            // 6歳チェック (進路告知イベント)
            if (currentYear == pathAnnouncementAge && currentMonth == 1 && currentDay == 1)
            {
                Debug.Log("6歳になりました。メンターが話しかけてきます。");
                Time.timeScale = 0f;
                DialogueManager.instance.StartPathAnnouncementDialogue(OnPathAnnouncementDialogueEnd);
                return;
            }
            // 7歳チェック (進路決定イベント)
            if (currentYear == pathFinalizationAge && currentMonth == 1 && currentDay == 1)
            {
                Debug.Log("7歳になりました。進路を最終決定します。");
                Time.timeScale = 0f;
                DialogueManager.instance.StartPathFinalizationDialogue(OnPathFinalizationDialogueEnd);
                return;
            }
        }
    }

    bool ShouldUnlockHiddenConstellations()
    {
        // 例えば、プレイヤーの採集経験値が100以上の場合に開放
        if (currentPlayerStatus != null && currentPlayerStatus.gatheringExperience >= 100)
        {
            return true;
        }

        return false; // 最初は常にfalseで隠し星座は表示しない
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

    // 星座選択プロセスを開始するメソッド (チュートリアルから呼び出される想定)
    public void StartConstellationSelectionProcess()
    {
        // 修正: ConstellationDatabaseが割り当てられているか確認
        if (constellationDatabase == null)
        {
            Debug.LogError("Constellation Database is not assigned in GameManager! Please assign MasterConstellationDatabase asset.");
            enabled = false;
            return;
        }

        List<ConstellationData> availableConstellations = new List<ConstellationData>();

        // 黄道十二星座を追加 (セオリーの選択肢)
        if (constellationDatabase.zodiacConstellations != null)
        {
            availableConstellations.AddRange(constellationDatabase.zodiacConstellations);
        }

        // TODO: ここで隠し星座の開放条件をチェックし、開放されていれば追加
        // 企画書: 「特定の初期条件（例: チュートリアル期間中の特定の行動、隠しNPCとの交流、特定のアイテム発見など）を満たしていた場合」
        // 現状はテスト用に仮の開放条件を設定
        if (ShouldUnlockHiddenConstellations()) // 仮の開放条件チェックメソッド
        {
            if (constellationDatabase.hiddenConstellations != null)
            {
                availableConstellations.AddRange(constellationDatabase.hiddenConstellations);
                Debug.Log("隠し星座が開放されました！");
            }
        }

        // UIManagerに表示可能な星座リストを渡し、選択された際のコールバックを登録
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowConstellationSelectionUI(availableConstellations, SelectConstellation);

            isGameStarted = true;
            // 以降のゲーム開始ロジック (例: 5年間の自由な期間の開始、プレイヤー操作開始など)
            Debug.Log("ゲームが開始されます（5年間の自由期間開始）");
            // この時点で5年間のゲーム内時間が動き出す
        }
    }

    // 職業選択プロセスを開始するメソッド (5年経過後に呼ばれる)
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

        // 全ての職業をループし、プレイヤーの経験値が条件を満たしているかチェック
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

        // 少なくとも1つは選択肢があるように保証する（例: 全ての職業が条件を満たさない場合）
        if (availableVocations.Count == 0 && vocationDatabase.allVocations.Count > 0)
        {
            Debug.LogWarning("No vocations meet the player's experience requirements. Presenting all available vocations as fallback.");
            availableVocations.AddRange(vocationDatabase.allVocations); // 全ての職業を提示する例
        }
        else if (availableVocations.Count == 0) // VocationDatabase自体が空の場合
        {
            Debug.LogError("No vocations defined in VocationDatabase! Cannot present vocation choices.");
            enabled = false;
            return;
        }


        // UIManagerに表示可能な職業リストを渡し、選択された際のコールバックを登録
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowVocationSelectionUI(availableVocations, SelectVocation);
        }
    }

    // 職業が選択された時の処理 (UIManagerからのコールバック)
    void SelectVocation(VocationData selectedVocationData)
    {
        currentPlayerStatus.SetVocation(selectedVocationData); // 選択された職業データを保持
        // プレイヤーに職業ボーナスを適用する


        Time.timeScale = 1f; // ゲーム時間を再開
        Debug.Log("信託の儀が完了しました。新たな人生が始まります！");
        // ここからゲームの次の段階（冒険開始、王都への移動など）に進む
    }


    // ゲームオーバー処理 (PlayerHealthから呼ばれる)
    public void HandleGameOver()
    {
        Debug.Log("GameManager: ゲームオーバーを処理します。");
        Time.timeScale = 0f; // ゲームを一時停止
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowGameOverUI();
        }
    }

    void OnPathAnnouncementDialogueEnd()
    {
        Debug.Log("進路告知の会話が終了しました。7歳までの準備期間が始まります。");
        Time.timeScale = 1f; // ゲームを再開
        gameTimeTimer = 0f; // タイマーをリセットし、再び時間を進める
    }

    // 7歳時の進路決定ダイアログ終了時にDialogueManagerから呼ばれる
    void OnPathFinalizationDialogueEnd()
    {
        Debug.Log("進路決定の会話が終了しました。");
        Time.timeScale = 1f;
        // TODO: ここに7歳時の進路決定後のロジックを実装
    }


    // DialogueManagerから呼び出され、選択肢に応じて処理を分岐
    public void HandlePathChoice(DialogueData.DialogueOption selectedOption)
    {
        Debug.Log("GameManager: 進路選択を受け付けました。");

        if (selectedOption.optionText == "学園へ行く")
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
        // TODO: ここで実際のシーン遷移やプレイヤーへの影響を実装
    }

    // 現在のシーンをリロードする関数 (ボタンから呼ばれる)
    public void LoadCurrentScene()
    {
        Time.timeScale = 1f; // ゲーム時間スケールを元に戻す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // GameManagerがDontDestroyOnLoadでない場合、シーンロードでStartが再度呼ばれ、
        // InitializeGameStart()でUIが初期化されるため、特にここでの追加処理は不要。
    }

    // CollectItem メソッド (アイテム収集ロジックはここに維持)
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