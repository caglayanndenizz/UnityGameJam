# Lucky Rod — Game Design Rules

Canonical ruleset for the game, transcribed from `Lucky Rod Game Design Document (1).md`'s
Claude Code starter prompt (final design, 2026-11-06). This file is the reference for
implementation decisions at every step; update it if the design changes.

This replaces the earlier dial/needle/day-bait design. The game is now a single-screen
push-your-luck fishing game, closer to a Blackjack-style bust mechanic than a timing
minigame.

## Setup

- Single-screen, single-player, minimalist pixel art.
- Engine: Unity.

## Core loop

- Each morning, the harbor fortune teller announces the day's lucky number, chosen from
  12 to 24.
- The player casts the rod and catches a fish with a number on it.
- The fish goes into the bucket and its number is added to the bucket total.
- After each catch, the player chooses to cast again or return to the harbor.
- The day lasts until the player returns or the net tears.

## Results (on returning to the harbor)

- Bucket total below the lucky number: normal sale, earn the bucket total.
- Bucket total exactly equal to the lucky number: jackpot, earn the bucket total × 3.
- Bucket total above the lucky number: the net tears, all fish escape, earn 0.

## Fish

| Fish | Number |
| --- | --- |
| Anchovy | 2 |
| Sea bass | 5 |
| Bonito | 7 |
| Cursed fish | 13 |

### Cursed fish

- It does not add to the bucket total.
- A random fish escapes from the bucket, and the total drops by its number.
- If the bucket is empty, nothing happens.

## Fortune teller

- The screen shows the day's fish list.
- The fortune teller can give hints such as "Bonito is plentiful today."
- Rarely, the lucky number is 13. On those days, the cursed fish becomes a lucky fish.
  Exact effect is a placeholder for now (see Open questions).

## Debt

- Rent or boat debt rises every day.
- The game ends if the player cannot pay.
- Use placeholder values in the settings file for now.

## Shop and charms

- Money buys bait and rods that work like Balatro's charms and change how a run plays.
- Deep Bait: only high-number fish come.
- Amulet: cancels the cursed 13 once.
- Second Bucket: lets the player play with two separate targets (left for later).

## Technical rules

- Keep fish, shop and debt values in a single settings file.
- `StartDay()` picks the lucky number and fish list, then empties the bucket.
- `ResetRun()` resets the values for a new game and calls `StartDay()`.
- Define charms as data with small effect hooks, so new ones are cheap to add.

## Open questions

- What does the lucky fish do on a 13 day?
- Is the daily cost rent or boat debt, how much does it rise, and when is it paid?
- What is the full fish list, and how likely is each fish to appear?
- How do the bait and rod items differ, and how much do they cost?
- How does the Second Bucket assign fish and set its second target?

## Implementation progress

- [ ] Day 1: Lucky number, casting, bucket and return decision
- [ ] Day 2: Sale, jackpot, torn net and cursed fish
- [ ] Day 3: Day cycle, debt and game over
- [ ] Day 4: Shop and first charms
- [ ] Day 5: Pixel art scene and interface
- [ ] Day 6: Sound, small animations and game feel
- [ ] Day 7: Balancing, testing and upload

## Superseded

The original dial/needle design (1-100 horizontal dial, stop-the-needle, bait and day
system) in `Assets/Scripts/GameSettings.cs` and `Assets/Scripts/Dial/DialView.cs` is no
longer part of the design above and needs to be replaced.
