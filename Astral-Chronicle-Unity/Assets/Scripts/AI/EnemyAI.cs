using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f; // �G�̈ړ����x
    public float detectionRange = 5f; // �v���C���[�����m����͈�
    public float stopDistance = 0.1f; // �v���C���[�ɂǂꂭ�炢�߂Â�����~�܂邩

    private Transform playerTransform; // �v���C���[��Transform�ւ̎Q��
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // �G�̃X�v���C�g�𔽓]�����邽��

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �V�[������"Player"�^�O�����I�u�W�F�N�g��T��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("EnemyAI: Player object not found! Make sure your player has the 'Player' tag.");
            enabled = false; // �v���C���[��������Ȃ��ꍇ��AI�𖳌��ɂ���
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

        // �v���C���[�ƓG�̋������v�Z
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // �v���C���[�����m�͈͓��ɂ��邩�`�F�b�N
        if (distanceToPlayer <= detectionRange)
        {
            // �v���C���[�ɏ\���߂Â��Ă��Ȃ��ꍇ�݈̂ړ�
            if (distanceToPlayer > stopDistance)
            {
                // �v���C���[�̕����ֈړ��x�N�g�����v�Z
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

                // Rigidbody2D���g���Ĉړ�
                rb.velocity = directionToPlayer * moveSpeed;

                // �G�̃X�v���C�g�̍��E���]�i�v���C���[�Ɠ������W�b�N�j
                if (directionToPlayer.x < 0) // �������ֈړ����Ă����
                {
                    spriteRenderer.flipX = true;
                }
                else if (directionToPlayer.x > 0) // �E�����ֈړ����Ă����
                {
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                // �v���C���[�ɏ\���߂Â������~
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            // �v���C���[�����m�͈͊O�Ȃ��~
            rb.velocity = Vector2.zero;
        }
    }

    // �G���v���C���[�ɐG�ꂽ�Ƃ��̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �Փ˂����I�u�W�F�N�g��"Player"�^�O�������Ă��邩�m�F
        if (collision.gameObject.CompareTag("Player"))
        {
            // �v���C���[��PlayerHealth�R���|�[�l���g���擾
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // PlayerHealth�R���|�[�l���g�����݂��邩�m�F
            if (playerHealth != null)
            {
                // �v���C���[�Ƀ_���[�W��^����
                int damageAmount = 10; // �����ŗ^����_���[�W�ʂ�ݒ�i��: 10�_���[�W�j
                playerHealth.TakeDamage(damageAmount);
                Debug.Log("�G���v���C���[�ɐڐG���A" + damageAmount + " �_���[�W��^���܂����B");
            }
            else
            {
                Debug.LogWarning("PlayerHealth�R���|�[�l���g���v���C���[�I�u�W�F�N�g�Ɍ�����܂���I");
            }
        }
    }

    // �`�揇���̓��I�Ȓ�����DynamicSortingOrder�X�N���v�g�ɔC����
    // ���̃X�N���v�g�ł͓��ɏ������Ȃ�
}
