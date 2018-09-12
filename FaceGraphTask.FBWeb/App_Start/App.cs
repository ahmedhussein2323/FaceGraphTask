using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FaceGraphTask.Core.Entities;
using FaceGraphTask.Infrastructure.Commands.Interfaces;
using FrameWork.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Utilities.FileManager;
using Utilities.RuntimeStorage;
using Utilities.RuntimeStorage.Interfaces;

namespace FaceGraphTask.FBWeb
{
    public class App
    {
        public static IContainer Container { get; set; }
        public static IDependencyResolver MvcResolver { get; set; }
        public static System.Web.Http.Dependencies.IDependencyResolver WebApiResolver { get; set; }
        public static void Init()
        {
            Container = GetContainer();
        }
        static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(e => e.FullName.Contains("FaceGraphTask") ||
                e.FullName.Contains("CQRS")).ToArray();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(ICommand).IsAssignableFrom(t))
                .InstancePerLifetimeScope().AsImplementedInterfaces();


            builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(ICommandValidator<>))
                .AsImplementedInterfaces();


            builder.RegisterAssemblyTypes(assemblies)
              .Where(t => t.BaseType == typeof(HttpApplicationStateBase))
              .AsSelf()
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
              .Where(t => t.BaseType == typeof(HttpSessionStateBase))
              .AsSelf()
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assemblies)
              .Where(t => t == typeof(FileManager))
              .AsSelf()
              .WithParameter("rootPath", "~/app_data/")
              .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                 .AsClosedTypesOf(typeof(Application<>))
                 .AsSelf()
                 .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(Session<>))
            .AsSelf()
            .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
             .AssignableTo<ICacheManager>()
             .AsImplementedInterfaces()
             .As(typeof(ICacheManager));

            builder.RegisterAssemblyTypes(assemblies)
              .Where(t => t.BaseType == typeof(BaseEntity)).InstancePerDependency()
              .AsSelf()
              .AsImplementedInterfaces();


            #region Entity Framework
            builder.RegisterAssemblyTypes(assemblies)
             .AsClosedTypesOf(typeof(Infrastructure.Commands.Interfaces.IFrameWorkCommandHandler<>))
             .AsSelf()
             .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
            .AssignableTo<ICommandExecuter>()
            .AsImplementedInterfaces()
            //.WithParameter("connectionString", "DefaultConnection")
            .As(typeof(ICommandExecuter));

            builder.RegisterAssemblyTypes(assemblies)
              .AssignableTo<ICommandService>()
               .AsSelf()
              .AsImplementedInterfaces()
              .As(typeof(ICommandService));

            var container = builder.Build();

            builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(assemblies)
              .AssignableTo<ICommandDispatcher>()
              .AsImplementedInterfaces()
              .As(typeof(ICommandDispatcher))
              .WithParameter("container", container);

            //builder.RegisterAssemblyTypes(assemblies)
            //   .AsClosedTypesOf(typeof(BaseApiController))
            //   .AsSelf()
            //   .AsImplementedInterfaces();

            //builder.RegisterAssemblyTypes(assemblies)
            //  .AsClosedTypesOf(typeof(BaseController))
            //  .AsSelf()
            //  .AsImplementedInterfaces();

            //builder.Update(container);
            #endregion

            builder.RegisterModule(new AutofacWebTypesModule());

            #region Mvc
            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            MvcResolver = new AutofacDependencyResolver(container);
            #endregion

            #region Web Api
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //WebApi resolver.
            WebApiResolver = new AutofacWebApiDependencyResolver(container);
            #endregion

            builder.Update(container);
            return container;
        }
    }

}