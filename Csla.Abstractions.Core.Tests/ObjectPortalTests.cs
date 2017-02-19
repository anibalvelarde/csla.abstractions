using System;
using System.Threading.Tasks;
using Autofac;
using Csla.Abstractions.Tests.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spackle;
using Csla.Abstractions.Utils;

namespace Csla.Abstractions.Tests
{
    [TestClass]
	public class ObjectPortalTests
	{
        [TestMethod][ExpectedException(typeof(NotImplementedException))]
		public void BeginCreate()
		{
            Action obj = (() => new ObjectPortal<IObjectPortalTest>().BeginCreate());
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void BeginCreatePassingNull()
        {
            Action obj = (() => new ObjectPortal<IObjectPortalTest>().BeginCreate(null));
            obj.Invoke();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginCreateWithCriteria()
		{
            Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginCreate(null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginCreateWithCriteriaAndUserState()
		{
            Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginCreate(null, null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginDeleteWithCriteria()
		{
            Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginDelete(null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginDeleteWithCriteriaAndUserState()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginDelete(null, null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginExecuteWithObject()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginExecute(null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginExecuteWithObjectAndUserState()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginExecute(null, null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginFetch()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginFetch());
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginFetchWithCriteria()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginFetch(null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginFetchWithCriteriaAndUserState()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginFetch(null, null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginFetchWithObject()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginUpdate(null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginFetchWithObjectAndUserState()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginUpdate(null, null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginUpdateWithObject()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginUpdate(null));
            obj.Invoke();
		}

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
		public void BeginUpdateWithObjectAndUserState()
		{
			Action obj = (() => new ObjectPortal<ObjectPortalTest>().BeginUpdate(null, null));
            obj.Invoke();
		}

		[TestMethod]
		public void Create()
		{
			var test = new ObjectPortal<ObjectPortalTest>().Create();
			Assert.AreEqual(string.Empty, test.Data);
		}

		[TestMethod]
		public void CreateWithCriteria()
		{
			var data = new RandomObjectGenerator().Generate<string>();
			var test = new ObjectPortal<ObjectPortalTest>().Create(data);
			Assert.AreEqual(data, test.Data);
		}

		[TestMethod]
		public async Task CreateAsync()
		{
			var test = await new ObjectPortal<ObjectPortalTest>().CreateAsync();
			Assert.AreEqual(string.Empty, test.Data);
		}

		[TestMethod]
		public async Task CreateAsyncWithCriteria()
		{
			var data = new RandomObjectGenerator().Generate<string>();
			var test = await new ObjectPortal<ObjectPortalTest>().CreateAsync(data);
			Assert.AreEqual(data, test.Data);
		}

		[TestMethod]
		public void Delete()
		{
			var data = new RandomObjectGenerator().Generate<string>();
			new ObjectPortal<ObjectPortalTest>().Delete(data);
		}

		[TestMethod]
		public async Task DeleteAsync()
		{
			var data = new RandomObjectGenerator().Generate<string>();
			await new ObjectPortal<ObjectPortalTest>().DeleteAsync(data);
		}

		[TestMethod]
		public void Fetch()
		{
			var test = new ObjectPortal<ObjectPortalTest>().Fetch();
			Assert.AreEqual(string.Empty, test.Data);
		}

		[TestMethod]
		public void FetchWithCriteria()
		{
			var data = new RandomObjectGenerator().Generate<string>();
			var test = new ObjectPortal<ObjectPortalTest>().Fetch(data);
			Assert.AreEqual(data, test.Data);
		}

		[TestMethod]
		public async Task FetchAsync()
		{
			var test = await new ObjectPortal<ObjectPortalTest>().FetchAsync();
			Assert.AreEqual(string.Empty, test.Data);
		}

		[TestMethod]
		public async Task FetchAsyncWithCriteria()
		{
			var data = new RandomObjectGenerator().Generate<string>();
			var test = await new ObjectPortal<ObjectPortalTest>().FetchAsync(data);
			Assert.AreEqual(data, test.Data);
		}

        [TestMethod]
        public void FetchChild()
        {
            var test = new ObjectPortal<ObjectPortalTest>().FetchChild();
            Assert.AreEqual(string.Empty, test.Data);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FetchChildWithcriteria()
        {
            Action obj = (() => new ObjectPortal<ObjectPortalTest>().FetchChild(null));
            obj.Invoke();
        }

		[TestMethod]
		public void Execute()
		{
			var factory = new ObjectPortal<ObjectPortalTestCommand>();
			var test = factory.Execute(factory.Create());
			Assert.AreEqual("done", test.Data);
		}

		[TestMethod]
		public async Task ExecuteAsync()
		{
			var factory = new ObjectPortal<ObjectPortalTestCommand>();
			var test = await factory.ExecuteAsync(factory.Create());
			Assert.AreEqual("done", test.Data);
		}

		[TestMethod]
		public void GetGlobalContext()
		{
			var factory = new ObjectPortal<ObjectPortalTestCommand>();
			Assert.AreSame(ApplicationContext.GlobalContext, factory.GlobalContext);
		}

		[TestMethod]
		public void Update()
		{
			var factory = new ObjectPortal<ObjectPortalTest>();
			var test = factory.Fetch();
			factory.Update(test);
		}

		[TestMethod]
		public async Task UpdateAsync()
		{
			var factory = new ObjectPortal<ObjectPortalTest>();
			var test = factory.Fetch();
			await factory.UpdateAsync(test);
		}
	}

	[Serializable]
	public sealed class ObjectPortalTest
		: BusinessBase<ObjectPortalTest>, IObjectPortalTest
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
			PropertyInfoRegistration.Register<ObjectPortalTest, string>(_ => _.Data);
		public string Data
		{
			get { return this.GetProperty(ObjectPortalTest.DataProperty); }
			set { this.SetProperty(ObjectPortalTest.DataProperty, value); }
		}
	}

	[Serializable]
	public sealed class ObjectPortalTestCommand
		: CommandBase<ObjectPortalTestCommand>, IObjectPortalTestCommand
	{
		private void DataPortal_Create() { }

		protected override void DataPortal_Execute()
		{
			this.Data = "done";
		}

		public readonly static PropertyInfo<string> DataProperty =
			PropertyInfoRegistration.Register<ObjectPortalTestCommand, string>(_ => _.Data);
		public string Data
		{
			get { return this.ReadProperty(ObjectPortalTestCommand.DataProperty); }
			set { this.LoadProperty(ObjectPortalTestCommand.DataProperty, value); }
		}

		public ILifetimeScope Scope { get; set; }
	}

	namespace Contracts
    {
        public interface IObjectPortalTest
			: IBusinessBase
		{
			string Data { get; set; }
		}

		public interface IObjectPortalTestCommand
			: ICommandBase
		{
			string Data { get; set; }
		}
	}
}
