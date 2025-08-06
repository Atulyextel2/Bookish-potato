using UnityEngine;
[CreateAssetMenu(fileName = "CardData", menuName = "CardMatch/CardData")]
public class CardData : ScriptableObject
{
    public string matchId;
    public int faceAtlasIndex;
    public int backAtlasIndex;
}