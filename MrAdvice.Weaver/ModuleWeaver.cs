﻿#region Mr. Advice
// Mr. Advice
// A simple post build weaving package
// http://mradvice.arxone.com/
// Released under MIT license http://opensource.org/licenses/mit-license.php
#endregion

using System;
using ArxOne.MrAdvice.IO;
using ArxOne.MrAdvice.Weaver;
using Mono.Cecil;

/// <summary>
/// This is the entry point, automatically found by Fody
/// </summary>
// ReSharper disable once UnusedMember.Global
// ReSharper disable once CheckNamespace
public class ModuleWeaver
{
    /// <summary>
    /// Gets or sets the module definition (injected by Fody).
    /// </summary>
    /// <value>
    /// The module definition.
    /// </value>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public ModuleDefinition ModuleDefinition { get; set; }

    /// <summary>
    /// Gets or sets the logger at information level (injected by Fody).
    /// </summary>
    /// <value>
    /// The log information.
    /// </value>
    // ReSharper disable once MemberCanBePrivate.Global
    public Action<string> LogInfo { get; set; }
    /// <summary>
    /// Gets or sets the log at warning level (injected by Fody).
    /// </summary>
    /// <value>
    /// The log warning.
    /// </value>
    // ReSharper disable once MemberCanBePrivate.Global
    public Action<string> LogWarning { get; set; }
    /// <summary>
    /// Gets or sets the log at error level (injected by Fody).
    /// </summary>
    /// <value>
    /// The log warning.
    /// </value>
    // ReSharper disable once MemberCanBePrivate.Global
    public Action<string> LogError { get; set; }

    /// <summary>
    /// Gets or sets the assembly resolver (injected by Fody).
    /// </summary>
    /// <value>
    /// The assembly resolver.
    /// </value>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public IAssemblyResolver AssemblyResolver { get; set; }

    /// <summary>
    /// Gets or sets the assembly path.
    /// For some unkown reason, Fody won't weave on some platforms (Vista + VS2010) if this property is missing
    /// </summary>
    /// <value>
    /// The assembly path.
    /// </value>
    // ReSharper disable once UnusedMember.Global
    public string AssemblyPath { get; set; }

    public ModuleWeaver()
    {
        LogInfo = m => { };
        LogWarning = m => { };
    }

    /// <summary>
    /// Executes this instance.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public void Execute()
    {
        // instances are created here
        // please also note poor man's dependency injection (which is enough for us here)
        Logger.LogInfo = LogInfo;
        Logger.LogWarning = LogWarning;
        Logger.LogError = LogError;
        var typeResolver = new TypeResolver { AssemblyResolver = AssemblyResolver };
        var aspectWeaver = new AspectWeaver { TypeResolver = typeResolver };
        aspectWeaver.Weave(ModuleDefinition);
    }
}
