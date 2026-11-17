using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer
{
    public class Result <T>
    {
        private bool? IsSuccess { get; set; }
        public T Value { get; private set; }
        public string Message { get; private set; }
        public enMessageType? messageType { get; private set; }
        public enLogicCode? LogicCode { get; private set; }

        public bool HasValue => IsSuccess != null;
        public bool isSuccess => IsSuccess == true;


        public static Result<T> Success (T value, string Message, enMessageType? Type = enMessageType.Info, enLogicCode? LogicCode = null) =>
            new Result<T> { IsSuccess = true, Value = value, Message = Message, messageType = Type, LogicCode = LogicCode};

        public static Result<T> Failure (T Value, string Message, enMessageType? Type = enMessageType.Warning, enLogicCode? LogicCode = null) =>
            new Result<T> { IsSuccess = false, Value = Value, Message = Message, messageType = Type, LogicCode = LogicCode };

        public static Result<T> TechnicalFailure(T Value, string Message = "حصل خطأ في قواعد البيانات، يرجى المحاولة مرة أخرى!", enMessageType? Type = enMessageType.Error, enLogicCode? LogicCode = null) =>
            new Result<T> { IsSuccess = null, Value = Value, Message = Message, messageType = Type, LogicCode = LogicCode };
    }
}
