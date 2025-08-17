using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    // シングルトンインスタンスへの静的参照
    // 他のスクリプトから簡単にアクセスできるようにします
    public static PersistentObject instance;

    void Awake()
    {
        // もしインスタンスがまだ存在しない場合
        if (instance == null)
        {
            // このオブジェクトをインスタンスに設定する
            instance = this;

            // シーンをロードしてもこのオブジェクトが破棄されないように設定する
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // もしすでにインスタンスが存在する場合、新しい方を破棄する
            // これにより、ゲーム中に複数のインスタンスが作られるのを防ぐ
            Destroy(gameObject);
        }
    }
}
