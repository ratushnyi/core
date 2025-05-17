using System;
using MemoryPack;
using TendedTarsier.Core.Services.Profile;
using Zenject;

namespace TendedTarsier.Core.Modules.General
{
    [MemoryPackable(GenerateType.VersionTolerant)]
    public partial class GeneralProfile : ProfileBase
    {
        private GeneralConfig _generalConfig;
        public override string Name => "General";

        [MemoryPackOrder(0)]
        public DateTime? FirstStartDate { get; set; }

        [MemoryPackOrder(1)]
        public string LastScene { get; set; }

        [Inject]
        private void Construct(GeneralConfig generalConfig)
        {
            _generalConfig = generalConfig;
        }

        public void StartNewGame()
        {
            FirstStartDate = DateTime.UtcNow;
            LastScene = _generalConfig.NewGameScene;
        }
    }
}