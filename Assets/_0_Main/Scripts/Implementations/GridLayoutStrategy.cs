using UnityEngine;

public class GridLayoutStrategy : ILayoutStrategy
{
    public Rect[] CalculateCells(int rows, int cols, RectTransform container)
    {
        float cellWidth = container.rect.width / cols;
        float cellHeight = container.rect.height / rows;
        var cells = new Rect[rows * cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int idx = r * cols + c;
                float x = c * cellWidth;
                float y = -r * cellHeight;
                cells[idx] = new Rect(x, y, cellWidth, cellHeight);
            }
        }

        return cells;
    }
}