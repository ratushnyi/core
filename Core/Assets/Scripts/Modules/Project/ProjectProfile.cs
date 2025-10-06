using System;
using MemoryPack;
using TendedTarsier.Core.Services.Profile;

namespace TendedTarsier.Core.Modules.Project
{
    [MemoryPackable(GenerateType.VersionTolerant)]
    public partial class ProjectProfile : ProfileBase
    {
        public override string Name => "Project";

        [MemoryPackOrder(0)]
        public DateTime FirstStartDate { get; set; }

        [MemoryPackOrder(1)]
        public string ServerId { get; set; }

        public override void OnSectionCreated()
        {
            FirstStartDate = DateTime.UtcNow;
        }
    }
}