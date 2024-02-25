using MessageTrack.BLL.Interfaces;
using MessageTrack.BLL.Services;
using MessageTrack.PL.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using MessageTrack.DAL.Interfaces.UnitOfWork;
using MessageTrack.DAL.Repositories.UnitOfWork;
using AutoMapper;
using MessageTrack.BLL.Infrastructure.Profiles;
using MessageTrack.PL.Pages;

namespace MessageTrack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Configure the DI container
            var serviceProvider = new ServiceCollection()
                .AddScoped<IBaseService, BaseService>()
                .AddScoped<IOutboxMessageService, OutboxMessageService>()
                .AddScoped<IExternalRecipientService, ExternalRecipientService>()
                .AddScoped<IUnitOfWork>(s => new SqliteUnitOfWork("MessageTrack.db"))
                .AddSingleton(s =>
                {
                    var configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<ExternalRecipientProfile>();
                        cfg.AddProfile<OutboxMessageProfile>();
                    });

                    configuration.CompileMappings();

                    return configuration.CreateMapper();
                })
                .AddSingleton<MainWindow>()
                .AddScoped<MainPage>()
                .AddScoped<MainPageViewModel>()
                .AddTransient<DataPage>()
                .AddTransient<DataViewModel>()
                .BuildServiceProvider();

            // Resolve the MainWindow and set it as the application's main window

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }

}
