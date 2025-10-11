using System;
using System.Collections.Generic;
using TendedTarsier.Core.Services;
using TendedTarsier.Core.Services.Input;
using UniRx;
using Zenject;

namespace TendedTarsier.Core.Panels
{
    public class BackButtonService : ServiceBase, IInitializable
    {
        private class BackButtonHandler : IDisposable
        {
            private readonly Action _onPerform;
            private readonly Action _onDispose;
            
            public BackButtonHandler(Action onPerform, Action onDispose)
            {
                _onPerform = onPerform;
                _onDispose = onDispose;
            }
            
            public void Perform()
            {
                _onPerform.Invoke();
            }
            
            public void Dispose()
            {
                _onDispose.Invoke();
            }
        }
        private InputService _inputService;
        private readonly Stack<BackButtonHandler> _backButtonStack = new();

        [Inject]
        private void Construct(InputService inputService)
        {
            _inputService = inputService;
        }
        
        public void Initialize()
        {
            _inputService.OnMenuButtonStarted.Subscribe(_ => OnBackButtonClicked()).AddTo(CompositeDisposable);
        }

        public IDisposable AddAction(Action action)
        {
            var backButtonHandler = new BackButtonHandler(action, () =>
            {
                _backButtonStack.Pop();
            });
            _backButtonStack.Push(backButtonHandler);
            return backButtonHandler;
        }

        private void OnBackButtonClicked()
        {
            _backButtonStack.Peek().Perform();
        }
    }
}