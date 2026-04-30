using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MonitorLayoutManager
{
    public static class DisplayHelper
    {
        #region Win32 structs / constants

        const int ENUM_CURRENT_SETTINGS = -1;
        const int CDS_UPDATEREGISTRY = 0x01;
        const int CDS_NORESET = 0x10000000;
        const int CDS_RESET = 0x40000000;
        const int CDS_SET_PRIMARY = 0x00000010;
        const int DISP_CHANGE_SUCCESSFUL = 0;

        const int DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x1;
        const int DISPLAY_DEVICE_PRIMARY_DEVICE = 0x4;

        [Flags]
        enum DM : uint
        {
            Position = 0x00000020,
            PelsWidth = 0x00080000,
            PelsHeight = 0x00100000,
            BitsPerPel = 0x00040000,
            DisplayFrequency = 0x00400000,
            DisplayOrientation = 0x00000080,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public DM dmFields;
            public POINTL dmPosition;
            public uint dmDisplayOrientation;
            public uint dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public uint dmBitsPerPel;
            public uint dmPelsWidth;
            public uint dmPelsHeight;
            public uint dmDisplayFlags;
            public uint dmDisplayFrequency;
            public uint dmICMMethod;
            public uint dmICMIntent;
            public uint dmMediaType;
            public uint dmDitherType;
            public uint dmReserved1;
            public uint dmReserved2;
            public uint dmPanningWidth;
            public uint dmPanningHeight;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINTL
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct DISPLAY_DEVICE
        {
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            public int StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int ChangeDisplaySettingsEx(string lpszDeviceName, IntPtr lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

        #endregion

        public static List<MonitorLayout> GetCurrentLayout()
        {
            var list = new List<MonitorLayout>();
            var dd = new DISPLAY_DEVICE();
            dd.cb = Marshal.SizeOf(dd);

            for (uint i = 0; EnumDisplayDevices(null, i, ref dd, 0); i++)
            {
                if ((dd.StateFlags & DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) == 0)
                {
                    dd.cb = Marshal.SizeOf(dd);
                    continue;
                }

                var dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(dm);

                if (!EnumDisplaySettings(dd.DeviceName, ENUM_CURRENT_SETTINGS, ref dm))
                {
                    dd.cb = Marshal.SizeOf(dd);
                    continue;
                }

                list.Add(new MonitorLayout
                {
                    DeviceName = dd.DeviceName,
                    DeviceString = dd.DeviceString,
                    PositionX = dm.dmPosition.x,
                    PositionY = dm.dmPosition.y,
                    Width = (int)dm.dmPelsWidth,
                    Height = (int)dm.dmPelsHeight,
                    BitsPerPel = (int)dm.dmBitsPerPel,
                    DisplayFrequency = (int)dm.dmDisplayFrequency,
                    DisplayOrientation = (int)dm.dmDisplayOrientation,
                    IsPrimary = (dd.StateFlags & DISPLAY_DEVICE_PRIMARY_DEVICE) != 0,
                    IsAttached = true,
                });

                dd.cb = Marshal.SizeOf(dd);
            }

            return list;
        }

        public static HashSet<string> GetAttachedDeviceNames()
        {
            var attached = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var dd = new DISPLAY_DEVICE();
            dd.cb = Marshal.SizeOf(dd);
            for (uint i = 0; EnumDisplayDevices(null, i, ref dd, 0); i++)
            {
                if ((dd.StateFlags & DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) != 0)
                    attached.Add(dd.DeviceName);
                dd.cb = Marshal.SizeOf(dd);
            }
            return attached;
        }

        public static ApplyResult ApplyLayout(List<MonitorLayout> monitors)
        {
            var attached = GetAttachedDeviceNames();
            var skipped = new List<string>();
            int applied = 0;

            // Primário deve vir primeiro para o Windows aceitar as posições
            var ordered = new List<MonitorLayout>(monitors);
            ordered.Sort((a, b) => b.IsPrimary.CompareTo(a.IsPrimary));

            foreach (var m in ordered)
            {
                if (!attached.Contains(m.DeviceName))
                {
                    skipped.Add(m.DeviceName);
                    continue;
                }

                var dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(dm);
                dm.dmDeviceName = m.DeviceName;
                dm.dmPosition = new POINTL { x = m.PositionX, y = m.PositionY };
                dm.dmPelsWidth = (uint)m.Width;
                dm.dmPelsHeight = (uint)m.Height;
                dm.dmBitsPerPel = (uint)m.BitsPerPel;
                dm.dmDisplayFrequency = (uint)m.DisplayFrequency;
                dm.dmDisplayOrientation = (uint)m.DisplayOrientation;
                dm.dmFields = DM.Position | DM.PelsWidth | DM.PelsHeight |
                              DM.BitsPerPel | DM.DisplayFrequency | DM.DisplayOrientation;

                uint flags = CDS_UPDATEREGISTRY | CDS_NORESET;
                if (m.IsPrimary)
                    flags |= CDS_SET_PRIMARY;

                int result = ChangeDisplaySettingsEx(m.DeviceName, ref dm, IntPtr.Zero, flags, IntPtr.Zero);
                if (result != DISP_CHANGE_SUCCESSFUL)
                    return new ApplyResult { Success = false, Error = string.Format("Falha em {0}: código {1}", m.DeviceName, result) };

                applied++;
            }

            if (applied == 0)
                return new ApplyResult { Success = false, Error = "Nenhum monitor do layout está conectado no momento." };

            // aplica todas as mudanças de uma vez
            ChangeDisplaySettingsEx(null, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero);
            return new ApplyResult { Success = true, Skipped = skipped };
        }

        public class ApplyResult
        {
            public bool Success { get; set; }
            public string Error { get; set; }
            public List<string> Skipped { get; set; } = new List<string>();
        }
    }
}
