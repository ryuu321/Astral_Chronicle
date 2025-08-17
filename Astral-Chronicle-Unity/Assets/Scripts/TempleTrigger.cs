using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleTrigger : MonoBehaviour
{
    private bool eventTriggered = false; // イベントが一度だけ発生するようにするフラグ

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 衝突したのがプレイヤーであり、かつまだイベントが発生していない場合
        if (other.CompareTag("Player") && !eventTriggered)
        {
            eventTriggered = true; // イベントが発生済みとしてフラグを立てる
            Debug.Log("プレイヤーが神殿に到達しました！星座の加護を受ける儀式が始まります。");

            // GameManagerに星座選択プロセスを開始してもらう
            if (GameManager.instance != null)
            {
                GameManager.instance.StartConstellationSelectionProcess();
            }

            // トリガーの役割を果たしたので、このトリガーオブジェクトを無効にする
            gameObject.SetActive(false);
        }
    }
}
