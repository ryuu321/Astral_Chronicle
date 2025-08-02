using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    // ���̃A�C�e�����ǂ�ItemData��������Inspector�Őݒ�
    public ItemData itemData;

    // OnTriggerEnter2D�́A����Collider2D�ƁuIs Trigger�v��ԂŏՓ˂����Ƃ��ɌĂ΂��
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �Փ˂����I�u�W�F�N�g�̃^�O��"Player"�ł��邩���m�F
        if (other.CompareTag("Player"))
        {
            // GameManager�̃C���X�^���X�ɃA�N�Z�X���A�ėp�I�ȃA�C�e�����W�֐����Ăяo��
            if (GameManager.instance != null && itemData != null)
            {
                GameManager.instance.CollectItem(itemData);
            }

            // ���̃A�C�e���I�u�W�F�N�g���V�[������폜
            Destroy(gameObject);
        }
    }
}
