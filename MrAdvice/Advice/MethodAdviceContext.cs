﻿#region Mr. Advice
// Mr. Advice
// A simple post build weaving package
// http://mradvice.arxone.com/
// Released under MIT license http://opensource.org/licenses/mit-license.php
#endregion
namespace ArxOne.MrAdvice.Advice
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// Method advice context, passed to method advisors
    /// </summary>
    [DebuggerDisplay("MethodInfo {TargetMethod}")]
    public class MethodAdviceContext : AdviceContext
    {
        /// <summary>
        /// Gets the parameters.
        /// Each parameter can be individually changed before Call.Proceed()
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IList<object> Parameters { get { return AdviceValues.Parameters; } }

        /// <summary>
        /// Gets a value indicating whether the advised method has a return value.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has return value; otherwise, <c>false</c>.
        /// </value>
        public bool HasReturnValue
        {
            get
            {
                var methodInfo = TargetMethod as MethodInfo;
                if (methodInfo == null) // ctor
                    return false;
                return methodInfo.ReturnType != typeof (void);
            }
        }

        /// <summary>
        /// Gets or sets the return value (after Call.Proceed()).
        /// </summary>
        /// <value>
        /// The return value.
        /// </value>
        public object ReturnValue
        {
            get
            {
                if(!HasReturnValue)
                    throw new InvalidOperationException("Method has no ReturnValue");
                return AdviceValues.ReturnValue;
            }
            set
            {
                if (!HasReturnValue)
                    throw new InvalidOperationException("Method has no ReturnValue");
                AdviceValues.ReturnValue = value;
            }
        }

        /// <summary>
        /// Gets the target method.
        /// </summary>
        /// <value>
        /// The target method.
        /// </value>
        public MethodBase TargetMethod { get; private set; }

        private readonly IMethodAdvice _methodAdvice;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodAdviceContext" /> class.
        /// </summary>
        /// <param name="methodAdvice">The method advice.</param>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="adviceValues">The call values.</param>
        /// <param name="nextAdviceContext">The next advice context.</param>
        internal MethodAdviceContext(IMethodAdvice methodAdvice, MethodBase targetMethod, AdviceValues adviceValues, AdviceContext nextAdviceContext)
            : base(adviceValues, nextAdviceContext)
        {
            _methodAdvice = methodAdvice;
            TargetMethod = targetMethod;
        }

        /// <summary>
        /// Invokes the current aspect (related to this instance).
        /// </summary>
        internal override void Invoke()
        {
            _methodAdvice.Advise(this);
        }
    }
}