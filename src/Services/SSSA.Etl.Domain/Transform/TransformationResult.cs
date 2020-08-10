using System.Collections.Generic;

namespace SSSA.Etl.Domain.Transform
{
    public class TransformationResult
    {
        public IEnumerable<object> Value { get; }
        public string ErrorMessage { get; private set; }
        public bool Succeeded => string.IsNullOrWhiteSpace(ErrorMessage);

        public TransformationResult(IEnumerable<object> value)
        {
            Value = value;
        }

        public TransformationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
