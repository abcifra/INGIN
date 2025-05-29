using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace INGIN.Sevices
{
    class SoftwareUpdateService
    {
        static async Task Main()
        {
            Console.WriteLine("Тестовый пуск апдейтера");
            /*
            // Путь к папке с плагином и версионным файлом
            string pluginPath = @"C:\RevitPlugins";
            string versionFile = Path.Combine(pluginPath, "plugin_version.txt");

            // Название GitHub репозитория в формате "owner/repo"
            string repo = "your-org/revit-plugin";

            // URL для получения информации о последнем релизе
            string latestReleaseUrl = $"https://api.github.com/repos/{repo}/releases/latest";

            // Временный путь для скачанного архива
            string zipPath = Path.Combine(pluginPath, "plugin_update.zip");

            // Чтение локальной версии плагина из файла
            string localVersion = File.Exists(versionFile) ? File.ReadAllText(versionFile).Trim() : "";

            // Создаём HTTP клиент и добавляем обязательный заголовок User-Agent
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("RevitPluginUpdater");

            try
            {
                // Получаем JSON с последней версией релиза
                var response = await client.GetAsync(latestReleaseUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);

                // Получаем номер последней версии (тег)
                string latestVersion = doc.RootElement.GetProperty("tag_name").GetString();

                // Сравниваем с локальной версией
                if (localVersion == latestVersion)
                {
                    Console.WriteLine("Установлена актуальная версия плагина.");
                    return;
                }

                Console.WriteLine($"Доступна новая версия: {latestVersion}");

                // Ищем zip-архив среди файлов релиза
                string assetUrl = null;
                foreach (var asset in doc.RootElement.GetProperty("assets").EnumerateArray())
                {
                    if (asset.GetProperty("name").GetString().EndsWith(".zip"))
                    {
                        assetUrl = asset.GetProperty("browser_download_url").GetString();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(assetUrl))
                {
                    Console.WriteLine("Не удалось найти zip-файл в релизе.");
                    return;
                }

                Console.WriteLine($"Скачивание архива: {assetUrl}");

                // Скачиваем zip-файл
                var zipBytes = await client.GetByteArrayAsync(assetUrl);
                File.WriteAllBytes(zipPath, zipBytes);

                // Распаковываем архив с перезаписью файлов
                Console.WriteLine("Распаковка архива...");
                ZipFile.ExtractToDirectory(zipPath, pluginPath);

                // Удаляем временный архив
                File.Delete(zipPath);

                // Обновляем файл с версией
                File.WriteAllText(versionFile, latestVersion);

                Console.WriteLine($"Обновление завершено. Установлена версия: {latestVersion}");
            }
            catch (Exception ex)
            {
                // Логируем ошибки (в консоль, но можно в файл)
                Console.WriteLine("Ошибка при обновлении плагина:");
                Console.WriteLine(ex.Message);
            }
            */
        }
    }
}
