/// Single place for Lucky Rod's balance values, per GAME_DESIGN.md's technical rules.
/// Add new tunables here (fish list, shop prices, debt, ...) as they're needed instead
/// of scattering magic numbers across scripts.
public static class GameSettings
{
    public const int LuckyNumberMin = 12;
    public const int LuckyNumberMax = 24;

    // Every fish/card is worth a number in this range, like a playing card.
    public const int FishValueMin = 1;
    public const int FishValueMax = 13;
}
