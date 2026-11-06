using UnityEngine;

// The cursed fish's special rule from the design doc: it doesn't add to the bucket.
// Instead, a random already-caught fish escapes and the total drops by that fish's
// value. Add this component to the Cursed fish prefab only.
public class CursedFishEffect : MonoBehaviour
{
    public void Apply(Bucket bucket)
    {
        bucket.RemoveRandomFish();
    }
}
