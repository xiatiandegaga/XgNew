using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
namespace Cloud.EntityFrameworkCore
{
    public class AutoAddMigration
    {
        public static void RunEfMigrationCommand()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet", // 执行 dotnet 命令
                    Arguments = "ef migrations add AutoMigration ", // 参数
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false, // 必须设置为 false 以重定向输出
                    CreateNoWindow = true,   // 不创建新窗口
                    WorkingDirectory = Directory.GetCurrentDirectory() // 设置工作目录（项目根目录）
                }
            };

            try
            {
                // 启动进程
                process.Start();

                // 读取输出和错误信息
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // 等待命令执行完成
                process.WaitForExit();

                // 检查执行结果
                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Migration added successfully:");
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine($"Error executing command (Exit Code: {process.ExitCode}):");
                    Console.WriteLine(error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                process.Dispose();
            }
        }

        public static void GenerateAndSaveMigration(IServiceScopeFactory scopeFactory,string migrationName = "auto", string outputDir = "Migrations")
        {
            // 生成迁移文件
            RunEfMigrationCommand();
            // 获取当前项目目录
            var projectDir = Directory.GetCurrentDirectory();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate(); // 执行迁移


        }
    }
}
