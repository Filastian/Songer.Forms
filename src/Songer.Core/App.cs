using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using Songer.Core.Client;
using Songer.Core.ViewModels;
using System;

namespace Songer.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Validator")
                .AsTypes()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterSingleton<IRestClient>(new RestClient(new Uri("https://b5690f24.ngrok.io")));

            RegisterAppStart<AuthViewModel>();
        }
    }
}