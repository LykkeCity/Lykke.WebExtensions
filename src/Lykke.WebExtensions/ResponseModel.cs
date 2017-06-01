namespace Lykke.WebExtensions
{
    public class ResponseModel
    {
        public ErrorModel Error { get; set; }

        public class ErrorModel
        {
            /// <summary>
            /// Error code
            /// </summary>
            public int Code { get; set; }

            /// <summary>
            /// Localized error message to show to the client
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// In case when ErrorCoderType = 0 contains field name
            /// </summary>
            public string Field { get; set; }
        }

        public static ResponseModel CreateInvalidFieldError(string field, string message)
        {
            return new ResponseModel
            {
                Error = new ErrorModel
                {
                    Code = 0,
                    Field = field,
                    Message = message
                }
            };
        }

        public static ResponseModel CreateFail(int errorCode, string message)
        {
            return new ResponseModel
            {
                Error = new ErrorModel
                {
                    Code = errorCode,
                    Message = message
                }
            };
        }

        private static readonly ResponseModel OkInstance = new ResponseModel();
        public static ResponseModel CreateOk()
        {
            return OkInstance;
        }

        public static ResponseModel<TResult> CreateOk<TResult>(TResult result)
        {
            return new ResponseModel<TResult>
            {
                Result = result
            };
        }
    }

    public class ResponseModel<TResult> : ResponseModel
    {
        public TResult Result { get; set; }

        public new static ResponseModel<TResult> CreateInvalidFieldError(string field, string message)
        {
            return new ResponseModel<TResult>
            {
                Error = new ErrorModel
                {
                    Code = 0,
                    Field = field,
                    Message = message
                }
            };
        }

        public new static ResponseModel<TResult> CreateFail(int errorCode, string message)
        {
            return new ResponseModel<TResult>
            {
                Error = new ErrorModel
                {
                    Code = errorCode,
                    Message = message
                }
            };
        }
    }
}