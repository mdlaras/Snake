using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] GameManager Game;
    [SerializeField] GridLayout PlayGround;
    [SerializeField] Tilemap PlayMap;
    Vector3Int PlayerPosition;
    Vector3Int[] PlayerBody;
    Vector3Int PositionChanger;
    [SerializeField] Tile PlayerAvatar;
    [SerializeField] Tile GroundAvatar;
    [SerializeField] Sprite Obstacle;
    [SerializeField] Sprite AvailableGround;
    [SerializeField] Sprite FoodGround;
    [SerializeField] float TimeInterval;
    float MoveTimeStamp;
    Sprite NextTile;
    // Start is called before the first frame update
    void Start()
    {
        Game.GenerateFood();
        PlayerPosition = PlayGround.LocalToCell(transform.position);
        PlayerBody = new Vector3Int[1];
        PlayerBody.SetValue(PlayerPosition, 0);
        transform.localPosition = PlayGround.CellToLocal(PlayerPosition);
        PlayMap.SetTile(PlayerPosition, PlayerAvatar);
        MoveTimeStamp = Time.time;
        PositionChanger = new Vector3Int(0,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            PositionChanger = new Vector3Int(1,0,0);
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            PositionChanger = new Vector3Int(-1,0,0);
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            PositionChanger = new Vector3Int(0,-1,0);
        }
        else if(Input.GetKey(KeyCode.UpArrow))
        {
            PositionChanger = new Vector3Int(0,1,0);
        }

        var NewPosition = PlayerPosition + PositionChanger;
        NextTile = PlayMap.GetSprite(NewPosition);
        if(NextTile.Equals(AvailableGround) && Time.time - MoveTimeStamp > TimeInterval)
        {
            PlayMap.SetTile(PlayerBody[0], GroundAvatar);
            PlayerPosition = NewPosition;
            var TemporaryBody = new Vector3Int[PlayerBody.Length];
            for (int BodyCell = 0; BodyCell < PlayerBody.Length-1; BodyCell++)
            {
                TemporaryBody[BodyCell] = PlayerBody[BodyCell + 1];
                PlayMap.SetTile(TemporaryBody[BodyCell], PlayerAvatar);
            }
            TemporaryBody.SetValue(PlayerPosition, TemporaryBody.Length - 1);
            PlayerBody = TemporaryBody;
            PlayMap.SetTile(PlayerBody[PlayerBody.Length-1], PlayerAvatar);
            MoveTimeStamp = Time.time;
        }
        else if (NextTile.Equals(FoodGround) && Time.time - MoveTimeStamp > TimeInterval)
        {
            PlayMap.SetTile(NewPosition, GroundAvatar);
            PlayMap.SetTile(PlayerBody[0], GroundAvatar);
            
            var TemporaryBody = new Vector3Int[PlayerBody.Length + 1];
             for (int BodyCell = 0; BodyCell < PlayerBody.Length-1; BodyCell++)
            {
                TemporaryBody[BodyCell] = PlayerBody[BodyCell + 1];
                PlayMap.SetTile(TemporaryBody[BodyCell], PlayerAvatar);
            }
            TemporaryBody.SetValue(PlayerPosition, TemporaryBody.Length - 2);
            PlayerPosition = NewPosition;
            TemporaryBody.SetValue(PlayerPosition, TemporaryBody.Length - 1);
            PlayerBody = new Vector3Int[PlayerBody.Length + 1];
            PlayerBody = TemporaryBody;
            PlayMap.SetTile(PlayerBody[PlayerBody.Length-1], PlayerAvatar);
            MoveTimeStamp = Time.time;
            Game.GenerateFood();
        }
        else if (NextTile.Equals(Obstacle) && Time.time - MoveTimeStamp > TimeInterval)
        {
            Game.endGame();
        }
        else 
        {
            transform.localPosition = PlayGround.CellToLocal(PlayerPosition) * Time.deltaTime;
        }


    }
}
