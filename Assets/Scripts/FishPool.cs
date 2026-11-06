using TMPro;
using UnityEngine;

// Each day, one fish type becomes plentiful - far more likely to come up when
// something asks for a random fish - and the hint label tells the player which one,
// like the design doc's fortune teller ("Bonito is plentiful today").
public class FishPool : MonoBehaviour
{
    [SerializeField] private FishCard[] fishPrefabs;
    [SerializeField] private TMP_Text hintLabel;
    [SerializeField] private int plentifulWeight = 3;

    private FishCard _plentifulFish;

    // Called by DayManager at the start of each day.
    public void RollDay()
    {
        _plentifulFish = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        hintLabel.text = $"{_plentifulFish.FishName} is plentiful today.";
    }

    // Called by PlayerFishing whenever a cast finishes, to decide what got caught.
    public FishCard GetRandomFish()
    {
        int totalWeight = 0;
        foreach (FishCard fish in fishPrefabs)
        {
            totalWeight += WeightOf(fish);
        }

        int roll = Random.Range(0, totalWeight);
        int runningWeight = 0;
        foreach (FishCard fish in fishPrefabs)
        {
            runningWeight += WeightOf(fish);
            if (roll < runningWeight) return fish;
        }

        return fishPrefabs[fishPrefabs.Length - 1];
    }

    private int WeightOf(FishCard fish)
    {
        return fish == _plentifulFish ? plentifulWeight : 1;
    }
}
