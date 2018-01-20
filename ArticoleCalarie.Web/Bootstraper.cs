using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ArticoleCalarie.Repository.IRepository;
using ArticoleCalarie.Repository.Repository;
using ArticoleCalarie.Infrastructure.MailService;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Logic.Logic;
using ArticoleCalarie.Repository;
using ArticoleCalarie.Repository.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using Unity.Lifetime;

namespace ArticoleCalarie.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(UnityContainer container)
        {
            //Identity
            container.RegisterType<DbContext, ArticoleCalarieDataContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<UserModel>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<UserModel>, UserStore<UserModel>>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => System.Web.HttpContext.Current.GetOwinContext().Authentication));

            //Logic
            container.RegisterType<IAccountLogic, AccountLogic>();
            container.RegisterType<IProductLogic, ProductLogic>();
            container.RegisterType<ICategoryLogic, CategoryLogic>();
            container.RegisterType<ISizeChartLogic, SizeChartLogic>();
            container.RegisterType<IColorLogic, ColorLogic>();

            //Repository
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<ISizeChartRepository, SizeChartRepository>();
            container.RegisterType<IColorRepository, ColorRepository>();

            //Infrastructure
            container.RegisterType<IMailService, MailService>();
        }
    }
}