using Microsoft.AspNetCore.Components;

namespace Sindvamb.Web.Shared.Utils
{
    public abstract class AppsServiceBase<TOption>
    {
        protected List<(ComponentBase Key, Func<TOption, Task> Callback)> Cache { get; } = new();


        protected async Task Invoke<T>(TOption option, ComponentBase? component = null)
            where T : IComponent
        {
            var (Key, Callback) = component != null
                ? Cache.FirstOrDefault(k => k.Key == component)
                : Cache.FirstOrDefault();
            if (Callback == null)
            {
                //DynamicComponent.CreateComponent<T>().Render();
                //throw new InvalidOperationException($"{GetType().Name} not registerd. refer doc https://www.blazor.zone/install-server step 7 for BootstrapBlazorRoot");
                return;
            }

            await Callback.Invoke(option);
        }


        internal void Register(ComponentBase key, Func<TOption, Task> callback)
        {
            Cache.Add((key, callback));
        }

        internal void UnRegister(ComponentBase key)
        {
            var item = Cache.FirstOrDefault(i => i.Key == key);
            if (item.Key != null)
            {
                Cache.Remove(item);
            }
        }
    }
}
