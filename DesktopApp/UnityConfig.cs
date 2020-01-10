using DesktopApp.Common.Attribute;
using DesktopApp.Service;
using System;
using System.Reflection;
using Unity;

namespace DesktopApp
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> LazyContainer =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => LazyContainer.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // container.RegisterType<IProductRepository, ProductRepository>();
            foreach (var serviceType in Assembly.GetAssembly(typeof(BaseService)).GetTypes())
            {
                if (serviceType.GetCustomAttribute<UnityServiceAttribute>() is UnityServiceAttribute serviceAttribute)
                {
                    container.RegisterType(serviceAttribute.Interface, serviceType);
                }
            }
        }
    }
}