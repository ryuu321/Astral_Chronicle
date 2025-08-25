using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    // SFX�Đ��p��AudioSource��ǉ� (�ʏ��Empty GameObject�ɃA�^�b�`)
    public AudioSource sfxSource;
    public AudioSource sfxHitSource;
    // BGM�Đ��p��AudioSource (���������)
    // public AudioSource bgmSource; 

    [Header("Audio Clips")]
    public AudioClip sfxAttack; // �U�����̃N���b�v
    public AudioClip sfxHit;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // AudioSource�����݂��Ȃ��ꍇ�A�����I�ɒǉ����� (�I�v�V����)
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }


    // SFX���Đ�����p�u���b�N���\�b�h
    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
            Debug.Log(clip);
        }
    }

    // ������PlaySFX���I�[�o�[���[�h����`�Ŏc�����Ƃ��ł��܂�
    // public void PlaySFX(AudioClip clip) { PlaySFX(sfxSource, clip); } // �f�t�H���g�Đ��p

    // ... (���̃��\�b�h�͏ȗ�)
}
