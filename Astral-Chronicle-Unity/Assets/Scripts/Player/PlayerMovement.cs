using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // ���E�ƑO��i���䂫�j�̈ړ����x
    public float jumpForce = 8f; // �W�����v�̗́i�΂ߌ����낵�ł�Z�������̓�����z��j

    private Rigidbody2D rb; // Rigidbody2D�R���|�[�l���g�ւ̎Q��
    public bool isGrounded; // �n�ʂɐڒn���Ă��邩�ǂ����̃t���O

    // Start�֐��́A�X�N���v�g���ŏ��ɗL���ɂȂ����Ƃ��Ɉ�x�����Ă΂�܂�
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody2D component not found!");
            enabled = false;
        }
    }

    // FixedUpdate�֐��́A�Œ肳�ꂽ�t���[�����[�g�ŕ������Z�̍X�V���ɌĂ΂�܂�
    void FixedUpdate()
    {
        // X���i���E�j�̓��͒l���擾
        float horizontalInput = Input.GetAxis("Horizontal");
        // Y���i�O��E���䂫�j�̓��͒l���擾
        float verticalInput = Input.GetAxis("Vertical");

        // Rigidbody2D��Velocity�i���x�j���v�Z
        // �΂ߌ����낵�Ȃ̂ŁAY���̓��͂�Y���̓����i���䂫�j�ɕϊ�����܂�
        // �W�����v�̓����͕������Z�ɔC���邽�߁Arb.velocity.z �͂����ł͕ύX���Ȃ�
        // 2D�Ȃ̂ŁAZ���͕����I�ɂ͂���܂���BY�������䂫�����˂�
        Vector2 movement = new Vector2(horizontalInput, verticalInput) * moveSpeed;

        // �����̃W�����v���x�͈ێ����A���������̑��x���X�V
        // Rigidbody2D��XY���ʂœ������߁AZ�����̓�����Y���ɏd�Ȃ�܂��B
        // �������A�����ڂ́u�����v�̃W�����v��Y�����x���g���̂ł͂Ȃ��A
        // ���o�I��Z���W�̑���iSorting Order�j�〈���ڂ̃X�P�[�����O�ŕ\�����邱�Ƃ������ł��B
        // �����ł́A��ʓI��2D�W�����v�Ƃ���Y�����x���g���܂����A
        // ��̕`�揇���̒����Łu�����v��\�����܂��B
        rb.velocity = new Vector2(movement.x, movement.y);
    }

    // Update�֐��́A�t���[�����ƂɈ�x�Ă΂�܂�
    void Update()
    {
        // �X�y�[�X�L�[�������ꂽ�Ƃ��ɃW�����v�i���������ւ̈ړ��j
        // �΂ߌ����낵�ŃW�����v�́A�L�����N�^�[����ɓ������Ƃ�������
        // ���o�I�Ɂu��ɔ�яオ��v�悤�Ɍ�����H�v���K�v�ł��B
        // �����ł͂܂��P����Y���i��ʏ�����j�ւ̃W�����v�Ƃ��Ĉ����܂��B
        // ���ۂ̃W�����v�͕����I�ȍ����iZ���j�̕ω��ƌ������������̂ŁA
        // 2D�����ł�Y����Velocity�Œ������ASort Order���H�v���܂��B
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // ������̗͂������ăW�����v�����܂��B
            // ���������̈ړ���Y�����͂Ƃ͕ʂɐ��䂵�܂��B
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
            isGrounded = false; // �W�����v�����̂Őڒn��Ԃ�����
        }
    }

    // OnCollisionEnter2D�́A����Collider2D�ƏՓ˂����Ƃ��ɌĂ΂�܂�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O��"Ground"�̏ꍇ�A�ڒn��Ԃɂ���
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // OnCollisionExit2D�́A����Collider2D�Ƃ̏Փ˂��I�������Ƃ��ɌĂ΂�܂�
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �Փ˂��I�������I�u�W�F�N�g�̃^�O��"Ground"�̏ꍇ�A�ڒn��Ԃ�����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

     //--- ��������ǉ�: �`�揇���̓��I�Ȓ��� ---
     //�`�揇���́A�L�����N�^�[��Y���W�Ɋ�Â��ē��I�ɕύX���邱�ƂŁA
     //���ɂ�����̂���O�ɂ�����̂��u���ɕ`��v�����悤�Ɍ����܂��B
    void LateUpdate()
    {
        // SpriteRenderer���擾�i����Start�Ŏ擾�ς݂Ȃ炻������g���j
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Y���W���������i��ʂ̉��ɂ���j�قǁASorting Order��傫������
            // �Ⴆ�΁A-10000����0�܂ł͈̔͂�Mapping����
            // ��̓I�Ȕ͈͂̓Q�[����Y���W�͈͂ɍ��킹�Ē���
            int sortingOrder = -(int)(transform.position.y * 100);
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
}