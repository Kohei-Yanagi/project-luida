using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.KaomoLab.CSEmulator.Editor.Preview
{
    public class DebugLogger
        : ILogger
    {
        readonly UnityEngine.GameObject gameObject;
        readonly IProgramStatus programStatus;
        readonly EmulatorOptions options;
        readonly ILoggerLow logger;
        readonly string path;
        readonly string name;
        readonly string id;

        public DebugLogger(
            UnityEngine.GameObject gameObject,
            IProgramStatus programStatus,
            EmulatorOptions options,
            ILoggerLow logger
        )
        {
            this.gameObject = gameObject;
            this.programStatus = programStatus;
            this.options = options;
            this.logger = logger;
            this.path = gameObject.GetFullPath();
            this.name = gameObject.GetComponent<ClusterVR.CreatorKit.Item.IItem>()?.ItemName;
            this.id = gameObject.GetInstanceID().ToString("X8");
        }

        string BuildMessage(string message)
        {
            if (!options.debug) return message;
            var ret = String.Format(
                "[i:{0}][{1}][{2}]{3}",
                id,
                name,
                programStatus.GetLineInfo(),
                message
            );
            return ret;
        }
        string BuildMessage(string message, string stack)
        {
            if (!options.debug) return message;
            var ret = String.Format(
                "[i:{0}][{1}][{2}]{3}\n{4}",
                id,
                name,
                programStatus.GetLineInfo(),
                message,
                stack
            );
            return ret;
        }
        string BuildMessage(string message, Acornima.SourceLocation location, string stack)
        {
            if (!options.debug) return message;
            var ret = String.Format(
                "[i:{0}][{1}][{2}]{3}\n{4}",
                id,
                name,
                Engine.JintProgramStatus.FormattedLocation(location),
                message,
                stack
            );
            return ret;
        }

        public void Error(string message)
        {
            logger.LogError(BuildMessage(message));
        }

        public void Info(string message)
        {
            logger.Log(BuildMessage(message));
        }

        public void Warning(string message)
        {
            logger.LogWarning(BuildMessage(message));
        }

        public void Exception(Jint.Native.JsError e)
        {
            var ps = e.GetOwnProperties()
                .ToDictionary(kv => kv.Key.ToString(), kv => kv.Value.Value.ToString());
            var message = BuildMessage(ps["message"], ps["stack"]);
            logger.LogError(message);
        }

        public void Exception(Exception e)
        {
            if (e is Jint.Runtime.JavaScriptException jse)
            {
                var message = BuildMessage(jse.Message, jse.Location, jse.JavaScriptStackTrace);
                logger.LogError(message);
            }
            else if (e.InnerException is Jint.Runtime.JavaScriptException ijse)
            {
                var message = BuildMessage(ijse.Message, ijse.Location, ijse.JavaScriptStackTrace);
                logger.LogError(message);
            }
            else
            {
                logger.LogError(
                    String.Format("[i:{0}][{1}][{2}]{3}:{4}\n{5}\n-----------\n{6}",
                        id,
                        name,
                        programStatus.GetLineInfo(),
                        e.GetType().Name,
                        e.Message,
                        programStatus.GetStack(),
                        e.StackTrace
                    )
                );
            }
        }
    }
}
