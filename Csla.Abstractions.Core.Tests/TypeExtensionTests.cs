using System;
using Csla.Abstractions.Extensions;
using Csla.Abstractions.Core;
using Csla.Abstractions.Core.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csla.Abstractions.Tests
{
    [TestClass]
    public class TypeExtensionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ConcreteTypeResolutionException))]
        public void TypeExt_Should_ThrowException_When_FindingMoreThanOneConcreteImplementationOfInterface()
        {
            // Arrange...
            Type anInterfaceType = typeof(IImplementedByMultipleTypes);

            // Acct...
            Type aConcreteType = anInterfaceType.ScanForFirstConcreteType();

            // Fail...
            Assert.Fail("Expected an exception to be thrown.");
        }
        [TestMethod]
        [ExpectedException(typeof(ConcreteTypeResolutionException))]
        public void TypeExt_Should_ThrowException_When_NotFindingAConcreteImplementationOfInterface()
        {
            // Arrange...
            Type anInterfaceType = typeof(INotImplementedAnywhere);

            // Acct...
            Type aConcreteType = anInterfaceType.ScanForFirstConcreteType();

            // Fail...
            Assert.Fail("Expected an exception to be thrown.");
        }
        [TestMethod]
        public void TypeExt_Should_ReturnConcreteType_When_LookingForImplementationOfInterfaceInADifferentAssembly()
        {
            // Arrange...
            Type anInterfaceType = typeof(Helper.ImplementedOnceByTypeInDifferentAssembly);

            // Acct...
            Type aConcreteType = anInterfaceType.ScanForFirstConcreteType();

            // Assert...
            Assert.IsInstanceOfType(aConcreteType, typeof(System.Type));
            Assert.AreEqual(typeof(ConcreteTypeB), aConcreteType);
        }
        [TestMethod]
        [ExpectedException(typeof(ConcreteTypeResolutionException))]
        public void TypeExt_Should_ThrowException_When_LookingForImplementationOfInterfaceByMultipleTypesInADifferentAssembly()
        {
            // Arrange...
            Type anInterfaceType = typeof(Helper.ImplementedMultipleByTypeInDifferentAssembly);

            // Acct...
            Type aConcreteType = anInterfaceType.ScanForFirstConcreteType();

            // Fail...
            Assert.Fail("Expected to receive an exception.");
        }
        [TestMethod]
        [ExpectedException(typeof(ConcreteTypeResolutionException))]
        public void TypeExt_Should_ThrowException_When_NotFindingAConcreteImplementationOfInterfaceInADifferentAssembly()
        {
            // Arrange...
            Type anInterfaceType = typeof(Helper.ImplementedByNobodyInDifferentAssembly);

            // Acct...
            Type aConcreteType = anInterfaceType.ScanForFirstConcreteType();

            // Fail...
            Assert.Fail("Expected an exception to be thrown.");
        }

        public interface INotImplementedAnywhere
        {
            int someInteger { get; set; }
        }
        public interface IImplementedByMultipleTypes
        {
            int anInt { get; }
            string aString { get; }
        }
        public interface IImplementedBySingleType
        {
            int someInt { get; }
        }

        [Serializable]
        public sealed class ConcreteTypeA : BusinessBaseCore<ConcreteTypeA>, IBusinessBaseCore, IImplementedByMultipleTypes, IImplementedBySingleType
        {
            public int ANumber { get { return 1; } }
            public string AString { get { return "some string"; } }

            public int anInt
            {
                get { return 1; }
            }

            public string aString
            {
                get { return "some string"; }
            }

            public int someInt
            {
                get { return this.ANumber; }
            }
        }

        [Serializable]
        public sealed class ConcreteTypeB : BusinessBaseCore<ConcreteTypeB>, IBusinessBaseCore, IImplementedByMultipleTypes, Helper.ImplementedOnceByTypeInDifferentAssembly
        {
            public int anInt
            {
                get { return 2; }
            }

            public string aString
            {
                get { return "some other string"; }
            }

            public int aNumber
            {
                get { return this.anInt; }
            }
        }

        [Serializable]
        public sealed class ConcreteTypeC : BusinessBaseCore<ConcreteTypeC>, IBusinessBaseCore, Helper.ImplementedMultipleByTypeInDifferentAssembly
        {

            public string aString
            {
                get { return "some string from TypeC"; }
            }
        }

        [Serializable]
        public sealed class ConcreteTypeD: BusinessBaseCore<ConcreteTypeD>, IBusinessBaseCore, Helper.ImplementedMultipleByTypeInDifferentAssembly
        {

            public string aString
            {
                get { return "some string from type D"; }
            }
        }
    }

}
