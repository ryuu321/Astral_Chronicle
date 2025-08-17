using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Game Data/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    // DialogueData.cs �� DialogueEntry �\����
    [System.Serializable]
    public struct DialogueEntry
    {
        public string speakerName; // �ǉ�: �b������̖��O
        public Sprite speakerIcon; // �ǉ�: �b������̃A�C�R��

        [TextArea(3, 5)]
        public string text; // NPC�̃Z���t
        public List<DialogueOption> options;
    }

    [System.Serializable]
    public struct DialogueOption
    {
        public string optionText; // �I�����̃e�L�X�g (��: �u�͂��v)
        public int nextEntryIndex; // ���̑I������I�񂾌�ɕ\�����鎟�̃Z���t�̃C���f�b�N�X
        public bool isPathChoice;  // ���ꂪ�i�H�I���̕���ł��邩
        // TODO: �i�H�I���̎�� (�`����, �Ӌ��Ȃ�) ���`���� Enum ��ǉ�
    }


    public DialogueEntry[] dialogueEntries; // �Z���t�̃��X�g
}
