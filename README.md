# Utility Toolkit for Unity

A collection of lightweight extension methods and static helpers covering the most common Unity development patterns. No dependencies, no allocations beyond what Unity itself requires.

## Modules

### `CoroutineUtils` — deferred & repeated actions

Extension methods on `MonoBehaviour`. All methods return the `Coroutine` so it can be stopped, and are no-ops when the behaviour is disabled.

```csharp
// Invoke once after a delay
this.DelayInvoke(() => Explode(), 2f);

// Invoke once a condition is met
this.DelayInvokeUntil(() => Reload(), () => _ammo > 0);

// Repeat every frame while active (receives delta time)
this.RepeatInvoke(dt => Move(dt));

// Repeat every 0.5s while a condition holds
this.RepeatInvokeWhile(dt => Burn(dt), () => _isOnFire, 0.5f);
```

### `AnimationSequence` — fluent timed action chain

```csharp
AnimationSequence.Create(this)
    .Wait(0.2f).Then(() => PlaySwingFX())
    .Wait(0.1f).Then(() => DealDamage())
    .Wait(0.4f).Then(() => ResetPose())
    .Run();
```

### `MathUtils` — math extensions

Extension methods on `float`, `int`, `Vector2`, `Vector3`, `Color`, and `Transform`. Covers clamp, abs, floor/ceil/round, lerp, log, sqrt, pow, and 2D look-rotation utilities.

```csharp
float t = rawValue.Clamp01();
Color faded = color.WithAlpha(0.5f);
Quaternion rot = MathUtils.Get2DLookAtRotation(transform.position, target.position);
bool close = MathUtils.IsDistanceBetweenPointsCloserThan(a, b, 3f);
```

### `RandomUtils` — random values

```csharp
float f = RandomUtils.RandomFloatBetween(0.5f, 1.5f);
bool hit = RandomUtils.Chance(0.25f); // 25% chance
Vector3 pos = RandomUtils.RandomInsideSphere(origin, radius);
float noise = RandomUtils.Perlin(Time.time, frequency: 2f);
```

### `CollectionUtils` — shuffle & random selection

```csharp
var shuffled = myList.Shuffle();
var picked = myList.GetRandomElement();
var subset = myList.GetRandomElements(3);
```

### `EnumUtils` — enum iteration & conversion

```csharp
EnumUtils.ForEach<WeaponType>(type => Register(type));
var lookup = EnumUtils.CreateDictionary<WeaponType, int>(defaultValue: 0);
WeaponType random = EnumUtils<WeaponType>.RandomEnum;
int raw = WeaponType.Sword.ToInt();
```

### `StringUtils` — game UI formatting

```csharp
string pct  = 0.75f.GetPercentageString();          // "75%"
string big  = 12500f.GetHighDecimalString();         // "12.5K"
string time = 90f.GetTimerString();                  // "1:30"
string nice = "playerHealth".AddSpaces();            // "player Health"
```

### `LogUtils` — structured logging

Level-based wrapper around `UnityEngine.Debug` with colour-coded output and clickable call-site links in the Editor.

```csharp
LogUtils.Debug("Pool initialised");
LogUtils.Warn("Ammo low", context: "WeaponSystem");
LogUtils.Error("Save failed");
```

### `AudioUtils` — audio math

```csharp
float db = AudioUtils.AmplitudeToDecibels(slider.value); // for AudioMixer volume
```

### `TimeUtils` — time access

```csharp
TimeUtils.TimeScale = 0f; // pause
float dt = TimeUtils.DeltaTime;
```

### `DebugUtils` — hierarchy path

```csharp
Debug.Log(gameObject.GetFullHierarchyName()); // e.g. "Player/Visuals/Mesh"
```

### `ComponentUtils` — child component access by path

```csharp
var mesh = this.GetComponentFromChild<MeshRenderer>("Visuals/Mesh");
```
