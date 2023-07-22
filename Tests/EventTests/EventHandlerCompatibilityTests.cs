using System.Reflection;
using NUnit.Framework;
using OutlookInspired.Module.Services;
using Shouldly;

namespace Tests.EventTests{
    public class EventHandlerCompatibilityTests{
        [Test]
        public async Task FromEventPattern_ShouldWorkWithBaseClassEvents(){
            var derivedClass = new Derived();
            var asyncEnumerable = derivedClass.WhenEventFired<EventArgs>("BaseEvent");

            derivedClass.RaiseBaseEvent();

            var e = await asyncEnumerable.FirstAsync();
            e.ShouldNotBeNull();
        }

        [Test]
        public async Task FromEventPattern_ShouldWorkWithInterfaceEvents(){
            var interfaceImplementer = new InterfaceImplementingClass();
            var asyncEnumerable = interfaceImplementer.WhenEventFired<EventArgs>(nameof(ITestInterface.TestEvent));

            interfaceImplementer.RaiseInterfaceEvent();

            var e = await asyncEnumerable.FirstAsync();
            e.ShouldNotBeNull();
        }

        [Test]
        public async Task FromEventPattern_ShouldWorkWithDerivedClassEvents(){
            var derivedClass = new Derived();
            var asyncEnumerable = derivedClass.WhenEventFired<EventArgs>(nameof(Derived.TestEvent));

            derivedClass.RaiseDerivedEvent();

            var e = await asyncEnumerable.FirstAsync();
            e.ShouldNotBeNull();
        }

        [Test]
        public async Task FromEventPattern_ShouldWorkWithCustomEventAccessors(){
            var testClass = new TestClassWithCustomEvent();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>(nameof(TestClassWithCustomEvent.CustomEvent));

            testClass.RaiseCustomEvent();

            var e = await asyncEnumerable.FirstAsync();
            e.ShouldNotBeNull();
        }


        [Test]
        public async Task FromEventPattern_ShouldWorkWithPrivateEvents(){
            var testClass = new TestClassWithPrivateEvent();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("PrivateEvent");

            testClass.RaisePrivateEvent();
            var e = await asyncEnumerable.FirstAsync();
            e.ShouldNotBeNull();
        }

        [Test]
        public async Task FromEventPattern_ShouldWorkWithProtectedEvents(){
            var testClass = new TestClassWithProtectedEvent();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("ProtectedEvent");

            testClass.RaiseProtectedEvent();
            var e = await asyncEnumerable.FirstAsync();
            e.ShouldNotBeNull();
        }

        [Test]
        public async Task FromEventPattern_ShouldWorkWithInternalEvents(){
            var testClass = new TestClassWithInternalEvent();
            var asyncEnumerable = testClass.WhenEventFired<EventArgs>("InternalEvent");

            testClass.RaiseInternalEvent();
            var e = await asyncEnumerable.FirstAsync();
            e.ShouldNotBeNull();
        }
    }

    public class TestClassWithInternalEvent{
        internal event EventHandler? InternalEvent;

        public void RaiseInternalEvent(){
            InternalEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public class TestClassWithPrivateEvent{
        private event EventHandler? PrivateEvent;

        public void RaisePrivateEvent(){
            PrivateEvent?.Invoke(this, EventArgs.Empty);
        }
    }


    public class TestClassWithExposedEvent : TestClassWithPrivateEvent{
        public event EventHandler? ExposedEvent{
            add => typeof(TestClassWithPrivateEvent)
                    .GetEvent("PrivateEvent", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .AddEventHandler(this, value);
            remove => typeof(TestClassWithPrivateEvent)
                .GetEvent("PrivateEvent", BindingFlags.NonPublic | BindingFlags.Instance)!
                    .RemoveEventHandler(this, value);
        }
    }

    public class TestClassWithProtectedEvent{
        protected event EventHandler? ProtectedEvent;

        public void RaiseProtectedEvent(){
            ProtectedEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}