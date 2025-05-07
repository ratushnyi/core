using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace TendedTarsier.Core.Panels
{
    public class PanelBase : MonoBehaviour
    {
        [SerializeField]
        private bool _showInstantly;
        public virtual bool ShowInstantly => _showInstantly;

        protected readonly CompositeDisposable CompositeDisposable = new();

        public virtual UniTask InitializeAsync()
        {
            Initialize();
            return UniTask.CompletedTask;
        }

        protected virtual void Initialize()
        {
        }

        public virtual UniTask DisposeAsync()
        {
            Dispose();
            return UniTask.CompletedTask;
        }

        protected virtual void Dispose()
        {
            CompositeDisposable.Dispose();
        }

        public virtual UniTask ShowAnimation()
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public virtual UniTask HideAnimation()
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}