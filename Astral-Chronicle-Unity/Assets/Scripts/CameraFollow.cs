using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 追従するターゲット（プレイヤー）
    public float smoothSpeed = 0.125f; // カメラの追従の滑らかさ
    public Vector3 offset; // ターゲットからのオフセット（カメラの位置調整）

    void LateUpdate() // Updateの後でカメラを動かすのが一般的
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optional: 特定の軸でカメラの動きを固定する場合
        // transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, offset.z);
    }
}
