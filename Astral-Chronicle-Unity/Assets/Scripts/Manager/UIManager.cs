using UnityEngine;
using UnityEngine.UI; // UI�v�f�p
using TMPro; // TextMeshPro�p
using System.Collections.Generic; // List<T> ���g������

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    // UI�Q��
    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public Button retryButton; // �ǉ�: ���g���C�{�^���̎Q��

    [Header("Constellation Selection UI")]
    public GameObject constellationSelectionPanel;
    public Transform constellationGridParent;

    [Header("Vocation Selection UI")]
    public GameObject vocationSelectionPanel;
    public Transform vocationGridParent;

    [Header("C&V Button")]
    public GameObject conAndVocButtonPrefab;

    // ����UI�v�f�������Ɉړ�
    [Header("Player Health UI")]
    public Slider healthSlider; // PlayerHealth����PlayerHealthUI�Ɉړ�
    // public TextMeshProUGUI scoreText; // �X�R�AUI���K�v�ȏꍇ

    [Header("�Q�[�����ԕ\��UI")]
    public TextMeshProUGUI gameTimeText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject); // UIManager��DontDestroyOnLoad�ɂ��邱�Ƃ�����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // UI�̏�����Ԃ�ݒ�
        gameOverUI.SetActive(false);
        // �C��: retryButton��������ԂŔ�\���ɂ���
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
        }
        constellationSelectionPanel.SetActive(false); // �����͔�\����
        retryButton.onClick.AddListener(() => {
            GameManager.instance.LoadCurrentScene(); // �{�^���������ꂽ�炱�̒��g�����s�����
        });
        // �C��: gameTimeText��������ԂŔ�\���ɂ��邩�A�K�؂ȏ����l�ɐݒ�
        if (gameTimeText != null)
        {
            gameTimeText.gameObject.SetActive(true); // �Q�[���J�n����\��������
            gameTimeText.text = "Age: 0, Month: 1"; // �����\��
        }
    }

    // ---------- �Q�[���I�[�o�[UI�֘A ----------
    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
        // �C��: retryButton���\������
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(true);
        }
    }

    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
        // �C��: retryButton����\���ɂ���
        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(false);
        }
    }

    // ---------- �����I��UI�֘A ----------
    public void ShowConstellationSelectionUI(List<ConstellationData> allAvailableConstellations, System.Action<ConstellationData> onSelectCallback)
    {
        if (constellationSelectionPanel == null || constellationGridParent == null || conAndVocButtonPrefab == null)
        {
            Debug.LogError("UIManager: Constellation Selection UI references are not assigned!");
            return;
        }

        constellationSelectionPanel.SetActive(true);

        // �����{�^�����N���A (�ĕ\�������ꍇ�̂���)
        foreach (Transform child in constellationGridParent)
        {
            Destroy(child.gameObject);
        }

        // �\���\�ȑS�Ă̐����f�[�^�Ɋ�Â��ă{�^���𐶐�
        if (allAvailableConstellations != null && allAvailableConstellations.Count > 0)
        {
            foreach (ConstellationData data in allAvailableConstellations)
            {
                GameObject buttonGO = Instantiate(conAndVocButtonPrefab, constellationGridParent);
                Button button = buttonGO.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                Image buttonIcon = buttonGO.GetComponent<Image>(); // �{�^�����̂�Image�R���|�[�l���g (�A�C�R���\���p)

                if (buttonText != null) buttonText.text = data.constellationName;

                // �A�C�R�����ݒ肳��Ă���Ε\�� (ConstellationData��icon�t�B�[���h������O��)
                if (buttonIcon != null && data.icon != null)
                {
                    buttonIcon.sprite = data.icon;
                }

                // �{�^���N���b�N����GameManager�̃R�[���o�b�N���Ăяo��
                button.onClick.AddListener(() =>
                {
                    onSelectCallback?.Invoke(data); // GameManager�ɑI����ʒm
                    HideConstellationSelectionUI(); // �I����UI���B��
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

    // �V�������\�b�h��ǉ�
    public void ShowVocationSelectionUI(List<VocationData> vocations, System.Action<VocationData> onSelectCallback)
    {
        if (vocationSelectionPanel == null || vocationGridParent == null || conAndVocButtonPrefab == null)
        {
            Debug.LogError("UIManager: Vocation Selection UI references are not assigned!");
            return;
        }

        vocationSelectionPanel.SetActive(true);

        // �����{�^�����N���A
        foreach (Transform child in vocationGridParent)
        {
            Destroy(child.gameObject);
        }

        // �{�^������
        if (vocations != null && vocations.Count > 0)
        {
            foreach (VocationData data in vocations)
            {
                GameObject buttonGO = Instantiate(conAndVocButtonPrefab, vocationGridParent);
                Button button = buttonGO.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
                Image buttonIcon = buttonGO.GetComponent<Image>(); // �A�C�R���\���p

                if (buttonText != null) buttonText.text = data.vocationName;
                if (buttonIcon != null && data.icon != null)
                {
                    buttonIcon.sprite = data.icon;
                }

                button.onClick.AddListener(() =>
                {
                    onSelectCallback?.Invoke(data); // GameManager�ɑI����ʒm
                    HideVocationSelectionUI(); // �I����UI���B��
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

    // ---------- �̗�UI�֘A ----------
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // ---------- ����UI�֘A ----------
    public void UpdateGameTimeDisplay(int year, int month)
    {
        if (gameTimeText != null)
        {
            gameTimeText.text = $"Age: {year}, Month: {month + 1}"; // ����0-11�Ȃ̂�+1����
        }
    }

    // �X�R�AUI�̍X�V�Ȃǂ������ɒǉ�
    // public void UpdateScoreDisplay(int score) { /* ... */ }
}