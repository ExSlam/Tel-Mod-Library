
# Going Viral 1.0.1

## Crash/data-corruption fixes

- Removed the single-sales integer divide-by-zero path when a trending release has zero casual new fans.
- Reworked the save marker so the private trending value is consumed instead of being written into vanilla Buzz. New markers cannot collide with vanilla Buzz, old `10000 + trend` saves (including negative trends) are migrated, and loading a markerless save resets stale static trend state.
- Replaced hard-coded fan-tooltip child indices with named lines, preventing UI hierarchy changes from producing index errors/writing to the wrong line.

## Math/gameplay fixes

- Negative trending now uses a positive churn multiplier instead of a sign-flipping coefficient that could turn fan loss into fan gain.
- Fixed integer-division errors in TV trending chance calculations (`/20`, `/5`, and days/365).
- Removed the broken fame interpolation from fan-acquisition weights; new-fan distribution now follows non-negative appeal with the documented 3x casual weighting and ignores opinion.
- Fan churn uses non-negative appeal, clamped opinion, and a stable 3x casual weighting. Crisis strength scales the total churn instead of reversing or distorting the demographic weights.
- Singles and shows now apply the trending multiplier to total new fans, with the bonus allocated primarily to casual buckets, matching the mod description.
- Weekly tooltip totals now include cafe fans.
- TV genre tooltips now compute the most recent TV show per displayed genre button, not from the currently selected genre for every button.
- Trend/crisis duration notifications use the clamped saved duration; negative trends are discarded if Fan Attrition is not installed.

## Compatibility hardening

- Replaced the brittle show-sales IL-local transpiler with scoped patches around `SetSales`, `AddFans_Equally`, and `SetNewFans`.
- Replaced fan distribution IL-local transpilers with scoped Harmony contexts. Vanilla allocation/rounding remains in place while only weighting calls are substituted.
- Added exception-safe context cleanup and null guards throughout show, theater, tooltip, save/load, contract and marketing-roll hooks.
- Preserved explicit integration with `com.tel.fanattrition` and `com.tel.unofficialpatch`.

---
# Worker Rights 1.0.1 

## Fixes

- Fixed the low-salary graduation penalty: `DateTime.AddDays()` is immutable, so the original call discarded the new date. The returned date is now assigned.
- Applies the complete advertised 10x low-salary penalty (-10 or -30 days), because the supplied vanilla `Graduation_Date_Update()` has the same discarded-`AddDays` no-op for its own -1/-3 salary adjustment.
- Kept this adjustment on the game's existing weekly `Graduation_Date_Update()` cadence. It is not silently converted into a daily 7x balance change.
- The hard-mode max-fame 10%-of-earnings rule is now a salary floor and can no longer lower a larger vanilla/third-party expected salary.
- The low-fame expected salary is likewise enforced as a floor rather than lowering a larger result.
- Added null/invalid-value guards for staff, policies, generated idols and earnings.

---
# Going Viral 1.0.1


## Crash/data-corruption fixes

- Removed the single-sales integer divide-by-zero path when a trending release has zero casual new fans.
- Reworked the save marker so the private trending value is consumed instead of being written into vanilla Buzz. New markers cannot collide with vanilla Buzz, old `10000 + trend` saves (including negative trends) are migrated, and loading a markerless save resets stale static trend state.
- Replaced hard-coded fan-tooltip child indices with named lines, preventing UI hierarchy changes from producing index errors/writing to the wrong line.

## Math/gameplay fixes

- Negative trending now uses a positive churn multiplier instead of a sign-flipping coefficient that could turn fan loss into fan gain.
- Fixed integer-division errors in TV trending chance calculations (`/20`, `/5`, and days/365).
- Removed the broken fame interpolation from fan-acquisition weights; new-fan distribution now follows non-negative appeal with the documented 3x casual weighting and ignores opinion.
- Fan churn uses non-negative appeal, clamped opinion, and a stable 3x casual weighting. Crisis strength scales the total churn instead of reversing or distorting the demographic weights.
- Singles and shows now apply the trending multiplier to total new fans, with the bonus allocated primarily to casual buckets, matching the mod description.
- Weekly tooltip totals now include cafe fans.
- TV genre tooltips now compute the most recent TV show per displayed genre button, not from the currently selected genre for every button.
- Trend/crisis duration notifications use the clamped saved duration; negative trends are discarded if Fan Attrition is not installed.

## Compatibility hardening

- Replaced the brittle show-sales IL-local transpiler with scoped patches around `SetSales`, `AddFans_Equally`, and `SetNewFans`.
- Replaced fan distribution IL-local transpilers with scoped Harmony contexts. Vanilla allocation/rounding remains in place while only weighting calls are substituted.
- Added exception-safe context cleanup and null guards throughout show, theater, tooltip, save/load, contract and marketing-roll hooks.
- Preserved explicit integration with `com.tel.fanattrition` and `com.tel.unofficialpatch`.
***

# Never Graduate 1.2.0

- Harmony patches data_girls.girls.Graduation_Date_Update and always skips the original method.
- Graduation_Date values are therefore inert while Never Graduate is enabled.
- Worker Rights can no longer turn low salary satisfaction into an automatic graduation.
- Traits Expansion / Job Hopper can no longer turn its shortened default graduation date into an automatic graduation.
- Already-announced idols cannot complete date-driven graduation while the mod is enabled.
- Direct firing still works, preserving the original "unless the girl is fired" behavior.
---

## Correctness fixes

- **MBTI Personalities**: fixed ISTP concert accident handling so the trait halves the remaining failure chance while keeping AccidentSuccessChance in 0-100 percentage units.
- **Extended SSK**: fame bonuses for ranks beyond 10 now read GetFameBaseVal from the current election instance instead of a delegate permanently bound to the first election.
- **Fan Attrition**: restored floating-point division in the MC fame coefficient so fame 1-3 and other low-fame values receive the intended quadratic boost.
- **Growing Distant**: salary-based positive influence is now capped at the vanilla 512-point relationship maximum, preventing hidden overflow points that delay later decay.
- **ModMenus**: corrected the omitted slider default to the arithmetic midpoint, (min + max) / 2.
- **Stale Theater Shows**: repaired the attendance transpiler branch target so normal ticket-price execution passes through the custom attendance multiplier.
- **Traits Expansion**: Sadistic now detects active bullies rather than bullied victims, and Wooden Acting / Quick Wit now apply true -50% / +50% multiplicative business reward modifiers.
- **Targeted Auditions**: settings are now scoped to Auditions.GenerateGirls, preventing age/stat/sexuality settings from leaking into rival, story, unique, or other non-audition girl generation.
- **Targeted Auditions**: body IDs remain unique until the currently eligible audition body pool is exhausted; only then are IDs recycled for larger candidate counts.
- **Targeted Auditions**: birthday generation now produces ages exactly within the configured inclusive minimum/maximum range, eliminating the max-age + 1 boundary case.