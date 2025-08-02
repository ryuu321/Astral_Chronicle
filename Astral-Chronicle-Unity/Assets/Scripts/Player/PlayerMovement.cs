using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 左右と前後（奥ゆき）の移動速度
    public float jumpForce = 8f; // ジャンプの力（斜め見下ろしではZ軸方向の動きを想定）

    private Rigidbody2D rb; // Rigidbody2Dコンポーネントへの参照
    public bool isGrounded; // 地面に接地しているかどうかのフラグ

    // Start関数は、スクリプトが最初に有効になったときに一度だけ呼ばれます
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody2D component not found!");
            enabled = false;
        }
    }

    // FixedUpdate関数は、固定されたフレームレートで物理演算の更新時に呼ばれます
    void FixedUpdate()
    {
        // X軸（左右）の入力値を取得
        float horizontalInput = Input.GetAxis("Horizontal");
        // Y軸（前後・奥ゆき）の入力値を取得
        float verticalInput = Input.GetAxis("Vertical");

        // Rigidbody2DのVelocity（速度）を計算
        // 斜め見下ろしなので、Y軸の入力はY軸の動き（奥ゆき）に変換されます
        // ジャンプの動きは物理演算に任せるため、rb.velocity.z はここでは変更しない
        // 2Dなので、Z軸は物理的にはありません。Y軸が奥ゆきを兼ねる
        Vector2 movement = new Vector2(horizontalInput, verticalInput) * moveSpeed;

        // 既存のジャンプ速度は維持しつつ、水平方向の速度を更新
        // Rigidbody2DはXY平面で動くため、Z方向の動きはY軸に重なります。
        // ただし、見た目の「高さ」のジャンプはY軸速度を使うのではなく、
        // 視覚的なZ座標の操作（Sorting Order）や見た目のスケーリングで表現することが多いです。
        // ここでは、一般的な2DジャンプとしてY軸速度を使いますが、
        // 後の描画順序の調整で「高さ」を表現します。
        rb.velocity = new Vector2(movement.x, movement.y);
    }

    // Update関数は、フレームごとに一度呼ばれます
    void Update()
    {
        // スペースキーが押されたときにジャンプ（高さ方向への移動）
        // 斜め見下ろしでジャンプは、キャラクターを上に動かすというよりは
        // 視覚的に「上に飛び上がる」ように見せる工夫が必要です。
        // ここではまだ単純なY軸（画面上方向）へのジャンプとして扱います。
        // 実際のジャンプは物理的な高さ（Z軸）の変化と見せかけたいので、
        // 2D物理ではY軸のVelocityで調整し、Sort Orderを工夫します。
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // 上向きの力を加えてジャンプさせます。
            // 垂直方向の移動はY軸入力とは別に制御します。
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
            isGrounded = false; // ジャンプしたので接地状態を解除
        }
    }

    // OnCollisionEnter2Dは、他のCollider2Dと衝突したときに呼ばれます
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトのタグが"Ground"の場合、接地状態にする
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // OnCollisionExit2Dは、他のCollider2Dとの衝突が終了したときに呼ばれます
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 衝突が終了したオブジェクトのタグが"Ground"の場合、接地状態を解除
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

     //--- ここから追加: 描画順序の動的な調整 ---
     //描画順序は、キャラクターのY座標に基づいて動的に変更することで、
     //奥にあるものが手前にあるものより「奥に描画」されるように見せます。
    void LateUpdate()
    {
        // SpriteRendererを取得（既にStartで取得済みならそこから使う）
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Y座標が小さい（画面の奥にある）ほど、Sorting Orderを大きくする
            // 例えば、-10000から0までの範囲でMappingする
            // 具体的な範囲はゲームのY座標範囲に合わせて調整
            int sortingOrder = -(int)(transform.position.y * 100);
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}