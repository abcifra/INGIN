using System;
using System.IO;
using System.Linq;
using Installer;
using WixSharp;
using WixSharp.CommonTasks;
using WixSharp.Controls;
using Assembly = System.Reflection.Assembly;

const string outputName = "INGIN";
const string projectName = "INGIN";

var project = new Project
{
    OutDir = "output",
    Name = projectName,
    Platform = Platform.x64,
    UI = WUI.WixUI_FeatureTree,
    MajorUpgrade = MajorUpgrade.Default,
    Language = "ru-ru",
    //GUID = new Guid("5B08327F-93EB-4640-96BB-934F820EDB2A"),
    GUID = new Guid("7eba78d2-c69d-412e-9e26-ba042b7380f4"),
    BannerImage = @"install\Resources\Icons\BannerImage.png",
    BackgroundImage = @"install\Resources\Icons\BackgroundImage.png",
    Version = Assembly.GetExecutingAssembly().GetName().Version.ClearRevision(),
    //Version = Version.Parse(Common.VersionInfo.Version),
    ControlPanelInfo =
    {
        Manufacturer = "INGIN",
        ProductIcon = @"install\Resources\Icons\ShellIcon.ico"
    }
};


var wixEntities = Generator.GenerateWixEntities(args);
project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.CustomizeDlg);

BuildSingleUserMsi();
//BuildMultiUserUserMsi();

//void BuildSingleUserMsi()
//{
//    string[] updaterFiles =
//    {
//        "InginUpdater.exe",
//        "InginUpdater.deps.json",
//        "InginUpdater.dll",
//        "InginUpdater.pdb",
//        "InginUpdater.runtimeconfig.json",
//        "Octokit.dll"
//    };

//    string updaterBasePath = @"..\InginUpdater\bin\Release\net8.0\";
//    string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ingin");

//    project.InstallScope = InstallScope.perUser;
//    project.OutFileName = $"{outputName}-{project.Version}-SingleUser";

//    project.Dirs = new[]
//    {
//        new InstallDir(@"%AppDataFolder%\Autodesk\Revit\Addins\", wixEntities),
//        new Dir(targetPath, updaterFiles.Select(file => new WixSharp.File(Path.Combine(updaterBasePath, file))).ToArray())
//    };

//    project.BuildMsi();
//}

void BuildSingleUserMsi()
{
    string[] updaterFiles =
    {
        "InginUpdater.exe",
        "InginUpdater.deps.json",
        "InginUpdater.dll",
        "InginUpdater.pdb",
        "InginUpdater.runtimeconfig.json",
        "Octokit.dll"
    };

    string updaterBasePath = @"..\InginUpdater\bin\Release\net8.0\";

    // Преобразуем файлы Updater в WixSharp.File[]
    var updaterFileObjects = updaterFiles
        .Select(file => new WixSharp.File(Path.Combine(updaterBasePath, file)))
        .ToArray();

    project.InstallScope = InstallScope.perUser;
    project.OutFileName = $"{outputName}-{project.Version}-SingleUser";

    // Добавим файлы обновления в подпапку "Updater" внутри папки Addins
    project.Dirs = new[]
    {
        new InstallDir(@"%AppDataFolder%\Autodesk\Revit\Addins\",
            wixEntities.Concat(new[]
            {
                new Dir("InginUpdater", updaterFileObjects)
            }).ToArray()
        )
    };

    project.BuildMsi();
}


//void BuildMultiUserUserMsi()
//{
//    project.InstallScope = InstallScope.perMachine;
//    project.OutFileName = $"{outputName}-{project.Version}-MultiUser";
//    project.Dirs =
//    [
//        new InstallDir(@"%CommonAppDataFolder%\Autodesk\Revit\Addins\", wixEntities)
//    ];
//    project.BuildMsi();
//}

void BuildMultiUserUserMsi()
{
    string[] updaterFiles =
{
        "InginUpdater.exe",
        "InginUpdater.deps.json",
        "InginUpdater.dll",
        "InginUpdater.pdb",
        "InginUpdater.runtimeconfig.json",
        "Octokit.dll"
    };
    string updaterBasePath = @"..\InginUpdater\bin\Release\net8.0\";
    string targetPath = @"C:\ProgramData\Ingin";

    project.InstallScope = InstallScope.perMachine;
    project.OutFileName = $"{outputName}-{project.Version}-MultiUser";
    project.Dirs =
    [
        new InstallDir(@"%CommonAppDataFolder%\Autodesk\Revit\Addins\", wixEntities),
        new Dir(targetPath, updaterFiles.Select(file => new WixSharp.File(Path.Combine(updaterBasePath, file))).ToArray())
    ];
    project.BuildMsi();
}