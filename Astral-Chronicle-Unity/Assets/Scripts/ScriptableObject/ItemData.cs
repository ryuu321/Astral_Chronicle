using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity�G�f�B�^��Create���j���[���炱�̃A�Z�b�g���쐬�ł���悤�ɂ���
[CreateAssetMenu(fileName = "NewItemData", menuName = "Game Data/Item Data")]
public class ItemData : ScriptableObject
{
    public ItemType itemType; // �A�C�e���̎��
    public int value;         // �A�C�e�������l�i��: �R�C���Ȃ�X�R�A�A�|�[�V�����Ȃ�񕜗ʁj
    public Sprite itemSprite; // �A�C�e���̌����ځi�A�C�R���Ȃǁj

    // �A�C�e�����W���̌��ʁi�I�v�V����: ���b�Z�[�W�\���Ȃǁj
    public string collectMessage = "";
}
