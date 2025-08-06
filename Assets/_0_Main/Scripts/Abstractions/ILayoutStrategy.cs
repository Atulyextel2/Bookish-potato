using UnityEngine;

public interface ILayoutStrategy
{
    Rect[] CalculateCells(int rows, int cols, RectTransform container);
}