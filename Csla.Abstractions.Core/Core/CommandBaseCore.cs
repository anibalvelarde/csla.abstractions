using System;
using Csla.Abstractions.Core.Contracts;
using Csla.Abstractions.Utils;

namespace Csla.Abstractions.Core
{
    [Serializable]
    public abstract class CommandBaseCore<T> : CommandBase<T>, ICommandBaseCore where T : CommandBaseCore<T>
    {
        protected CommandBaseCore()
            : base()
        { }

        public abstract bool Execute();


        public static readonly PropertyInfo<bool> ResultProperty =
            PropertyInfoRegistration.Register<CommandBaseCore<T>, bool>(_ => _.Result);
        public bool Result
        {
            get { return this.ReadProperty(CommandBaseCore<T>.ResultProperty); }
            protected set { this.LoadProperty(CommandBaseCore<T>.ResultProperty, value); }
        }
    }
}
