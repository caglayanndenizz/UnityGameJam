using UnityEngine;

// Single place other scripts go to find the handful of per-run objects (bucket, day
// rules, fishing state, fish pool) instead of each one carrying its own
// Inspector-dragged copy of the same references.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private BucketGrid bucketGrid;
    [SerializeField] private DayManager dayManager;
    [SerializeField] private PlayerFishing playerFishing;
    [SerializeField] private FishPool fishPool;

    public BucketGrid BucketGrid => bucketGrid;
    public DayManager DayManager => dayManager;
    public PlayerFishing PlayerFishing => playerFishing;
    public FishPool FishPool => fishPool;

    private void Awake()
    {
        Instance = this;
    }

    // Started explicitly (rather than relying on Unity's Awake/Start order between
    // separate objects) so the fish pool is rolled for the day before anything reads
    // from it.
    private void Start()
    {
        dayManager.StartDay();
    }
}
