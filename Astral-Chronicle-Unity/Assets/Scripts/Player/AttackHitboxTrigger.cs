using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxTrigger : MonoBehaviour
{
    // 攻撃力をInspectorで設定できるようにする
    public int attackDamage = 15;

    // OnTriggerEnter2D は Is Trigger がオンのコライダーと接触した時に呼ばれる
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したGameObjectが"Enemy"タグを持っているか確認
        if (other.CompareTag("Enemy"))
        {
            // 敵のEnemyHealthコンポーネントを取得
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            // EnemyHealthコンポーネントがあれば、ダメージを与える
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }
}
