using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f; // 敵の移動速度
    public float detectionRange = 5f; // プレイヤーを検知する範囲
    public float stopDistance = 0.1f; // プレイヤーにどれくらい近づいたら止まるか

    private Transform playerTransform; // プレイヤーのTransformへの参照
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // 敵のスプライトを反転させるため

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // シーン内の"Player"タグを持つオブジェクトを探す
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("EnemyAI: Player object not found! Make sure your player has the 'Player' tag.");
            enabled = false; // プレイヤーが見つからない場合はAIを無効にする
        }

        if (rb == null || spriteRenderer == null)
        {
            Debug.LogError("EnemyAI: Missing required components (Rigidbody2D or SpriteRenderer) on " + gameObject.name);
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // プレイヤーと敵の距離を計算
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // プレイヤーが検知範囲内にいるかチェック
        if (distanceToPlayer <= detectionRange)
        {
            // プレイヤーに十分近づいていない場合のみ移動
            if (distanceToPlayer > stopDistance)
            {
                // プレイヤーの方向へ移動ベクトルを計算
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

                // Rigidbody2Dを使って移動
                rb.linearVelocity = directionToPlayer * moveSpeed;

                // 敵のスプライトの左右反転（プレイヤーと同じロジック）
                if (directionToPlayer.x < 0) // 左方向へ移動していれば
                {
                    spriteRenderer.flipX = true;
                }
                else if (directionToPlayer.x > 0) // 右方向へ移動していれば
                {
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                // プレイヤーに十分近づいたら停止
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            // プレイヤーが検知範囲外なら停止
            rb.velocity = Vector2.zero;
        }
    }

    // 敵がプレイヤーに触れたときの処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトが"Player"タグを持っているか確認
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーのPlayerHealthコンポーネントを取得
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // PlayerHealthコンポーネントが存在するか確認
            if (playerHealth != null)
            {
                // プレイヤーにダメージを与える
                int damageAmount = 10; // ここで与えるダメージ量を設定（例: 10ダメージ）
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("敵がプレイヤーに接触し、" + damageAmount + " ダメージを与えました。");
            }
            else
            {
                Debug.LogWarning("PlayerHealthコンポーネントがプレイヤーオブジェクトに見つかりません！");
            }
        }
    }

    // 描画順序の動的な調整はDynamicSortingOrderスクリプトに任せる
    // このスクリプトでは特に処理しない
}
