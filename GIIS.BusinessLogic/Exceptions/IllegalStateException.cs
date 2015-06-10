using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIIS.BusinessLogic.Exceptions
{
    /// <summary>
    /// Illegal state exception indicates an operation failed because of an illegal state guard condition
    /// </summary>
    public class IllegalStateException : InvalidOperationException
    {
        /// <summary>
        /// The current state which caused the exception
        /// </summary>
        public OrderStatusType CurrentState { get; private set; }

        /// <summary>
        /// Gets the name of the class that caused the exception
        /// </summary>
        public String ClassName { get; private set; }

        /// <summary>
        /// Gets the attempted action that caused the exception
        /// </summary>
        public String Action { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="T:GIIS.BusinessLogic.Exception.IllegalStateException"/> class
        /// </summary>
        /// <param name="currentState">The current state of the object that caused the exception</param>
        /// <param name="className">The name of the class in the state which caused the exception</param>
        /// <param name="action">The name of the action which caused the exception</param>
        public IllegalStateException(OrderStatusType currentState, String className, String action) : base(String.Format("Could not perform {0} on instance of {1} as object is invalid state (currentState: {2})", action, className, currentState))
        {
            this.CurrentState = currentState;
            this.Action = action;
            this.ClassName = className;
        }
    }
}
