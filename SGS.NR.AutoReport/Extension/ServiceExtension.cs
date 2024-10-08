using Microsoft.Extensions.DependencyInjection;
using SGS.NR.Repository.Implement;
using SGS.NR.Repository.Interface;
using SGS.NR.Service.Implement;
using SGS.NR.Service.Interface;

namespace SGS.NR.AutoReport.Extension
{
    /// <summary>
    /// 註冊服務擴充方法
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// 註冊 Service
        /// </summary>
        /// <param name="services">服務集合</param>
        /// <returns>服務集合</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IContainerLoadingService, ContainerLoadingService>();
            return services;
        }

        /// <summary>
        /// 註冊 Repository
        /// </summary>
        /// <param name="services">服務集合</param>
        /// <returns>服務集合</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IContainerLoadingRepository, ContainerLoadingRepository>();
            services.AddSingleton<IVesselLoadingRepository, VesselLoadingRepository>();
            return services;
        }

        /// <summary>
        /// 註冊其他服務
        /// </summary>
        /// <param name="services">服務集合</param>
        /// <returns>服務集合</returns>
        public static IServiceCollection AddMiscs(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        /// <summary>
        /// 取得或建立服務
        /// </summary>
        /// <typeparam name="T">服務類型</typeparam>
        /// <param name="serviceProvider">服務提供者</param>
        /// <returns>服務實例</returns>
        public static T GetOrCreateService<T>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<T>() ?? ActivatorUtilities.CreateInstance<T>(serviceProvider);
        }
    }
}
