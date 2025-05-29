using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using INGIN.Commands;
using INGIN.Helper;
using INGIN.ScheduleSpecification;
using Nice3point.Revit.Toolkit.External;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace INGIN
{
    /// <summary>
    ///     Application entry point
    /// </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateLogger();
            CreateRibbon();
        }

        public override void OnShutdown()
        {
            Log.CloseAndFlush();
            OnApplicationClosing();
        }

        private void CreateRibbon()
        {
            //var panel = Application.CreatePanel("Commands", "INGIN");

            //panel.AddPushButton<StartupCommand>("Execute")
            //    .SetImage("/INGIN;component/Resources/Icons/RibbonIcon16.png")
            //    .SetLargeImage("/INGIN;component/Resources/Icons/RibbonIcon32.png");

            string assemblyName = Assembly.GetExecutingAssembly().Location;
            string tabName = "ИНЖИН";

            Application.CreateRibbonTab(tabName);

            RibbonPanel panelHelper = Application.CreateRibbonPanel(tabName, "О программе");
            RibbonPanel panelGeneral = Application.CreateRibbonPanel(tabName, "Общее");

            PushButtonData helper_BtnData = new PushButtonData(name: "Helper", text: "О программе", assemblyName, typeof(HelperCommand).FullName)
            {
                LargeImage = new BitmapImage(new Uri("pack://application:,,,/INGIN;component/Resources/Icons/ingin_32x32.ico"))
            };
            PushButtonData scheduleSpecification_BtnData = new PushButtonData(name: "ScheduleSpecification", text: "Ведомость спецификаций", assemblyName, typeof(ScheduleSpecificationCommand).FullName)
            {
                LargeImage = new BitmapImage(new Uri("pack://application:,,,/INGIN;component/Resources/Icons/ScheduleSpecification.ico")),
                ToolTip = "Создание Ведомости спецификаций",
                LongDescription = "Плагин анализирует, вынесенные на листы спецификации, и создает на их основе «Ведомость спецификаций»."
            };
            scheduleSpecification_BtnData.SetContextualHelp(new ContextualHelp(
            ContextualHelpType.Url,
            "https://wiki.ing-in.ru/wordpress/ingin-%d0%b2%d0%b5%d0%b4%d0%be%d0%bc%d0%be%d1%81%d1%82%d1%8c-%d1%81%d0%bf%d0%b5%d1%86%d0%b8%d1%84%d0%b8%d0%ba%d0%b0%d1%86%d0%b8%d0%b9/"));


            panelHelper.AddItem(helper_BtnData);
            panelGeneral.AddItem(scheduleSpecification_BtnData);
        }

        private static void CreateLogger()
        {
            const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Debug(LogEventLevel.Debug, outputTemplate)
                .MinimumLevel.Debug()
                .CreateLogger();

            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
            {
                var exception = (Exception)args.ExceptionObject;
                Log.Fatal(exception, "Domain unhandled exception");
            };
        }

        private void OnApplicationClosing()
        {
            //Process.Start(@"%AppDataFolder%\Ingin\InginUpdater.exe");
            var a = "dcdvfkjklgfgfgcl;v,";
            string updaterPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Autodesk\Revit\Addins\InginUpdater\InginUpdater.exe");
            Process.Start(updaterPath);
        }

        //private void OnApplicationClosing()
        //{
        //    string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ingin", "InginUpdater.exe");
        //    Process.Start(appDataPath);
        //}

    }
}