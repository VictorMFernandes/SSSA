using SSSA.Core.Api.Communication.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSSA.Etl.Api.Commands
{
    public class CreateSalesReportCommand : CommandBase<bool>
    {
        public IEnumerable<string> InputFilePaths { get; }
        public string OutputFilePath { get; }

        public CreateSalesReportCommand(string outputFilePath, IEnumerable<string> inputFilePath)
        {
            OutputFilePath = string.IsNullOrWhiteSpace(outputFilePath) ? throw new ArgumentNullException(nameof(outputFilePath)) : outputFilePath;
            InputFilePaths = !inputFilePath.Any() ? throw new ArgumentNullException(nameof(inputFilePath)) : inputFilePath;
        }
    }
}
