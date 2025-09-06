using System;
using System.IO;
using System.Text.Json;

namespace HtmlPaperManager.Models
{
    /// <summary>
    /// 应用程序配置类
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 上次打开的HTML文件路径
        /// </summary>
        public string LastOpenedFile { get; set; } = "";

        /// <summary>
        /// 窗口位置X坐标
        /// </summary>
        public int WindowX { get; set; } = 100;

        /// <summary>
        /// 窗口位置Y坐标
        /// </summary>
        public int WindowY { get; set; } = 100;

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public int WindowWidth { get; set; } = 800;

        /// <summary>
        /// 窗口高度
        /// </summary>
        public int WindowHeight { get; set; } = 600;

        private static readonly string ConfigFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HtmlPaperManager",
            "config.json");

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        public void Save()
        {
            try
            {
                var directory = Path.GetDirectoryName(ConfigFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                // 忽略配置保存错误，不影响程序正常运行
                System.Diagnostics.Debug.WriteLine($"保存配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <returns>配置对象</returns>
        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    var json = File.ReadAllText(ConfigFilePath);
                    var config = JsonSerializer.Deserialize<AppConfig>(json);
                    return config ?? new AppConfig();
                }
            }
            catch (Exception ex)
            {
                // 忽略配置加载错误，使用默认配置
                System.Diagnostics.Debug.WriteLine($"加载配置失败: {ex.Message}");
            }

            return new AppConfig();
        }
    }
}
