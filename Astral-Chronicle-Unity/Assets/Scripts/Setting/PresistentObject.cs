using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    // �V���O���g���C���X�^���X�ւ̐ÓI�Q��
    // ���̃X�N���v�g����ȒP�ɃA�N�Z�X�ł���悤�ɂ��܂�
    public static PersistentObject instance;

    void Awake()
    {
        // �����C���X�^���X���܂����݂��Ȃ��ꍇ
        if (instance == null)
        {
            // ���̃I�u�W�F�N�g���C���X�^���X�ɐݒ肷��
            instance = this;

            // �V�[�������[�h���Ă����̃I�u�W�F�N�g���j������Ȃ��悤�ɐݒ肷��
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �������łɃC���X�^���X�����݂���ꍇ�A�V��������j������
            // ����ɂ��A�Q�[�����ɕ����̃C���X�^���X�������̂�h��
            Destroy(gameObject);
        }
    }
}
