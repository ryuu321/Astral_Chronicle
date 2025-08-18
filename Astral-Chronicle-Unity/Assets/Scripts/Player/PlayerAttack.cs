using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 15; // 敵に与えるダメージ量
    public float attackCooldown = 0.5f; // 攻撃のクールダウン
    public GameObject attackHitbox; // ヒットボックスの参照

    private Animator animator;
    private float nextAttackTime = 0f;
    private Vector2 attackDirection; // 攻撃方向を保持する変数

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // PlayerManagerから呼び出される攻撃処理のメソッド
    public void HandleAttack()
    {
        // クールダウンが終了しているかチェック
        if (Time.time > nextAttackTime)
        {
            // マウスカーソルの位置を取得し、攻撃方向を決定
            attackDirection = GetMouseDirection();

            // アニメーションを再生（攻撃方向によってアニメーションを切り替える）
            if (animator != null)
            {
                animator.SetTrigger("Attack");
                // 攻撃方向をアニメーターに伝えるロジックを追加
                // 例: animator.SetFloat("AttackX", attackDirection.x);
                // 例: animator.SetFloat("AttackY", attackDirection.y);
            }

            // ヒットボックスを有効にする
            if (attackHitbox != null)
            {
                // 攻撃方向に合わせてヒットボックスの位置と回転を調整
                attackHitbox.transform.position = transform.position + (Vector3)attackDirection * 0.5f; // 0.5fはプレイヤーからの距離
                // 攻撃方向への回転も必要に応じて調整

                attackHitbox.SetActive(true);
                // 一定時間後にヒットボックスを無効にする
                Invoke("DisableHitbox", 0.1f); // 例: 0.1秒後に無効化
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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        return direction;
    }
}