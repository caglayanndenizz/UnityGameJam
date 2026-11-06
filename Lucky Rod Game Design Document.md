# Lucky Rod Game Design Document

Nov 5, 2026 · @chaggy

## Summary

Lucky Rod (working title) is a single-player game where you catch fish by trying to stop the needle of a horizontal dial on your lucky number. The goal is to complete each day by catching a Golden Fish before your bait runs out, and to reach as many days as possible.

| Topic | Decision |
| --- | --- |
| Jam theme | Lucky number |
| Genre | Timing and score-driven fishing game |
| Players | Single-player |
| Development time | 1 week |
| AI use | Allowed, development will use Claude Code |
| Score | Days reached |
| Engine | Unity |

## Core loop

Each cast costs 1 bait, and the result depends on the zone where the needle stops on the dial.

1. A lucky number from 1 to 100 is set at the start of the game. The player can type it or get a random one.
2. The player casts and spends 1 bait.
3. The needle starts at a random number and moves between 1 and 100.
4. The player stops the needle.
5. The result is calculated and the needle speeds up slightly.

The random start is essential. If the needle always started at 1, the player could break the game by picking an easy number such as 1 or 100.

If the Golden zone reaches the end of the dial, it continues from the other end. For example, if the lucky number is 100, the zone wraps around to 1 and 2.

| Dial share | Result | Extra effect |
| --- | --- | --- |
| Lucky number ±2 (5%) | Golden Fish | Completes the day. |
| 8% | Rare Fish | Widens the Golden zone and the Rare zone by 1 number. This makes the lucky number easier to reach. |
| 10% | Good Fish | You can shred this into a bait. +2 |
| 10% | Common Fish | Not impressive but still great. You can shred this to a bait +1 |
| Remaining area | Crap | Adds one more unlucky zone. |

## Dial

The counter is a horizontal dial with a needle that moves between 1 and 100, and it is the most important part of the game.

- The dial always shows the full 1-100 range, with numbers under the tick marks.
- The lucky number stays in its real position on the dial and is clearly marked.
- The current number is shown above the needle.
- Zones use subtle colors that do not stand out too much.
- Zones are not placed side by side. The code distributes them according to their shares.
- The layout is regenerated every day and stays fixed for that day.
- At the start, the unlucky zone takes up 5% and Crap takes up about 62%.

The code places zones in this order:

1. The Golden zone is placed on the lucky number.
2. The 3 numbers on each side of the Golden zone cannot be Crap. Any numbers left empty here become Common Fish.
3. The unlucky zone is placed outside the Golden zone and this safe area.
4. Rare, Good and Common zones are distributed across the empty spaces. Each share is split into 1-2 pieces, and each piece is at least 3 numbers wide.
5. Everything left over becomes Crap.

If fixed zones feel good, they turn into swimming fish shadows during polish. Rare Fish are small and fast, and Good Fish are large and slow. The Golden zone stays fixed on the lucky number.

## Day system and Last Chance

The Golden Fish ends the day. The game ends when the Last Chance is missed too.

&#91;embedded content: game flow · day loop and Last Chance\]

The Golden Fish check comes first after every cast. The bait check only runs if no Golden Fish was caught.

### Unlucky zones

- The first unlucky zone is chosen again every day and is about 5 numbers wide.
- Unlucky zones start hidden. Once hit, a zone stays red until the end of the day.
- If the needle stops in this zone, the player loses 2 bait in total.
- Bait cannot go below 0.
- A Crap result adds one more unlucky zone for that day.
- There can be at most 5 unlucky zones in a day.
- Unlucky zones are placed outside the Golden zone and the safe area.
- If the needle stops in an unlucky zone, only the bait penalty applies. The Crap effect does not trigger.
- Added unlucky zones are removed at the end of the day.

### Zone expansion

- The Golden zone starts at ±2 every day.
- Each Rare Fish widens the Golden zone and the Rare zone by 1 number.
- Expansion takes space from the Crap area and resets at the end of the day.
- The daily expansion limit is stored in the settings file, with a starting value of 2.

### Days

- The player starts day 1 with 10 bait.
- The day ends when a Golden Fish is caught.
- Remaining bait carries over to the next day, and 5 bait are added.
- The lucky number stays the same for the whole run.
- The needle starts slightly faster each new day.

| Day | Bait added at the start of the day |
| --- | --- |
| 1 | Start: 10 |
| 2 | +5 |
| 3 | +5 |
| 4 | +5 |
| 5 | +5 |

### Last Chance

- When bait runs out, the player gets one free last cast and the needle slows down by one step.
- An exact hit gives a Golden Fish and completes the day.
- Every other result, including an unlucky zone, ends the game.
- The Golden Fish check runs before the bait check. If the player catches a Golden Fish with their last bait, the Last Chance screen does not appear.

