/// Single place for Lucky Rod's balance values, per GAME_DESIGN.md's technical rules.
/// Add new tunables here (bait, zone shares, per-cast speed-up, ...) instead of
/// scattering magic numbers across scripts.
public static class GameSettings
{
    public const int DialMin = 1;
    public const int DialMax = 100;

    /// Numbers per second the needle sweeps across the dial.
    public const float NeedleSpeed = 40f;
}
