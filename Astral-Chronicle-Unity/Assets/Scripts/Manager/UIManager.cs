using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Dont't Destroy")]
    public Canvas canvas_m;
    public GameObject player;

    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public Button retryButton;

    [Header("Constellation Selection UI")]
    public GameObject constellationSelectionPanel;
    public Transform constellationGridParent;
    public GameObject constellationButtonPrefab;

    [Header("Vocation Selection UI")]
    public GameObject vocationSelectionPanel;
    public Transform vocationGridParent;
    public GameObject vocationButtonPrefab;

    [Header("Player Health UI")]
    public Slider healthSlider;
    public TextMeshProUGUI gameTimeText;

    [Header("Character Creation UI")]
    public GameObject characterCreationPanel;
    public TMP_InputField nameInputField;
    // 各パーツの選択ボタンやプレビューUIの参照を追加
    public Button hairNextButton;
    public Button hairPrevButton;

    public PlayerAppearanceController appearanceController;
    // データベースへの参照
    public PlayerAppearanceDatabase appearanceDatabase;

    private int currentHairIndex = 0;

    // 修正: DialogueUI関連の参照を削除
    // [Header("Dialogue UI")]
    // public GameObject dialoguePanel;
    // ... 他のDialogueUI関連の参照も削除

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(canvas_m);
            DontDestroyOnLoad(player);
        }
        else
        {
            Destroy(gameObject);
            Destroy(canvas_m);
            Destroy(player);
        }
    }

    void Start()
    {
        gameOverUI.SetActive(false);
        if (retryButton != null) retryButton.gameObject.SetActive(false);
        if (constellationSelectionPanel != null) constellationSelectionPanel.SetActive(false);
        if (vocationSelectionPanel != null) vocationSelectionPanel.SetActive(false);
        if (gameTimeText != null) gameTimeText.gameObject.SetActive(true);
        if (appearanceController != null && appearanceDatabase != null)
        {
            // UIのボタンにイベントを登録
            hairNextButton.onClick.AddListener(NextHairStyle);
            hairPrevButton.onClick.AddListener(PrevHairStyle);
        }
    }

    public void ShowGameOverUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);
        if (retryButton != null) retryButton.gameObject.SetActive(true);
        retryButton.onClick.AddListener(() =>
        { 
            GameManager.instance.LoadCurrentScene();
            HideGameOverUI();
        });
    }

    public void HideGameOverUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (retryButton != null) retryButton.gameObject.SetActive(false);
    }

    public void ShowConstellationSelectionUI(List<ConstellationData> allAvailableConstellations, System.Action<ConstellationData> onSelectCallback)
    {
        if (constellationSelectionPanel == null || constellationGridParent == null || constellationButtonPrefab == null)
        {
            Debug.LogError("UIManager: Constellation Selection UI references are not assigned!");
            return;
        }

        constellationSelectionPanel.SetActive(true);
        foreach (Transform child in constellationGridParent)
        {
            Destroy(child.gameObject);
        }

        if (allAvailableConstellations != null && allAvailableConstellations.Count > 0)
        {
            foreach (ConstellationData data in allAvailableConstellations)
            {
                GameObject buttonGO = Instantiate(constellationButtonPrefab, constellationGridParent);
                Button button = buttonGO.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null) buttonText.text = data.constellationName;

                button.onClick.AddListener(() =>
                {
                    onSelectCallback?.Invoke(data);
                    HideConstellationSelectionUI();
                });
            }
        }
    }

    public void HideConstellationSelectionUI()
    {
        if (constellationSelectionPanel != null) constellationSelectionPanel.SetActive(false);
    }

    public void ShowVocationSelectionUI(List<VocationData> vocations, System.Action<VocationData> onSelectCallback)
    {
        if (vocationSelectionPanel != null)
        {
            vocationSelectionPanel.SetActive(true);
            foreach (Transform child in vocationGridParent)
            {
                Destroy(child.gameObject);
            }

            if (vocations != null && vocations.Count > 0)
            {
                foreach (VocationData data in vocations)
                {
                    GameObject buttonGO = Instantiate(vocationButtonPrefab, vocationGridParent);
                    Button button = buttonGO.GetComponent<Button>();
                    TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                    if (buttonText != null) buttonText.text = data.vocationName;

                    button.onClick.AddListener(() =>
                    {
                        onSelectCallback?.Invoke(data);
                        HideVocationSelectionUI();
                    });
                }
            }
        }
    }

    public void HideVocationSelectionUI()
    {
        if (vocationSelectionPanel != null) vocationSelectionPanel.SetActive(false);
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void UpdateGameTimeDisplay(int year, int month, int day)
    {
        if (gameTimeText != null)
        {
            gameTimeText.text = $"Age: {year}, Month: {month}, Day: {day}";
        }
    }

    public void NextHairStyle()
    {
        currentHairIndex = (currentHairIndex + 1) % appearanceDatabase.hairStyles.Count;
        appearanceController.UpdateAppearance(appearanceDatabase.hairStyles[currentHairIndex], null, null);
    }

    public void PrevHairStyle()
    {
        currentHairIndex = (currentHairIndex - 1 + appearanceDatabase.hairStyles.Count) % appearanceDatabase.hairStyles.Count;
        appearanceController.UpdateAppearance(appearanceDatabase.hairStyles[currentHairIndex], null, null);
    }
}