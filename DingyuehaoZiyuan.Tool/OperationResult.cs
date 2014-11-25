using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DingyuehaoZiyuan.Tool
{
    public class OperationResult
    {
        public OperationResult()
        { }

        public OperationResult(bool result, string message)
        {
            Result = result;
            Message = message;
        }
        public bool Result { get; set; }
        public string Message { get; set; }
    }
}
