using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MonitorLayoutManager
{
    public static class BrightnessHelper
    {
        #region Win32

        const byte VCP_BRIGHTNESS = 0x10;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct PHYSICAL_MONITOR
        {
            public IntPtr hPhysicalMonitor;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPhysicalMonitorDescription;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT { public int left, top, right, bottom; }

        delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        [DllImport("dxva2.dll")]
        static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, out uint pdwNumber);

        [DllImport("dxva2.dll")]
        static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwSize, [Out] PHYSICAL_MONITOR[] arr);

        [DllImport("dxva2.dll")]
        static extern bool SetVCPFeature(IntPtr hMonitor, byte bVCPCode, uint dwNewValue);

        [DllImport("dxva2.dll")]
        static extern bool DestroyPhysicalMonitors(uint dwSize, [In] PHYSICAL_MONITOR[] arr);

        #endregion

        public class BrightnessResult
        {
            public int Applied { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }

        static List<IntPtr> _enumBuffer;
        static bool MonitorEnumCallback(IntPtr hMon, IntPtr hdc, ref RECT rect, IntPtr data)
        {
            _enumBuffer.Add(hMon);
            return true;
        }

        public static BrightnessResult SetAllBrightness(uint percent)
        {
            var result = new BrightnessResult();
            _enumBuffer = new List<IntPtr>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnumCallback, IntPtr.Zero);
            var hMonitors = _enumBuffer;

            foreach (var hMon in hMonitors)
            {
                uint count;
                if (!GetNumberOfPhysicalMonitorsFromHMONITOR(hMon, out count) || count == 0)
                    continue;

                var physicals = new PHYSICAL_MONITOR[count];
                if (!GetPhysicalMonitorsFromHMONITOR(hMon, count, physicals))
                    continue;

                foreach (var pm in physicals)
                {
                    string name = pm.szPhysicalMonitorDescription;
                    if (!SetVCPFeature(pm.hPhysicalMonitor, VCP_BRIGHTNESS, percent))
                        result.Errors.Add(string.Format("{0}: DDC/CI não suportado ou sem permissão", name));
                    else
                        result.Applied++;
                }

                DestroyPhysicalMonitors(count, physicals);
            }

            return result;
        }
    }
}
