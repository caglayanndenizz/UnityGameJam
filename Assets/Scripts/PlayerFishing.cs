using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Quick test rig: a Cast button that plays the player's fishing animation and rolls
// the day's lucky number. The water splash is its own animation now (done by hand),
// and the real catching logic isn't here yet - this is only the first look.
//
// The button and label are built by hand in the scene (not from code) so they can
// actually be styled - drag them into the fields below in the Inspector.
[RequireComponent(typeof(Animator))]
public class PlayerFishing : MonoBehaviour
{
    [SerializeField] private Button castButton;
    [SerializeField] private TMP_Text luckyNumberLabel;

    private Animator _animator;
    private int _luckyNumber;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // The animator's only state currently auto-plays on a loop. Turn it off so the
        // player just stands still until the Cast button is actually pressed.
        _animator.enabled = false;

        castButton.onClick.AddListener(Cast);

        RollLuckyNumber();
    }

    // Wired to the Cast button's onClick.
    public void Cast()
    {
        _animator.enabled = true;
        _animator.Play("player_fishing", 0, 0f);
    }

    private void RollLuckyNumber()
    {
        _luckyNumber = Random.Range(GameSettings.LuckyNumberMin, GameSettings.LuckyNumberMax + 1);
        luckyNumberLabel.text = $"Lucky number: {_luckyNumber}";
    }
}