## Screens and warnings

The player must always be able to see their bait status and why the game ended.

| Moment | What to show |
| --- | --- |
| During play | Day, bait count, lucky number and dial |
| When a Rare Fish is caught | "Golden zone widened: ±3" message |
| When Crap is caught | "New unlucky zone added" message |
| When 1 bait is left | The bait counter turns red and a "Last bait!" warning appears |
| When bait runs out | "You're out of bait! One last cast with a bare hook. You need an exact hit: \[lucky number\]" |
| When a day ends | Day number and a "Continue" button |
| When the game ends | "You're out of bait", days reached and a "Play Again" button |

## Technical notes and scope

Keeping balance values in one place reduces testing time.

- Keep all balance values in a single settings file.
- `StartDay()` handles all start-of-day setup in one place. The dial layout, zone expansions and unlucky zones are refreshed here.
- `ResetRun()` resets the values for a new game and calls `StartDay()`.
- Tune balance with speed increase, zone shares and daily bait first.
- Simulate the game with bots of different hit rates to measure the average days reached.

The first version has no money system, shop, skill tree, depth choice, streak multiplier, "sell or turn into bait" option or persistent saves. If time allows, the first addition is a fish album.

## One-week plan

By the end of day 4, the game must be playable from start to finish, even with basic visuals.

- [ ] Day 1: Lucky number selection, dial and stopping
- [ ] Day 2: Bait, Golden Fish, day end and losing
- [ ] Day 3: Zone layout, fish effects and rising speed
- [ ] Day 4: Unlucky zones, day system and Last Chance
- [ ] Day 5: Visuals and UI
- [ ] Day 6: Sound, small animations, fish shadows and game feel
- [ ] Day 7: Balancing, testing and upload

## Claude Code starter prompt

Paste this prompt into Claude Code to get started.

```text
We are making a single-player fishing game in Unity.
First, write these rules to a GAME_DESIGN.md file.
Then build a working first version using simple shapes for visuals.
The dial is the most important part of the game. Focus on making it feel good first.

Goal:
- Complete each day by catching a Golden Fish before bait runs out, and reach as many days as possible.
- The game has no money or economy system.

Core loop:
- A lucky number from 1 to 100 is set at the start of the game. The player can type it or get a random one.
- Each cast costs 1 bait.
- The needle starts at a random number on a horizontal dial and moves between 1 and 100.
- The player stops the needle. The result depends on the zone where it stops.
- The needle speeds up slightly with each cast.

Zones:
- Golden zone (lucky number ±2, 5%): The day is completed
- Rare zone (8%): The Golden and Rare zones widen by 1 number
- Good zone (10%): The fish can be shredded for 2 bait
- Common zone (10%): The fish can be shredded for 1 bait
- Crap (remaining area): One more unlucky zone is added for that day

Dial:
- It always shows the full 1-100 range, with numbers under the tick marks.
- The lucky number stays in its real position and is clearly marked.
- The current number is shown above the needle.
- Zones use subtle colors, and their layout is regenerated every day.
- If the Golden zone reaches the end of the dial, it continues from the other end.

Zone placement order:
1. Place the Golden zone on the lucky number.
2. The 3 numbers on each side of the Golden zone cannot be Crap. Any numbers left empty here become Common Fish.
3. Place the unlucky zone outside the Golden zone and the safe area.
4. Distribute the Rare, Good and Common zones across the empty spaces. Split each share into 1-2 pieces, with each piece at least 3 numbers wide.
5. Make everything left over Crap.

Zone expansion:
- The Golden zone starts at ±2 every day.
- Expansion takes space from the Crap area and resets at the end of the day.
- Keep the daily expansion limit in the settings file, with a starting value of 2.

Unlucky zones:
- The first unlucky zone is chosen again every day and is about 5 numbers wide.
- It starts hidden. Once hit, it stays red until the end of the day.
- If the needle stops in this zone, the player loses 2 bait in total and the Crap effect does not trigger. Bait cannot go below 0.
- There can be at most 5 unlucky zones in a day. Added zones are removed at the end of the day.

Day system:
- The day ends when a Golden Fish is caught. This check runs before the out-of-bait check.
- The player starts with 10 bait. Remaining bait carries over to the next day, and 5 bait are added.
- The lucky number stays the same for the whole run.
- The needle starts slightly faster each new day.

Last Chance:
- When bait runs out, the player gets one free last cast and the needle slows down by one step.
- An exact hit gives a Golden Fish and completes the day.
- Every other result ends the game. The score is the number of days reached.

Technical rules:
- Keep all balance values and zone shares in a single settings file.
- Put day setup in StartDay() and new game setup in ResetRun().
- Refresh the dial layout, zone expansions and unlucky zones in StartDay().
- Do not add a shop, skill tree, fish shadows or persistent saves at this stage.
```
