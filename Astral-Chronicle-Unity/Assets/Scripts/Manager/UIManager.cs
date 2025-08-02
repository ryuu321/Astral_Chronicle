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
    public GameObject constellationButtonPrefab;

    // ����UI�v�f�������Ɉړ�
    [Header("Player Health UI")]
    public Slider healthSlider; // PlayerHealth����PlayerHealthUI�Ɉړ�
    // public TextMeshProUGUI scoreText; // �X�R�AUI���K�v�ȏꍇ

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
        if (constellationSelectionPanel == null || constellationGridParent == null || constellationButtonPrefab == null)
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
                GameObject buttonGO = Instantiate(constellationButtonPrefab, constellationGridParent);
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

    // ---------- �̗�UI�֘A ----------
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // �X�R�AUI�̍X�V�Ȃǂ������ɒǉ�
    // public void UpdateScoreDisplay(int score) { /* ... */ }
}