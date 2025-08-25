using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    // SFX再生用のAudioSourceを追加 (通常はEmpty GameObjectにアタッチ)
    public AudioSource sfxSource;
    public AudioSource sfxHitSource;
    // BGM再生用のAudioSource (もしあれば)
    // public AudioSource bgmSource; 

    [Header("Audio Clips")]
    public AudioClip sfxAttack; // 攻撃音のクリップ
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

        // AudioSourceが存在しない場合、自動的に追加する (オプション)
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }


    // SFXを再生するパブリックメソッド
    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
            Debug.Log(clip);
        }
    }

    // 既存のPlaySFXをオーバーロードする形で残すこともできます
    // public void PlaySFX(AudioClip clip) { PlaySFX(sfxSource, clip); } // デフォルト再生用

    // ... (他のメソッドは省略)
}
