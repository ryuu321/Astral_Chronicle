using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 15; // �G�ɗ^����_���[�W��
    public float attackCooldown = 0.5f; // �U���̃N�[���_�E��
    public GameObject attackHitbox; // �q�b�g�{�b�N�X�̎Q��

    private Animator animator;
    private float nextAttackTime = 0f;
    private Vector2 attackDirection; // �U��������ێ�����ϐ�

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // PlayerManager����Ăяo�����U�������̃��\�b�h
    public void HandleAttack()
    {
        // �N�[���_�E�����I�����Ă��邩�`�F�b�N
        if (Time.time > nextAttackTime)
        {
            // �}�E�X�J�[�\���̈ʒu���擾���A�U������������
            attackDirection = GetMouseDirection();

            // �A�j���[�V�������Đ��i�U�������ɂ���ăA�j���[�V������؂�ւ���j
            if (animator != null)
            {
                animator.SetTrigger("Attack");
                // �U���������A�j���[�^�[�ɓ`���郍�W�b�N��ǉ�
                // ��: animator.SetFloat("AttackX", attackDirection.x);
                // ��: animator.SetFloat("AttackY", attackDirection.y);
            }

            // �q�b�g�{�b�N�X��L���ɂ���
            if (attackHitbox != null)
            {
                // �U�������ɍ��킹�ăq�b�g�{�b�N�X�̈ʒu�Ɖ�]�𒲐�
                attackHitbox.transform.position = transform.position + (Vector3)attackDirection * 0.5f; // 0.5f�̓v���C���[����̋���
                // �U�������ւ̉�]���K�v�ɉ����Ē���

                attackHitbox.SetActive(true);
                // ��莞�Ԍ�Ƀq�b�g�{�b�N�X�𖳌��ɂ���
                Invoke("DisableHitbox", 0.1f); // ��: 0.1�b��ɖ�����
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

    // �}�E�X�J�[�\���̕������v�Z���郁�\�b�h
    Vector2 GetMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        return direction;
    }
}