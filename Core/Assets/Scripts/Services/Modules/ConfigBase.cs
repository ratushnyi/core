using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TendedTarsier.Core.Services.Modules
{
    public class ConfigBase : ScriptableObject
    {
        public virtual IEnumerable InjectItems()
        {
            return new List<object>();
        }
    }
}