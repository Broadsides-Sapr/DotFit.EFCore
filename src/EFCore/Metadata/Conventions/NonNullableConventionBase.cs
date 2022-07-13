// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

/// <summary>
///     A base type for conventions that configure model aspects based on whether the member type
///     is a non-nullable reference type.
/// </summary>
/// <remarks>
///     See <see href="https://aka.ms/efcore-docs-conventions">Model building conventions</see> for more information and examples.
/// </remarks>
public abstract class NonNullableConventionBase : IModelFinalizingConvention
{
    private const string StateAnnotationName = "NonNullableConventionState";

    /// <summary>
    ///     Creates a new instance of <see cref="NonNullableConventionBase" />.
    /// </summary>
    /// <param name="dependencies">Parameter object containing dependencies for this convention.</param>
    protected NonNullableConventionBase(ProviderConventionSetBuilderDependencies dependencies)
    {
        Dependencies = dependencies;
    }

    /// <summary>
    ///     Dependencies for this service.
    /// </summary>
    protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

    /// <summary>
    ///     Returns a value indicating whether the member type is a non-nullable reference type.
    /// </summary>
    /// <param name="modelBuilder">The model builder used to build the model.</param>
    /// <param name="memberInfo">The member info.</param>
    /// <returns><see langword="true" /> if the member type is a non-nullable reference type.</returns>
    protected virtual bool IsNonNullableReferenceType(
        IConventionModelBuilder modelBuilder,
        MemberInfo memberInfo)
    {
        if (memberInfo.GetMemberType().IsValueType)
        {
            return false;
        }

        var annotation =
            modelBuilder.Metadata.FindAnnotation(StateAnnotationName)
            ?? modelBuilder.Metadata.AddAnnotation(StateAnnotationName, new NullabilityInfoContext());

        var nullabilityInfoContext = (NullabilityInfoContext)annotation.Value!;

        var nullabilityInfo = memberInfo switch
        {
            PropertyInfo propertyInfo => nullabilityInfoContext.Create(propertyInfo),
            FieldInfo fieldInfo => nullabilityInfoContext.Create(fieldInfo),
            _ => null
        };

        return nullabilityInfo?.ReadState == NullabilityState.NotNull;
    }

    /// <inheritdoc />
    public virtual void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context)
        => modelBuilder.Metadata.RemoveAnnotation(StateAnnotationName);
}
