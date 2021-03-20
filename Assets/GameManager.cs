using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] GridLayout PlayGround;
    [SerializeField] Tilemap PlayMap;
    [SerializeField] Tile FoodAvatar;
    [SerializeField] Sprite AvailableGround;
    float randomPositionX;
    float randomPositionY;
    Vector3 randomPosition;
    Vector3Int FoodPosition;

    public void GenerateFood()
    {
        randomPositionX = Random.Range(-7.5f, 7.5f);
        randomPositionY = Random.Range(-5, 5);
        randomPosition = new Vector3(randomPositionX,randomPositionY,0);
        FoodPosition = PlayGround.LocalToCell(randomPosition);
        PlayMap.SetTile(FoodPosition, FoodAvatar);
    }
}
