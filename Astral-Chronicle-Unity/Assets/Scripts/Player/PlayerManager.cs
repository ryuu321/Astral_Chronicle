using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // 各コンポーネントへの参照
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerStatus status;
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerAttack attack;

    // Input Systemのコントロールクラスへの参照
    private PlayerControls playerControls;

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

        // InputControlsのインスタンスを生成
        playerControls = new PlayerControls();

        // 初期状態を設定
        currentState = PlayerState.Idle;
    }

    void OnEnable()
    {
        // 移動アクションのイベントを購読
        playerControls.Player.Move.performed += OnMovePerformed;
        playerControls.Player.Move.canceled += OnMoveCanceled;

        // 攻撃アクションのイベントを購読
        playerControls.Player.Attack.performed += OnAttackPerformed;

        // アクションマップを有効化
        playerControls.Player.Enable();
    }

    void OnDisable()
    {
        // イベントの購読を解除
        playerControls.Player.Move.performed -= OnMovePerformed;
        playerControls.Player.Move.canceled -= OnMoveCanceled;
        playerControls.Player.Attack.performed -= OnAttackPerformed;

        // アクションマップを無効化
        playerControls.Player.Disable();
    }

    // Input Systemからのイベントハンドラー (引数あり)
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.Attacking && currentState != PlayerState.Talking)
        {
            movement.HandleMoveInput(context.ReadValue<Vector2>());
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // 入力が終わったら動きを止める
        movement.HandleMoveInput(Vector2.zero);
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.Attacking && currentState != PlayerState.Talking)
        {
            attack.HandleAttack();
        }
    }

    // 状態の切り替えメソッド
    public void SetState(PlayerState newState)
    {
        currentState = newState;
    }
}