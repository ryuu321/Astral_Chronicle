using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    // �L�����N�^�[�f�[�^���i�[����CharacterDatabase�A�Z�b�g
    public CharacterDatabase characterDatabase;

    [Header("�v���n�u")]
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

    // NPC�ƃ����X�^�[�𐶐����鋤�ʂ̃v���C�x�[�g���\�b�h
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

    // �S�Ă�NPC���}�b�v��ɔz�u���郁�\�b�h
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

    // �S�Ẵ����X�^�[���}�b�v��ɔz�u���郁�\�b�h
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