using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UnityエディタのCreateメニューからこのアセットを作成できるようにする
[CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject
{
    public ItemType itemType; // アイテムの種類
    public int value;         // アイテムが持つ値（例: コインならスコア、ポーションなら回復量）
    public Sprite itemSprite; // アイテムの見た目（アイコンなど）

    // アイテム収集時の効果（オプション: メッセージ表示など）
    public string collectMessage = "";
}
