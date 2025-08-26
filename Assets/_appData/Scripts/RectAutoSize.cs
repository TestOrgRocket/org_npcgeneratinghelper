using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectAutoSize : MonoBehaviour
{
    float cellSize = 300f;
    void OnEnable()
    {
        ChangeSize();
    }
    public void ChangeSize()
    {
        int childCount = transform.childCount;
        RectTransform rt = GetComponent<RectTransform>();
        float newYSize = childCount * cellSize + 50;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, newYSize);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -newYSize / 2 - 50);
    }
}
