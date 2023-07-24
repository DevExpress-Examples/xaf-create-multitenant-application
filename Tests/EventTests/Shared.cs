namespace Tests.EventTests{
    public class StringEventArgs : EventArgs{
        public string Value{ get; }

        public StringEventArgs(string value) => Value = value;
    }

    public class Derived : TestClass{
        public void RaiseDerivedEvent() => RaiseTestEvent();
    }

    public interface ITestInterface{
        event EventHandler TestEvent;

        void RaiseInterfaceEvent();
    }

    public class InterfaceImplementingClass : ITestInterface{
        public event EventHandler? TestEvent;

        public void RaiseInterfaceEvent() => TestEvent?.Invoke(this, EventArgs.Empty);
    }

    public class TestClassWithCustomEvent{
        private EventHandler? _testEvent;


        public event EventHandler? CustomEvent{
            add{
                Console.WriteLine("Adding an event handler");
                _testEvent += value;
            }
            remove{
                Console.WriteLine("Removing an event handler");
                _testEvent -= value;
            }
        }

        public void RaiseCustomEvent() => _testEvent?.Invoke(this, EventArgs.Empty);
    }

    public class MultiStringEventArgs : EventArgs{
        public string Value1{ get; }
        public string Value2{ get; }

        public MultiStringEventArgs(string value1, string value2){
            Value1 = value1;
            Value2 = value2;
        }
    }
    public class ExceptionThrowingEventArgs : EventArgs
    {
        public void ThrowException()
        {
            throw new Exception("Exception thrown from ExceptionThrowingEventArgs");
        }
    }

    public class TestClass
    {
        public event EventHandler<ExceptionThrowingEventArgs> TestEventException;

        public void RaiseTestEventException()
        {
            var args = new ExceptionThrowingEventArgs();
            TestEventException?.Invoke(this, args);
            args.ThrowException();
        }
        
        public event EventHandler TestEvent4;
        public event EventHandler TestEvent5;
        




        public void RaiseTestEvent4() {
            TestEvent4?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseTestEvent5() {
            TestEvent5?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? TestEvent;
        public event EventHandler<StringEventArgs>? TestEventWithParameter;
        public event EventHandler<MultiStringEventArgs>? TestEventWithMultipleParameters;
        public event EventHandler? BaseEvent;
        public event EventHandler? TestEvent1;
        public event EventHandler? TestEvent2;
        public event EventHandler? TestEvent3;

        public void RaiseTestEvent() => TestEvent?.Invoke(this, EventArgs.Empty);
        public void RaiseTestEventWithParameter(string parameter) => TestEventWithParameter?.Invoke(this, new StringEventArgs(parameter));
        public void RaiseTestEventWithMultipleParameters(string parameter1, string parameter2) => TestEventWithMultipleParameters?.Invoke(this, new MultiStringEventArgs(parameter1, parameter2));
        public void RaiseBaseEvent() => BaseEvent?.Invoke(this, EventArgs.Empty);
        public void RaiseTestEvent1() => TestEvent1?.Invoke(this, EventArgs.Empty);
        public void RaiseTestEvent2() => TestEvent2?.Invoke(this, EventArgs.Empty);
        public void RaiseTestEvent3() => TestEvent3?.Invoke(this, EventArgs.Empty);
    }

}