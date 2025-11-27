using Unity.Netcode.Components;

namespace TendedTarsier.Core.Utilities.Extensions
{
    public class ClientNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative() => false;
    }
}