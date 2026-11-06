using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum FishingState
{
    Idle,
    Fishing,
    AfterFishing,
}

// Drives the player's fishing animation through 3 states:
// Idle (waiting, button clickable) -> Fishing (animation playing, button disabled,
// a fish arrives automatically after fishingDuration) -> AfterFishing (shows the
// catch). The UI for each state is built separately: it reads
// State/CaughtFishName/CaughtFishValue (or listens to StateChanged) to decide what to
// show, and calls CloseResult() once the player dismisses the AfterFishing screen to
// go back to Idle.
[RequireComponent(typeof(Animator))]
public class PlayerFishing : MonoBehaviour
{
    [SerializeField] private Button castButton;
    [SerializeField] private float fishingDuration = 2f;

    private Animator _animator;

    public FishingState State { get; private set; }
    public string CaughtFishName { get; private set; }
    public int CaughtFishValue { get; private set; }
    public Sprite CaughtFishIcon { get; private set; }

    public event Action<FishingState> StateChanged;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // The animator's only state currently auto-plays on a loop. Turn it off so the
        // player just stands still until fishing actually starts.
        _animator.enabled = false;

        castButton.onClick.AddListener(StartFishing);
    }

    private void StartFishing()
    {
        castButton.interactable = false;
        _animator.enabled = true;
        _animator.Play("player_fishing", 0, 0f);
        SetState(FishingState.Fishing);

        StartCoroutine(WaitThenFinishFishing());
    }

    private IEnumerator WaitThenFinishFishing()
    {
        yield return new WaitForSeconds(fishingDuration);

        _animator.enabled = false;

        FishCard caught = GameManager.Instance.FishPool.GetRandomFish();
        CaughtFishName = caught.FishName;
        CaughtFishValue = caught.Value;
        CaughtFishIcon = caught.Icon;

        SetState(FishingState.AfterFishing);
    }

    // Called by the AfterFishing UI once the player dismisses it.
    public void CloseResult()
    {
        castButton.interactable = true;
        SetState(FishingState.Idle);
    }

    private void SetState(FishingState newState)
    {
        State = newState;
        StateChanged?.Invoke(State);
    }
}
