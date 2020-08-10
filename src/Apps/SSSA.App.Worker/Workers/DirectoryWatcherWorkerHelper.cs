using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SSSA.Core.Api.Communication.Commands;
using SSSA.Core.Api.Communication.Errors;
using SSSA.Core.Api.Communication.Mediator;
using SSSA.Etl.Api.Commands;
using SSSA.Etl.Api.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SSSA.App.Worker.Workers
{
    internal class DirectoryWatcherWorkerHelper
    {
        private const string AcceptedExtensions = ".txt";
        private const string TempFileStart = "~$";
        private const string RetroactiveReportsMessage = " There are already files in the source directory, do you wish to generate new reports based on them? (y/n)";
        private const string AffirmativeAnswer = "y";
        private const string GenericCommandErrorMessage = "Something went wrong when trying to execute the command {@command}. Notifications: {@notifications}";

        private readonly ILogger<DirectoryWatcherWorker> _logger;
        private readonly IStringLocalizer<DirectoryWatcherWorker> _localizer;
        private readonly IMediatorHandler _mediator;
        private readonly DataAppSettings _dataSettings;
        private readonly ErrorHandler _errorHandler;

        public DirectoryWatcherWorkerHelper(
            ILogger<DirectoryWatcherWorker> logger,
            IStringLocalizer<DirectoryWatcherWorker> localizer,
            IMediatorHandler mediator,
            DataAppSettings dataSettings,
            ErrorHandler errorHandler)
        {
            _logger = logger;
            _localizer = localizer;
            _mediator = mediator;
            _dataSettings = dataSettings;
            _errorHandler = errorHandler;
        }

        public async Task ResolveExistingFiles()
        {
            if (Directory.GetFiles(_dataSettings.Source, $"*{AcceptedExtensions}").Length == 0)
            {
                return;
            }

            Console.WriteLine(_localizer[RetroactiveReportsMessage]);
            var result = Console.ReadLine();
            if (result.Trim().ToLower() == _localizer[AffirmativeAnswer])
            {
                var filePaths = Directory.GetFiles(_dataSettings.Source, $"*{AcceptedExtensions}", SearchOption.TopDirectoryOnly);
                await SendCreateSalesReportCommand(filePaths);
            }
        }

        public async void OnFileCreated(object source, FileSystemEventArgs e) =>
            await SendCreateSalesReportCommand(e.FullPath);

        private async Task SendCreateSalesReportCommand(params string[] fullFilePaths)
        {
            // Ignoring temp files
            var filePaths = fullFilePaths.Where(x => !x.StartsWith(TempFileStart));

            // Ignoring files with invalid extension
            filePaths = fullFilePaths.Where(x => !string.IsNullOrWhiteSpace(Path.GetExtension(x)) && AcceptedExtensions.Contains(Path.GetExtension(x)));

            if (!filePaths.Any())
            {
                return;
            }

            var command = new CreateSalesReportCommand(_dataSettings.Destination, filePaths);

            try
            {
                var commandSucceded = await _mediator.SendCommandAsync(command);

                if (!commandSucceded)
                {
                    HandleError(command);
                }
            }
            catch (Exception ex)
            {
                HandleError(command, ex);
            }
        }

        private void HandleError<T>(CommandBase<T> command, Exception ex = null)
        {
            _logger.LogError(ex, _localizer[GenericCommandErrorMessage], command, _errorHandler.GetNotifications().Select(x => x.Value));
        }
    }
}
