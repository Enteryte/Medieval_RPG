using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop List Base Profile", menuName = "Scriptable Objects/Shop/Shop List Base Profile")]
public class ShopListBaseProfile : ScriptableObject
{
    public ItemBaseProfile[] itemBaseProfiles;
}
