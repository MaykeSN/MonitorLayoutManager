# Monitor Layout Manager

A lightweight Windows utility to save and restore multi-monitor configurations — especially useful when a portable monitor disconnects and Windows scrambles your display layout.

## The problem

When a portable monitor is disconnected, Windows resets the positions and extended display settings of all monitors. Every reconnect requires manually dragging monitors back into place in Display Settings.

## The solution

Save your ideal layout once. Restore it with one click whenever things get scrambled.

![screenshot](docs/screenshot.png)

## Features

- **Save Layout** — snapshots all connected monitors (position, resolution, refresh rate, orientation, primary flag) to a JSON file
- **Configure Monitors** — reads the JSON and reapplies every setting via `ChangeDisplaySettingsEx`; monitors that are no longer connected are silently skipped instead of causing an error
- **Brightness 100%** — sets brightness to 100% on all monitors via DDC/CI (`SetVCPFeature`); useful for portable monitors that ignore the Windows brightness slider

## Requirements

- Windows 10 / 11
- [.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) (pre-installed on Windows 10 1903+ and all Windows 11)

## Building

Requires the [.NET SDK](https://dotnet.microsoft.com/download) (any version ≥ 6).

```bash
dotnet build -c Release
```

Output: `bin\Release\net48\MonitorLayoutManager.exe`

## Usage

1. Arrange your monitors exactly the way you want in Windows Display Settings
2. Open **Monitor Layout Manager** and click **Save Layout** — this creates `layout.json` next to the `.exe`
3. Next time the portable monitor disconnects and Windows scrambles everything, reconnect it, open the app and click **Configure Monitors**
4. Optionally click **Brightness 100%** if your portable monitor resets its brightness on reconnect

You can keep multiple layout files and use **Browse…** to switch between them (e.g. `home.json`, `office.json`).

## Notes on Brightness 100%

Brightness control uses DDC/CI, the standard protocol for sending commands to external monitors over the display cable. It works on most portable monitors connected via HDMI or DisplayPort. It will **not** work if:

- The monitor does not support DDC/CI
- The cable or adapter does not pass DDC signals (common with cheap USB-C → HDMI adapters)
- A monitor driver is blocking low-level access

The app reports which monitors were affected and which were skipped.

## License

MIT
