using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public GameObject[] PiecePrefabs;
    public GameObject EffectPrefab;

    private Transform TileParent;
    private Transform PieceParent;
    private Transform EffectParent;
    private MovementManager movementManager;
    private UIManager uiManager;
    
    public int CurrentTurn = 1;
    public Tile[,] Tiles = new Tile[Utils.FieldWidth, Utils.FieldHeight];
    public Piece[,] Pieces = new Piece[Utils.FieldWidth, Utils.FieldHeight];

    void Awake()
    {
        TileParent = GameObject.Find("TileParent").transform;
        PieceParent = GameObject.Find("PieceParent").transform;
        EffectParent = GameObject.Find("EffectParent").transform;
        
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        movementManager = gameObject.AddComponent<MovementManager>();
        movementManager.Initialize(this, EffectPrefab, EffectParent);
        
        InitializeBoard();
    }

    void InitializeBoard()
    {
        // 8x8로 타일들을 배치
        // --- TODO ---
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject newTile = Instantiate(TilePrefab);
                Tiles[x, y] = newTile.GetComponent<Tile>();
                Tiles[x, y].Set((x, y));
            }
        }
        // ------

        PlacePieces(1);
        PlacePieces(-1);
    }

    void PlacePieces(int direction)
    {
        // 체스 말들을 배치
        // --- TODO ---
        int y1 = direction == 1 ? 0 : 7;
        int y2 = direction == 1 ? 1 : 6;
        for (int i = 0; i < 8; i++) {
            Pieces[i, y1] = PlacePiece(i, Tiles[i, y1].MyPos, direction);
            Pieces[i, y2] = PlacePiece(8, Tiles[i, y2].MyPos, direction);
        }
        // ------
    }

    Piece PlacePiece(int pieceType, (int, int) pos, int direction)
    {
        // 체스 말 하나를 배치 후 initialize
        // --- TODO ---
        GameObject newPieceObj = Instantiate(PiecePrefabs[pieceType]);
        Piece newPieceCom = newPieceObj.GetComponent<Piece>();
        newPieceCom.initialize(pos, direction);
        return newPieceCom;
        // ------
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        return movementManager.IsValidMove(piece, targetPos);
    }

    public void ShowPossibleMoves(Piece piece)
    {
        movementManager.ShowPossibleMoves(piece);
    }

    public void ClearEffects()
    {
        movementManager.ClearEffects();
    }


    public void Move(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMove(piece, targetPos)) return;

        // 체스 말을 이동하고, 만약 해당 자리에 상대 말이 있다면 삭제
        // --- TODO ---
        (int x, int y) = piece.MyPos;
        Pieces[x, y] = null;
        (int px, int py) = targetPos;
        if (Pieces[px, py] != null)
        {
            Destroy(Pieces[px, py].GameObject());
        }
        Pieces[px, py] = piece;
        piece.MoveTo(targetPos);
        ChangeTurn();
        // ------
    }

    void ChangeTurn()
    {
        // 턴을 변경하고, UI에 표시
        // --- TODO ---
        CurrentTurn *= -1;
        uiManager.UpdateTurn(CurrentTurn);
        // ------
    }
}
