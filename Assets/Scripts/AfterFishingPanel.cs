using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Shows the fish the player just caught - icon and name. Appears the moment
// PlayerFishing leaves the Fishing state and enters AfterFishing, hides again
// otherwise.
public class AfterFishingPanel : MonoBehaviour
{
    [SerializeField] private PlayerFishing playerFishing;
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private Image fishImage;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private Button okButton;

    private void Awake()
    {
        playerFishing.StateChanged += OnStateChanged;
        okButton.onClick.AddListener(CloseResult);
        panelRoot.SetActive(false);
    }

    // Wired to the OK button's onClick.
    public void CloseResult()
    {
        playerFishing.CloseResult();
    }

    private void OnDestroy()
    {
        playerFishing.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(FishingState state)
    {
        bool showing = state == FishingState.AfterFishing;
        panelRoot.SetActive(showing);

        if (!showing) return;

        fishImage.sprite = playerFishing.CaughtFishIcon;
        nameLabel.text = playerFishing.CaughtFishName;
    }
}
