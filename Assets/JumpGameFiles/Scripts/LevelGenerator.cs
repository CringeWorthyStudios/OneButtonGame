using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 10f;

    [SerializeField] private Transform levelPart_Start;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private Player player;
    private List<Transform> platforms = new List<Transform>();

    private Vector3 lastEndPosition;

    private void Awake()
    {
        lastEndPosition = levelPart_Start.Find("Level_End").position;
        lastEndPosition = new Vector3(lastEndPosition.x, lastEndPosition.y + 4.75f, lastEndPosition.z);
        platforms.Add(levelPart_Start.GetComponent<Transform>());

        int startingSpawnLevelParts = 5;
        for(int i = 0; i < startingSpawnLevelParts; i++)
        {
            SpawnLevelPart();
        }
    }

    private void Update()
    {
        if(Vector3.Distance(player.transform.position, lastEndPosition) < PLAYER_DISTANCE_SPAWN_LEVEL_PART)
        {
            SpawnLevelPart();
        }
        MovePlatformDown();
    }

    private void MovePlatformDown()
    {
        while(player.transform.position.y > 1.5)
        {
            foreach(Transform platform in platforms)
            {
                platform.transform.position = new Vector2(platform.transform.position.x, platform.transform.position.y - 0.05f);
            }
            player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - 0.05f);
        }
    }

    private void SpawnLevelPart()
    {
        Transform randomLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
        Transform lastLevelPartTransform = SpawnLevelPart(randomLevelPart, lastEndPosition);
        lastEndPosition = lastLevelPartTransform.Find("Level_End").position;
        lastEndPosition = new Vector3(lastEndPosition.x, lastEndPosition.y + 4.75f, lastEndPosition.z);
    }

    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition)
    {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        platforms.Add(levelPartTransform.GetComponent<Transform>());
        return levelPartTransform;
    }
}
