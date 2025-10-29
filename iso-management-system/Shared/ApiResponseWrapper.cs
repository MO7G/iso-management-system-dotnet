using System;
using iso_management_system.Constants;

namespace iso_management_system.Shared
{
    /// <summary>
    /// Represents a standardized wrapper for API responses.
    /// I use this class to ensure that all API endpoints return a consistent structure,
    /// including status code, message, optional data, and timestamp.
    /// </summary>
    /// <typeparam name="T">The type of the data payload returned in the response.</typeparam>
    public class ApiResponseWrapper<T>
    {
        /// <summary>
        /// HTTP-like status code representing the result of the API request.
        /// Typically cast from <see cref="ApiStatusCode"/>.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// A human-readable message describing the result of the API request.
        /// Can contain success messages, error descriptions, or warnings.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The data payload returned by the API request.
        /// This is optional and can be null if there is no data to return.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// The UTC timestamp when the response object was created.
        /// Useful for logging and debugging purposes on the client-side.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResponseWrapper{T}"/>.
        /// I use this constructor to create a consistent API response with status, message, and optional data.
        /// </summary>
        /// <param name="statusCode">The status code representing success or error.</param>
        /// <param name="message">A descriptive message for the response.</param>
        /// <param name="data">Optional data payload to include in the response.</param>
        public ApiResponseWrapper(ApiStatusCode statusCode, string message, T? data = default)
        {
            Status = (int)statusCode;
            Message = message;
            Data = data;
        }
    }
}