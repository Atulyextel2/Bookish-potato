using UnityEngine;
[CreateAssetMenu(fileName = "CardData", menuName = "CardMatch/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] private string matchId;
    [SerializeField] private Sprite faceSprite, backSprite;
}