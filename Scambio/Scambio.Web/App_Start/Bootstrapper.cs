using System;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Scambio.DataAccess.EntityFramework;
using Scambio.DataAccess.Infrastructure;
using Scambio.Web.Identity;

namespace Scambio.Web
{
    class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<ScambioDbFactory>().As<IDbFactory<ScambioContext>>().InstancePerRequest();
            builder.RegisterType<UserStore>().As<IUserStore<IdentityUser, Guid>>().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

