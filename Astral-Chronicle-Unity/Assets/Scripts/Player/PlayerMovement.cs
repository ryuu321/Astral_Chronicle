using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleMoveInput(Vector2 moveVector)
    {
        // �v���C���[�̈ړ�����
        Vector2 movement = moveVector * moveSpeed;
        rb.velocity = movement;
    }
}

// //--- ��������ǉ�: �`�揇���̓��I�Ȓ��� ---
// //�`�揇���́A�L�����N�^�[��Y���W�Ɋ�Â��ē��I�ɕύX���邱�ƂŁA
// //���ɂ�����̂���O�ɂ�����̂��u���ɕ`��v�����悤�Ɍ����܂��B
//void LateUpdate()
//{
//    // SpriteRenderer���擾�i����Start�Ŏ擾�ς݂Ȃ炻������g���j
//    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
//    if (spriteRenderer != null)
//    {
//        // Y���W���������i��ʂ̉��ɂ���j�قǁASorting Order��傫������
//        // �Ⴆ�΁A-10000����0�܂ł͈̔͂�Mapping����
//        // ��̓I�Ȕ͈͂̓Q�[����Y���W�͈͂ɍ��킹�Ē���
//        int sortingOrder = -(int)(transform.position.y * 100);
//        spriteRenderer.sortingOrder = sortingOrder;
//    }
//}
