using UnityEngine;
using UnityEngine.UI;

// This is the dial the player uses to try and land on their lucky number.
//
// It stays hidden until the player casts, so it's not just sitting on screen doing
// nothing between throws. One button drives everything: "Cast" starts the needle,
// "Stop" freezes it, "Continue" hides the dial again so the next cast feels like a
// fresh event.
//
// Attach this to an empty GameObject under a Canvas and press Play - it builds its
// own numbers, needle and button out of plain Unity UI pieces.
[RequireComponent(typeof(RectTransform))]
public class DialView : MonoBehaviour
{
    private const float TrackWidth = 1700f;

    private enum CastState
    {
        Idle,
        NeedleMoving,
        ShowingResult,
    }

    private CastState _state;

    private GameObject _dialGroup;
    private GameObject _actionButton;
    private RectTransform _needle;
    private Text _currentNumberLabel;
    private Text _resultLabel;
    private Text _actionButtonLabel;

    private int _luckyNumber;
    private float _needleValue;
    private int _needleDirection;

    private void Awake()
    {
        _luckyNumber = Random.Range(GameSettings.DialMin, GameSettings.DialMax + 1);

        BuildDial();
        BuildActionButton();

        GoToIdle();
    }

    private void Update()
    {
        if (_state == CastState.NeedleMoving)
        {
            MoveNeedle();
        }
    }

    private void MoveNeedle()
    {
        _needleValue += _needleDirection * GameSettings.NeedleSpeed * Time.deltaTime;

        // The needle bounces back and forth instead of wrapping around.
        if (_needleValue >= GameSettings.DialMax)
        {
            _needleValue = GameSettings.DialMax;
            _needleDirection = -1;
        }
        else if (_needleValue <= GameSettings.DialMin)
        {
            _needleValue = GameSettings.DialMin;
            _needleDirection = 1;
        }

        _needle.anchoredPosition = new Vector2(NumberToX(_needleValue), 0f);
        _currentNumberLabel.text = CurrentNumber().ToString();
    }

    private int CurrentNumber()
    {
        return Mathf.Clamp(Mathf.RoundToInt(_needleValue), GameSettings.DialMin, GameSettings.DialMax);
    }

    private float NumberToX(float number)
    {
        float progress = (number - GameSettings.DialMin) / (float)(GameSettings.DialMax - GameSettings.DialMin);
        return Mathf.Lerp(-TrackWidth / 2f, TrackWidth / 2f, progress);
    }

    // The single action button calls this, and what it actually does depends on
    // which part of the cast we're currently in.
    private void OnActionButtonPressed()
    {
        if (_state == CastState.Idle)
        {
            StartCasting();
        }
        else if (_state == CastState.NeedleMoving)
        {
            StopNeedle();
        }
        else
        {
            GoToIdle();
        }
    }

    private void GoToIdle()
    {
        _state = CastState.Idle;
        _dialGroup.SetActive(false);
        _resultLabel.text = "";
        _actionButtonLabel.text = "Cast";

        // There's no player/world yet to decide when a cast is possible, so for now
        // the button just stays hidden until something calls ShowCastButton().
        _actionButton.SetActive(false);
    }

    // The future player/world code calls this once it knows the player is actually
    // in a position to cast (e.g. standing at the water). Nothing calls it yet.
    public void ShowCastButton()
    {
        _actionButton.SetActive(true);
    }

    private void StartCasting()
    {
        _state = CastState.NeedleMoving;
        _dialGroup.SetActive(true);
        _resultLabel.text = "";
        _actionButtonLabel.text = "Stop";

        // Random start and direction, so the player can't just memorize a pattern.
        _needleValue = Random.Range((float)GameSettings.DialMin, (float)GameSettings.DialMax);
        _needleDirection = Random.value < 0.5f ? 1 : -1;
    }

    private void StopNeedle()
    {
        _state = CastState.ShowingResult;
        _actionButtonLabel.text = "Continue";

        int stoppedOn = CurrentNumber();
        int distance = Mathf.Abs(stoppedOn - _luckyNumber);
        _resultLabel.text = $"Stopped on {stoppedOn} - lucky number is {_luckyNumber} (distance {distance})";
    }

    // ---------------------------------------------------------------------
    // Everything below just builds plain Unity UI pieces. No art yet - this
    // is only meant to prove the dial feels right.
    // ---------------------------------------------------------------------

    private void BuildDial()
    {
        _dialGroup = new GameObject("DialGroup", typeof(RectTransform));
        RectTransform group = (RectTransform)_dialGroup.transform;
        group.SetParent(transform, false);
        group.anchoredPosition = new Vector2(0f, 60f);

        RectTransform track = CreateUIElement("Track", group, new Vector2(TrackWidth, 6f), Vector2.zero);
        track.gameObject.AddComponent<Image>().color = new Color(0.25f, 0.25f, 0.28f);

        for (int number = GameSettings.DialMin; number <= GameSettings.DialMax; number++)
        {
            float x = NumberToX(number);

            RectTransform tick = CreateUIElement("Tick" + number, track, new Vector2(2f, 14f), new Vector2(x, 0f));
            tick.gameObject.AddComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);

            CreateLabel("Label" + number, track, number.ToString(), 11, new Vector2(34f, 20f), new Vector2(x, -22f));
        }

        // A small gold marker so the lucky number stands out from the rest.
        RectTransform luckyMarker = CreateUIElement("LuckyMarker", track, new Vector2(10f, 24f), new Vector2(NumberToX(_luckyNumber), 0f));
        luckyMarker.gameObject.AddComponent<Image>().color = new Color(1f, 0.85f, 0.2f);
        luckyMarker.SetAsFirstSibling();

        _needle = CreateUIElement("Needle", track, new Vector2(4f, 40f), Vector2.zero);
        _needle.gameObject.AddComponent<Image>().color = new Color(0.9f, 0.2f, 0.2f);

        _currentNumberLabel = CreateLabel("CurrentNumber", _needle, "", 18, new Vector2(80f, 30f), new Vector2(0f, 32f));
        _resultLabel = CreateLabel("Result", group, "", 20, new Vector2(900f, 60f), new Vector2(0f, -80f));
    }

    private void BuildActionButton()
    {
        RectTransform buttonRect = CreateUIElement("ActionButton", transform, new Vector2(180f, 50f), new Vector2(0f, -170f));
        buttonRect.gameObject.AddComponent<Image>().color = new Color(0.2f, 0.5f, 0.9f);

        Button button = buttonRect.gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnActionButtonPressed);

        _actionButtonLabel = CreateLabel("ActionButtonLabel", buttonRect, "Cast", 20, buttonRect.sizeDelta, Vector2.zero);
        _actionButton = buttonRect.gameObject;
    }

    private RectTransform CreateUIElement(string name, Transform parent, Vector2 size, Vector2 position)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        RectTransform rect = (RectTransform)go.transform;
        rect.SetParent(parent, false);
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = position;
        return rect;
    }

    private Text CreateLabel(string name, Transform parent, string text, int fontSize, Vector2 size, Vector2 position)
    {
        RectTransform rect = CreateUIElement(name, parent, size, position);
        Text label = rect.gameObject.AddComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.fontSize = fontSize;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = Color.white;
        label.text = text;
        return label;
    }
}
