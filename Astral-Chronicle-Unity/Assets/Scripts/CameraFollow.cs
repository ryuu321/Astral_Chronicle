using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // �Ǐ]����^�[�Q�b�g�i�v���C���[�j
    public float smoothSpeed = 0.125f; // �J�����̒Ǐ]�̊��炩��
    public Vector3 offset; // �^�[�Q�b�g����̃I�t�Z�b�g�i�J�����̈ʒu�����j

    void LateUpdate() // Update�̌�ŃJ�����𓮂����̂���ʓI
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Optional: ����̎��ŃJ�����̓������Œ肷��ꍇ
        // transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, offset.z);
    }
}
