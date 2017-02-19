using System;
using Autofac;
using Csla.Abstractions.Tests.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Csla.Abstractions.Utils;
using Csla.Abstractions.Core.Contracts;
using Csla.Abstractions.Attributes;
using Csla.Abstractions.Core;
using Csla.Serialization.Mobile;
using System.Reflection;
using Csla.Abstractions.Tests.Helper;

namespace Csla.Abstractions.Tests
{
    [TestClass]
    public sealed class ObjectActivatorTests
    {
        private static void TestActivator(Action<IContainer> runTestCode)
        {
            runTestCode(new ContainerBuilder().Build());
        }

        private static void TestActivator<T>(Action<IContainer> runTestCode)
            where T : class, IMobileObject
        {
            var container = new ContainerBuilder();
            container.RegisterInstance<IObjectPortal<T>>(new ObjectPortal<T>());
            runTestCode(container.Build());
        }

        private static void TestActivator<I, T>(Action<IContainer> runTestCode)
            where T : class, IMobileObject where I : class, IMobileObject
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<T>()
                .As<I>();
            builder
                .RegisterType<ObjectPortal<I>>()
                .As<IObjectPortal<I>>();
            runTestCode(builder.Build());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateActivatorWithNullContainer()
        {
            var obj = new ObjectActivator(null);
            Assert.Fail("An exception was expected.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateInstanceWhenRequestedTypeIsNull()
        {
            Action obj = (() => ObjectActivatorTests.TestActivator(container =>
            {
                new ObjectActivator(container).CreateInstance(null);
            }));
            obj.Invoke();
            Assert.Fail("An exception was expected.");
        }

        [TestMethod]
        public void CreateInstanceWithInterfaceType()
        {
            ObjectActivatorTests.TestActivator<IDependentActivatorTest, ActivatorWithDependencyTest>(container =>
            {
                var activator = new ObjectActivator(container);
                var result = activator.CreateInstance(typeof(IDependentActivatorTest));
                Assert.IsInstanceOfType(result, typeof(IDependentActivatorTest));
            });
        }

        [TestMethod]
        public void CreateInstanceWithInterfaceTypeForAList()
        {
            ObjectActivatorTests.TestActivator<IActivatorListTest, ActivatorListTest>(container =>
            {
                var activator = new ObjectActivator(container);
                var result = activator.CreateInstance(typeof(IActivatorListTest));
                Assert.IsInstanceOfType(result, typeof(IActivatorListTest));
            });
        }

        [TestMethod]
        public void CreateInstanceWithInterfaceTypeForAReadOnlyItem()
        {
            ObjectActivatorTests.TestActivator<IActivatorROTest, ActivatorROTest>(container =>
            {
                var activator = new ObjectActivator(container);
                var result = activator.CreateInstance(typeof(IActivatorROTest));
                Assert.IsInstanceOfType(result, typeof(IActivatorROTest));
            });
        }

        [TestMethod]
        public void CreateInstanceWithConcreteType()
        {
            ObjectActivatorTests.TestActivator(container =>
            {
                var activator = new ObjectActivator(container);
                var result = activator.CreateInstance(typeof(ActivatorTest));
                Assert.IsInstanceOfType(result, typeof(IActivatorTest));
            });
        }

        [TestMethod]
        public void InitializeInstanceWithDependencyNotDefined()
        {
            try
            {
                ObjectActivatorTests.TestActivator<IDependentActivatorTest, ActivatorWithDependencyTest>(container =>
                {
                    var activator = new ObjectActivator(container);
                    var obj = activator.CreateInstance(typeof(IDependentActivatorTest));
                    activator.InitializeInstance(obj);
                });
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(IoCRegistrationException));
            }
        }

        [TestMethod]
        public void InitializeInstanceWithDependencyDefined()
        {
            ObjectActivatorTests.TestActivator<IActivatorTest, ActivatorTest>(container =>
            {
                // Arrange...
                // ... register more items into the container...
                var newBuilder = new ContainerBuilder();
                newBuilder
                .RegisterType<ActivatorWithDependencyTest>()
                .As<IDependentActivatorTest>();
                newBuilder
                    .RegisterType<ObjectPortal<IDependentActivatorTest>>()
                    .As<IObjectPortal<IDependentActivatorTest>>();
                // ... append new registrations to the container given by the TestActivator Action...
                newBuilder.Update(container);
              
                // ... still arranging...
                var activator = new ObjectActivator(container);
                var obj = activator.CreateInstance(typeof(IDependentActivatorTest));

                // Act...
                activator.InitializeInstance(obj);

                // Assert...
                ActivatorWithDependencyTest target = (ActivatorWithDependencyTest)obj;
                var aDependency = target.GetType().GetProperty("ADependency", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(target, null);
                Assert.IsInstanceOfType(aDependency, typeof(IObjectPortal<IActivatorTest>));
            });
        }

        [TestMethod]
        public void FinalizeInstanceWithDependencyDefined()
        {
            ObjectActivatorTests.TestActivator<IActivatorTest, ActivatorTest>(container =>
            {
                // Arrange...
                // ... register more items into the container...
                var newBuilder = new ContainerBuilder();
                newBuilder
                .RegisterType<ActivatorWithDependencyTest>()
                .As<IDependentActivatorTest>();
                newBuilder
                    .RegisterType<ObjectPortal<IDependentActivatorTest>>()
                    .As<IObjectPortal<IDependentActivatorTest>>();
                // ... append new registrations to the container given by the TestActivator Action...
                newBuilder.Update(container);
                // ... still arranging...
                var activator = new ObjectActivator(container);
                var obj = activator.CreateInstance(typeof(IDependentActivatorTest));
                activator.InitializeInstance(obj);

                // Act...
                activator.FinalizeInstance(obj);

                // Assert...
                ActivatorWithDependencyTest target = (ActivatorWithDependencyTest)obj;
                var aDependency = target.GetType().GetProperty("ADependency", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(target, null);
                Assert.IsNull(aDependency);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FinalizeInstanceForNullType()
        {
            ObjectActivatorTests.TestActivator<IActivatorTest>(container =>
            {
                var activator = new ObjectActivator(container);
                activator.FinalizeInstance(null);
                Assert.Fail("Expected an exception.");
            });
        }

        [TestMethod]
        public void CreateInstanceOfInterfaceMappingToSeveralConcreteClasses()
        {
            ObjectActivatorTests.TestActivator<ImplementedMultipleByTypeInDifferentAssembly, ActivatorTest>(container =>
            {
                var activator = new ObjectActivator(container);
                var obj = activator.CreateInstance(typeof(ImplementedMultipleByTypeInDifferentAssembly));
                Assert.IsInstanceOfType(obj, typeof(ImplementedMultipleByTypeInDifferentAssembly));
            });
            ObjectActivatorTests.TestActivator<ImplementedMultipleByTypeInDifferentAssembly, ActivatorWithDependencyTest>(container =>
            {
                var activator = new ObjectActivator(container);
                var obj = activator.CreateInstance(typeof(ImplementedMultipleByTypeInDifferentAssembly));
                Assert.IsInstanceOfType(obj, typeof(ImplementedMultipleByTypeInDifferentAssembly));
            });
        }

        [TestMethod]
        public void CreateInstanceOfInterfaceNotImplementedByAnyConcreteClass()
        {
            try
            {
                ObjectActivatorTests.TestActivator(container =>
                {
                    var activator = new ObjectActivator(container);
                    var obj = activator.CreateInstance(typeof(ImplementedByNobodyInDifferentAssembly));
                });
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ConcreteTypeResolutionException));
            }
        }

        [TestMethod]
        public void CreateInstanceOfInterfaceImplementedByAParentType()
        {
            ObjectActivatorTests.TestActivator<IParentType, ParentType>(container =>
            {
                var activator = new ObjectActivator(container);
                var obj = activator.CreateInstance(typeof(IParentType));
                Assert.IsInstanceOfType(obj, typeof(IParentType));
            });
        }

        [TestMethod]
        public void CreateInstanceOfInterfaceImplementedByAChildType()
        {
            ObjectActivatorTests.TestActivator<IChildType, ChildType>(container =>
             {
                 var activator = new ObjectActivator(container);
                 var obj = activator.CreateInstance(typeof(IObjectPortal<IChildType>));
                 Assert.IsInstanceOfType(obj, typeof(IObjectPortal<IChildType>));
             });
        }

        [TestMethod]
        public void CreateInstanceOfInterfaceImplementedByCommand()
        {
            ObjectActivatorTests.TestActivator<ICommandExample, CommandExample>(container =>
            {
                ApplicationContext.DataPortalActivator = new ObjectActivator(container);
                var cmd = DataPortal.Create<ICommandExample>();

                Assert.AreEqual(0, cmd.FirstValue);
                Assert.AreEqual(0, cmd.SecondValue);
                Assert.IsFalse(cmd.Result);

                cmd.FirstValue = 100;
                cmd.SecondValue = 200;
                var result = cmd.Execute();

                Assert.AreEqual(300, cmd.CalculationResult);
                Assert.AreEqual(100, cmd.FirstValue);
                Assert.AreEqual(200, cmd.SecondValue);
                Assert.IsTrue(cmd.Result);
                Assert.IsTrue(result);
            });
        }
    }

    [Serializable]
    internal sealed class ActivatorTest
        : BusinessBaseCore<ActivatorTest>, IActivatorTest, ImplementedMultipleByTypeInDifferentAssembly
    {
        private void Child_Create(string data)
        {
            this.Data = data;
        }
        private void Child_Fetch(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Create(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Delete(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Fetch(string data)
        {
            this.Data = data;
        }
        public readonly static PropertyInfo<string> DataProperty =
            PropertyInfoRegistration.Register<ActivatorTest, string>(_ => _.Data);
        public string Data
        {
            get { return this.GetProperty(ActivatorTest.DataProperty); }
            set { this.SetProperty(ActivatorTest.DataProperty, value); }
        }

        public string aString
        {
            get { return "some string"; }
        }
    }

    [Serializable]
    internal sealed class ActivatorROTest
        : ReadOnlyBaseCore<ActivatorROTest>, IActivatorROTest
    {
        private void Child_Create(string data)
        {
            this.Data = data;
        }
        private void Child_Fetch(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Create(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Fetch(string data)
        {
            this.Data = data;
        }
        public static PropertyInfo<string> DataProperty =
            PropertyInfoRegistration.Register<ActivatorROTest, string>(_ => _.Data);
        public string Data
        {
            get { return this.GetProperty(ActivatorROTest.DataProperty); }
            private set { this.LoadProperty(ActivatorROTest.DataProperty, value); }
        }

        public string aString
        {
            get { return "some string"; }
        }
    }

    [Serializable]
    internal sealed class ActivatorWithDependencyTest
        : BusinessBaseScopeCore<ActivatorWithDependencyTest>, IReadOnlyBaseCore, IDependentActivatorTest, ImplementedMultipleByTypeInDifferentAssembly
    {
        private IObjectPortal<IActivatorTest> _dependency;
        private void Child_Create(string data)
        {
            this.Data = data;
        }
        private void Child_Fetch(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Create(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Delete(string data)
        {
            this.Data = data;
        }
        private void DataPortal_Fetch(string data)
        {
            this.Data = data;
        }
        public readonly static PropertyInfo<string> DataProperty =
            PropertyInfoRegistration.Register<ActivatorWithDependencyTest, string>(_ => _.Data);
        public string Data
        {
            get { return this.GetProperty(ActivatorWithDependencyTest.DataProperty); }
            set { this.SetProperty(ActivatorWithDependencyTest.DataProperty, value); }
        }
        [Dependency]
        internal IObjectPortal<IActivatorTest> ADependency
        {
            private get { return _dependency; }
            set { _dependency = value; }
        }

        public string aString
        {
            get { return "some string"; }
        }
    }

    [Serializable]
    internal sealed class ActivatorListTest
        : BusinessListBaseCore<ActivatorListTest, IActivatorTest>, IActivatorListTest
    {

    }

    [Serializable]
    internal class ParentType : BusinessBaseScopeCore<ParentType>, IParentType
    {
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }

    [Serializable]
    internal class ChildType : ParentType, IChildType
    {
        public DateTime DateProperty { get; set; }
    }

    [Serializable]
    internal class CommandExample : CommandBaseScopeCore<CommandExample>, ICommandExample
    {
        private void DataPortal_Create()
        {
            this.FirstValue = 0;
            this.SecondValue = 0;
        }
        protected override void DataPortal_Execute()
        {
            // do something, server-side....
            // 
            this.CalculationResult = this.FirstValue + this.SecondValue;
            this.Result = true;
        }

        public override bool Execute()
        {   
            BeforeServer();
            var cmd = DataPortal.Execute<CommandExample>(this);
            AfterServer(cmd);
            return cmd.Result;
        }

        private void BeforeServer()
        {
            // TODO: implement code to run on client
            // before server is called.
        }
        private void AfterServer(CommandExample cmd)
        {
            this.CalculationResult = cmd.CalculationResult;
            this.Result = cmd.Result;
        }

        public static readonly PropertyInfo<int> FirstValueProperty =
           PropertyInfoRegistration.Register<CommandExample, int>(_ => _.FirstValue);
        public int FirstValue
        {
            get { return this.ReadProperty(CommandExample.FirstValueProperty); }
            set { this.LoadProperty(CommandExample.FirstValueProperty, value); }
        }
        public static readonly PropertyInfo<int> SecondValueProperty =
            PropertyInfoRegistration.Register<CommandExample, int>(_ => _.SecondValue);
        public int SecondValue
        {
            get { return this.ReadProperty(CommandExample.SecondValueProperty); }
            set { this.LoadProperty(CommandExample.SecondValueProperty, value); }
        }

        public static readonly PropertyInfo<int> CalculationResultProperty =
            PropertyInfoRegistration.Register<CommandExample, int>(_ => _.CalculationResult);
        public int CalculationResult
        {
            get { return this.ReadProperty(CommandExample.CalculationResultProperty); }
            set { this.LoadProperty(CommandExample.CalculationResultProperty, value); }
        }
    }

    namespace Contracts
    {
        public interface ICommandExample
            : ICommandBaseCore
        {
            int FirstValue { get; set; }
            int SecondValue { get; set; }
            int CalculationResult { get; }
        }
        public interface IParentType
            : IBusinessBaseCore
        {
            string StringProperty { get; set; }
            int IntegerProperty { get; set; }
        }

        public interface IChildType
            : IBusinessBaseCore
        {
            DateTime DateProperty { get; set; }
        }

        public interface IActivatorTest
            : IBusinessBaseCore
        {
            string Data { get; set; }
        }

        public interface IActivatorROTest
            : IReadOnlyBaseCore
        {
            string Data { get; }
        }

        public interface IDependentActivatorTest
            : IBusinessBaseCore
        {
            string Data { get; set; }
        }

        public interface IActivatorListTest
            : IBusinessListBaseCore<IActivatorTest>
        {

        }
    }
}
