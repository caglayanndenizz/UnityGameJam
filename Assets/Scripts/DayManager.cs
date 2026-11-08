using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The "21" rule from the design doc: rolls the day's lucky number and judges the
// bucket against it. Going over tears the net immediately (checked after every catch).
// Returning at or under the lucky number cashes in - exactly on it is a jackpot.
public class DayManager : MonoBehaviour
{
    [SerializeField] private Button returnButton;
    [SerializeField] private TMP_Text luckyNumberLabel;
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private TMP_Text dayLabel;

    private int _luckyNumber;
    private bool _dayOver;
    private int _day = 1;

    private void Awake()
    {
        returnButton.onClick.AddListener(ReturnToHarbor);
        UpdateDayLabel();
    }

    // Called by GameManager once everything's Instance is ready, and again whenever a
    // new day should start.
    public void StartDay()
    {
        _luckyNumber = Random.Range(GameSettings.LuckyNumberMin, GameSettings.LuckyNumberMax + 1);
        _dayOver = false;
        GameManager.Instance.BucketGrid.EmptyAll();
        GameManager.Instance.FishPool.RollDay();
        luckyNumberLabel.text = $"Lucky number: {_luckyNumber}";
        resultLabel.text = "";
        UpdateDayLabel();
    }

    // Called by PlayerFishing right after a catch is added to a bucket - each bucket
    // is its own target, so only that one can tear.
    public void CheckForBust(Bucket bucket)
    {
        if (_dayOver || bucket.Total <= _luckyNumber) return;

        _dayOver = true;
        resultLabel.text = "The net tears! You lose everything.";
    }

    private void ReturnToHarbor()
    {
        if (_dayOver) return;

        int earned = 0;
        bool jackpot = false;
        foreach (Bucket bucket in GameManager.Instance.BucketGrid.Buckets)
        {
            if (bucket.Total == _luckyNumber)
            {
                jackpot = true;
                earned += bucket.Total * 3;
            }
            else
            {
                earned += bucket.Total;
            }
        }

        resultLabel.text = jackpot
            ? $"Jackpot! Earned {earned}"
            : $"Returned safely. Earned {earned}";

        _day++;
        StartDay();
    }

    private void UpdateDayLabel()
    {
        dayLabel.text = $"Day: {_day}";
    }
}
