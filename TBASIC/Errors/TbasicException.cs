﻿/**
 *  TBASIC
 *  Copyright (C) 2013-2016 Timothy Baxendale
 *  
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *  
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *  
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301
 *  USA
 **/
using System;
using System.IO;
using System.Security;

namespace Tbasic.Errors
{
    /// <summary>
    /// These codes indicate the at least partial success of a function
    /// </summary>
    public struct ErrorSuccess
    {
        /// <summary>
        /// The function is reporting no error
        /// </summary>
        public const int OK = 200;
        /// <summary>
        /// The function has created the requested data
        /// </summary>
        public const int Created = 201;
        /// <summary>
        /// The function believes the request was completed but cannot confirm it absolutely, 
        /// or the request is being processed and its success cannot be determined at the functions termination
        /// </summary>
        public const int Accepted = 202;
        /// <summary>
        /// The function is returning information from another source
        /// </summary>
        public const int NonAuthoritative = 203;
        /// <summary>
        /// The function completed successfully but has no content to return
        /// </summary>
        public const int NoContent = 204;
        /// <summary>
        /// The function completed with warnings, or its task was only partially completed
        /// </summary>
        public const int Warnings = 206;

        /// <summary>
        /// Determines whether the given code is a success code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsSuccess(int code)
        {
            return code >= 200 && code < 300;
        }
    }

    /// <summary>
    /// These codes represent errors because certain preconditions have not been met
    /// </summary>
    public struct ErrorClient
    {
        /// <summary>
        /// The function could not be completed because it was formatted incorrectly
        /// </summary>
        public const int BadRequest = 400;
        /// <summary>
        /// The function does not have permission to complete the command, but credentials may be supplied
        /// </summary>
        public const int Unauthorized = 401;
        /// <summary>
        /// The function has no way of executing with the current permissions
        /// </summary>
        public const int Forbidden = 403;
        /// <summary>
        /// The function could not locate requested data
        /// </summary>
        public const int NotFound = 404;
        /// <summary>
        /// There was a conflict accessing certain data, either too many instances of an object were requested or multiple objects were requested to occupy the same memory
        /// </summary>
        public const int Conflict = 409;
        /// <summary>
        /// The resource may currently be in use
        /// </summary>
        public const int Locked = 423;

        /// <summary>
        /// Determines whether the given code is an ErrorClient code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsError(int code)
        {
            return code >= 400 && code < 500;
        }
    }

    /// <summary>
    /// These codes represent errors because
    /// </summary>
    public struct ErrorServer
    {
        /// <summary>
        /// A generic exception occoured
        /// </summary>
        public const int GenericError = 500;
        /// <summary>
        /// The function has not been implemented
        /// </summary>
        public const int NotImplemented = 501;
        /// <summary>
        /// Data received through a gateway was invalid or poorly formatted
        /// </summary>
        public const int BadGateway = 502;
        /// <summary>
        /// There is no memory to perform the requested task
        /// </summary>
        public const int NoMemory = 507;

        /// <summary>
        /// Determines whether the given code is an ErrorServer code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsError(int code)
        {
            return code >= 500 && code < 600;
        }
    }

    /// <summary>
    /// An exception that has been associated with a status code
    /// </summary>
    public class TbasicException : Exception
    {
        /// <summary>
        /// Gets the status code for this exception
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// Constructs a new CustomException with a given status code
        /// </summary>
        /// <param name="status">the status code for this exception</param>
        /// <param name="innerException">the inner exception</param>
        public TbasicException(int status, Exception innerException = null)
            : this(status, GetGenericMessage(status), innerException)
        {
        }

        /// <summary>
        /// Constructs a new CustomException with a given status code and message
        /// </summary>
        /// <param name="status">the status code for this exception</param>
        /// <param name="msg">the message for this exception</param>
        /// /// <param name="innerException">the inner exception</param>
        public TbasicException(int status, string msg, Exception innerException = null)
            : this(status, msg, true)
        {
        }

        /// <summary>
        /// Constructs a new CustomException with a given status code and message
        /// </summary>
        /// <param name="status">the status code for this exception</param>
        /// <param name="msg">the message for this exception</param>
        /// <param name="prependGeneric">true to add the generic message, false otherwise</param>
        /// <param name="innerException">the inner exception</param>
        public TbasicException(int status, string msg, bool prependGeneric, Exception innerException = null)
            : base(prependGeneric ? GetGenericMessage(status) + ": " + msg : msg, innerException)
        {
            Status = status;
        }

        /// <summary>
        /// Gets a generic message for an exception
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetGenericMessage(int status)
        {
            switch (status) {
                // success
                case ErrorSuccess.OK:
                    return "OK";
                case ErrorSuccess.Created:
                    return "Created";
                case ErrorSuccess.Accepted:
                    return "Accepted";
                case ErrorSuccess.NonAuthoritative:
                    return "Non-Authoritative Information";
                case ErrorSuccess.NoContent:
                    return "No Content";
                case ErrorSuccess.Warnings:
                    return "Warning";
                // client errors
                case ErrorClient.BadRequest:
                    return "Bad Request";
                case ErrorClient.Unauthorized:
                    return "Unauthorized";
                case ErrorClient.Forbidden:
                    return "Forbidden";
                case ErrorClient.NotFound:
                    return "Not Found";
                case ErrorClient.Conflict:
                    return "Conflict";
                case ErrorClient.Locked:
                    return "Locked";
                // server errors
                case ErrorServer.GenericError:
                    return "Internal Error";
                case ErrorServer.NotImplemented:
                    return "Not Implemented";
                case ErrorServer.BadGateway:
                    return "Bad Gateway";
                case ErrorServer.NoMemory:
                    return "Insufficient Memory";
            }
            return string.Empty;
        }

        internal static Exception WrapException(Exception ex)
        {
            if (ex is ArgumentException || ex is FormatException) {
                return new TbasicException(ErrorClient.BadRequest, ex.Message, ex);
            }
            else if (ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is DriveNotFoundException) {
                return new TbasicException(ErrorClient.NotFound, ex);
            }
            else if (ex is UnauthorizedAccessException || ex is SecurityException) {
                return new TbasicException(ErrorClient.Forbidden, ex.Message, ex);
            }
            else if (ex is IOException) {
                return new TbasicException(ErrorClient.Locked, ex.Message, ex);
            }
            return ex;
        }
    }
}