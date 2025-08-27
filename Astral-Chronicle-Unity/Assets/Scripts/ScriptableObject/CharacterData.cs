using UnityEngine;

// ScriptableObjectとしてアセットを作成できるようにする
[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    // 基本情報
    public string characterName;
    public Sprite characterSprite;
    public int age;
    public int hp;
    

    // ステータス（PlayerStatusから分離）
    public int baseStrength;
    public int baseDexterity;
    public int baseIntelligence;
    public int baseVitality;

    // 見た目情報（PlayerAppearanceDataから分離）
    public PlayerAppearanceData hairStyle;
    public PlayerAppearanceData face;
    public PlayerAppearanceData body;
    // 他のパーツもここに追加

    // 職業（ここではVocationDataへの参照を持たせる）
    public VocationData initialVocation;
    public ConstellationData initialConstellation; // 選択した星座名
}