using System;
using System.Collections.Generic;
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

        [Inject] protected EventSystem EventSystem;
        [Inject] protected ProfileService ProfileService;
        [Inject] protected ModuleService ModuleService;
        [Inject] protected ProjectProfile ProjectProfile;
        [Inject] protected ProjectConfig ProjectConfig;

        private readonly List<Button> _listButtons = new();

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
            InitContinueButton(ProjectProfile.FirstStartDate != default);
        }

        protected void InitContinueButton(bool isInteractable)
        {
            _continueButton.interactable = isInteractable;
            EventSystem.SetSelectedGameObject(_continueButton.interactable ? _continueButton.gameObject : _newGameButton.gameObject);
        }

        protected virtual void SubscribeButtons()
        {
            _continueButton.OnClickAsObservable().Subscribe(_ => OnContinueButtonClick()).AddTo(this);
            _newGameButton.OnClickAsObservable().Subscribe(_ => OnNewGameButtonClick()).AddTo(this);
            _exitButton.OnClickAsObservable().Subscribe(_ => OnExitButtonClick()).AddTo(this);
        }

        private async UniTask ShowButtons()
        {
            var showSequence = createSequence();

            Disposable.Create(() => showSequence.Kill()).AddTo(this);

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
            ProfileService.SetNewGame(false);
            return ModuleService.LoadModule(ProjectConfig.GameplayScene);
        }

        protected virtual UniTask OnNewGameButtonClick()
        {
            ProfileService.ClearAll();
            ProfileService.SetNewGame(true);
            return ModuleService.LoadModule(ProjectConfig.GameplayScene);
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}