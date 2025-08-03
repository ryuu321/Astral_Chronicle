using UnityEngine;
using UnityEngine.UI; // UI要素用
using TMPro; // TextMeshPro用
using System.Collections.Generic; // List<T> を使うため

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // UI参照
    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public Button retryButton; // 追加: リトライボタンの参照

    [Header("Constellation Selection UI")]
    public GameObject constellationSelectionPanel;
    public Transform constellationGridParent;

    [Header("Vocation Selection UI")]
    public GameObject vocationSelectionPanel;
    public Transform vocationGridParent;

    [Header("C&V Button")]
    public GameObject conAndVocButtonPrefab;

    // 他のUI要素もここに移動
    [Header("Player Health UI")]
    public Slider healthSlider; // PlayerHealthからPlayerHealthUIに移動
    // public TextMeshProUGUI scoreText; // スコアUIが必要な場合

    [Header("ゲーム時間表示UI")]
    public TextMeshProUGUI gameTimeText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject); // UIManagerもDontDestroyOnLoadにすることが多い
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // UIの初期状態を設定
        gameOverUI.SetActive(false);
        // 修正: retryButtonも初期状態で非表示にする
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
        }
        constellationSelectionPanel.SetActive(false); // 初期は非表示に
        retryButton.onClick.AddListener(() => {
            GameManager.instance.LoadCurrentScene(); // ボタンが押されたらこの中身が実行される
        });
        // 修正: gameTimeTextも初期状態で非表示にするか、適切な初期値に設定
        if (gameTimeText != null)
        {
            gameTimeText.gameObject.SetActive(true); // ゲーム開始から表示させる
            gameTimeText.text = "Age: 0, Month: 1"; // 初期表示
        }
    }

    // ---------- ゲームオーバーUI関連 ----------
    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
        // 修正: retryButtonも表示する
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(true);
        }
    }

    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
        // 修正: retryButtonも非表示にする
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
        }
    }

    // ---------- 星座選択UI関連 ----------
    public void ShowConstellationSelectionUI(List<ConstellationData> allAvailableConstellations, System.Action<ConstellationData> onSelectCallback)
    {
        if (constellationSelectionPanel == null || constellationGridParent == null || conAndVocButtonPrefab == null)
        {
            Debug.LogError("UIManager: Constellation Selection UI references are not assigned!");
            return;
        }

        constellationSelectionPanel.SetActive(true);

        // 既存ボタンをクリア (再表示される場合のため)
        foreach (Transform child in constellationGridParent)
        {
            Destroy(child.gameObject);
        }

        // 表示可能な全ての星座データに基づいてボタンを生成
        if (allAvailableConstellations != null && allAvailableConstellations.Count > 0)
        {
            foreach (ConstellationData data in allAvailableConstellations)
            {
                GameObject buttonGO = Instantiate(conAndVocButtonPrefab, constellationGridParent);
                Button button = buttonGO.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                Image buttonIcon = buttonGO.GetComponent<Image>(); // ボタン自体のImageコンポーネント (アイコン表示用)

                if (buttonText != null) buttonText.text = data.constellationName;

                // アイコンが設定されていれば表示 (ConstellationDataにiconフィールドがある前提)
                if (buttonIcon != null && data.icon != null)
                {
                    buttonIcon.sprite = data.icon;
                }

                // ボタンクリック時にGameManagerのコールバックを呼び出す
                button.onClick.AddListener(() =>
                {
                    onSelectCallback?.Invoke(data); // GameManagerに選択を通知
                    HideConstellationSelectionUI(); // 選択後UIを隠す
                });
            }
        }
        else
        {
            Debug.LogWarning("Constellation Data List provided to UIManager is empty!");
        }
    }

    public void HideConstellationSelectionUI()
    {
        if (constellationSelectionPanel != null)
        {
            constellationSelectionPanel.SetActive(false);
        }
    }

    // 新しいメソッドを追加
    public void ShowVocationSelectionUI(List<VocationData> vocations, System.Action<VocationData> onSelectCallback)
    {
        if (vocationSelectionPanel == null || vocationGridParent == null || conAndVocButtonPrefab == null)
        {
            Debug.LogError("UIManager: Vocation Selection UI references are not assigned!");
            return;
        }

        vocationSelectionPanel.SetActive(true);

        // 既存ボタンをクリア
        foreach (Transform child in vocationGridParent)
        {
            Destroy(child.gameObject);
        }

        // ボタン生成
        if (vocations != null && vocations.Count > 0)
        {
            foreach (VocationData data in vocations)
            {
                GameObject buttonGO = Instantiate(conAndVocButtonPrefab, vocationGridParent);
                Button button = buttonGO.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                Image buttonIcon = buttonGO.GetComponent<Image>(); // アイコン表示用

                if (buttonText != null) buttonText.text = data.vocationName;
                if (buttonIcon != null && data.icon != null)
                {
                    buttonIcon.sprite = data.icon;
                }

                button.onClick.AddListener(() =>
                {
                    onSelectCallback?.Invoke(data); // GameManagerに選択を通知
                    HideVocationSelectionUI(); // 選択後UIを隠す
                });
            }
        }
        else
        {
            Debug.LogWarning("Vocation Data List provided to UIManager is empty!");
        }
    }

    public void HideVocationSelectionUI()
    {
        if (vocationSelectionPanel != null)
        {
            vocationSelectionPanel.SetActive(false);
        }
    }

    // ---------- 体力UI関連 ----------
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // ---------- 時間UI関連 ----------
    public void UpdateGameTimeDisplay(int year, int month)
    {
        if (gameTimeText != null)
        {
            gameTimeText.text = $"Age: {year}, Month: {month + 1}"; // 月は0-11なので+1する
        }
    }

    // スコアUIの更新などもここに追加
    // public void UpdateScoreDisplay(int score) { /* ... */ }
}