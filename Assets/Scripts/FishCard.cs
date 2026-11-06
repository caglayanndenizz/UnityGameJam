using UnityEngine;

// A fish's data: a fixed value, name and icon set by hand per prefab (Anchovy, Sea
// bass, Bonito, Cursed fish). FishPool reads these straight off the prefabs - fish
// are never instantiated or shown on their own, the AfterFishing panel is what
// actually displays the icon/name/value once one is caught.
public class FishCard : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private string fishName;
    [SerializeField] private Sprite icon;

    public int Value => value;
    public string FishName => fishName;
    public Sprite Icon => icon;
}
