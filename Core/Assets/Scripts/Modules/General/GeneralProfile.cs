using System;
using MemoryPack;
using TendedTarsier.Core.Services.Profile;

namespace TendedTarsier.Core.Modules.General
{
    [MemoryPackable(GenerateType.VersionTolerant)]
    public partial class GeneralProfile : ProfileBase
    {
        public override string Name => "General";
        
        [MemoryPackOrder(0)]
        public DateTime? FirstStartDate { get; set; }

        [MemoryPackOrder(1)]
        public string LastScene { get; set; }
    }
}
