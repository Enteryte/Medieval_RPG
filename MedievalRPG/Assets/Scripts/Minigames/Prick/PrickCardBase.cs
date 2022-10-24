using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prick Card Base Profile", menuName = "Scriptable Objects/Minigames/Prick Card Base Profile", order = 0)]
public class PrickCardBase : ScriptableObject
{
    public Sprite cardSprite;

    public enum CardColor
    {
        none,
        cross = 4,
        spades = 1,
        heart = 2,
        diamonds = 3
    }

    public CardColor cardColor = CardColor.none;

    public enum CardNumber
    {
        none,
        ace = 8,
        ten = 7,
        king = 6,
        queen = 5,
        jack = 4,
        nine = 3,
        eight = 2,
        seven = 1
    }

    public CardNumber cardNumber = CardNumber.none;
}
