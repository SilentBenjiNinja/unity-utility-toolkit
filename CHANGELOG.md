# 1.2.0

### Improvements

* `MathUtils` — added fast approximation methods: `SqrtFast`, `SinFast`, `Log2Fast`, `Log10Fast`
* `MathUtils` — fixed `Floor` to correctly handle negative values (was truncating toward zero, now floors toward negative infinity)
* `MathUtils` — fixed `LocalToWorldPosition` double-counting translation; now delegates to `TransformPoint`
* `MathUtils` — `Round(int)` and `Round(float, int)` now use `MidpointRounding.AwayFromZero` for intuitive rounding
* `StringUtils` — `GetDecimalString` now uses `#` format specifier to strip trailing zeros; `maxPrecision = 0` no longer produces a trailing dot
* `StringUtils` — `GetTimerString` now uses `D2` format specifier for zero-padded seconds
* `StringUtils` — `GetHighDecimalString` threshold corrected to `> 999` so that `1000` abbreviates to `1.0K`
* Added XML documentation to all public members in `MathUtils` and `StringUtils`

---

# 1.1.2

### Bug Fixes

* Marked Editor assembly (`com.bnj.utility-toolkit.Editor.asmdef`) as editor-only — fixes a build error caused by editor code being included in player builds

---

# 1.1.1

### Improvements

* Added XML documentation to all public APIs across all modules
* Improved `package.json` description
* Expanded README with per-module usage examples

---

# 1.1.0

* Change some namespaces

---

# 1.0.1

* Updated README + package description
