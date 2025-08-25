using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{
    // ステータスウィンドウのルートとなるパネルへの参照
    public GameObject statusWindowPanel;

    // UI要素への参照
    [Header("UI Element References")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI vocationText;
    public TextMeshProUGUI constellationText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI dexterityText;
    public TextMeshProUGUI intelligenceText;
    public TextMeshProUGUI vitalityText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI defensePowerText;
    public TextMeshProUGUI magicPowerText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI coExpText;
    public TextMeshProUGUI gaExpText;
    public TextMeshProUGUI crExpText;
    public TextMeshProUGUI exExpText;
    public TextMeshProUGUI soExpText;
    // スキル、バフ、装備など、今後追加するUI要素もここに追加

    // PlayerStatusコンポーネントへの参照
    private PlayerStatus playerStatus;

    void Awake()
    {
        // PlayerStatusコンポーネントを自動で取得
        playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogError("StatusUIController: PlayerStatus component not found in the scene.");
            enabled = false;
        }

        // 初期状態として、ステータスウィンドウを非表示にする
        if (statusWindowPanel != null)
        {
            statusWindowPanel.SetActive(false);
        }
    }

    // 外部（例: PlayerManager）から呼び出してUIを更新する
    public void UpdateStatusUI()
    {
        if (playerStatus != null)
        {
            // 名前、職業、星座の更新
            if (playerNameText != null) playerNameText.text = "名前: プレイヤー"; // プレイヤー名も将来的にPlayerStatusで管理
            if (levelText != null) levelText.text = "レベル: " + playerStatus.level.ToString();
            if (vocationText != null && playerStatus.currentVocation != null)
            {
                vocationText.text = "職業: " + playerStatus.currentVocation.vocationName;
            }
            if (constellationText != null && playerStatus.selectedConstellation != null)
            {
                constellationText.text = "星座: " + playerStatus.selectedConstellation.constellationName;
            }

            // 基本能力値の更新
            if (strengthText != null) strengthText.text = "筋力: " + playerStatus.strength.ToString();
            if (dexterityText != null) dexterityText.text = "器用さ: " + playerStatus.dexterity.ToString();
            if (intelligenceText != null) intelligenceText.text = "知力: " + playerStatus.intelligence.ToString();
            if (vitalityText != null) vitalityText.text = "生命力: " + playerStatus.vitality.ToString();

            // 派生ステータスの更新
            if (healthText != null && playerStatus.playerHealth != null)
            {
                healthText.text = $"体力: {playerStatus.playerHealth.currentHealth}/{playerStatus.playerHealth.maxHealth}";
            }
            if (attackPowerText != null) attackPowerText.text = "攻撃力: " + playerStatus.attackPower.ToString();
            if (defensePowerText != null) defensePowerText.text = "防御力: " + playerStatus.defense.ToString();
            if (magicPowerText != null) magicPowerText.text = "魔力: " + playerStatus.magicPower.ToString();

            //EXPの更新
            if(expText != null) expText.text = "総経験値:" + playerStatus.currentExp.ToString();
            if(coExpText != null) coExpText.text = "攻撃経験値:" + playerStatus.combatExperience.ToString();
            if(gaExpText != null) gaExpText.text = "収集経験値:" + playerStatus.gatheringExperience.ToString();
            if(crExpText != null) crExpText.text = "制作経験値:" + playerStatus.craftingExperience.ToString();
            if(exExpText != null) exExpText.text = "探検経験値:" + playerStatus.explorationExperience.ToString();
            if(soExpText != null) soExpText.text = "社交経験値:" + playerStatus.socialExperience.ToString();
        }
    }

    // ステータスウィンドウの表示/非表示を切り替えるメソッド
    public void HandletatusWindow()
    {
        if (statusWindowPanel != null)
        {
            // 現在の状態の逆を設定
            bool isActive = statusWindowPanel.activeSelf;
            statusWindowPanel.SetActive(!isActive);

            // ウィンドウが表示されたらUIを更新する
            if (!isActive)
            {
                UpdateStatusUI();
            }
        }
    }
}