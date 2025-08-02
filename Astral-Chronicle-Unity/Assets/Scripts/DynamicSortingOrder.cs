using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このスクリプトがSpriteRendererコンポーネントを必要とすることを示す
[RequireComponent(typeof(SpriteRenderer))]
public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Sorting Orderの計算に使用するY座標の乗数（調整用）
    // Inspectorで調整可能にする
    public float sortingMultiplier = 100f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("DynamicSortingOrder: SpriteRenderer component not found on " + gameObject.name);
            enabled = false; // SpriteRendererがない場合はスクリプトを無効にする
        }
    }

    // LateUpdateは、Updateの後に呼ばれるため、全てのオブジェクトの移動が完了した後に
    // 描画順序を調整するのに適しています。
    void LateUpdate()
    {
        if (spriteRenderer != null)
        {
            // Y座標が小さい（画面の奥にある）ほど、Sorting Orderを大きくする
            // これにより、奥のオブジェクトが手前のオブジェクトより後ろに描画される
            // intにキャストすることで整数値にする
            int newSortingOrder = -(int)(this.transform.position.y * sortingMultiplier);
            spriteRenderer.sortingOrder = newSortingOrder;
        }
    }
}
