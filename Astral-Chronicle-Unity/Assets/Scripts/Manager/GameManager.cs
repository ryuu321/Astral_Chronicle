using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic; // List<T> を使うため

public class GameManager : MonoBehaviour
{
    // UIManagerが管理するUI参照はGameManagerから削除済みであることを確認

    [Header("Game Data")]
    // 修正: List<ConstellationData> の代わりに ConstellationDatabase を参照
    public ConstellationDatabase constellationDatabase;

    public static GameManager instance;

    public ConstellationData selectedConstellation { get; private set; }

    private PlayerHealth currentPlayerHealth;
    private PlayerStatus currentPlayerStatus; // 追加: PlayerStatusへの参照


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
                Debug.LogWarning("PlayerStatus component not found on Player object! Player stats will not be affected by constellation.");
            }
        }
        else
        {
            Debug.LogWarning("Player object with 'Player' tag not found in scene!");
        }

        // 星座選択UIの表示プロセスを開始
        StartConstellationSelectionProcess();
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
        // ここからチュートリアル後のゲーム進行が始まる
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