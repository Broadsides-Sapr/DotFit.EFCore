// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore.Query;

/// <summary>
///     This is an experimental API used by the Entity Framework Core feature and it is not subject to
///     the same compatibility standards as public APIs. It may be changed or removed without notice in
///     any release. You should only use it directly in your code with extreme caution and knowing that
///     doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
[Experimental(EFDiagnostics.PrecompiledQueryExperimental)]
public class RelationalLiftableConstantFactory(
    LiftableConstantExpressionDependencies dependencies,
    RelationalLiftableConstantExpressionDependencies relationalDependencies) : LiftableConstantFactory(dependencies), IRelationalLiftableConstantFactory
{
    /// <summary>
    ///     This is an experimental API used by the Entity Framework Core feature and it is not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual RelationalLiftableConstantExpressionDependencies RelationalDependencies { get; } = relationalDependencies;

    /// <summary>
    ///     This is an experimental API used by the Entity Framework Core feature and it is not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual LiftableConstantExpression CreateLiftableConstant(
        object? originalValue,
        Expression<Func<RelationalMaterializerLiftableConstantContext, object>> resolverExpression,
        string variableName,
        Type type)
        => new(originalValue, resolverExpression, variableName, type);
}
