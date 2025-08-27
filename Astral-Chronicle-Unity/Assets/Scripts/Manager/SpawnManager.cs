using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    // キャラクターデータを格納するCharacterDatabaseアセット
    public CharacterDatabase characterDatabase;

    [Header("プレハブ")]
    public GameObject npcPrefab;
    public GameObject monsterPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // NPCとモンスターを生成する共通のプライベートメソッド
    private void SpawnCharacter(CharacterData data, GameObject prefab, Vector3 position)
    {
        if (prefab == null || data == null)
        {
            Debug.LogError("Prefab or CharacterData is not assigned!");
            return;
        }

        GameObject newCharacter = Instantiate(prefab, position, Quaternion.identity);
        Character characterComponent = newCharacter.GetComponent<Character>();
        if (characterComponent != null)
        {
            characterComponent.characterData = data;
            characterComponent.InitializeCharacter();
        }
    }

    // 全てのNPCをマップ上に配置するメソッド
    public void SpawnAllNPCsInScene()
    {
        if (characterDatabase == null || characterDatabase.allNPCs == null)
        {
            Debug.LogError("Character Database is not assigned or has no NPC data!");
            return;
        }

        foreach (CharacterData data in characterDatabase.allNPCs)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            SpawnCharacter(data, npcPrefab, randomPosition);
        }
    }

    // 全てのモンスターをマップ上に配置するメソッド
    public void SpawnAllMonstersInScene()
    {
        if (characterDatabase == null || characterDatabase.allMonsters == null)
        {
            Debug.LogError("Character Database is not assigned or has no Monster data!");
            return;
        }

        foreach (CharacterData data in characterDatabase.allMonsters)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            SpawnCharacter(data, monsterPrefab, randomPosition);
        }
    }
}