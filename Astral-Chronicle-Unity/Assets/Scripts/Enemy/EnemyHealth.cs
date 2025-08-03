using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30; // �G�̍ő�̗� (��: �X���C���Ȃ�30)
    public int currentHealth;   // �G�̌��݂̗̑�
    // �X�R�A�@�\�͌��ݕs�v�Ȃ̂ŁAscoreOnDefeat �͍폜�ς�

    void Awake()
    {
        currentHealth = maxHealth; // �Q�[���J�n���ɑ̗͂��ő�ɐݒ�
    }

    // �_���[�W���󂯂鏈��
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // �̗͂����炷
        currentHealth = Mathf.Max(0, currentHealth); // �̗͂�0�����ɂȂ�Ȃ��悤�ɂ���

        Debug.Log(gameObject.name + "�� " + damageAmount + " �_���[�W���󂯂܂����B���݂̗̑�: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // �̗͂�0�ȉ��ɂȂ����玀�S����
        }
    }

    // �G�����S�����Ƃ��̏���
    void Die()
    {
        Debug.Log(gameObject.name + "�͓|����܂����I");

        // �퓬�o���̉��Z�iPlayerStatus�ɔ��f�j
        // GameManager�����݂��A���v���C���[��PlayerStatus���擾�ł��Ă���΁A�o���l�����Z
        if (GameManager.instance != null && GameManager.instance.currentPlayerStatus != null)
        {
            GameManager.instance.currentPlayerStatus.combatExperience += 5; // ��Ƃ���5�̐퓬�o��
            Debug.Log("�퓬�o����5�l�����܂����B���݂̐퓬�o��: " + GameManager.instance.currentPlayerStatus.combatExperience);
            // �K�v�ɉ����āAUI�X�V�Ȃǂ�GameManager�o�R�Ŏw��
        }

        // �G�I�u�W�F�N�g���V�[������폜����
        Destroy(gameObject);
    }
}
