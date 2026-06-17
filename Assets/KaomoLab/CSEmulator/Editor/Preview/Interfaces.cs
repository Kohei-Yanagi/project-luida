using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.KaomoLab.CSEmulator.Editor.Preview
{
    public enum LogSource
    {
        ItemScript, PlayerScript
    }
    public enum LogLevel
    {
        Info, Warning, Error, Exception, JsError
    }

    public readonly struct ScriptMessageLoggedArgs
    {
        public readonly GameObject gameObject;
        public readonly string message;
        public readonly IProgramStatus programStatus;
        public ScriptMessageLoggedArgs(GameObject gameObject, string message, IProgramStatus programStatus)
            => (this.gameObject, this.message, this.programStatus) = (gameObject, message, programStatus);
    }
    public readonly struct ScriptLoggedArgs
    {
        public readonly GameObject gameObject;
        public readonly string message; //nullあり
        public readonly Exception exception; //nullあり
        public readonly Jint.Native.JsError jsError; //nullあり
        public readonly IProgramStatus programStatus;
        public ScriptLoggedArgs(
            GameObject gameObject, string message, Exception exception, Jint.Native.JsError jsError,
            IProgramStatus programStatus
        )
            => (this.gameObject, this.message, this.exception, this.jsError, this.programStatus)
            = (gameObject, message, exception, jsError, programStatus);
    }

}
