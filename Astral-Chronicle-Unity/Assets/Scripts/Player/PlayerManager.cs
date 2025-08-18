using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // �e�R���|�[�l���g�ւ̎Q��
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerAttack attack;

    // Input System�̃R���g���[���N���X�ւ̎Q��
    private PlayerControls playerControls;

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

        // InputControls�̃C���X�^���X�𐶐�
        playerControls = new PlayerControls();

        // ������Ԃ�ݒ�
        currentState = PlayerState.Idle;
    }

    void OnEnable()
    {
        // �ړ��A�N�V�����̃C�x���g���w��
        playerControls.Player.Move.performed += OnMovePerformed;
        playerControls.Player.Move.canceled += OnMoveCanceled;

        // �U���A�N�V�����̃C�x���g���w��
        playerControls.Player.Attack.performed += OnAttackPerformed;

        // �A�N�V�����}�b�v��L����
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        // �C�x���g�̍w�ǂ�����
        playerControls.Player.Move.performed -= OnMovePerformed;
        playerControls.Player.Move.canceled -= OnMoveCanceled;
        playerControls.Player.Attack.performed -= OnAttackPerformed;

        // �A�N�V�����}�b�v�𖳌���
        playerControls.Player.Disable();
    }

    // Input System����̃C�x���g�n���h���[ (��������)
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.Attacking && currentState != PlayerState.Talking)
        {
            movement.HandleMoveInput(context.ReadValue<Vector2>());
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // ���͂��I������瓮�����~�߂�
        movement.HandleMoveInput(Vector2.zero);
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.Attacking && currentState != PlayerState.Talking)
        {
            attack.HandleAttack();
        }
    }

    // ��Ԃ̐؂�ւ����\�b�h
    public void SetState(PlayerState newState)
    {
        currentState = newState;
    }
}