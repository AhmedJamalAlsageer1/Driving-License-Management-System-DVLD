using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer
{
    public class OperationResult
    {
        public bool? Success { get; private set;}
        public string Message { get; private set; }
        public enMessageType? MessageType { get; private set; }
        public bool IsSuccess => Success == true;
        public bool HasValue => Success.HasValue;

        public static OperationResult Seccuss(string Message, enMessageType? MessageType = enMessageType.Info) =>
            new OperationResult {  Success = true, Message = Message, MessageType = MessageType };

        public static OperationResult Failure(string Message, enMessageType? MessageType = enMessageType.Warning) =>
            new OperationResult { Success = false, Message = Message, MessageType = MessageType };

        public static OperationResult TechnicalFailure(string Message, enMessageType? MessageType = enMessageType.Error) =>
            new OperationResult { Success = null, Message = Message, MessageType = MessageType };
    }
}
