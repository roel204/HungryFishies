using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public string id;
    public string upgradeName;
    [TextArea]
    public string description;
    public int baseCost;
    public int priceIncrement;
    public int maxLevel;
    public Sprite icon;
}
