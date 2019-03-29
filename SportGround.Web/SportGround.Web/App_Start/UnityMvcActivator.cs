using System.Linq;
using System.Web.Mvc;
using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SportGround.Web.UnityMvcActivator), nameof(SportGround.Web.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(SportGround.Web.UnityMvcActivator), nameof(SportGround.Web.UnityMvcActivator.Shutdown))]

namespace SportGround.Web
{
    public static class UnityMvcActivator
    {
        public static void Start() 
        {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(UnityConfig.Container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityConfig.Container));
        }

        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}