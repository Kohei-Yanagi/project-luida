using Jint.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.KaomoLab.CSEmulator.Editor.Preview
{
    public class DebugLogFactory
        : Engine.ILoggerFactory
    {
        class CallbackLogger : ILogger
        {
            readonly UnityEngine.GameObject gameObject;
            readonly Action<LogLevel, ScriptLoggedArgs> OnScriptLogged;
            readonly IProgramStatus programStatus;
            readonly ILogger logger;

            public CallbackLogger(
                UnityEngine.GameObject gameObject,
                Action<LogLevel, ScriptLoggedArgs> OnScriptLogged,
                IProgramStatus programStatus,
                ILogger logger
            )
            {
                this.gameObject = gameObject;
                this.OnScriptLogged = OnScriptLogged;
                this.programStatus = programStatus;
                this.logger = logger;
            }

            public void Error(string message)
            {
                logger.Error(message);
                OnScriptLogged(LogLevel.Error, new ScriptLoggedArgs(gameObject, message, null, null, programStatus));
            }
            public void Exception(JsError e)
            {
                logger.Exception(e);
                OnScriptLogged(LogLevel.JsError, new ScriptLoggedArgs(gameObject, null, null, e, programStatus));
            }
            public void Exception(Exception e)
            {
                logger.Exception(e);
                OnScriptLogged(LogLevel.Exception, new ScriptLoggedArgs(gameObject, null, e, null, programStatus));
            }
            public void Info(string message)
            {
                logger.Info(message);
                OnScriptLogged(LogLevel.Info, new ScriptLoggedArgs(gameObject, message, null, null, programStatus));
            }
            public void Warning(string message)
            {
                logger.Warning(message);
                OnScriptLogged(LogLevel.Warning, new ScriptLoggedArgs(gameObject, message, null, null, programStatus));
            }
        }
        class CallbackLoggerLow : ILoggerLow
        {
            readonly UnityEngine.GameObject gameObject;
            readonly Action<LogLevel, ScriptMessageLoggedArgs> OnScriptLogged;
            readonly IProgramStatus programStatus;

            public CallbackLoggerLow(
                UnityEngine.GameObject gameObject,
                Action<LogLevel, ScriptMessageLoggedArgs> OnScriptLogged,
                IProgramStatus programStatus
            )
            {
                this.gameObject = gameObject;
                this.OnScriptLogged = OnScriptLogged;
                this.programStatus = programStatus;
            }
            public void Log(string message)
            {
                UnityEngine.Debug.Log(message);
                OnScriptLogged(LogLevel.Info, new ScriptMessageLoggedArgs(gameObject, message, programStatus));
            }
            public void LogError(string message)
            {
                UnityEngine.Debug.LogError(message);
                OnScriptLogged(LogLevel.Error, new ScriptMessageLoggedArgs(gameObject, message, programStatus));
            }
            public void LogWarning(string message)
            {
                UnityEngine.Debug.LogWarning(message);
                OnScriptLogged(LogLevel.Warning, new ScriptMessageLoggedArgs(gameObject, message, programStatus));
            }
        }

        readonly EmulatorOptions options;
        readonly Action<LogLevel, ScriptMessageLoggedArgs> OnScriptMessageLogged;
        readonly Action<LogLevel, ScriptLoggedArgs> OnScriptLogged;

        public DebugLogFactory(
            Action<LogLevel, ScriptMessageLoggedArgs> OnScriptMessageLogged,
            Action<LogLevel, ScriptLoggedArgs> OnScriptLogged,
            EmulatorOptions options
        )
        {
            this.options = options;
            this.OnScriptMessageLogged = OnScriptMessageLogged;
            this.OnScriptLogged = OnScriptLogged;
        }

        public ILogger Create(UnityEngine.GameObject gameObject, IProgramStatus programStatus)
        {
            return new CallbackLogger(
                gameObject,
                OnScriptLogged,
                programStatus,
                new DebugLogger(
                    gameObject,
                    programStatus,
                    options,
                    new CallbackLoggerLow(gameObject, OnScriptMessageLogged, programStatus)
                )
            );
        }
    }
}
