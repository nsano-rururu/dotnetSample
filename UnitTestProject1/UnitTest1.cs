using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private string fileInputPath;
        private string fileOutputPath;

        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{    
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.fileInputPath = TestContext.TestRunResultsDirectory;
            this.fileOutputPath = TestContext.DeploymentDirectory;
        }
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        // C# からbatファイルを呼ぶにはSystem.Diagnostics.Processを使う
        // http://c4se.hatenablog.com/entry/2012/07/28/192511
        // C＃アプリケーションでVBScriptファイルを呼び出す方法？ 
        // http://www.freeshow.net.cn/ja/questions/d3d5bdb7cbb6a00b413ad71f948dd066197f476cc19faf9c01f0b5294f8ab389/
        // C# Process
        // http://www.dotnetperls.com/process
        // PowerShell C#でInvoke-commandのリモート処理の戻り値を取得する方法 
        // https://social.technet.microsoft.com/Forums/ja-JP/e9084418-626b-4b94-aeed-b9ab7686a321/powershell-cinvokecommand?forum=powershellja
        // Microsoft TechNet Windows PowerShell
        // https://social.technet.microsoft.com/Forums/ja-JP/home?forum=powershellja
        // Powershell retrieving pipeline errors in C#
        // https://social.msdn.microsoft.com/Forums/exchange/en-US/b2bece71-72d7-4305-ad81-02139959e643/powershell-retrieving-pipeline-errors-in-c
        //【Windows PowerShell】スクリプトの途中でスクリプトを強制終了する
        // http://munibus.hatenablog.com/entry/2014/01/22/053159
        // PowerShell/Windows7にPowerShell4.0をインストールする手順
        // http://win.just4fun.biz/PowerShell/Windows7%E3%81%ABPowerShell4.0%E3%82%92%E3%82%A4%E3%83%B3%E3%82%B9%E3%83%88%E3%83%BC%E3%83%AB%E3%81%99%E3%82%8B%E6%89%8B%E9%A0%86.html
        // コマンドレットの作成方法
        // http://csharper.blog57.fc2.com/blog-entry-55.html
        // PowerShell を C# から実行する
        // http://tech.tanaka733.net/entry/2013/12/10/powershell-from-csharp
        //【C#】C# から PowerShell を使うには
        // http://blogs.yahoo.co.jp/dk521123/archive/2013/11/25 


        #region sample.batのテスト

        [TestMethod]
        public void samplebat_正常系_引数に指定したファイルが存在した時に戻り値が0であること()
        {
            // Inフォルダに引数2に指定するファイルを作成
            // 例)
            // C:\Users\\sano\documents\visual studio 2013\Projects\UnitTestProject1\TestResults
            // \Deploy_sano 2015-01-05 00_54_37\In\SANO-PC
            var directoryName = Path.Combine(this.fileInputPath, "01");
            var fileName = Path.Combine(directoryName, "test.txt");
            Directory.CreateDirectory(directoryName);
            File.Create(fileName);

            var startInfo = new ProcessStartInfo();
            
            // 起動する実行ファイルのパス
            startInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);

            // 作業フォルダの設定
            startInfo.WorkingDirectory = Path.Combine(scriptPath, "script");
            
            // コマンドライン引数を指定
            startInfo.Arguments = string.Format(@"/c {0} {1}", "sample.bat" , @"""" + fileName + @"""");
            
            // プロセス用の新しいウィンドウを作成せずにプロセスを起動する場合は true、
            // それ以外の場合は false。 既定値は、false です。 
            startInfo.CreateNoWindow = true;
            
            // プロセスを起動するときにシェルを使用する場合は true、
            // プロセスを実行可能ファイルから直接作成する場合は false。 既定値は、true です。 
            startInfo.UseShellExecute = false;
            
            // 戻り値が「0」の場合、テスト成功
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var expectedCode = 0;
                Assert.AreEqual(expectedCode, process.ExitCode);
            }
        }

        [TestMethod]
        public void samplebat_正常系_引数無しで実行した時に戻り値が1であること()
        {
            var startInfo = new ProcessStartInfo();
            
            // 起動する実行ファイルのパス
            startInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);
            
            // 作業フォルダの設定
            startInfo.WorkingDirectory = Path.Combine(scriptPath, "script");
            
            // コマンドライン引数を指定
            startInfo.Arguments = string.Format(@"/c {0}", "sample.bat");
            
            // プロセス用の新しいウィンドウを作成せずにプロセスを起動する場合は true、
            // それ以外の場合は false。 既定値は、false です。 
            startInfo.CreateNoWindow = true;
            
            // プロセスを起動するときにシェルを使用する場合は true、
            // プロセスを実行可能ファイルから直接作成する場合は false。 既定値は、true です。 
            startInfo.UseShellExecute = false;           
            
            // 戻り値が「1」の場合、テスト成功
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var expectedCode = 1;
                Assert.AreEqual(expectedCode, process.ExitCode);
            }
        }

        [TestMethod]
        public void samplebat_正常系_引数で指定したファイルが存在しない時に戻り値が2であること()
        {
            var startInfo = new ProcessStartInfo();
            
            // 起動する実行ファイルのパス
            startInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);
            
            // 作業フォルダの設定
            startInfo.WorkingDirectory = Path.Combine(scriptPath, "script");
            
            // コマンドライン引数を指定
            startInfo.Arguments = string.Format(@"/c {0} {1}", "sample.bat", "C:\\test.txt");
            
            // プロセス用の新しいウィンドウを作成せずにプロセスを起動する場合は true、
            // それ以外の場合は false。 既定値は、false です。 
            startInfo.CreateNoWindow = true;
            
            // プロセスを起動するときにシェルを使用する場合は true、
            // プロセスを実行可能ファイルから直接作成する場合は false。 既定値は、true です。 
            startInfo.UseShellExecute = false;
            
            // 戻り値が「2」の場合、テスト成功
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var expectedCode = 2;
                Assert.AreEqual(expectedCode, process.ExitCode);
            }
        }

        #endregion


        #region sample.vbsのテスト

        [TestMethod]
        public void samplevbs_正常系_引数に指定したファイルが存在した時に戻り値が0であること()
        {
            // Inフォルダに引数に指定するファイルを作成
            // 例)
            // C:\Users\\sano\documents\visual studio 2013\Projects\UnitTestProject1\TestResults
            // \Deploy_sano 2015-01-05 00_54_37\In\SANO-PC
            var directoryName = Path.Combine(this.fileInputPath, "10");
            var fileName = Path.Combine(directoryName, "test.txt");
            Directory.CreateDirectory(directoryName);
            File.Create(fileName);

            var startInfo = new ProcessStartInfo();
            
            // 起動する実行ファイルのパス
            startInfo.FileName = @"cscript";
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);
            
            // 作業フォルダの設定
            startInfo.WorkingDirectory = Path.Combine(scriptPath, "script");
            
            // コマンドライン引数を指定
            startInfo.Arguments = string.Format("//B //Nologo {0} {1}", "sample.vbs", @"""" + fileName + @"""");
            
            // プロセスの起動時のウィンドウを、最大化、最小化、通常 (最大化も最小化もしていない状態)、
            // または非表示のどの状態にするかを示す列挙値のいずれか。 既定値は、Normal です。 
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            // 戻り値が「0」の場合、テスト成功
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var expectedCode = 0;
                Assert.AreEqual(expectedCode, process.ExitCode);
            }
        }

        [TestMethod]
        public void samplevbs_正常系_引数無しで実行した時に戻り値が1であること()
        {
            var startInfo = new ProcessStartInfo();
            
            // 起動する実行ファイルのパス
            startInfo.FileName = @"cscript";
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);
            
            // 作業フォルダの設定
            startInfo.WorkingDirectory = Path.Combine(scriptPath, "script");
            
            // コマンドライン引数を指定
            startInfo.Arguments = string.Format("//B //Nologo {0}", "sample.vbs");
            
            // プロセスの起動時のウィンドウを、最大化、最小化、通常 (最大化も最小化もしていない状態)、
            // または非表示のどの状態にするかを示す列挙値のいずれか。 既定値は、Normal です。 
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            // 戻り値が「1」の場合、テスト成功
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var expectedCode = 1;
                Assert.AreEqual(expectedCode, process.ExitCode);
            }
        }

        [TestMethod]
        public void samplevbs_正常系_引数1つ以外で実行した時に戻り値が2であること()
        {
            var startInfo = new ProcessStartInfo();
            
            // 起動する実行ファイルのパス
            startInfo.FileName = @"cscript";
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);
            
            // 作業フォルダの設定
            startInfo.WorkingDirectory = Path.Combine(scriptPath, "script");
            
            // コマンドライン引数を指定
            startInfo.Arguments = string.Format("//B //Nologo {0} {1}", "sample.vbs", "1 2");
            
            // プロセスの起動時のウィンドウを、最大化、最小化、通常 (最大化も最小化もしていない状態)、
            // または非表示のどの状態にするかを示す列挙値のいずれか。 既定値は、Normal です。 
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            // 戻り値が「2」の場合、テスト成功
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var expectedCode = 2;
                Assert.AreEqual(expectedCode, process.ExitCode);
            }
        }

        [TestMethod]
        public void samplevbs_正常系_引数に指定したファイルが存在しない時に戻り値が3であること()
        {
            var startInfo = new ProcessStartInfo();
            
            // 起動する実行ファイルのパス
            startInfo.FileName = @"cscript";
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);
            
            // 作業フォルダの設定
            startInfo.WorkingDirectory = Path.Combine(scriptPath, "script");
            
            // コマンドライン引数を指定
            startInfo.Arguments = string.Format("//B //Nologo {0} {1}", "sample.vbs", "C:\\test.txt");
            
            // プロセスの起動時のウィンドウを、最大化、最小化、通常 (最大化も最小化もしていない状態)、
            // または非表示のどの状態にするかを示す列挙値のいずれか。 既定値は、Normal です。 
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            // 戻り値が「3」の場合、テスト成功
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var expectedCode = 3;
                Assert.AreEqual(expectedCode, process.ExitCode);
            }
        }

        #endregion

        #region sample.ps1のテスト

        [TestMethod]
        public void sampleps1_正常系_引数nameとfilePathの値が設定されていて引数filePathに指定したファイルが存在する時に戻り値が0であること()
        {
            // Inフォルダに引数に指定するファイルを作成
            // 例)
            // C:\Users\\sano\documents\visual studio 2013\Projects\UnitTestProject1\TestResults
            // \Deploy_sano 2015-01-05 00_54_37\In\SANO-PC
            var directoryName = Path.Combine(this.fileInputPath, "20");
            var fileName = Path.Combine(directoryName, "test.txt");
            Directory.CreateDirectory(directoryName);
            File.Create(fileName);

            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);

            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    var psCmd = new PSCommand();
                    psCmd.AddScript(
                        string.Format(@"& {0} {1}",
                            @"""" + Path.Combine(scriptPath, "script", "sample.ps1") + @"""",
                            "-name 'テスト太郎' -filePath " + @"""" + fileName + @""""));
                    ps.Commands = psCmd;
                    ps.Runspace = rs;
                    var psoList = ps.Invoke();
            
                    // PowerShell内でエラーが発生した場合はテスト失敗
                    var errors = ps.Streams.Error;
                    if (errors.Count > 0)
                    {
                        Assert.Fail();
                    }
                    
                    // 戻り値が「0」の場合はテスト成功
                    var acual = "";
                    foreach (var pso in psoList)
                    {
                        if (pso != null)
                        {
                            acual = pso.ToString();
                        }
                    }
                    var expectedCode = "0";
                    Assert.AreEqual(expectedCode, acual);
                }
            }
        }

        [TestMethod]
        public void sampleps1_正常系_引数nameの長さ0で実行した時に戻り値が1であること()
        {
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);

            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    var psCmd = new PSCommand();
                    psCmd.AddScript(
                        string.Format(@"& {0} {1}", 
                            @"""" + Path.Combine(scriptPath, "script", "sample.ps1") + @"""",
                            "-name '' -filePath ''"));
                    ps.Commands = psCmd;
                    ps.Runspace = rs;
                    var psoList = ps.Invoke();
                    
                    // PowerShell内でエラーが発生した場合はテスト失敗
                    var errors = ps.Streams.Error;
                    if (errors.Count > 0)
                    {
                        Assert.Fail();
                    }
                    
                    // 戻り値が「1」の場合はテスト成功
                    var acual = "";
                    foreach (var pso in psoList)
                    {
                        if (pso != null)
                        {
                            acual = pso.ToString();
                        }
                    }                    
                    var expectedCode = "1";
                    Assert.AreEqual(expectedCode, acual);
                }
            } 
        }

        [TestMethod]
        public void sampleps1_正常系_引数nameの値が設定されていて引数filePathの長さ0で実行した時に戻り値が2であること()
        {
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);

            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    var psCmd = new PSCommand();
                    psCmd.AddScript(
                        string.Format(@"& {0} {1}",
                            @"""" + Path.Combine(scriptPath, "script", "sample.ps1") + @"""",
                            "-name 'テスト太郎' -filePath ''"));
                    ps.Commands = psCmd;
                    ps.Runspace = rs;
                    var psoList = ps.Invoke();
                    
                    // PowerShell内でエラーが発生した場合はテスト失敗
                    var errors = ps.Streams.Error;
                    if (errors.Count > 0)
                    {
                        Assert.Fail();
                    }
                    
                    // 戻り値が「2」の場合はテスト成功
                    var acual = "";
                    foreach (var pso in psoList)
                    {
                        if (pso != null)
                        {
                            acual = pso.ToString();
                        }
                    }
                    var expectedCode = "2";
                    Assert.AreEqual(expectedCode, acual);
                }
            }
        }

        [TestMethod]
        public void sampleps1_正常系_引数nameの値が設定されていて引数filePathに指定したファイルが存在しない時に戻り値が3であること()
        {
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin\Debug
            var scriptPath = Directory.GetCurrentDirectory();
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1\bin
            scriptPath = Path.GetDirectoryName(scriptPath);
            // 例)
            // C:\Users\sano\Documents\Visual Studio 2013\Projects\UnitTestProject1\UnitTestProject1
            scriptPath = Path.GetDirectoryName(scriptPath);

            using (var rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (var ps = PowerShell.Create())
                {
                    var psCmd = new PSCommand();
                    psCmd.AddScript(
                        string.Format(@"& {0} {1}",
                            @"""" + Path.Combine(scriptPath, "script", "sample.ps1") + @"""",
                            "-name 'テスト太郎' -filePath 'C:\\test.txt'"));
                    ps.Commands = psCmd;
                    ps.Runspace = rs;
                    var psoList = ps.Invoke();
                    
                    // PowerShell内でエラーが発生した場合はテスト失敗
                    var errors = ps.Streams.Error;
                    if (errors.Count > 0)
                    {
                        Assert.Fail();
                    }
                    
                    // 戻り値が「3」の場合はテスト成功
                    var acual = "";
                    foreach (var pso in psoList)
                    {
                        if (pso != null)
                        {
                            acual = pso.ToString();
                        }
                    }
                    var expectedCode = "3";
                    Assert.AreEqual(expectedCode, acual);
                }
            }
        }

        #endregion
    }
}
