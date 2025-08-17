using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    // �V���O���g���C���X�^���X
    public static InputManager instance;

    // ���̓C�x���g�̒�`
    // Action�́A���������Ȃ����\�b�h�̃f���Q�[�g
    // �Ⴆ�΁AOnAttackPressed?.Invoke(); �ŁA���̃C�x���g�ɓo�^���ꂽ�S�Ẵ��\�b�h�����s�����
    public event Action OnInteractPressed;
    public event Action OnAttackPressed;
    public event Action OnPausePressed;

    void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (instance == null)
        {
            instance = this;
            // �V�[�����ׂ��ő��݂�����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���ɃC���X�^���X������΁A�V�������̂�j������
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // �Q�[�����ꎞ��~���̏ꍇ�͓��͂��󂯕t���Ȃ�
        if (Time.timeScale == 0f)
        {
            return;
        }

        // �L�[�{�[�h���͂̌��m
        // Input.GetButtonDown("Interact") �́AProject Settings��Input Manager�Őݒ肳�ꂽ
        // "Interact"�Ƃ������O�̃{�^���������ꂽ�u�Ԃ�true��Ԃ�
        if (Input.GetButtonDown("Interact"))
        {
            // �C�x���g�ɓo�^���ꂽ���\�b�h���Ăяo��
            OnInteractPressed?.Invoke();
            Debug.Log("Input Interact");
        }

        if (Input.GetButtonDown("Fire1")) // �U���{�^���i�ʏ��Ctrl�L�[�Ȃǁj
        {
            OnAttackPressed?.Invoke();
        }

        if (Input.GetButtonDown("Cancel")) // �|�[�Y�{�^���i�ʏ��Esc�L�[�Ȃǁj
        {
            OnPausePressed?.Invoke();
        }
    }
}