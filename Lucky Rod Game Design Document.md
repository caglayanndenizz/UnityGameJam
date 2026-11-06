# Lucky Rod Game Design Document

Nov 6, 2026 · @chaggy

## Summary

Lucky Rod (working title) is a single-screen push-your-luck fishing game. The player keeps fishing to land exactly on the day's lucky number and tries to stay ahead of rising debt.

| Topic | Decision |
| --- | --- |
| Jam theme | Lucky number |
| Genre | Push-your-luck fishing game with Balatro-style charms |
| Players | Single-player |
| Visual style | Single-screen, minimalist pixel art |
| Development time | 1 week |
| AI use | Allowed, development will use Claude Code |
| Engine | Unity |

## Core loop

A day lasts until the player chooses to return to the harbor or tears the net.

1. Each morning, the harbor fortune teller announces the day's lucky number.
2. The player casts the rod and catches a numbered fish.
3. The fish goes into the bucket and its number is added to the bucket total.
4. The player chooses to cast again or return to the harbor.
5. The player sells the catch and spends money on bait and rods.
6. The next day starts with a new lucky number.

&#91;embedded content: game flow · one day\]

The flow assumes debt is paid before the shop opens.

## Bucket and fish

Returning with a bucket total exactly equal to the lucky number triples the payout, but going over it loses the whole catch.

| Bucket total | Outcome | Money earned |
| --- | --- | --- |
| Below the lucky number | Normal sale | Bucket total |
| Equal to the lucky number | Jackpot | Bucket total × 3 |
| Above the lucky number | Net tears, all fish escape | 0 |

With a lucky number of 17, returning with 12 earns 12 and returning with 17 earns 51.

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

The fortune teller gives the player the information needed to judge each cast.

- The lucky number is chosen each morning from 12 to 24.
- The screen shows the day's fish list.
- The fortune teller can give hints such as "Bonito is plentiful today."
- Rarely, the lucky number is 13. On those days, the cursed fish becomes a lucky fish.

## Shop and charms

Money buys bait and rods that work like Balatro's charms and change how a run plays.

| Item | Effect |
| --- | --- |
| Deep Bait | Only high-number fish come |
| Amulet | Cancels the cursed 13 once |
| Second Bucket | Lets the player play with two separate targets |

## Debt

Rent or boat debt rises every day, and the game ends if the player cannot pay.

## Single screen

The whole game runs on one minimalist pixel art screen, with both decision buttons always in view.

&#91;embedded content: screen layout · example values\]

## Technical notes and scope

Game values stay in data so balance and scope can change during development.

- Keep fish, shop and debt values in a single settings file.
- `StartDay()` picks the lucky number and fish list, then empties the bucket.
- `ResetRun()` resets the values for a new game and calls `StartDay()`.
- Define charms as data with small effect hooks, so new ones are cheap to add.
- Use bot simulations to measure how well safe play performs against rising debt.

## One-week plan

The game must be playable from start to finish by the end of day 4, even with basic visuals.

- [ ] Day 1: Lucky number, casting, bucket and return decision
- [ ] Day 2: Sale, jackpot, torn net and cursed fish
- [ ] Day 3: Day cycle, debt and game over
- [ ] Day 4: Shop and first charms
- [ ] Day 5: Pixel art scene and interface
- [ ] Day 6: Sound, small animations and game feel
- [ ] Day 7: Balancing, testing and upload

## Open questions

- What does the lucky fish do on a 13 day?
- Is the daily cost rent or boat debt, how much does it rise, and when is it paid?
- What is the full fish list, and how likely is each fish to appear?
- How do the bait and rod items differ, and how much do they cost?
- How does the Second Bucket assign fish and set its second target?

## Claude Code starter prompt

Paste this prompt into Claude Code to get started.

```text
We are making a single-screen, single-player fishing game in Unity with minimalist pixel art.
First, write these rules to a GAME_DESIGN.md file.
Then build a working first version using simple shapes for visuals.

Core loop:
- Each morning, the harbor fortune teller announces the day's lucky number from 12 to 24.
- The player casts the rod and catches a fish with a number on it.
- The fish goes into the bucket and its number is added to the bucket total.
- After each catch, the player chooses to cast again or return to the harbor.
- The day lasts until the player returns or the net tears.

Results:
- Return below the lucky number: earn the bucket total.
- Return exactly on the lucky number: jackpot, earn the bucket total × 3.
- Go above the lucky number: the net tears, all fish escape and the player earns 0.

Fish:
- Anchovy: 2
- Sea bass: 5
- Bonito: 7
- Cursed fish: 13

Cursed fish:
- It does not add to the bucket total.
- A random fish escapes from the bucket, and the total drops by its number.
- If the bucket is empty, nothing happens.

Fortune teller:
- The screen shows the day's fish list.
- The fortune teller can give hints such as "Bonito is plentiful today."
- Rarely, the lucky number is 13. On those days, the cursed fish becomes a lucky fish. Leave its exact effect as a placeholder for now.

Debt:
- Rent or boat debt rises every day.
- The game ends if the player cannot pay.
- Use placeholder values in the settings file for now.

Shop:
- Money buys bait and rods that work like Balatro's charms.
- Add Deep Bait: only high-number fish come.
- Add Amulet: cancels the cursed 13 once.
- Leave Second Bucket for later.

Technical rules:
- Keep fish, shop and debt values in a single settings file.
- StartDay() picks the lucky number and fish list, then empties the bucket.
- ResetRun() resets the values for a new game and calls StartDay().
- Define charms as data with small effect hooks.
```
