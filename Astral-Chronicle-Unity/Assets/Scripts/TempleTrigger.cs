using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleTrigger : MonoBehaviour
{
    private bool eventTriggered = false; // �C�x���g����x������������悤�ɂ���t���O

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �Փ˂����̂��v���C���[�ł���A���܂��C�x���g���������Ă��Ȃ��ꍇ
        if (other.CompareTag("Player") && !eventTriggered)
        {
            eventTriggered = true; // �C�x���g�������ς݂Ƃ��ăt���O�𗧂Ă�
            Debug.Log("�v���C���[���_�a�ɓ��B���܂����I�����̉�����󂯂�V�����n�܂�܂��B");

            // GameManager�ɐ����I���v���Z�X���J�n���Ă��炤
            if (GameManager.instance != null)
            {
                GameManager.instance.StartConstellationSelectionProcess();
            }

            // �g���K�[�̖������ʂ������̂ŁA���̃g���K�[�I�u�W�F�N�g�𖳌��ɂ���
            gameObject.SetActive(false);
        }
    }
}
