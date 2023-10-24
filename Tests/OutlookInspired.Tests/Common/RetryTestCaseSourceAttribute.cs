using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace OutlookInspired.Tests.Common{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RetryTestCaseSourceAttribute : TestCaseSourceAttribute, IRepeatTest{
        public RetryTestCaseSourceAttribute(string sourceName) : base(sourceName){
        }

        public RetryTestCaseSourceAttribute(Type sourceType) : base(sourceType){
        }

        public RetryTestCaseSourceAttribute(Type sourceType, string sourceName) : base(sourceType, sourceName){
        }

        public RetryTestCaseSourceAttribute(string sourceName, object[] methodParams) : base(sourceName, methodParams){
        }

        public RetryTestCaseSourceAttribute(Type sourceType, string sourceName, object[] methodParams) : base(
            sourceType, sourceName, methodParams){
        }

        public int MaxTries{ get; set; } = 3;

        TestCommand ICommandWrapper.Wrap(TestCommand command) => new RetryCommand(command, MaxTries);
    }

    public class RetryCommand : DelegatingTestCommand{
        private readonly int _tryCount;
        public RetryCommand(TestCommand innerCommand, int tryCount) : base(innerCommand) => _tryCount = tryCount;

        public override TestResult Execute(TestExecutionContext context){
            var count = _tryCount;
            while (count-- > 0){
                try{
                    context.CurrentResult = innerCommand.Execute(context);
                }
                catch (Exception ex){
                    context.CurrentResult ??= context.CurrentTest.MakeTestResult();
                    context.CurrentResult.RecordException(ex);
                }
                if (context.CurrentResult.ResultState != ResultState.Error)
                    break;
                if (count <= 0) continue;
                context.CurrentResult = context.CurrentTest.MakeTestResult();
                context.CurrentRepeatCount++; 
            }
            return context.CurrentResult;
        }
    }
}