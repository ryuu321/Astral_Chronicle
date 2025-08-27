using UnityEngine;

public class Character : MonoBehaviour
{
    // このキャラクターのデータを保持するScriptableObject
    public CharacterData characterData;

    // キャラクターの外見を制御するコンポーネント
    protected PlayerAppearanceController appearanceController;
    // キャラクターのステータスを制御するコンポーネント
    protected PlayerStatus statusController;
    // キャラクターの体力を制御するコンポーネント
    protected PlayerHealth healthController;

    protected virtual void Awake()
    {
        // 必要なコンポーネントを取得
        appearanceController = GetComponent<PlayerAppearanceController>();
        statusController = GetComponent<PlayerStatus>();
        healthController = GetComponent<PlayerHealth>();

        // キャラクターデータを初期化
        if (characterData != null)
        {
            InitializeCharacter();
        }
    }

    // CharacterDataに基づいてキャラクターを初期化するメソッド
    public void InitializeCharacter()
    {
        if (characterData == null) return;
        GetComponent<SpriteRenderer>().sprite = characterData.characterSprite;

        // 見た目の初期化
        if (appearanceController != null)
        {
            appearanceController.UpdateAppearance(
                characterData.hairStyle,
                characterData.face,
                characterData.body
            );
        }

        // ステータスの初期化
        if (statusController != null)
        {
            statusController.InitializeStatus(
                characterData.baseStrength,
                characterData.baseDexterity,
                characterData.baseIntelligence,
                characterData.baseVitality
            );
            // 職業と星座もここで初期化できる
        }

        // 体力の初期化
        if (healthController != null)
        {
            healthController.InitializeHealth(characterData.hp) ;
            Debug.Log("hp");
        }

        // 名前をゲームオブジェクト名に設定
        gameObject.name = characterData.characterName;
    }
}