using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 15; // 敵に与えるダメージ量
    public float attackCooldown = 0.5f; // 攻撃のクールダウン
    public GameObject attackHitbox; // ヒットボックスの参照
    public GameObject pipod; 

    private float nextAttackTime = 0f;
    public Vector2 attackDirection; // 攻撃方向を保持する変数
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // PlayerManagerから呼び出される攻撃処理のメソッド
    public void HandleAttack()
    {
        // クールダウンが終了しているかチェック
        if (Time.time > nextAttackTime)
        {
            // マウスカーソルの位置を取得し、攻撃方向を決定
            attackDirection = GetMouseDirection();

            //// アニメーションを再生（攻撃方向によってアニメーションを切り替える）
            //if (animator != null)
            //{
                
            //    // 攻撃方向をアニメーターに伝えるロジックを追加
            //    // 例: animator.SetFloat("AttackX", attackDirection.x);
            //    // 例: animator.SetFloat("AttackY", attackDirection.y);
            //}

            // ヒットボックスを有効にする
            if (attackHitbox != null)
            {
                // 攻撃方向に合わせてヒットボックスの位置と回転を調整
                attackHitbox.transform.position = pipod.transform.position + (Vector3)attackDirection * 0.5f; // 0.5fはプレイヤーからの距離
                // 攻撃方向への回転も必要に応じて調整

                attackHitbox.SetActive(true);
                // 一定時間後にヒットボックスを無効にする
                Invoke("DisableHitbox", 0.05f); // 例: 0.1秒後に無効化
            }

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.sfxSource, AudioManager.instance.sfxAttack);
            }

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    // マウスカーソルの方向を計算するメソッド
    Vector2 GetMouseDirection()
    {
        Vector2 direction;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction.x = (mousePosition.x - pipod.transform.position.x);
        direction.y = (mousePosition.y - pipod.transform.position.y-5);
        direction = direction.normalized;
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
        return direction;
    }
}