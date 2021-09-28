using Microsoft.Extensions.Logging;
using Ragnarok.AgentApi.Exceptions;
using Ragnarok.AgentApi.Helpers;
using Ragnarok.AgentApi.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ragnarok.AgentApi.Services
{
    internal sealed class NgrokProcessManager : IDisposable
    {
        private readonly NgrokConfiguration _configs;
        private readonly ILogger _logger;

        public Process Process;
        public bool IsManaged { get; private set; }
        public bool IsActive { get; private set; } 

        private NgrokProcessManager() { }

        public NgrokProcessManager(NgrokConfiguration configs, ILogger logger)
        {
            _configs = configs;
            _logger = logger;
        }

        public async Task StartProcessAsync(ProcessStartInfo startInfo)
        {          
            Process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

            var showConsole = _configs.ConsoleUI == ConsoleOptions.True;
            if (!showConsole) AttachProcessListeners();

            Process.Exited += ProcessExited;
            Process.Start();

            if (!showConsole)
            {
                Process.BeginOutputReadLine();
                Process.BeginErrorReadLine();
            }

            await Task.Delay(250);
            if (Process.HasExited) throw new NgrokProcessException();

            IsManaged = true;
            IsActive = true;

            _logger?.LogInformation("{App} successfully initialized. Spawned a new ngrok instance [Process Id: {ID}]", nameof(RagnarokClient), Process.Id);
        }

        public void AttachProcess(Process process)
        {
            if (Process != null) throw new DuplicateProcessException();

            Process = process ?? throw new ArgumentNullException(nameof(process), "Cannot attach a null process");

            Process.EnableRaisingEvents = true;
            Process.Exited += ProcessExited;

            IsManaged = false;
            IsActive = true;

            _logger?.LogInformation("{App} successfully initialized. Attached an existing ngrok instance [Process ID: {ID}]", nameof(RagnarokClient), process.Id);
            _logger?.LogWarning("Events require the ngrok process to be initiated by {App}. The attached process was initiated externally", nameof(RagnarokClient));
        }

        public void StopProcess()
        {
            if (Process == null || !IsActive) return;

            if (IsManaged)
                KillProcess();
            else
                _logger?.LogWarning("The attached ngrok process was not spawned by this application. " +
                                   "To close an independent ngrok instance use the {Kill} method.", nameof(KillProcess));
        }

        public void KillProcess()
        {
            if (Process == null || !IsActive) return;

            Process.Refresh();

            if (!Process.HasExited) Process.Kill();

            IsActive = false;
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            IsActive = false;
            _logger?.LogInformation("The ngrok process has been terminated [Process Id: {ID}]", ((Process)sender).Id);
        }

        private void AttachProcessListeners()
        {
            var stdout = _configs.Log.Equals("stdout", StringComparison.OrdinalIgnoreCase);
            var stderr = _configs.Log.Equals("stderr", StringComparison.OrdinalIgnoreCase);

            if (stdout) 
                Process.OutputDataReceived += ProcessStandardOutput;
            else if (stderr) 
                Process.ErrorDataReceived += ProcessStandardError;
            else
                _logger?.LogWarning("Log configuration is set to {CONFIG_LOG}. Client events require stdout or stderr to be set.", _configs.Log);

            if (_configs.LogLevel > NgrokConfigLogLevel.Info)
                _logger?.LogWarning("Log_Level configuration is set to {CONFIG_LOGLEVEL}. Client events require a log_level of info", _configs.Log); 
        }

        private void ProcessStandardError(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data)) _logger?.Log(e.Data);
        }

        private void ProcessStandardOutput(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))_logger?.Log(e.Data);              
        }

        public void Dispose() => Process?.Dispose();
    }
}
