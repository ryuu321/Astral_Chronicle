using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // �e�R���|�[�l���g�ւ̎Q��
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerAttack attack;
    [SerializeField] StatusUIController statusUI;

    // Input System�̃R���g���[���N���X�ւ̎Q��
    private PlayerControls playerControls;
    private Animator animator;

    // �v���C���[�̏�Ԃ��Ǘ�����Enum
    public enum PlayerState { Idle, Moving, Attacking, Talking, Damaged }
    public PlayerState currentState;

    void Awake()
    {
        // �N�����ɃR���|�[�l���g���擾
        health = GetComponent<PlayerHealth>();
        status = GetComponent<PlayerStatus>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        statusUI = GetComponent<StatusUIController>();

        // InputControls�̃C���X�^���X�𐶐�
        playerControls = new PlayerControls();

        // ������Ԃ�ݒ�
        currentState = PlayerState.Idle;
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // �ړ��A�N�V�����̃C�x���g���w��
        playerControls.Player.OnMove.performed += OnMovePerformed;
        playerControls.Player.OnMove.canceled += OnMoveCanceled;

        // �U���A�N�V�����̃C�x���g���w��
        playerControls.Player.OnAttack.performed += OnAttackPerformed;

        playerControls.Player.OnStatus.performed += OnStatusPerformed;

        // �A�N�V�����}�b�v��L����
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        // �C�x���g�̍w�ǂ�����
        playerControls.Player.OnMove.performed -= OnMovePerformed;
        playerControls.Player.OnMove.canceled -= OnMoveCanceled;
        playerControls.Player.OnAttack.performed -= OnAttackPerformed;

        // �A�N�V�����}�b�v�𖳌���
        playerControls.Player.Disable();
    }

    // Input System����̃C�x���g�n���h���[ (��������)
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.Talking)
        {
            animator.SetBool("Move", true);
            movement.HandleMoveInput(context.ReadValue<Vector2>());
            currentState = PlayerState.Moving;
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        animator.SetBool("Move", false);
        // ���͂��I������瓮�����~�߂�
        movement.HandleMoveInput(Vector2.zero);
        currentState = PlayerState.Idle;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.Talking)
        {
            animator.SetTrigger("Attack");
            attack.HandleAttack();
            currentState = PlayerState.Attacking;
        }
    }

    private void OnStatusPerformed(InputAction.CallbackContext context)
    {
        if(currentState != PlayerState.Moving && currentState != PlayerState.Attacking)
        {
            statusUI.HandletatusWindow();
        }
    }

    // ��Ԃ̐؂�ւ����\�b�h
    public void SetState(PlayerState newState)
    {
        currentState = newState;
    }
}