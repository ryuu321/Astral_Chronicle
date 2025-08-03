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


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoadは、GameManagerがシーン遷移後もデータを保持する必要がある場合に検討
            // 今回はシーンリロードでGameManagerも再生成される前提とする（ハイスコア機能がないため）
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

        // 星座選択UIの表示プロセスを開始
        //StartConstellationSelectionProcess();
        StartVocationSelectionProcess();
    }

    // ゲーム時間管理 (以前の追加分)
    [Header("ゲーム時間管理")]
    public float timePerGameMonth = 5f; // 現実世界の5秒でゲーム内1ヶ月経過 (調整可能)
    private float gameTimeTimer = 0f;
    private int currentMonth = 0; // 現在のゲーム内月数
    private int currentYear = 0;  // 現在のゲーム内年数

    [Header("幼少期設定")]
    public int childhoodYears = 5; // 幼少期の期間 (ゲーム内年数)

    // Update メソッド (ゲーム時間進行と信託の儀呼び出し)
    void Update()
    {
        // ゲームが停止中でない場合のみ時間を進める
        if (Time.timeScale > 0f)
        {
            gameTimeTimer += Time.deltaTime; // 現実の時間を加算

            if (gameTimeTimer >= timePerGameMonth)
            {
                gameTimeTimer -= timePerGameMonth; // タイマーをリセット
                currentMonth++; // 月を進める

                if (currentMonth >= 12) // 12ヶ月で1年
                {
                    currentMonth = 1;
                    currentYear++; // 年を進める
                                   // Debug.Log("ゲーム内時間: " + currentYear + "歳 " + (currentMonth + 1) + "ヶ月"); // デバッグログはUI表示に置き換える

                    // UIの更新
                    if (UIManager.instance != null)
                    {
                        UIManager.instance.UpdateGameTimeDisplay(currentYear, currentMonth);
                    }

                    // 5年経過チェック (幼少期終了)
                    if (currentYear >= childhoodYears)
                    {
                        Debug.Log("5歳になりました！信託の儀の準備をします。");
                        Time.timeScale = 0f; // ゲーム時間を停止

                        // 幼少期タイマーを停止させるため、非常に大きな値にするか、boolフラグで制御
                        // これにより、職業選択中に時間が進むのを防ぎ、一度しか呼ばれないようにする
                        gameTimeTimer = float.MaxValue; // これでこれ以上Updateが条件を満たさないようにする
                                                        // あるいは bool childhoodEnded = true; If (!childhoodEnded) { ... } とする

                        StartVocationSelectionProcess(); // 職業選択プロセスを開始
                    }
                }
            }
        }
    }


    // 星座選択プロセスを開始するメソッド (チュートリアルから呼び出される想定)
    void StartConstellationSelectionProcess()
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
        }
    }

    // 仮の隠し星座開放条件 (テスト用)
    // 実際のゲームでは、プレイヤーのパラメータやフラグに基づいて複雑なロジックを記述
    bool ShouldUnlockHiddenConstellations()
    {
        // 例: (デバッグ目的で常にtrueにするか、特定のボタンを押した時など)
        return false; // 最初は常にfalseで隠し星座は表示しない
    }


    // 星座が選択された時の処理 (UIManagerからのコールバック)
    void SelectConstellation(ConstellationData selectedConstellationData)
    {
        this.selectedConstellation = selectedConstellationData; // 選択された星座データを保持
        Debug.Log(selectedConstellationData.constellationName + "が選択されました！");

        // プレイヤーにバフを適用する
        ApplyConstellationBuff(selectedConstellationData);

        // 以降のゲーム開始ロジック (例: 5年間の自由な期間の開始、プレイヤー操作開始など)
        Debug.Log("ゲームが開始されます（5年間の自由期間開始）");
        // この時点で5年間のゲーム内時間が動き出す
    }

    // 選択された星座のバフをプレイヤーに適用するメソッド
    void ApplyConstellationBuff(ConstellationData constellation)
    {
        if (currentPlayerStatus != null) // PlayerStatusが存在すれば適用
        {
            Debug.Log("星座バフ適用: " + constellation.constellationName +
                      " 筋力+" + constellation.initialStrengthBonus + ", 器用さ+" + constellation.initialDexterityBonus +
                      ", 知力+" + constellation.initialIntelligenceBonus + ", 生命力+" + constellation.initialVitalityBonus);

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
        this.selectedVocation = selectedVocationData; // 選択された職業データを保持
        Debug.Log(selectedVocationData.vocationName + "が選択されました！");

        // プレイヤーに職業ボーナスを適用する
        ApplyVocationBonus(selectedVocationData);

        Time.timeScale = 1f; // ゲーム時間を再開
        Debug.Log("信託の儀が完了しました。新たな人生が始まります！");
        // ここからゲームの次の段階（冒険開始、王都への移動など）に進む
    }

    // 選択された職業のボーナスをプレイヤーに適用するメソッド
    void ApplyVocationBonus(VocationData vocation)
    {
        if (currentPlayerStatus != null)
        {
            Debug.Log("職業ボーナス適用: " + vocation.vocationName +
                      " 筋力+" + vocation.strengthBonus + ", 器用さ+" + vocation.dexterityBonus +
                      ", 知力+" + vocation.intelligenceBonus + ", 生命力+" + vocation.vitalityBonus);

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
}