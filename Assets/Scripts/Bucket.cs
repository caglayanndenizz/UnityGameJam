using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Keeps the running total of fish the player has caught so far today, along with
// each individual value - the cursed fish needs that list to make one escape.
public class Bucket : MonoBehaviour
{
    [SerializeField] private TMP_Text totalLabel;

    private readonly List<int> _caughtValues = new List<int>();

    public int Total { get; private set; }

    private void Awake()
    {
        UpdateLabel();
    }

    public void AddFish(FishCard fish)
    {
        _caughtValues.Add(fish.Value);
        Total += fish.Value;
        UpdateLabel();
    }

    // The cursed fish's effect: a random already-caught fish escapes, dropping the
    // total by its value. Does nothing if the bucket is empty.
    public void RemoveRandomFish()
    {
        if (_caughtValues.Count == 0) return;

        int index = Random.Range(0, _caughtValues.Count);
        Total -= _caughtValues[index];
        _caughtValues.RemoveAt(index);
        UpdateLabel();
    }

    public void Empty()
    {
        _caughtValues.Clear();
        Total = 0;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        totalLabel.text = $"Bucket: {Total}";
    }
}
