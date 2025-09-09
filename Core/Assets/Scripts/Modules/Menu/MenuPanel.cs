using Cysharp.Threading.Tasks;
using DG.Tweening;
using TendedTarsier.Core.Modules.Project;
using TendedTarsier.Core.Panels;
using TendedTarsier.Core.Services.Modules;
using TendedTarsier.Core.Services.Profile;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
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

        private EventSystem _eventSystem;
        private ProfileService _profileService;
        private ModuleService _moduleService;
        private ProjectProfile _projectProfile;
        private MenuModuleConfig _menuModuleConfig;

        [Inject]
        private void Construct(
            MenuModuleConfig menuModuleConfig,
            ProjectProfile projectProfile,
            ProfileService profileService,
            ModuleService moduleService,
            EventSystem eventSystem)
        {
            _menuModuleConfig = menuModuleConfig;
            _projectProfile = projectProfile;
            _profileService = profileService;
            _moduleService = moduleService;
            _eventSystem = eventSystem;
        }

        public override async UniTask ShowAnimation()
        {
            InitButtons();
            await ShowButtons();
            SubscribeButtons();
        }

        private void InitButtons()
        {
            _continueButton.targetGraphic.color = Color.clear;
            _newGameButton.targetGraphic.color = Color.clear;
            _exitButton.targetGraphic.color = Color.clear;
            _continueButton.interactable = !string.IsNullOrEmpty(_projectProfile.LastGameplayScene);
            _eventSystem.SetSelectedGameObject(_continueButton.interactable ? _continueButton.gameObject : _newGameButton.gameObject);
        }

        private void SubscribeButtons()
        {
            _continueButton.OnClickAsObservable().Subscribe(_ => OnContinueButtonClick()).AddTo(CompositeDisposable);
            _newGameButton.OnClickAsObservable().Subscribe(_ => OnNewGameButtonClick()).AddTo(CompositeDisposable);
            _exitButton.OnClickAsObservable().Subscribe(_ => OnExitButtonClick()).AddTo(CompositeDisposable);
        }

        private async UniTask ShowButtons()
        {
            var showSequence = createSequence();
            
            CompositeDisposable.Add(Disposable.Create(() => showSequence.Kill()));

            await showSequence.ToUniTask();

            Sequence createSequence()
            {
                var sequence = DOTween.Sequence();
                sequence.Join(_continueButton.targetGraphic.DOColor(Color.white, AnimationDuration));
                sequence.Join(_newGameButton.targetGraphic.DOColor(Color.white, AnimationDuration));
                sequence.Join(_exitButton.targetGraphic.DOColor(Color.white, AnimationDuration));
                sequence.SetEase(AnimationEase);
                
                return sequence;
            }
        }

        private void OnContinueButtonClick()
        {
            _moduleService.LoadModule(_projectProfile.LastGameplayScene).Forget();
        }

        private void OnNewGameButtonClick()
        {
            _profileService.ClearAll();
            _projectProfile.LastGameplayScene = _menuModuleConfig.NewGameScene;
            _projectProfile.Save();
            _moduleService.LoadModule(_menuModuleConfig.NewGameScene).Forget();
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}