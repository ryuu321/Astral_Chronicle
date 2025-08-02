using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    // このアイテムがどのItemDataを持つかをInspectorで設定
    public ItemData itemData;

    // OnTriggerEnter2Dは、他のCollider2Dと「Is Trigger」状態で衝突したときに呼ばれる
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したオブジェクトのタグが"Player"であるかを確認
        if (other.CompareTag("Player"))
        {
            // GameManagerのインスタンスにアクセスし、汎用的なアイテム収集関数を呼び出す
            if (GameManager.instance != null && itemData != null)
            {
                GameManager.instance.CollectItem(itemData);
            }

            // このアイテムオブジェクトをシーンから削除
            Destroy(gameObject);
        }
    }
}
