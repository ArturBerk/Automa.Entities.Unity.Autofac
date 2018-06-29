using System.Linq;
using System.Reflection;
using Autofac;
using Automa.Entities.Systems;

namespace Automa.Entities.Unity.Autofac
{
    public abstract class AutofacUnityContext : UnityContext
    {
        public IContainer Container { get; private set; }
        
        protected virtual void BuildContainer(ContainerBuilder builder)
        {
        }

        protected override void Setup()
        {
            var builder = new ContainerBuilder();
            BuildContainer(builder);
            Container = builder.Build();
            BuildSystems();
        }

        protected virtual void BuildSystems()
        {
            SystemTreeBuilder builder = new SystemTreeBuilder();
            foreach (var system in GetType().Assembly
                .GetAllSystems(type => type.GetCustomAttribute<IgnoreAttribute>() == null))
            {
                Container.Inject(system);
                builder.AddSystem(system);
            }
            builder.Build(SystemManager);
        }
    }
}