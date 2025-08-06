using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfig", menuName = "CardMatch/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Serializable]
    public struct LayoutPreset
    {
        public string name;
        public int rows;
        public int cols;
    }

    [Tooltip("Player-visible list of difficulty presets")]
    public List<LayoutPreset> presets = new List<LayoutPreset>
    {
        new LayoutPreset { name = "Easy",   rows = 2, cols = 2 },
        new LayoutPreset { name = "Medium", rows = 2, cols = 3 },
        new LayoutPreset { name = "Hard",   rows = 5, cols = 6 }
    };

    [Tooltip("Seconds to wait after flip animations before matching")]
    public float flipAnimationDuration = 0.5f;

    [Tooltip("Points awarded per match")]
    public int scorePerMatch = 10;
}