using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TendedTarsier.Core.Modules.General;
using TendedTarsier.Core.Panels;
using TendedTarsier.Core.Services.Profile;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuPanel : PanelBase
    {
        [Header("UI")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitButton;

        [Header("Animation")]
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] public Ease _fadeOutCurve = Ease.InSine;

        private EventSystem _eventSystem;
        private ProfileService _profileService;
        private GeneralProfile _generalProfile;
        private MenuConfig _menuConfig;
        private PanelLoader<LoadingPanel> _loadingPanel;

        [Inject]
        private void Construct(
            PanelLoader<LoadingPanel> loadingPanel,
            MenuConfig menuConfig,
            GeneralProfile generalProfile,
            ProfileService profileService,
            EventSystem eventSystem)
        {
            _loadingPanel = loadingPanel;
            _menuConfig = menuConfig;
            _generalProfile = generalProfile;
            _profileService = profileService;
            _eventSystem = eventSystem;
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitButtons();
        }

        private void InitButtons()
        {
            _continueButton.targetGraphic.color = Color.clear;
            _newGameButton.targetGraphic.color = Color.clear;
            _exitButton.targetGraphic.color = Color.clear;
            _continueButton.interactable = _generalProfile.FirstStartDate != null;
            _eventSystem.SetSelectedGameObject(_continueButton.interactable ? _continueButton.gameObject : _newGameButton.gameObject);
        }

        public async UniTask ShowButtons()
        {
            var sequence = DOTween.Sequence();
            sequence.Join(_continueButton.targetGraphic.DOColor(Color.white, _fadeOutDuration));
            sequence.Join(_newGameButton.targetGraphic.DOColor(Color.white, _fadeOutDuration));
            sequence.Join(_exitButton.targetGraphic.DOColor(Color.white, _fadeOutDuration));
            sequence.SetEase(_fadeOutCurve);

            CompositeDisposable.Add(Disposable.Create(() => sequence.Kill()));

            await sequence.ToUniTask();

            _continueButton.OnClickAsObservable().Subscribe(_ => OnContinueButtonClick()).AddTo(CompositeDisposable);
            _newGameButton.OnClickAsObservable().Subscribe(_ => OnNewGameButtonClick().Forget()).AddTo(CompositeDisposable);
            _exitButton.OnClickAsObservable().Subscribe(_ => OnExitButtonClick()).AddTo(CompositeDisposable);
        }

        private void OnContinueButtonClick()
        {
            CompositeDisposable.Dispose();
            SceneManager.LoadScene(_generalProfile.LastScene);
        }

        private async UniTaskVoid OnNewGameButtonClick()
        {
            CompositeDisposable.Dispose();
            _profileService.ClearAll();
            _generalProfile.FirstStartDate = DateTime.UtcNow;
            _generalProfile.LastScene = _menuConfig.NewGameScene;
            _generalProfile.Save();
            await _loadingPanel.Show();
            SceneManager.LoadScene(_menuConfig.NewGameScene);
        }

        private void OnExitButtonClick()
        {
            CompositeDisposable.Dispose();
            Application.Quit();
        }
    }
}