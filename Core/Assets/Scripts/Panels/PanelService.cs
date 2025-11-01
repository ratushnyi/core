using System.Collections.Generic;
using System.Linq;
using TendedTarsier.Core.Services;
using UniRx;
using UniRx.Triggers;

namespace TendedTarsier.Core.Panels
{
    public class PanelService : ServiceBase
    {
        public List<PanelBase> ActivePanels { get; private set; } = new();

        public bool IsAnyPopupOpen => ActivePanels.Any(t => t is PopupBase);
        
        public void RegisterPanel(PanelBase panel)
        {
            ActivePanels.Add(panel);
            panel.OnDestroyAsObservable().Subscribe(_ => ActivePanels.Remove(panel)).AddTo(CompositeDisposable);
        }
    }
}