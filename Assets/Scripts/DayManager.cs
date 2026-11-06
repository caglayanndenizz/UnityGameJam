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

    private int _luckyNumber;
    private bool _dayOver;

    private void Awake()
    {
        returnButton.onClick.AddListener(ReturnToHarbor);
    }

    // Called by GameManager once everything's Instance is ready, and again whenever a
    // new day should start.
    public void StartDay()
    {
        _luckyNumber = Random.Range(GameSettings.LuckyNumberMin, GameSettings.LuckyNumberMax + 1);
        _dayOver = false;
        GameManager.Instance.Bucket.Empty();
        GameManager.Instance.FishPool.RollDay();
        luckyNumberLabel.text = $"Lucky number: {_luckyNumber}";
        resultLabel.text = "";
    }

    // Called by FishCard right after it adds its value to the bucket.
    public void CheckForBust()
    {
        Bucket bucket = GameManager.Instance.Bucket;
        if (_dayOver || bucket.Total <= _luckyNumber) return;

        _dayOver = true;
        resultLabel.text = "The net tears! You lose everything.";
    }

    private void ReturnToHarbor()
    {
        if (_dayOver) return;

        Bucket bucket = GameManager.Instance.Bucket;

        _dayOver = true;
        resultLabel.text = bucket.Total == _luckyNumber
            ? $"Jackpot! Earned {bucket.Total * 3}"
            : $"Returned safely. Earned {bucket.Total}";

        bucket.Empty();
    }
}
