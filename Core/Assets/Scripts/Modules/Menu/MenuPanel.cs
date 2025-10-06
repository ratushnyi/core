using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TendedTarsier.Core.Modules.Loading;
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
        private ProjectConfig _projectConfig;

        private readonly List<Button> _listButtons = new();

        [Inject]
        private void Construct(
            ProjectConfig projectConfig,
            ProjectProfile projectProfile,
            ProfileService profileService,
            ModuleService moduleService,
            EventSystem eventSystem)
        {
            _projectConfig = projectConfig;
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

        protected void RegisterButton(Button button)
        {
            button.targetGraphic.color = Color.clear;
            _listButtons.Add(button);
        }

        protected virtual void InitButtons()
        {
            RegisterButton(_continueButton);
            RegisterButton(_newGameButton);
            RegisterButton(_exitButton);
            InitContinueButton(_projectProfile.FirstStartDate != default);
        }

        protected void InitContinueButton(bool isInteractable)
        {
            _continueButton.interactable = isInteractable;
            _eventSystem.SetSelectedGameObject(_continueButton.interactable ? _continueButton.gameObject : _newGameButton.gameObject);
        }

        protected virtual void SubscribeButtons()
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

                foreach (var button in _listButtons)
                {
                    sequence.Join(button.targetGraphic.DOColor(Color.white, AnimationDuration));
                }
                sequence.SetEase(AnimationEase);
                sequence.SetUpdate(true);
                
                return sequence;
            }
        }

        protected virtual UniTask OnContinueButtonClick()
        {
            _profileService.SetNewGame(false);
            return _moduleService.LoadModule(_projectConfig.GameplayScene);
        }

        protected virtual UniTask OnNewGameButtonClick()
        {
            _profileService.ClearAll();
            _profileService.SetNewGame(true);
            return _moduleService.LoadModule(_projectConfig.GameplayScene);
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}