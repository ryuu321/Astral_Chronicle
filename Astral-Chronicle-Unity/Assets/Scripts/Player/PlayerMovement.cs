using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HandleMoveInput(Vector2 moveVector)
    {
        // プレイヤーの移動処理
        if (moveVector.x < 0) // 左方向へ移動していれば
        {
            spriteRenderer.flipX = true;
        }
        else if (moveVector.x > 0) // 右方向へ移動していれば
        {
            spriteRenderer.flipX = false;
        }
        Vector2 movement = moveVector * moveSpeed;
        rb.linearVelocity = movement;
    }
}

// //--- ここから追加: 描画順序の動的な調整 ---
// //描画順序は、キャラクターのY座標に基づいて動的に変更することで、
// //奥にあるものが手前にあるものより「奥に描画」されるように見せます。
//void LateUpdate()
//{
//    // SpriteRendererを取得（既にStartで取得済みならそこから使う）
//    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
//    if (spriteRenderer != null)
//    {
//        // Y座標が小さい（画面の奥にある）ほど、Sorting Orderを大きくする
//        // 例えば、-10000から0までの範囲でMappingする
//        // 具体的な範囲はゲームのY座標範囲に合わせて調整
//        int sortingOrder = -(int)(transform.position.y * 100);
//        spriteRenderer.sortingOrder = sortingOrder;
//    }
//}
