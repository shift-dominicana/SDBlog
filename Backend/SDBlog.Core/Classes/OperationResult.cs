using SDBlog.Core.Interfaces;
using System;

namespace SDBlog.Core.Classes
{
    public sealed class OperationResult<T> : IOperationResult<T>
    {
        public bool Success { get; set; }
        public T Entity { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetail { get; set; }

        private OperationResult(bool success, T entity, string errorMessage, string errorDetail)
        {
            Success = success;
            Entity = entity;
            ErrorMessage = errorMessage;
            ErrorDetail = errorDetail;
        }

        private OperationResult(bool success, string errorMessage, string errorDetail)
        {
            Success = success;
            ErrorMessage = errorMessage;
            ErrorDetail = errorDetail;
        }

        public static OperationResult<T> Ok() => new OperationResult<T>(true, default(T), "", "");

        public static OperationResult<T> Ok(T entity) => new OperationResult<T>(true, entity, "", "");

        public static OperationResult<T> Fail(string errorMessage) => new OperationResult<T>(false, default(T), errorMessage, "");

        public static OperationResult<T> Fail(string errorMessage, string errorDetail) => new OperationResult<T>(false, default(T), errorMessage, errorDetail);
    }
}
