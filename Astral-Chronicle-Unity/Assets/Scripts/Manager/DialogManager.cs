using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System; // For Action
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue Data")]
    public DialogueData pathAnnouncementDialogue;
    public DialogueData pathFinalizationDialogue;
    // Add other dialogue data here as needed

    [Header("Dialogue UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Transform optionButtonsParent;
    public GameObject optionButtonPrefab;

    [Header("Dialogue UI - Speaker")]
    public GameObject speakerPanel;
    public Image speakerIcon;
    public TextMeshProUGUI speakerNameText;

    public DialogueData currentDialogueData;
    public int currentDialogueEntryIndex;
    public Action onDialogueEndCallback;

    private PlayerControls playerControls;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerControls = new PlayerControls();
    }

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (speakerPanel != null) speakerPanel.SetActive(false);
    }

    // GameObject���L���ɂȂ����Ƃ��ɌĂяo����܂�
    private void OnEnable()
    {
        Debug.Log("enamble");
        // 'HandleInteraction' ���\�b�h�� Interact�A�N�V������ 'performed' �C�x���g�ɍw�ǂ��܂�
        // ����ɂ��A���N���b�N�Ȃǂ�Interact�o�C���f�B���O���g���K�[���ꂽ�Ƃ��ɌĂяo����܂�
        playerControls.Dialogue.Interact.performed += HandleInteraction;

        // �C���v�b�g�A�N�V������L����
        playerControls.Dialogue.Enable();
    }

    // GameObject�������ɂȂ����Ƃ��ɌĂяo����܂�
    private void OnDisable()
    {
        Debug.Log("disamble");
        // ���������[�N��h�����߂ɃC�x���g�̍w�ǂ��������܂�
        playerControls.Dialogue.Interact.performed -= HandleInteraction;

        // �C���v�b�g�A�N�V�����𖳌���
        playerControls.Dialogue.Disable();
    }

    // �C���^���N�V�������͂��������邽�߂̐V�������\�b�h
    private void HandleInteraction(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
        // �v���C���[���C���^���N�g�����Ƃ��ɂ��̃��\�b�h���Ăяo����܂�
        // �I�v�V�����i�I�����j���Ȃ��ꍇ�ɂ̂݁A���̑Θb�ɐi�ނ悤�Ƀ`�F�b�N���܂�
        if (currentDialogueData.dialogueEntries[currentDialogueEntryIndex].options == null ||
            currentDialogueData.dialogueEntries[currentDialogueEntryIndex].options.Count == 0)
        {
            EndDialogue();
        }
    }


    // Dialogue start method for age 6
    public void StartPathAnnouncementDialogue(Action onEndCallback)
    {
        StartDialogue(pathAnnouncementDialogue, onEndCallback);
    }

    // Dialogue start method for age 7
    public void StartPathFinalizationDialogue(Action onEndCallback)
    {
        StartDialogue(pathFinalizationDialogue, onEndCallback);
    }

    // Common method to start dialogue
    private void StartDialogue(DialogueData dialogueData, Action onEndCallback)
    {
        if (dialogueData == null)
        {
            onEndCallback?.Invoke();
            return;
        }

        currentDialogueData = dialogueData;
        currentDialogueEntryIndex = 1;
        onDialogueEndCallback = onEndCallback;

        DisplayCurrentDialogue();
    }

    // Method to display current dialogue
    private void DisplayCurrentDialogue()
    {
        //if (currentDialogueData == null || currentDialogueData.dialogueEntries.Length <= currentDialogueEntryIndex)
        //{
        //    EndDialogue();
        //    return;
        //}

        var currentEntry = currentDialogueData.dialogueEntries[currentDialogueEntryIndex];

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (speakerPanel != null) speakerPanel.SetActive(true);

        if (speakerNameText != null) speakerNameText.text = currentEntry.speakerName;
        if (speakerIcon != null) speakerIcon.sprite = currentEntry.speakerIcon;
        if (dialogueText != null) dialogueText.text = currentEntry.text;

        foreach (Transform child in optionButtonsParent)
        {
            Destroy(child.gameObject);
        }

        if (currentEntry.options != null)
        {
            // Options exist, create buttons
            foreach (var option in currentEntry.options)
            {
                GameObject buttonGO = Instantiate(optionButtonPrefab, optionButtonsParent);
                Button button = buttonGO.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null) buttonText.text = option.optionText;
                button.onClick.AddListener(() => OnOptionSelected(option));
            }
        }
    }


    private void OnOptionSelected(DialogueData.DialogueOption selectedOption)
    {
        if (selectedOption.isPathChoice)
        {
            GameManager.instance.HandlePathChoice(selectedOption);
        }
        currentDialogueEntryIndex = selectedOption.nextEntryIndex;
        DisplayCurrentDialogue();
    }

    private void EndDialogue()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (speakerPanel != null) speakerPanel.SetActive(false);

        onDialogueEndCallback?.Invoke();
    }
}