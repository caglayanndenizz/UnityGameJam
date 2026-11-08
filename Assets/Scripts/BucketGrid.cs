using System.Collections.Generic;
using UnityEngine;

// Arranges one or more Bucket targets side by side. The scene starts with a single
// hand-placed bucket; AddBucket() grows the row by one. Not wired to anything yet -
// it'll be called once the boat upgrade system (buy a boat upgrade, get +1 bucket)
// is built.
// SelectedBucket is whichever one the player last clicked - that's where
// PlayerFishing drops the next catch.
public class BucketGrid : MonoBehaviour
{
    [SerializeField] private Bucket startingBucket;
    [SerializeField] private Bucket bucketPrefab;
    [SerializeField] private float spacing = 2.5f;

    private readonly List<Bucket> _buckets = new List<Bucket>();

    public Bucket SelectedBucket { get; private set; }
    public IReadOnlyList<Bucket> Buckets => _buckets;

    private void Awake()
    {
        _buckets.Add(startingBucket);
        Select(startingBucket);
    }

    // Called once the boat upgrade system exists and grants an extra bucket.
    public void AddBucket()
    {
        Bucket last = _buckets[_buckets.Count - 1];
        Bucket bucket = Instantiate(bucketPrefab, transform);
        bucket.transform.position = last.transform.position + new Vector3(spacing, 0f, 0f);
        _buckets.Add(bucket);
    }

    // Called by a Bucket when the player clicks it.
    public void Select(Bucket bucket)
    {
        SelectedBucket?.SetSelected(false);
        SelectedBucket = bucket;
        SelectedBucket.SetSelected(true);
    }

    public void EmptyAll()
    {
        foreach (Bucket bucket in _buckets) bucket.Empty();
    }
}
