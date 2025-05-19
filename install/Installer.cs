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
        Manufacturer = Environment.UserName,
        ProductIcon = @"install\Resources\Icons\ShellIcon.ico"
    }
};


var wixEntities = Generator.GenerateWixEntities(args);
project.RemoveDialogsBetween(NativeDialogs.WelcomeDlg, NativeDialogs.CustomizeDlg);

BuildSingleUserMsi();
//BuildMultiUserUserMsi();

//void BuildSingleUserMsi()
//{
//    var updaterPath = @"..\InginUpdater\bin\Release\net8.0\InginUpdater.exe";
//    string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ingin");
//    project.InstallScope = InstallScope.perUser;
//    project.OutFileName = $"{outputName}-{project.Version}-SingleUser";
//    project.Dirs = new[]
//    {
//        new InstallDir(@"%AppDataFolder%\Autodesk\Revit\Addins\", wixEntities),
//        new Dir(targetPath, new WixSharp.File(updaterPath)),
//    };
//    project.BuildMsi();
//}

void BuildSingleUserMsi()
{
    var updaterPath1 = @"..\InginUpdater\bin\Release\net8.0\InginUpdater.exe";
    var updaterPath2 = @"..\InginUpdater\bin\Release\net8.0\InginUpdater.deps.json";
    var updaterPath3 = @"..\InginUpdater\bin\Release\net8.0\InginUpdater.dll";
    var updaterPath4 = @"..\InginUpdater\bin\Release\net8.0\InginUpdater.pdb";
    var updaterPath5 = @"..\InginUpdater\bin\Release\net8.0\InginUpdater.runtimeconfig.json";
    var updaterPath6 = @"..\InginUpdater\bin\Release\net8.0\Octokit.dll";
    string targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ingin");
    project.InstallScope = InstallScope.perUser;
    project.OutFileName = $"{outputName}-{project.Version}-SingleUser";
    project.Dirs = new[]
    {
        new InstallDir(@"%AppDataFolder%\Autodesk\Revit\Addins\", wixEntities),
        new Dir(targetPath, new WixSharp.File(updaterPath1)),
        new Dir(targetPath, new WixSharp.File(updaterPath2)),
        new Dir(targetPath, new WixSharp.File(updaterPath3)),
        new Dir(targetPath, new WixSharp.File(updaterPath4)),
        new Dir(targetPath, new WixSharp.File(updaterPath5)),
        new Dir(targetPath, new WixSharp.File(updaterPath6)),
    };
    project.BuildMsi();
}


void BuildMultiUserUserMsi()
{
    project.InstallScope = InstallScope.perMachine;
    project.OutFileName = $"{outputName}-{project.Version}-MultiUser";
    project.Dirs =
    [
        new InstallDir(@"%CommonAppDataFolder%\Autodesk\Revit\Addins\", wixEntities)
    ];
    project.BuildMsi();
}
