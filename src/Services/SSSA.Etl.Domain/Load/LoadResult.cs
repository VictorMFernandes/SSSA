namespace SSSA.Etl.Domain.Load
{
    public class LoadResult
    {
        public string Result { get; }
        public string ErrorMessage { get; private set; }
        public bool Succeeded => string.IsNullOrWhiteSpace(ErrorMessage);

        public LoadResult(string result)
        {
            Result = result;
        }

        private LoadResult()
        {
        }

        public static LoadResult WithError(string errorMessage) => new LoadResult { ErrorMessage = errorMessage };
    }
}
