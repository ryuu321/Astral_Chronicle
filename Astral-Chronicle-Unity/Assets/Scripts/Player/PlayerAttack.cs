using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 15; // �G�ɗ^����_���[�W��
    public float attackCooldown = 0.5f; // �U���̃N�[���_�E��
    public GameObject attackHitbox; // �q�b�g�{�b�N�X�̎Q��
    public GameObject pipod; 

    private float nextAttackTime = 0f;
    public Vector2 attackDirection; // �U��������ێ�����ϐ�
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // PlayerManager����Ăяo�����U�������̃��\�b�h
    public void HandleAttack()
    {
        // �N�[���_�E�����I�����Ă��邩�`�F�b�N
        if (Time.time > nextAttackTime)
        {
            // �}�E�X�J�[�\���̈ʒu���擾���A�U������������
            attackDirection = GetMouseDirection();

            //// �A�j���[�V�������Đ��i�U�������ɂ���ăA�j���[�V������؂�ւ���j
            //if (animator != null)
            //{
                
            //    // �U���������A�j���[�^�[�ɓ`���郍�W�b�N��ǉ�
            //    // ��: animator.SetFloat("AttackX", attackDirection.x);
            //    // ��: animator.SetFloat("AttackY", attackDirection.y);
            //}

            // �q�b�g�{�b�N�X��L���ɂ���
            if (attackHitbox != null)
            {
                // �U�������ɍ��킹�ăq�b�g�{�b�N�X�̈ʒu�Ɖ�]�𒲐�
                attackHitbox.transform.position = pipod.transform.position + (Vector3)attackDirection * 0.5f; // 0.5f�̓v���C���[����̋���
                // �U�������ւ̉�]���K�v�ɉ����Ē���

                attackHitbox.SetActive(true);
                // ��莞�Ԍ�Ƀq�b�g�{�b�N�X�𖳌��ɂ���
                Invoke("DisableHitbox", 0.05f); // ��: 0.1�b��ɖ�����
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

    // �}�E�X�J�[�\���̕������v�Z���郁�\�b�h
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