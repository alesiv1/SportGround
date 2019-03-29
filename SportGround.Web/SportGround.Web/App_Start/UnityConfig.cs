using System;
using Unity;

namespace SportGround.Web
{

    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
			Data.UnityConfig.RegisterTypes(container);
			BusinessLogic.UnityConfig.RegisterTypes(container);
        }
    }
}