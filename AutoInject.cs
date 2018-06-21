using System;

namespace Automa.Entities.Unity.Autofac
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class AutoInject : Attribute
    {
        public object Key { get; set; }

        public AutoInject()
        {
        }

        public AutoInject(object key)
        {
            Key = key;
        }
    }
}