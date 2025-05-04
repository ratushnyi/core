using System;
using DG.Tweening;
using TendedTarsier.Core.Modules.General;
using TendedTarsier.Core.Services.Profile;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using GeneralProfile = TendedTarsier.Core.Modules.General.GeneralProfile;

namespace TendedTarsier.Core.Modules.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitButton;
        
        [Header("Animation")]
        [SerializeField] private float _fadeOutDuration = 1;
        [SerializeField] private Color _backgroundFadeOutColor = Color.white;
        [SerializeField] public Ease _backgroundFadeOutCurve = Ease.InSine;

        private GeneralConfig _generalConfig;
        private GeneralProfile _generalProfile;
        private ProfileService _profileService;
        private EventSystem _eventSystem;

        private readonly CompositeDisposable _compositeDisposable = new();

        [Inject]
        private void Construct(
            GeneralConfig generalConfig,
            GeneralProfile generalProfile,
            ProfileService profileService,
            EventSystem eventSystem)
        {
            _generalConfig = generalConfig;
            _generalProfile = generalProfile;
            _profileService = profileService;
            _eventSystem = eventSystem;
        }

        private void Start()
        {
            InitBackground();
            InitButtons();
        }

        private void InitBackground()
        {
            _background.color = Color.black;
            _continueButton.targetGraphic.color = Color.clear;
            _newGameButton.targetGraphic.color = Color.clear;
            _exitButton.targetGraphic.color = Color.clear;

            var sequence = DOTween.Sequence();
            sequence.Append(_background.DOColor(_backgroundFadeOutColor, _fadeOutDuration));
            sequence.Join(_continueButton.targetGraphic.DOColor(Color.white, _fadeOutDuration));
            sequence.Join(_newGameButton.targetGraphic.DOColor(Color.white, _fadeOutDuration));
            sequence.Join(_exitButton.targetGraphic.DOColor(Color.white, _fadeOutDuration));
            sequence.SetEase(_backgroundFadeOutCurve);

            _compositeDisposable.Add(Disposable.Create(() => sequence.Kill()));
        }

        private void InitButtons()
        {
            _continueButton.interactable = _generalProfile.FirstStartDate != null;
            _continueButton.OnClickAsObservable().Subscribe(OnContinueButtonClick).AddTo(_compositeDisposable);
            _newGameButton.OnClickAsObservable().Subscribe(OnNewGameButtonClick).AddTo(_compositeDisposable);
            _exitButton.OnClickAsObservable().Subscribe(OnExitButtonClick).AddTo(_compositeDisposable);
            _eventSystem.SetSelectedGameObject(_continueButton.interactable ? _continueButton.gameObject : _newGameButton.gameObject);
        }

        private void OnContinueButtonClick(Unit _)
        {
            SceneManager.LoadScene(_generalConfig.GameplayScene);
        }

        private void OnNewGameButtonClick(Unit _)
        {
            _profileService.ClearAll();
            _generalProfile.FirstStartDate = DateTime.UtcNow;
            _generalProfile.Save();
            SceneManager.LoadScene(_generalConfig.GameplayScene);
        }

        private void OnExitButtonClick(Unit _)
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}