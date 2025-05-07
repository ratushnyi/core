using System;
using Cysharp.Threading.Tasks;
using TendedTarsier.Core.Modules.General;
using TendedTarsier.Core.Panels;
using UnityEngine;
using Zenject;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private float _loadingDelay = 1f;
        private PanelLoader<MenuPanel> _menuPanel;
        private PanelLoader<LoadingPanel> _loadingPanel;

        [Inject]
        private void Construct(PanelLoader<MenuPanel> menuPanel, PanelLoader<LoadingPanel> loadingPanel)
        {
            _menuPanel = menuPanel;
            _loadingPanel = loadingPanel;
        }

        public void Start()
        {
            Show().Forget();
        }

        private async UniTaskVoid Show()
        {
            _loadingPanel.Show(true).Forget();
            _menuPanel.Show(true).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(_loadingDelay));
            await _loadingPanel.Hide();
            await _menuPanel.Instance.ShowButtons();
        }
    }
}