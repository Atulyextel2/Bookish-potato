using UnityEngine;

public class GridLayoutStrategy : ILayoutStrategy
{
    public Rect[] CalculateCells(int rows, int cols, RectTransform c)
    {
        var cells = new Rect[rows * cols];
        float w = c.rect.width / cols;
        float h = c.rect.height / rows;
        int i = 0;
        for (int r = 0; r < rows; r++)
            for (int col = 0; col < cols; col++)
            {
                float x = c.rect.xMin + col * w;
                float y = c.rect.yMax - (r + 1) * h;
                cells[i++] = new Rect(x, y, w, h);
            }
        return cells;
    }
}