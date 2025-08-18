using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxTrigger : MonoBehaviour
{
    // �U���͂�Inspector�Őݒ�ł���悤�ɂ���
    public int attackDamage = 15;

    // OnTriggerEnter2D �� Is Trigger ���I���̃R���C�_�[�ƐڐG�������ɌĂ΂��
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �Փ˂���GameObject��"Enemy"�^�O�������Ă��邩�m�F
        if (other.CompareTag("Enemy"))
        {
            // �G��EnemyHealth�R���|�[�l���g���擾
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            // EnemyHealth�R���|�[�l���g������΁A�_���[�W��^����
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }
}
