using System;
using PulseLeads.Zillow.Models;

namespace PulseLeads.Zillow.Controllers
{
    public sealed class NLogger
    {
        public static void Error(string logger, string module_name, string message)
        {
            var _error = ZillowLogger.GetLogger(logger);
            _error.Error(module_name + ": " + message);
        }

        public static void Error(string logger, string module_name, string message, Exception exception)
        {
            var _error = ZillowLogger.GetLogger(logger);
            _error.Error(module_name + ": " + message, exception);
        }

        public static void Warn(string logger, string message)
        {
            var _warn = ZillowLogger.GetLogger(logger);
            _warn.Warn(message);
        }

        public static void Debug(string logger, string message)
        {
            var _debug = ZillowLogger.GetLogger(logger);
            _debug.Debug(message);
        }

        public static void Info(string logger, string message)
        {
            var _info = ZillowLogger.GetLogger(logger);
            _info.Info(message);
        }

        public static void Info(string logger, string module_name, string message)
        {
            var _info = ZillowLogger.GetLogger(logger);
            _info.Info(module_name + ": " + message);
        }

    }
}
