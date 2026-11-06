using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// A single fish: it has a number (same idea as a playing card for the 21-style bust
// rule - catching fish adds up towards the lucky number) and shows that number on its
// TextMeshPro label. Clicking it adds its value to the bucket and removes the fish.
public class FishCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text valueLabel;
    [SerializeField] private Bucket bucket;

    public int Value { get; private set; }

    private void Awake()
    {
        RollValue();
        EnsureClickable();
    }

    public void RollValue()
    {
        Value = Random.Range(GameSettings.FishValueMin, GameSettings.FishValueMax + 1);
        valueLabel.text = Value.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bucket.AddFish(this);
        Destroy(gameObject);
    }

    // Clicking a world sprite needs a collider to hit-test against, and the camera
    // needs a Physics2DRaycaster so the EventSystem can find it there. Add both
    // automatically so this isn't yet another manual scene setup step.
    private void EnsureClickable()
    {
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                boxCollider.size = spriteRenderer.sprite.bounds.size;
            }
        }

        if (Camera.main != null && Camera.main.GetComponent<Physics2DRaycaster>() == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }
}
