using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    // シングルトンインスタンス
    public static InputManager instance;

    // 入力イベントの定義
    // Actionは、引数を取らないメソッドのデリゲート
    // 例えば、OnAttackPressed?.Invoke(); で、このイベントに登録された全てのメソッドが実行される
    public event Action OnInteractPressed;
    public event Action OnAttackPressed;
    public event Action OnPausePressed;

    void Awake()
    {
        // シングルトンパターンの実装
        if (instance == null)
        {
            instance = this;
            // シーンを跨いで存在させる
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 既にインスタンスがあれば、新しいものを破棄する
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // ゲームが一時停止中の場合は入力を受け付けない
        if (Time.timeScale == 0f)
        {
            return;
        }

        // キーボード入力の検知
        // Input.GetButtonDown("Interact") は、Project SettingsのInput Managerで設定された
        // "Interact"という名前のボタンが押された瞬間にtrueを返す
        if (Input.GetButtonDown("Interact"))
        {
            // イベントに登録されたメソッドを呼び出す
            OnInteractPressed?.Invoke();
            Debug.Log("Input Interact");
        }

        if (Input.GetButtonDown("Fire1")) // 攻撃ボタン（通常はCtrlキーなど）
        {
            OnAttackPressed?.Invoke();
        }

        if (Input.GetButtonDown("Cancel")) // ポーズボタン（通常はEscキーなど）
        {
            OnPausePressed?.Invoke();
        }
    }
}