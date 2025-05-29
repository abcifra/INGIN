using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Common;

namespace InginUpdater
{
    public class InginUpdater
    {
        static void Main()
        {
            try
            {
                Version currentVersion = new Version(VersionInfo.Version);
                var client = new GitHubClient(new ProductHeaderValue("RevitPluginUpdater"));
                var releases = client.Repository.Release.GetAll("abcifra", "INGIN").Result;

                if (releases.Count == 0)
                {
                    Console.WriteLine("Нет доступных релизов.");
                    return;
                }

                var latestRelease = releases[0];

                string tag = latestRelease.TagName?.TrimStart('v', 'V') ?? "0.0.0";
                Version latestVersion = new Version(tag);

                if (latestVersion <= currentVersion)
                {
                    Console.WriteLine("Установлена последняя версия.");
                    Thread.Sleep(2000);
                    return;
                }


                if (latestRelease.Assets.Count == 0)
                {
                    Console.WriteLine("Нет файлов для скачивания.");
                    Thread.Sleep(2000);
                    return;
                }

                string downloadUrl = latestRelease.Assets[0].BrowserDownloadUrl;
                string fileName = Path.GetFileName(new Uri(downloadUrl).AbsolutePath);
                string localPath = Path.Combine(Path.GetTempPath(), fileName);

                DownloadFile(downloadUrl, localPath);
                Console.WriteLine("Обновление загружено!");
                Thread.Sleep(2000);

                InstallUpdate(localPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
        }

        static void DownloadFile(string url, string destinationPath)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, destinationPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки файла: {ex.Message}");
                Thread.Sleep(2000);
            }
        }

        static void InstallUpdate(string msiPath)
        {
            var process = new Process();
            process.StartInfo.FileName = "msiexec.exe";
            process.StartInfo.Arguments = $"/i \"{msiPath}\" /quiet /norestart";
            process.StartInfo.UseShellExecute = true;  // Разрешает использование оболочки Windows

            try
            {
                process.Start();
                process.WaitForExit();  // Ожидание завершения установки

                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Обновление завершено успешно!");
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine($"Ошибка при установке, код завершения: {process.ExitCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запуске установки: {ex.Message}");
            }
        }

    }
}


