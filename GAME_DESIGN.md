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

## Main menu and lucky number roll

- The game opens on a main menu with a Start button.
- Clicking Start begins the run and plays the lucky number roll: a roulette wheel with
  a ball-throw animation picks the day's lucky number (12 to 24). This is the visual
  form of the fortune teller's announcement from the Core loop below.

## Core loop

- Each morning, the harbor fortune teller announces the day's lucky number (via the
  roulette roll above), chosen from 12 to 24.
- The player casts the rod and catches a fish with a number on it.
- The fish goes into whichever bucket is currently selected (see Buckets) and its
  number is added to that bucket's total.
- After each catch, the player chooses to cast again or return to the harbor.
- The day lasts until the player returns or the net tears.
- Returning to the harbor pays out and immediately rolls the next day (day count +1,
  new lucky number, buckets emptied) so the player keeps fishing run after run. There
  is no end to this loop yet - it keeps going until debt/game over is wired up.

## Results (on returning to the harbor)

- Checked per bucket, then summed into one payout:
  - Bucket total below the lucky number: normal sale, earn the bucket total.
  - Bucket total exactly equal to the lucky number: jackpot, earn the bucket total × 3.
- Any bucket's total going above the lucky number tears the net immediately (checked
  right after that catch, not only at return) and ends the day for every bucket at
  once - all fish in all buckets escape, earn 0.

## Buckets

- The player starts with one bucket. A boat upgrade (see Rod and boat upgrades) adds
  one more, laid out side by side.
- With more than one bucket, the player clicks a bucket to select it as the target -
  the next catch goes into whichever one is selected.
- Each bucket keeps its own total and is judged independently against the day's
  lucky number for sale/jackpot, but one bucket tearing its net currently ends the
  whole day (see Open questions).

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

- Money buys charms that work like Balatro's and change how a run plays.
- Deep Bait: only high-number fish come.
- Amulet: cancels the cursed 13 once.

## Rod and boat upgrades

- Separate from charms: the rod and the boat can each be upgraded.
- Boat upgrade: adds one bucket to the bucket grid (see Buckets).
- Not built yet - no shop flow or cost for these upgrades exists; `BucketGrid.AddBucket()`
  exists and is ready to be called once this system is built, but nothing calls it yet.
- What rod upgrades do, and what either upgrade costs, is still undecided.

## Technical rules

- Keep fish, shop and debt values in a single settings file.
- `StartDay()` picks the lucky number and fish list, then empties every bucket.
- `ResetRun()` resets the values for a new game and calls `StartDay()`.
- Define charms as data with small effect hooks, so new ones are cheap to add.
- `BucketGrid` holds the active buckets and which one is selected; `PlayerFishing` adds
  each catch to `BucketGrid.SelectedBucket` and checks that bucket for a bust.

## Open questions

- What does the lucky fish do on a 13 day?
- Is the daily cost rent or boat debt, how much does it rise, and when is it paid?
- What is the full fish list, and how likely is each fish to appear?
- How do the bait and rod items differ, and how much do they cost?
- Should one bucket tearing its net end the whole day, or just that bucket, while the
  others keep fishing? Currently it ends the whole day for all buckets.
- How does the boat upgrade purchase work, and is there a maximum bucket count?
- Does the roulette ball-throw animation play only once at the start of a run, or every
  time a new day rolls a lucky number?

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
