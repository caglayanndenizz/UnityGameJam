# Lucky Rod — Game Design Rules

Canonical ruleset for the game, transcribed from `Lucky Rod Game Design Document.md`'s
Claude Code starter prompt. This file is the reference for implementation decisions at
every step; update it if the design changes.

## Goal

- Complete each day by catching a Golden Fish before bait runs out, and reach as many
  days as possible.
- The game has no money or economy system.

## Core loop

- A lucky number from 1 to 100 is set at the start of the game. The player can type it
  or get a random one.
- Each cast costs 1 bait.
- The needle starts at a random number on a horizontal dial and moves between 1 and 100.
- The player stops the needle. The result depends on the zone where it stops.
- The needle speeds up slightly with each cast.

## Zones

- Golden zone (lucky number ±2, 5%): The day is completed.
- Rare zone (8%): The Golden and Rare zones widen by 1 number.
- Good zone (10%): The fish can be shredded for 2 bait.
- Common zone (10%): The fish can be shredded for 1 bait.
- Crap (remaining area): One more unlucky zone is added for that day.

## Dial

- It always shows the full 1-100 range, with numbers under the tick marks.
- The lucky number stays in its real position and is clearly marked.
- The current number is shown above the needle.
- Zones use subtle colors, and their layout is regenerated every day.
- If the Golden zone reaches the end of the dial, it continues from the other end.

### Zone placement order

1. Place the Golden zone on the lucky number.
2. The 3 numbers on each side of the Golden zone cannot be Crap. Any numbers left empty
   here become Common Fish.
3. Place the unlucky zone outside the Golden zone and the safe area.
4. Distribute the Rare, Good and Common zones across the empty spaces. Split each share
   into 1-2 pieces, with each piece at least 3 numbers wide.
5. Make everything left over Crap.

### Zone expansion

- The Golden zone starts at ±2 every day.
- Expansion takes space from the Crap area and resets at the end of the day.
- Keep the daily expansion limit in the settings file, with a starting value of 2.

## Unlucky zones

- The first unlucky zone is chosen again every day and is about 5 numbers wide.
- It starts hidden. Once hit, it stays red until the end of the day.
- If the needle stops in this zone, the player loses 2 bait in total and the Crap effect
  does not trigger. Bait cannot go below 0.
- There can be at most 5 unlucky zones in a day. Added zones are removed at the end of
  the day.

## Day system

- The day ends when a Golden Fish is caught. This check runs before the out-of-bait
  check.
- The player starts with 10 bait. Remaining bait carries over to the next day, and 5
  bait are added.
- The lucky number stays the same for the whole run.
- The needle starts slightly faster each new day.

## Last Chance

- When bait runs out, the player gets one free last cast and the needle slows down by
  one step.
- An exact hit gives a Golden Fish and completes the day.
- Every other result ends the game. The score is the number of days reached.

## Technical rules

- Keep all balance values and zone shares in a single settings file.
- Put day setup in `StartDay()` and new game setup in `ResetRun()`.
- Refresh the dial layout, zone expansions and unlucky zones in `StartDay()`.
- Do not add a shop, skill tree, fish shadows or persistent saves at this stage.

## Implementation progress

- [x] Day 1 (slice 1): Dial + needle + stop (`Assets/Scripts/Dial/`). Lucky number is
      currently random-per-session and only used for the marker; no selection UI yet.
- [ ] Lucky number selection (type or randomize)
- [ ] Bait, Golden Fish, day end and losing
- [ ] Zone layout, fish effects and rising speed
- [ ] Unlucky zones, day system and Last Chance
- [ ] Visuals and UI polish
- [ ] Sound, small animations, fish shadows and game feel
- [ ] Balancing, testing and upload
