using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // �K�v�ł���Ύc��

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // �̗̓o�[�̃t�B��Image�R���|�[�l���g�ւ̎Q�Ƃ�UIManager�����ׂ��Ȃ̂ō폜
    // public Image healthBarFillImage; // ������폜

    // UIManager�ւ̎Q�Ƃ�ǉ�
    private UIManager uiManager;

    void Awake()
    {
        currentHealth = maxHealth;
        // Awake�ł͂܂�UIManager������������Ă��Ȃ��\��������̂ŁAUpdateHealthUI�͌Ă΂Ȃ�
        // SetUIManager�ŌĂ΂��
    }

    // UIManager����̎Q�Ƃ�ݒ肷�郁�\�b�h
    public void SetUIManager(UIManager manager)
    {
        uiManager = manager;
        UpdateHealthUI(); // UIManager���ݒ肳�ꂽ�珉��X�V
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        Debug.Log("�v���C���[�� " + damageAmount + " �_���[�W���󂯂܂����B���݂̗̑�: " + currentHealth);
        UpdateHealthUI();
        if (currentHealth <= 0) Die();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        Debug.Log("�v���C���[�� " + healAmount + " �񕜂��܂����B���݂̗̑�: " + currentHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        // UI�̍X�V��UIManager�ɈϏ�
        if (uiManager != null)
        {
            uiManager.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    void Die()
    {
        Debug.Log("�v���C���[�͎��S���܂����I�Q�[���I�[�o�[�I");
        if (GameManager.instance != null)
        {
            GameManager.instance.HandleGameOver();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        gameObject.SetActive(false);
    }

    public void ApplyInitialStatBonus(int strength, int dexterity, int intelligence, int vitality)
    {
        Debug.Log("�����X�e�[�^�X�{�[�i�X�K�p: �ؗ�+" + strength + ", ��p��+" + dexterity + ", �m��+" + intelligence + ", ������+" + vitality);
        maxHealth += vitality;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
}