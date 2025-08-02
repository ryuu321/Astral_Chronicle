using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̃X�N���v�g��SpriteRenderer�R���|�[�l���g��K�v�Ƃ��邱�Ƃ�����
[RequireComponent(typeof(SpriteRenderer))]
public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Sorting Order�̌v�Z�Ɏg�p����Y���W�̏搔�i�����p�j
    // Inspector�Œ����\�ɂ���
    public float sortingMultiplier = 100f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("DynamicSortingOrder: SpriteRenderer component not found on " + gameObject.name);
            enabled = false; // SpriteRenderer���Ȃ��ꍇ�̓X�N���v�g�𖳌��ɂ���
        }
    }

    // LateUpdate�́AUpdate�̌�ɌĂ΂�邽�߁A�S�ẴI�u�W�F�N�g�̈ړ��������������
    // �`�揇���𒲐�����̂ɓK���Ă��܂��B
    void LateUpdate()
    {
        if (spriteRenderer != null)
        {
            // Y���W���������i��ʂ̉��ɂ���j�قǁASorting Order��傫������
            // ����ɂ��A���̃I�u�W�F�N�g����O�̃I�u�W�F�N�g�����ɕ`�悳���
            // int�ɃL���X�g���邱�ƂŐ����l�ɂ���
            int newSortingOrder = -(int)(this.transform.position.y * sortingMultiplier);
            spriteRenderer.sortingOrder = newSortingOrder;
        }
    }
}
