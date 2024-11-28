using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject effectPrefab;
    private Transform effectParent;
    private List<GameObject> currentEffects = new List<GameObject>();

    public void Initialize(GameManager gameManager, GameObject effectPrefab, Transform effectParent)
    {
        this.gameManager = gameManager;
        this.effectPrefab = effectPrefab;
        this.effectParent = effectParent;
    }

    private bool TryMove(Piece piece, (int, int) targetPos, MoveInfo moveInfo)
    {
        // moveInfo의 distance만큼 direction을 이동시키며 이동이 가능한지를 체크
        // 보드에 있는지, 다른 piece에 의해 막히는지 등을 체크
        // 폰에 대한 예외 처리를 적용
        // --- TODO ---
        if (piece is Pawn)
        {
            (int x, int y) = piece.MyPos;


            if (moveInfo.dirX != 0)
            {
                x += moveInfo.dirX;
                y += moveInfo.dirY;
                int d = 0;
                if (!Utils.IsInBoard((x, y))){
                    return false;
                }
                if (gameManager.Pieces[x, y] != null)
                {
                    d = gameManager.Pieces[x, y].PlayerDirection;
                }
                return d * piece.PlayerDirection == -1 && (x, y) == targetPos;
            }
            else
            {
                for (int q = 1; q <= moveInfo.distance; q++)
                {
                    y += moveInfo.dirY;
                    if (!Utils.IsInBoard((x, y)))
                    {
                        return false;
                    }
                    if (gameManager.Pieces[x, y] != null)
                    {
                        return false;
                    }
                    if ((x, y) == targetPos)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        for (int q = 1; q <= moveInfo.distance; q++)
        {
            (int x, int y) = piece.MyPos;
            int d = 0;
            x += moveInfo.dirX * q;
            y += moveInfo.dirY * q;
            if (!Utils.IsInBoard((x, y)))
                return false;
            if (gameManager.Pieces[x, y] != null)
            {
                d = gameManager.Pieces[x, y].PlayerDirection;
            }
            if (d * piece.PlayerDirection == 1)
            {
                return false;
            }
            if (targetPos.Item1 == x && targetPos.Item2 == y)
            {
                return true;
            }
            if (d * piece.PlayerDirection == -1)
                return false;
        }
        return false;
        // ------
    }

    private bool IsValidMoveWithoutCheck(Piece piece, (int, int) targetPos)
    {
        if (!Utils.IsInBoard(targetPos) || targetPos == piece.MyPos) return false;

        foreach (var moveInfo in piece.GetMoves())
        {
            if (TryMove(piece, targetPos, moveInfo))
                return true;
        }
        
        return false;
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMoveWithoutCheck(piece, targetPos)) return false;

        // 체크 상태 검증을 위한 임시 이동
        var originalPiece = gameManager.Pieces[targetPos.Item1, targetPos.Item2];
        var originalPos = piece.MyPos;

        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = piece;
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = null;
        piece.MyPos = targetPos;

        bool isValid = !IsInCheck(piece.PlayerDirection);

        // 원상 복구
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = piece;
        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = originalPiece;
        piece.MyPos = originalPos;

        return isValid;
    }

    private bool IsInCheck(int playerDirection)
    {
        (int, int) kingPos = (-1, -1); // 왕의 위치
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece is King && piece.PlayerDirection == playerDirection)
                {
                    kingPos = (x, y);
                    break;
                }
            }
            if (kingPos.Item1 != -1 && kingPos.Item2 != -1) break;
        }

        // 왕이 지금 체크 상태인지를 리턴
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece == null || piece.PlayerDirection == playerDirection)
                {
                    continue;
                }
                foreach (var moveInfo in piece.GetMoves())
                {
                    if (TryMove(piece, kingPos, moveInfo))
                        return true;
                }
            }
        }
        return false;
        // ------
    }

    public void ShowPossibleMoves(Piece piece)
    {
        ClearEffects();

        // 가능한 움직임을 표시
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                if (IsValidMove(piece, (x, y)))
                {
                    GameObject newEffect = Instantiate(effectPrefab);
                    newEffect.transform.position = Utils.ToRealPos((x, y));
                    currentEffects.Add(newEffect);
                }
            }
        }
        // ------
    }

    public void ClearEffects()
    {
        foreach (var effect in currentEffects)
        {
            if (effect != null) Destroy(effect);
        }
        currentEffects.Clear();
    }
}