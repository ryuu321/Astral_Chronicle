using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // 各コンポーネントへの参照
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerAttack attack;
    [SerializeField] StatusUIController statusUI;

    // Input Systemのコントロールクラスへの参照
    private PlayerControls playerControls;
    private Animator animator;

    // プレイヤーの状態を管理するEnum
    public enum PlayerState { Idle, Moving, Attacking, Talking, Damaged }
    public PlayerState currentState;

    void Awake()
    {
        // 起動時にコンポーネントを取得
        health = GetComponent<PlayerHealth>();
        status = GetComponent<PlayerStatus>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        statusUI = GetComponent<StatusUIController>();

        // InputControlsのインスタンスを生成
        playerControls = new PlayerControls();

        // 初期状態を設定
        currentState = PlayerState.Idle;
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // 移動アクションのイベントを購読
        playerControls.Player.OnMove.performed += OnMovePerformed;
        playerControls.Player.OnMove.canceled += OnMoveCanceled;

        // 攻撃アクションのイベントを購読
        playerControls.Player.OnAttack.performed += OnAttackPerformed;

        playerControls.Player.OnStatus.performed += OnStatusPerformed;

        // アクションマップを有効化
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        // イベントの購読を解除
        playerControls.Player.OnMove.performed -= OnMovePerformed;
        playerControls.Player.OnMove.canceled -= OnMoveCanceled;
        playerControls.Player.OnAttack.performed -= OnAttackPerformed;

        // アクションマップを無効化
        playerControls.Player.Disable();
    }

    // Input Systemからのイベントハンドラー (引数あり)
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
        // 入力が終わったら動きを止める
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

    // 状態の切り替えメソッド
    public void SetState(PlayerState newState)
    {
        currentState = newState;
    }
}