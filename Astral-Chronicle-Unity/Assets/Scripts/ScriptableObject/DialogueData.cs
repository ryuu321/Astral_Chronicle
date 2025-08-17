using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Game Data/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    // DialogueData.cs の DialogueEntry 構造体
    [System.Serializable]
    public struct DialogueEntry
    {
        public string speakerName; // 追加: 話し相手の名前
        public Sprite speakerIcon; // 追加: 話し相手のアイコン

        [TextArea(3, 5)]
        public string text; // NPCのセリフ
        public List<DialogueOption> options;
    }

    [System.Serializable]
    public struct DialogueOption
    {
        public string optionText; // 選択肢のテキスト (例: 「はい」)
        public int nextEntryIndex; // この選択肢を選んだ後に表示する次のセリフのインデックス
        public bool isPathChoice;  // これが進路選択の分岐であるか
        // TODO: 進路選択の種類 (冒険者, 辺境など) を定義する Enum を追加
    }


    public DialogueEntry[] dialogueEntries; // セリフのリスト
}
