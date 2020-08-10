using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SSSA.Core.Api.Communication.Errors;
using SSSA.Core.Api.Communication.Mediator;
using SSSA.Etl.Api.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SSSA.App.Worker.Workers
{
    internal class DirectoryWatcherWorker : BackgroundService
    {
        private const string WorkerStartedMessage = "{@workerName} started.";
        private const string WorkerStoppedMessage = "{@workerName} stopped.";
        private const string ListeningMessage = "{workerName} listening changes made to directory {directory} running at: {time}.";

        private readonly ILogger<DirectoryWatcherWorker> _logger;
        private readonly IStringLocalizer<DirectoryWatcherWorker> _localizer;
        private readonly DataAppSettings _dataSettings;
        private readonly DirectoryWatcherWorkerHelper _workerHelper;
        private FileSystemWatcher _fileSystemWatcher;

        public DirectoryWatcherWorker(
            ILogger<DirectoryWatcherWorker> logger,
            IStringLocalizer<DirectoryWatcherWorker> localizer,
            IMediatorHandler mediator,
            IOptions<DataAppSettings> dataSettings,
            INotificationHandler<ErrorNotification> errorHandler)
        {
            _logger = logger;
            _localizer = localizer;
            _dataSettings = dataSettings.Value;
            _workerHelper = new DirectoryWatcherWorkerHelper(_logger, _localizer, mediator, _dataSettings, (ErrorHandler)errorHandler);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[WorkerStartedMessage], nameof(DirectoryWatcherWorker));
            _ = Directory.CreateDirectory(_dataSettings.Source);
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = _dataSettings.Source,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size,
            };

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(_localizer[WorkerStoppedMessage], nameof(DirectoryWatcherWorker));
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _fileSystemWatcher.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _workerHelper.ResolveExistingFiles();
            SubscribeToFileCreation();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(_localizer[ListeningMessage], nameof(DirectoryWatcherWorker), _dataSettings.Source, DateTimeOffset.Now);
                await Task.Delay(3000, stoppingToken);
            }
        }

        private void SubscribeToFileCreation()
        {
            _fileSystemWatcher.Created += new FileSystemEventHandler(_workerHelper.OnFileCreated);
            _fileSystemWatcher.EnableRaisingEvents = true;
        }
    }
}
