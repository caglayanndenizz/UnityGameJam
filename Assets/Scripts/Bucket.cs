using TMPro;
using UnityEngine;

// Keeps the running total of fish the player has caught so far today. It just holds
// the number and shows it - the 21-style "did we bust or hit the lucky number" rule
// reads this total but doesn't live here, that comes next.
public class Bucket : MonoBehaviour
{
    [SerializeField] private TMP_Text totalLabel;

    public int Total { get; private set; }

    private void Awake()
    {
        UpdateLabel();
    }

    public void AddFish(FishCard fish)
    {
        Total += fish.Value;
        UpdateLabel();
    }

    public void Empty()
    {
        Total = 0;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        totalLabel.text = $"Bucket: {Total}";
    }
}
