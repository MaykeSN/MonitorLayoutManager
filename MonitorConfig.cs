using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MonitorLayoutManager
{
    [DataContract]
    public class MonitorLayout
    {
        [DataMember] public string DeviceName { get; set; }
        [DataMember] public string DeviceString { get; set; }
        [DataMember] public int PositionX { get; set; }
        [DataMember] public int PositionY { get; set; }
        [DataMember] public int Width { get; set; }
        [DataMember] public int Height { get; set; }
        [DataMember] public int BitsPerPel { get; set; }
        [DataMember] public int DisplayFrequency { get; set; }
        [DataMember] public int DisplayOrientation { get; set; }
        [DataMember] public bool IsPrimary { get; set; }
        [DataMember] public bool IsAttached { get; set; }
    }

    [DataContract]
    public class SavedLayout
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public List<MonitorLayout> Monitors { get; set; } = new List<MonitorLayout>();
    }
}
