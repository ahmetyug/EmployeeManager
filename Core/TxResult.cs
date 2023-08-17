using Utility;

namespace Core
{
    /// <summary>
    /// Generic object holding data regarding 3rd party api transactions
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class TxResult<TData> where TData : class
    {
        private const string DEFAULT_ERR_MESSAGE = "ERROR!";

        private readonly TxCode code;
        private readonly string? message;
        private readonly TData data;

        public TxCode Code => code;
        public string? Message => message;
        public TData Data => data;

        public bool IsSuccess => code == TxCode.Success;

        /// <summary>
        /// Private ctor. Creation is handled by factory methods.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private TxResult(TxCode code, TData data, string? message = null)
        {
            if (code == TxCode.Success)
            {
                if (!string.IsNullOrEmpty(message))
                    throw new InvalidOperationException("Message cannot exist for successfull transactions");

                if (data == null)
                    throw new InvalidOperationException("Data cannot be null for successfull transactions");
            }
            else
            {
                if (string.IsNullOrEmpty(message))
                    throw new InvalidOperationException("Message cannot be empty for unsuccessfull transactions");
            }

            this.code = code;
            this.data = data;
            this.message = message;
        }

        /// <summary>
        /// Factory method creating a result object representing a successfull transaction
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TxResult<TData> OfSuccess(TData data)
        {
            return new TxResult<TData>(TxCode.Success, data);
        }

        /// <summary>
        /// Factory method creating a result object representing a failed transaction
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static TxResult<TData> OfFail(TxCode code, string message)
        {
            return new TxResult<TData>(code, null, message.GetDefaultIfNullOrWhiteSpace(DEFAULT_ERR_MESSAGE));
        }

    }
}
