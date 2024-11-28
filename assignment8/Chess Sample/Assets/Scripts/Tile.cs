using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public (int, int) MyPos;
    Color tileColor = new Color(255 / 255f, 193 / 255f, 204 / 255f);
    SpriteRenderer MySpriteRenderer;

    private void Awake()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Set((int, int) targetPos)
    {
        // targetPos로 이동시키고, 색깔을 지정
        // --- TODO ---
        (int x, int y) = MyPos = targetPos;
        GetComponent<Transform>().position = Utils.ToRealPos(MyPos);
        MySpriteRenderer.color = ((x ^ y) & 1) == 1 ? tileColor : Color.white;
        MySpriteRenderer.sortingOrder = -1;
        // ------
    }
}
