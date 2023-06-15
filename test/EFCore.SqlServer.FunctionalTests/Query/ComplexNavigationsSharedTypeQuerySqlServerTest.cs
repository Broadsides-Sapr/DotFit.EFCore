// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit.Sdk;

namespace Microsoft.EntityFrameworkCore.Query;

public class ComplexNavigationsSharedTypeQuerySqlServerTest :
    ComplexNavigationsSharedTypeQueryRelationalTestBase<ComplexNavigationsSharedTypeQuerySqlServerFixture>
{
    public ComplexNavigationsSharedTypeQuerySqlServerTest(
        ComplexNavigationsSharedTypeQuerySqlServerFixture fixture,
        ITestOutputHelper testOutputHelper)
        : base(fixture)
    {
        Fixture.TestSqlLoggerFactory.Clear();
        Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
    }

    [ConditionalFact]
    public virtual void Check_all_tests_overridden()
        => TestHelpers.AssertAllMethodsOverridden(GetType());

    public override async Task Simple_level1_include(bool async)
    {
        await base.Simple_level1_include(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
""");
    }

    public override async Task Simple_level1(bool async)
    {
        await base.Simple_level1(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
""");
    }

    public override async Task Simple_level1_level2_include(bool async)
    {
        await base.Simple_level1_level2_include(async);
        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
""");
    }

    public override async Task Simple_level1_level2_GroupBy_Count(bool async)
    {
        await base.Simple_level1_level2_GroupBy_Count(async);

        AssertSql(
"""
SELECT COUNT(*)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
GROUP BY [t0].[Level3_Name]
""");
    }

    public override async Task Simple_level1_level2_GroupBy_Having_Count(bool async)
    {
        await base.Simple_level1_level2_GroupBy_Having_Count(async);

        AssertSql(
"""
SELECT COUNT(*)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
GROUP BY [t0].[Level3_Name]
HAVING (
    SELECT MIN(COALESCE(CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END, 0))
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l2].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Optional_Inverse3Id], [l4].[OneToMany_Required_Inverse3Id], [l4].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t2] ON CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END = CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l5].[Id], [l5].[OneToOne_Required_PK_Date], [l5].[Level1_Optional_Id], [l5].[Level1_Required_Id], [l5].[Level2_Name], [l5].[OneToMany_Optional_Inverse2Id], [l5].[OneToMany_Required_Inverse2Id], [l5].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l5]
        WHERE [l5].[OneToOne_Required_PK_Date] IS NOT NULL AND [l5].[Level1_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l2].[Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t0].[Level3_Name] = [t2].[Level3_Name] OR ([t0].[Level3_Name] IS NULL AND [t2].[Level3_Name] IS NULL)) > 0
""");
    }

    public override async Task Simple_level1_level2_level3_include(bool async)
    {
        await base.Simple_level1_level2_level3_include(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [t1].[Id], [t1].[Level3_Optional_Id], [t1].[Level3_Required_Id], [t1].[Level4_Name], [t1].[OneToMany_Optional_Inverse4Id], [t1].[OneToMany_Required_Inverse4Id], [t1].[OneToOne_Optional_PK_Inverse4Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Optional_Id], [l2].[Level3_Required_Id], [l2].[Level4_Name], [l2].[OneToMany_Optional_Inverse4Id], [l2].[OneToMany_Required_Inverse4Id], [l2].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = CASE
    WHEN [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t1].[Id]
END
""");
    }

    public override async Task Nested_group_join_with_take(bool async)
    {
        await base.Nested_group_join_with_take(async);

        AssertSql(
"""
@__p_0='2'

SELECT [t2].[Level2_Name]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [t0].[Id0] AS [Id00], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Required_Id], [t0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
    ORDER BY [l].[Id]
) AS [t1]
LEFT JOIN (
    SELECT [t3].[Level1_Optional_Id], [t3].[Level2_Name]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l2].[Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id00]
END = [t2].[Level1_Optional_Id]
ORDER BY [t1].[Id]
""");
    }

    public override async Task Explicit_GroupJoin_in_subquery_with_unrelated_projection2(bool async)
    {
        await base.Explicit_GroupJoin_in_subquery_with_unrelated_projection2(async);

        AssertSql(
"""
SELECT [t1].[Id]
FROM (
    SELECT DISTINCT [l].[Id], [l].[Date], [l].[Name]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
    WHERE [t0].[Level2_Name] <> N'Foo' OR [t0].[Level2_Name] IS NULL
) AS [t1]
""");
    }

    public override async Task Result_operator_nav_prop_reference_optional_via_DefaultIfEmpty(bool async)
    {
        await base.Result_operator_nav_prop_reference_optional_via_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT COALESCE(SUM(CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NULL OR [t0].[Level1_Required_Id] IS NULL OR [t0].[OneToMany_Required_Inverse2Id] IS NULL THEN 0
    ELSE [t0].[Level1_Required_Id]
END), 0)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task Complex_query_with_optional_navigations_and_client_side_evaluation(bool async)
    {
        await base.Complex_query_with_optional_navigations_and_client_side_evaluation(async);

        AssertSql();
    }

    public override async Task Member_over_null_check_ternary_and_nested_anonymous_type(bool async)
    {
        await base.Member_over_null_check_ternary_and_nested_anonymous_type(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Name], CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NULL OR [t].[Level1_Required_Id] IS NULL OR [t].[OneToMany_Required_Inverse2Id] IS NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END, CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END, [t].[Level2_Name], CASE
    WHEN [t0].[Level2_Required_Id] IS NULL OR [t0].[OneToMany_Required_Inverse3Id] IS NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END, CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END, [t0].[Level3_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NULL OR [t].[Level1_Required_Id] IS NULL OR [t].[OneToMany_Required_Inverse2Id] IS NULL THEN NULL
    ELSE [t0].[Level3_Name]
END <> N'L' OR CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NULL OR [t].[Level1_Required_Id] IS NULL OR [t].[OneToMany_Required_Inverse2Id] IS NULL THEN NULL
    ELSE [t0].[Level3_Name]
END IS NULL
""");
    }

    public override async Task Nested_SelectMany_correlated_with_join_table_correctly_translated_to_apply(bool async)
    {
        // DefaultIfEmpty on child collection. Issue #19095.
        await Assert.ThrowsAsync<EqualException>(
            async () => await base.Nested_SelectMany_correlated_with_join_table_correctly_translated_to_apply(async));

        AssertSql(
"""
SELECT [t2].[l1Name], [t2].[l2Name], [t2].[l3Name]
FROM [Level1] AS [l]
OUTER APPLY (
    SELECT [t0].[l1Name], [t0].[l2Name], [t0].[l3Name]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t] ON CASE
        WHEN [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l0].[Id]
    END = CASE
        WHEN [t].[Level2_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t].[Id]
    END
    CROSS APPLY (
        SELECT [l].[Name] AS [l1Name], [t].[Level3_Name] AS [l2Name], [t1].[Level3_Name] AS [l3Name]
        FROM [Level1] AS [l2]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[Level2_Required_Id], [l3].[Level3_Name], [l3].[OneToMany_Required_Inverse3Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[Level2_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t1] ON [l2].[OneToOne_Optional_PK_Inverse4Id] = CASE
            WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
        END
        WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL AND CASE
            WHEN [t].[Level2_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t].[Id]
        END IS NOT NULL AND (CASE
            WHEN [t].[Level2_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t].[Id]
        END = [l2].[OneToMany_Optional_Inverse4Id] OR (CASE
            WHEN [t].[Level2_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t].[Id]
        END IS NULL AND [l2].[OneToMany_Optional_Inverse4Id] IS NULL))
    ) AS [t0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l0].[OneToMany_Optional_Inverse2Id]
) AS [t2]
""");
    }

    public override async Task OrderBy_collection_count_ThenBy_reference_navigation(bool async)
    {
        await base.OrderBy_collection_count_ThenBy_reference_navigation(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[Level3_Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
ORDER BY (
    SELECT COUNT(*)
    FROM [Level1] AS [l2]
    WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = [l2].[OneToMany_Required_Inverse3Id]), [t0].[Level3_Name]
""");
    }

    public override async Task Element_selector_with_coalesce_repeated_in_aggregate(bool async)
    {
        await base.Element_selector_with_coalesce_repeated_in_aggregate(async);

        AssertSql(
"""
SELECT COUNT(*)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
GROUP BY [t0].[Level3_Name]
HAVING (
    SELECT MIN(COALESCE(CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END, 0) + COALESCE(CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END, 0))
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l2].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Optional_Inverse3Id], [l4].[OneToMany_Required_Inverse3Id], [l4].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t2] ON CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END = CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l5].[Id], [l5].[OneToOne_Required_PK_Date], [l5].[Level1_Optional_Id], [l5].[Level1_Required_Id], [l5].[Level2_Name], [l5].[OneToMany_Optional_Inverse2Id], [l5].[OneToMany_Required_Inverse2Id], [l5].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l5]
        WHERE [l5].[OneToOne_Required_PK_Date] IS NOT NULL AND [l5].[Level1_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l2].[Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t0].[Level3_Name] = [t2].[Level3_Name] OR ([t0].[Level3_Name] IS NULL AND [t2].[Level3_Name] IS NULL)) > 0
""");
    }

    public override async Task Sum_with_selector_cast_using_as(bool async)
    {
        await base.Sum_with_selector_cast_using_as(async);

        AssertSql(
"""
SELECT COALESCE(SUM([l].[Id]), 0)
FROM [Level1] AS [l]
""");
    }

    public override async Task Sum_with_filter_with_include_selector_cast_using_as(bool async)
    {
        await base.Sum_with_filter_with_include_selector_cast_using_as(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
WHERE [l].[Id] > (
    SELECT COALESCE(SUM(CASE
        WHEN [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l0].[Id]
    END), 0)
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l0].[OneToMany_Optional_Inverse2Id])
""");
    }

    public override async Task Distinct_take_without_orderby(bool async)
    {
        await base.Distinct_take_without_orderby(async);

        AssertSql(
"""
SELECT (
    SELECT TOP(1) [t1].[Level3_Name]
    FROM (
        SELECT DISTINCT TOP(1) [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        LEFT JOIN (
            SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l2]
            WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t0] ON CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END = CASE
            WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t1]
    ORDER BY CASE
        WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
    END)
FROM [Level1] AS [l]
WHERE [l].[Id] < 3
""");
    }

    public override async Task Let_let_contains_from_outer_let(bool async)
    {
        await base.Let_let_contains_from_outer_let(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t1].[Id0], [t1].[Id1], [t1].[Id], [t2].[Id], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id], [t1].[Level2_Optional_Id], [t1].[Level2_Required_Id], [t1].[Level3_Name], [t1].[OneToMany_Optional_Inverse3Id], [t1].[OneToMany_Required_Inverse3Id], [t1].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
OUTER APPLY (
    SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [l0].[Id] AS [Id0], [t].[Id] AS [Id1]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t0] ON CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND EXISTS (
        SELECT 1
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l3].[OneToMany_Required_Inverse2Id] AND (CASE
            WHEN [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l3].[Id]
        END = [t0].[Level2_Required_Id] OR (CASE
            WHEN [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l3].[Id]
        END IS NULL AND [t0].[Level2_Required_Id] IS NULL)))
) AS [t1]
LEFT JOIN (
    SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l4]
    WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON [l].[Id] = [t2].[OneToMany_Required_Inverse2Id]
ORDER BY [l].[Id], [t1].[Id0], [t1].[Id1], [t1].[Id]
""");
    }

    public override async Task Null_check_different_structure_does_not_remove_null_checks(bool async)
    {
        await base.Null_check_different_structure_does_not_remove_null_checks(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
LEFT JOIN (
    SELECT [l2].[Level3_Optional_Id], [l2].[Level4_Name]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level3_Optional_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NULL OR [t].[Level1_Required_Id] IS NULL OR [t].[OneToMany_Required_Inverse2Id] IS NULL THEN NULL
    WHEN [t0].[Level2_Required_Id] IS NULL OR [t0].[OneToMany_Required_Inverse3Id] IS NULL THEN NULL
    ELSE [t1].[Level4_Name]
END = N'L4 01'
""");
    }

    public override async Task Null_conditional_is_not_applied_explicitly_for_optional_navigation(bool async)
    {
        await base.Null_conditional_is_not_applied_explicitly_for_optional_navigation(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t].[Level2_Name] = N'L2 01'
""");
    }

    public override async Task Multiple_conditionals_in_projection(bool async)
    {
        await base.Multiple_conditionals_in_projection(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END, [t0].[Level3_Name], CASE
    WHEN [l2].[Id] IS NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Level2_Optional_Id], [l1].[Level3_Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
LEFT JOIN [Level1] AS [l2] ON [t].[Level1_Optional_Id] = [l2].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Multiple_joins_groupby_predicate(bool async)
    {
        await base.Multiple_joins_groupby_predicate(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Name], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NULL OR [t0].[Level1_Required_Id] IS NULL OR [t0].[OneToMany_Required_Inverse2Id] IS NULL THEN N'Foo'
    ELSE N'Bar'
END AS [Foo]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [t3].[Level3_Name] AS [Key], COUNT(*) AS [Count]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL
    GROUP BY [t3].[Level3_Name]
) AS [t1] ON [l].[Name] = [t1].[Key]
WHERE [t0].[Level2_Name] IS NOT NULL OR [t1].[Count] > 0
""");
    }

    public override async Task Nested_object_constructed_from_group_key_properties(bool async)
    {
        await base.Nested_object_constructed_from_group_key_properties(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[Name], [t1].[Date], [t1].[InnerId] AS [Id], [t1].[Level2_Name0] AS [Name], [t1].[OneToOne_Required_PK_Date] AS [Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], COALESCE(SUM(CAST(LEN([t1].[Name]) AS int)), 0) AS [Aggregate]
FROM (
    SELECT [l].[Id], [l].[Date], [l].[Name], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t0].[Level2_Name] AS [Level2_Name0], CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END AS [InnerId]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
    LEFT JOIN (
        SELECT [l1].[Level1_Required_Id], [l1].[Level2_Name]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Required_Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1]
GROUP BY [t1].[Id], [t1].[Date], [t1].[Name], [t1].[InnerId], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name0]
""");
    }

    public override async Task Contains_over_optional_navigation_with_null_parameter(bool async)
    {
        await base.Contains_over_optional_navigation_with_null_parameter(async);

        AssertSql(
"""
SELECT CASE
    WHEN EXISTS (
        SELECT 1
        FROM [Level1] AS [l]
        LEFT JOIN (
            SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l0]
            WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
        WHERE CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END IS NULL) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END
""");
    }

    public override async Task Member_over_null_check_ternary_and_nested_dto_type(bool async)
    {
        await base.Member_over_null_check_ternary_and_nested_dto_type(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Name], CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NULL OR [t].[Level1_Required_Id] IS NULL OR [t].[OneToMany_Required_Inverse2Id] IS NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END, CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END, [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
ORDER BY [t].[Level2_Name], [l].[Id]
""");
    }

    public override async Task Select_with_joined_where_clause_cast_using_as(bool async)
    {
        await base.Select_with_joined_where_clause_cast_using_as(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
""");
    }

    public override async Task Composite_key_join_on_groupby_aggregate_projecting_only_grouping_key(bool async)
    {
        await base.Composite_key_join_on_groupby_aggregate_projecting_only_grouping_key(async);

        AssertSql(
"""
SELECT [t1].[Key]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t0].[Key], (
        SELECT COALESCE(SUM(CASE
            WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
        END), 0)
        FROM (
            SELECT [l1].[Id], [l1].[Date], [l1].[Name], [t4].[Id] AS [Id0], [t4].[OneToOne_Required_PK_Date], [t4].[Level1_Optional_Id], [t4].[Level1_Required_Id], [t4].[Level2_Name], [t4].[OneToMany_Optional_Inverse2Id], [t4].[OneToMany_Required_Inverse2Id], [t4].[OneToOne_Optional_PK_Inverse2Id], CASE
                WHEN [t4].[OneToOne_Required_PK_Date] IS NOT NULL AND [t4].[Level1_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t4].[Id]
            END % 3 AS [Key]
            FROM [Level1] AS [l1]
            LEFT JOIN (
                SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
                FROM [Level1] AS [l2]
                WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
            ) AS [t4] ON [l1].[Id] = CASE
                WHEN [t4].[OneToOne_Required_PK_Date] IS NOT NULL AND [t4].[Level1_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t4].[Id]
            END
            WHERE [t4].[OneToOne_Required_PK_Date] IS NOT NULL AND [t4].[Level1_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t2]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t3] ON [t2].[Id] = CASE
            WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
        END
        WHERE [t0].[Key] = [t2].[Key] OR ([t0].[Key] IS NULL AND [t2].[Key] IS NULL)) AS [Sum]
    FROM (
        SELECT CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END % 3 AS [Key]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Required_Id], [l4].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l4]
            WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0]
    GROUP BY [t0].[Key]
) AS [t1] ON [l].[Id] = [t1].[Key] AND CAST(1 AS bit) = CASE
    WHEN [t1].[Sum] > 10 THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END
""");
    }

    public override async Task Contains_over_optional_navigation_with_null_entity_reference(bool async)
    {
        await base.Contains_over_optional_navigation_with_null_entity_reference(async);

        AssertSql(
"""
SELECT [l].[Name], [t].[Level2_Name] AS [OptionalName], CASE
    WHEN EXISTS (
        SELECT 1
        FROM [Level1] AS [l2]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t1] ON [l2].[Id] = [t1].[Level1_Optional_Id]
        WHERE CASE
            WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
        END = CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END OR (CASE
            WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
        END IS NULL AND CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END IS NULL)) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END AS [Contains]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[OneToOne_Optional_PK_Inverse2Id]
""");
    }

    [ConditionalTheory(Skip = "Issue #26104")]
    public override async Task GroupBy_aggregate_where_required_relationship_2(bool async)
    {
        await base.GroupBy_aggregate_where_required_relationship_2(async);

        AssertSql(
"""
SELECT [l2].[Id] AS [Key], MAX(CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END) AS [Max]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    INNER JOIN [Level1] AS [l1] ON [l0].[Id] = [l1].[Id]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l2] ON [t].[OneToMany_Required_Inverse2Id] = [l2].[Id]
LEFT JOIN (
    SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l3]
    INNER JOIN [Level1] AS [l4] ON [l3].[Id] = [l4].[Id]
    WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
GROUP BY [l2].[Id]
HAVING MAX(CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END) < 2 OR MAX(CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END) > 2
""");
    }

    public override async Task Including_reference_navigation_and_projecting_collection_navigation_2(bool async)
    {
        await base.Including_reference_navigation_and_projecting_collection_navigation_2(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[OneToMany_Required_Inverse2Id]
LEFT JOIN (
    SELECT [t2].[Id], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id]
    FROM (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id], ROW_NUMBER() OVER(PARTITION BY [l2].[OneToMany_Required_Inverse2Id] ORDER BY CASE
            WHEN [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l2].[Id]
        END DESC) AS [row]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2]
    WHERE [t2].[row] <= 1
) AS [t1] ON [l].[Id] = [t1].[OneToMany_Required_Inverse2Id]
ORDER BY [l].[Id], [t].[Id]
""");
    }

    public override async Task Union_over_entities_with_different_nullability(bool async)
    {
        await base.Union_over_entities_with_different_nullability(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[Date], [l0].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
UNION ALL
SELECT [l3].[Id]
FROM [Level1] AS [l2]
LEFT JOIN (
    SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l4]
    WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON [l2].[Id] = CASE
    WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t2].[Level1_Optional_Id] = [l3].[Id]
WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l3].[Id] IS NULL
""");
    }

    public override async Task Distinct_skip_without_orderby(bool async)
    {
        await base.Distinct_skip_without_orderby(async);

        AssertSql(
"""
SELECT (
    SELECT TOP(1) [t2].[Level3_Name]
    FROM (
        SELECT [t1].[Id], [t1].[Level2_Optional_Id], [t1].[Level2_Required_Id], [t1].[Level3_Name], [t1].[OneToMany_Optional_Inverse3Id], [t1].[OneToMany_Required_Inverse3Id], [t1].[OneToOne_Optional_PK_Inverse3Id]
        FROM (
            SELECT DISTINCT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l0]
            LEFT JOIN (
                SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
                FROM [Level1] AS [l1]
                WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
            ) AS [t] ON [l0].[Id] = CASE
                WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
            END
            LEFT JOIN (
                SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
                FROM [Level1] AS [l2]
                WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
            ) AS [t0] ON CASE
                WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
            END = CASE
                WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
            END
            WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t1]
        ORDER BY (SELECT 1)
        OFFSET 1 ROWS
    ) AS [t2]
    ORDER BY CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END)
FROM [Level1] AS [l]
WHERE [l].[Id] < 3
""");
    }

    [ConditionalTheory(Skip = "Issue #26104")]
    public override async Task GroupBy_aggregate_where_required_relationship(bool async)
    {
        await base.GroupBy_aggregate_where_required_relationship(async);

        AssertSql(
"""
SELECT [l2].[Id] AS [Key], MAX(CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END) AS [Max]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    INNER JOIN [Level1] AS [l1] ON [l0].[Id] = [l1].[Id]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l2] ON [t].[OneToMany_Required_Inverse2Id] = [l2].[Id]
LEFT JOIN (
    SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l3]
    INNER JOIN [Level1] AS [l4] ON [l3].[Id] = [l4].[Id]
    WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
GROUP BY [l2].[Id]
HAVING MAX(CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END) <> 2 OR MAX(CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END) IS NULL
""");
    }

    public override async Task Contains_over_optional_navigation_with_null_column(bool async)
    {
        await base.Contains_over_optional_navigation_with_null_column(async);

        AssertSql(
"""
SELECT [l].[Name], [t].[Level2_Name] AS [OptionalName], CASE
    WHEN EXISTS (
        SELECT 1
        FROM [Level1] AS [l1]
        LEFT JOIN (
            SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l2]
            WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l1].[Id] = [t0].[Level1_Optional_Id]
        WHERE [t0].[Level2_Name] = [t].[Level2_Name] OR ([t0].[Level2_Name] IS NULL AND [t].[Level2_Name] IS NULL)) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END AS [Contains]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Collection_FirstOrDefault_property_accesses_in_projection(bool async)
    {
        await base.Collection_FirstOrDefault_property_accesses_in_projection(async);

        AssertSql(
"""
SELECT [l].[Id], (
    SELECT TOP(1) [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l0].[OneToMany_Optional_Inverse2Id] AND [l0].[Level2_Name] = N'L2 02') AS [Pushdown]
FROM [Level1] AS [l]
WHERE [l].[Id] < 3
""");
    }

    public override async Task SelectMany_with_outside_reference_to_joined_table_correctly_translated_to_apply(bool async)
    {
        await base.SelectMany_with_outside_reference_to_joined_table_correctly_translated_to_apply(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Required_Id]
INNER JOIN (
    SELECT [t3].[Id] AS [Id1], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END = [t1].[Level2_Required_Id]
INNER JOIN (
    SELECT [t7].[Level3_Required_Id], [t7].[Level4_Name]
    FROM [Level1] AS [l5]
    LEFT JOIN (
        SELECT [l6].[Id], [l6].[OneToOne_Required_PK_Date], [l6].[Level1_Required_Id], [l6].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l6]
        WHERE [l6].[OneToOne_Required_PK_Date] IS NOT NULL AND [l6].[Level1_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t5] ON [l5].[Id] = CASE
        WHEN [t5].[OneToOne_Required_PK_Date] IS NOT NULL AND [t5].[Level1_Required_Id] IS NOT NULL AND [t5].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t5].[Id]
    END
    LEFT JOIN (
        SELECT [l7].[Id], [l7].[Level2_Required_Id], [l7].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l7]
        WHERE [l7].[Level2_Required_Id] IS NOT NULL AND [l7].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t6] ON CASE
        WHEN [t5].[OneToOne_Required_PK_Date] IS NOT NULL AND [t5].[Level1_Required_Id] IS NOT NULL AND [t5].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t5].[Id]
    END = CASE
        WHEN [t6].[Level2_Required_Id] IS NOT NULL AND [t6].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t6].[Id]
    END
    LEFT JOIN (
        SELECT [l8].[Id], [l8].[Level3_Required_Id], [l8].[Level4_Name], [l8].[OneToMany_Required_Inverse4Id]
        FROM [Level1] AS [l8]
        WHERE [l8].[Level3_Required_Id] IS NOT NULL AND [l8].[OneToMany_Required_Inverse4Id] IS NOT NULL
    ) AS [t7] ON CASE
        WHEN [t6].[Level2_Required_Id] IS NOT NULL AND [t6].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t6].[Id]
    END = CASE
        WHEN [t7].[Level3_Required_Id] IS NOT NULL AND [t7].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t7].[Id]
    END
    WHERE [t5].[OneToOne_Required_PK_Date] IS NOT NULL AND [t5].[Level1_Required_Id] IS NOT NULL AND [t5].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t6].[Level2_Required_Id] IS NOT NULL AND [t6].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [t7].[Level3_Required_Id] IS NOT NULL AND [t7].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t4] ON CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END = [t4].[Level3_Required_Id]
LEFT JOIN [Level1] AS [l9] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END >= [l9].[Id] AND ([t4].[Level4_Name] = [l9].[Name] OR ([t4].[Level4_Name] IS NULL AND [l9].[Name] IS NULL))
""");
    }

    public override async Task Contains_over_optional_navigation_with_null_constant(bool async)
    {
        await base.Contains_over_optional_navigation_with_null_constant(async);

        AssertSql(
"""
SELECT CASE
    WHEN EXISTS (
        SELECT 1
        FROM [Level1] AS [l]
        LEFT JOIN (
            SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l0]
            WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
        WHERE CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END IS NULL) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END
""");
    }

    public override async Task Join_navigation_self_ref(bool async)
    {
        await base.Join_navigation_self_ref(async);

        AssertSql();
    }

    public override async Task Join_condition_optimizations_applied_correctly_when_anonymous_type_with_multiple_properties(bool async)
    {
        await base.Join_condition_optimizations_applied_correctly_when_anonymous_type_with_multiple_properties(async);

        AssertSql();
    }

    public override async Task Join_condition_optimizations_applied_correctly_when_anonymous_type_with_single_property(bool async)
    {
        await base.Join_condition_optimizations_applied_correctly_when_anonymous_type_with_single_property(async);

        AssertSql();
    }

    public override async Task Multiple_SelectMany_with_nested_navigations_and_explicit_DefaultIfEmpty_joined_together(bool async)
    {
        await base.Multiple_SelectMany_with_nested_navigations_and_explicit_DefaultIfEmpty_joined_together(async);

        AssertSql();
    }

    public override async Task SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany(
        bool async)
    {
        await base.SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany(async);

        AssertSql();
    }

    public override async Task SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany2(
        bool async)
    {
        await base.SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany2(async);

        AssertSql();
    }

    public override async Task SelectMany_with_nested_navigations_and_additional_joins_outside_of_SelectMany(bool async)
    {
        await base.SelectMany_with_nested_navigations_and_additional_joins_outside_of_SelectMany(async);

        AssertSql();
    }

    public override async Task Include8(bool async)
    {
        await base.Include8(async);

        AssertSql();
    }

    public override async Task Include9(bool async)
    {
        await base.Include9(async);

        AssertSql();
    }

    public override async Task Join_with_navigations_in_the_result_selector2(bool async)
    {
        await base.Join_with_navigations_in_the_result_selector2(async);

        AssertSql();
    }

    public override async Task Member_pushdown_chain_3_levels_deep(bool async)
    {
        await base.Member_pushdown_chain_3_levels_deep(async);

        AssertSql();
    }

    public override async Task Member_pushdown_chain_3_levels_deep_entity(bool async)
    {
        await base.Member_pushdown_chain_3_levels_deep_entity(async);

        AssertSql();
    }

    public override async Task Member_pushdown_with_collection_navigation_in_the_middle(bool async)
    {
        await base.Member_pushdown_with_collection_navigation_in_the_middle(async);

        AssertSql();
    }

    public override async Task Complex_query_with_let_collection_SelectMany(bool async)
    {
        await base.Complex_query_with_let_collection_SelectMany(async);

        AssertSql();
    }

    public override async Task Select_projecting_queryable_followed_by_SelectMany(bool async)
    {
        await base.Select_projecting_queryable_followed_by_SelectMany(async);

        AssertSql();
    }

    public override async Task Join_with_result_selector_returning_queryable_throws_validation_error(bool async)
    {
        // Expression cannot be used for return type. Issue #23302.
        await Assert.ThrowsAsync<ArgumentException>(
            () => base.Join_with_result_selector_returning_queryable_throws_validation_error(async));

        AssertSql();
    }

    public override async Task Select_projecting_queryable_followed_by_Join(bool async)
    {
        await base.Select_projecting_queryable_followed_by_Join(async);

        AssertSql();
    }

    public override async Task Select_projecting_queryable_in_anonymous_projection_followed_by_Join(bool async)
    {
        await base.Select_projecting_queryable_in_anonymous_projection_followed_by_Join(async);

        AssertSql();
    }

    public override async Task SelectMany_with_navigation_filter_and_explicit_DefaultIfEmpty(bool async)
    {
        await base.SelectMany_with_navigation_filter_and_explicit_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l0].[Id] > 5
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task SelectMany_with_nested_navigation_and_explicit_DefaultIfEmpty(bool async)
    {
        await base.SelectMany_with_nested_navigation_and_explicit_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
WHERE [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task SelectMany_with_nested_navigation_filter_and_explicit_DefaultIfEmpty(bool async)
    {
        await base.SelectMany_with_nested_navigation_filter_and_explicit_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [l1].[Id] > 5
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
WHERE [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Prune_does_not_throw_null_ref(bool async)
    {
        await base.Prune_does_not_throw_null_ref(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[Date], [t1].[Name]
FROM (
    SELECT NULL AS [empty]
) AS [e]
LEFT JOIN (
    SELECT [t].[Level1_Required_Id]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END < 5
) AS [t0] ON 1 = 1
CROSS APPLY (
    SELECT [l1].[Id], [l1].[Date], [l1].[Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Id] <> COALESCE([t0].[Level1_Required_Id], 0)
) AS [t1]
""");
    }

    public override async Task Projecting_columns_with_same_name_from_different_entities_making_sure_aliasing_works_after_Distinct(
        bool async)
    {
        await base.Projecting_columns_with_same_name_from_different_entities_making_sure_aliasing_works_after_Distinct(async);

        AssertSql(
"""
@__p_0='10'

SELECT [t4].[Id1] AS [Foo], [t4].[Id2] AS [Bar], [t4].[Id3] AS [Baz]
FROM (
    SELECT DISTINCT TOP(@__p_0) [l].[Id] AS [Id1], CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
    END AS [Id2], CASE
        WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
    END AS [Id3], [l].[Name] AS [Name1], [t0].[Level2_Name] AS [Name2]
    FROM [Level1] AS [l]
    INNER JOIN (
        SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
    INNER JOIN (
        SELECT [t3].[Id] AS [Id1], [t3].[Level2_Optional_Id], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l2]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t2] ON [l2].[Id] = CASE
            WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
        END
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
            FROM [Level1] AS [l4]
            WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t3] ON CASE
            WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
        END = CASE
            WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
        END
        WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t1] ON CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
    END = [t1].[Level2_Optional_Id]
) AS [t4]
""");
    }

    public override async Task Multiple_collection_FirstOrDefault_followed_by_member_access_in_projection(bool async)
    {
        await base.Multiple_collection_FirstOrDefault_followed_by_member_access_in_projection(async);

        AssertSql(
"""
SELECT [l].[Id], (
    SELECT TOP(1) [l0].[Level3_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[Level2_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND (
        SELECT TOP(1) CASE
            WHEN [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l1].[Id]
        END
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l1].[OneToMany_Optional_Inverse2Id] AND [l1].[Level2_Name] = N'L2 02') IS NOT NULL AND ((
        SELECT TOP(1) CASE
            WHEN [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l2].[Id]
        END
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l2].[OneToMany_Optional_Inverse2Id] AND [l2].[Level2_Name] = N'L2 02') = [l0].[OneToMany_Optional_Inverse3Id] OR ((
        SELECT TOP(1) CASE
            WHEN [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l2].[Id]
        END
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l2].[OneToMany_Optional_Inverse2Id] AND [l2].[Level2_Name] = N'L2 02') IS NULL AND [l0].[OneToMany_Optional_Inverse3Id] IS NULL))
    ORDER BY CASE
        WHEN [l0].[Level2_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [l0].[Id]
    END) AS [Pushdown]
FROM [Level1] AS [l]
WHERE [l].[Id] < 2
""");
    }

    public override async Task Collection_FirstOrDefault_entity_collection_accesses_in_projection(bool async)
    {
        await base.Collection_FirstOrDefault_entity_collection_accesses_in_projection(async);

        AssertSql(
"""
SELECT [l].[Id], [t0].[Id], [t1].[Id], [t1].[Level2_Optional_Id], [t1].[Level2_Required_Id], [t1].[Level3_Name], [t1].[OneToMany_Optional_Inverse3Id], [t1].[OneToMany_Required_Inverse3Id], [t1].[OneToOne_Optional_PK_Inverse3Id], [t0].[c]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[c], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToMany_Optional_Inverse2Id]
    FROM (
        SELECT 1 AS [c], [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToMany_Optional_Inverse2Id], ROW_NUMBER() OVER(PARTITION BY [l0].[OneToMany_Optional_Inverse2Id] ORDER BY [l0].[Id]) AS [row]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l0].[Level2_Name] = N'L2 02'
    ) AS [t]
    WHERE [t].[row] <= 1
) AS [t0] ON [l].[Id] = [t0].[OneToMany_Optional_Inverse2Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[OneToMany_Optional_Inverse3Id]
WHERE [l].[Id] < 2
ORDER BY [l].[Id], [t0].[Id]
""");
    }

    public override async Task Collection_FirstOrDefault_entity_reference_accesses_in_projection(bool async)
    {
        await base.Collection_FirstOrDefault_entity_reference_accesses_in_projection(async);

        AssertSql(
"""
SELECT [l].[Id], [t1].[Id], [t1].[Level2_Optional_Id], [t1].[Level2_Required_Id], [t1].[Level3_Name], [t1].[OneToMany_Optional_Inverse3Id], [t1].[OneToMany_Required_Inverse3Id], [t1].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [t0].[OneToMany_Optional_Inverse2Id]
    FROM (
        SELECT [t].[Id], [t].[Level2_Optional_Id], [t].[Level2_Required_Id], [t].[Level3_Name], [t].[OneToMany_Optional_Inverse3Id], [t].[OneToMany_Required_Inverse3Id], [t].[OneToOne_Optional_PK_Inverse3Id], [l0].[OneToMany_Optional_Inverse2Id], ROW_NUMBER() OVER(PARTITION BY [l0].[OneToMany_Optional_Inverse2Id] ORDER BY [l0].[Id], [t].[Id]) AS [row]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t] ON CASE
            WHEN [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l0].[Id]
        END = [t].[Level2_Optional_Id]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l0].[Level2_Name] = N'L2 02'
    ) AS [t0]
    WHERE [t0].[row] <= 1
) AS [t1] ON [l].[Id] = [t1].[OneToMany_Optional_Inverse2Id]
WHERE [l].[Id] < 3
""");
    }

    public override async Task SelectMany_without_collection_selector_returning_queryable(bool async)
    {
        await base.SelectMany_without_collection_selector_returning_queryable(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
CROSS JOIN (
    SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END < 10
) AS [t0]
""");
    }

    public override async Task Null_reference_protection_complex_materialization(bool async)
    {
        await base.Null_reference_protection_complex_materialization(async);

        AssertSql(
"""
SELECT [t1].[Id00], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [t2].[Id0] AS [Id00], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [t3].[Id] AS [Id0], [t3].[OneToOne_Required_PK_Date], [t3].[Level1_Optional_Id], [t3].[Level1_Required_Id], [t3].[Level2_Name], [t3].[OneToMany_Optional_Inverse2Id], [t3].[OneToMany_Required_Inverse2Id], [t3].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l3]
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l4]
            WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t3] ON [l3].[Id] = CASE
            WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
        END
        WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = [t2].[Level1_Optional_Id]
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id00]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany4(
        bool async)
    {
        await base.SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany4(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[Level3_Optional_Id], [t1].[Level3_Required_Id], [t1].[Level4_Name], [t1].[OneToMany_Optional_Inverse4Id], [t1].[OneToMany_Required_Inverse4Id], [t1].[OneToOne_Optional_PK_Inverse4Id], [t2].[Id0], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Optional_Id], [l2].[Level3_Required_Id], [l2].[Level4_Name], [l2].[OneToMany_Optional_Inverse4Id], [l2].[OneToMany_Required_Inverse4Id], [l2].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[OneToMany_Required_Inverse4Id]
LEFT JOIN (
    SELECT [t3].[Id] AS [Id0], [t3].[OneToOne_Required_PK_Date], [t3].[Level1_Optional_Id], [t3].[Level1_Required_Id], [t3].[Level2_Name], [t3].[OneToMany_Optional_Inverse2Id], [t3].[OneToMany_Required_Inverse2Id], [t3].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l3]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l3].[Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON CASE
    WHEN [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t1].[Id]
END = CASE
    WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id0]
END
""");
    }

    public override async Task Select_join_with_key_selector_being_a_subquery(bool async)
    {
        await base.Select_join_with_key_selector_being_a_subquery(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t0].[Id0], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = COALESCE((
    SELECT TOP(1) CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l2].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    WHERE [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END), 0)
""");
    }

    public override async Task GroupJoin_on_a_subquery_containing_another_GroupJoin_with_orderby_on_inner_sequence_projecting_inner(
        bool async)
    {
        await base.GroupJoin_on_a_subquery_containing_another_GroupJoin_with_orderby_on_inner_sequence_projecting_inner(async);

        AssertSql(
"""
@__p_0='2'

SELECT [l2].[Name]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [t0].[Level1_Optional_Id]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [t].[Level1_Optional_Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
    ORDER BY [l].[Id]
) AS [t1]
LEFT JOIN [Level1] AS [l2] ON [t1].[Level1_Optional_Id] = [l2].[Id]
ORDER BY [t1].[Id]
""");
    }

    public override async Task Null_reference_protection_complex(bool async)
    {
        await base.Null_reference_protection_complex(async);

        AssertSql(
"""
SELECT [t1].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [t2].[Id0] AS [Id00], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [t3].[Id] AS [Id0], [t3].[OneToOne_Required_PK_Date], [t3].[Level1_Optional_Id], [t3].[Level1_Required_Id], [t3].[Level2_Name], [t3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l4]
            WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t3] ON [l3].[Id] = CASE
            WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
        END
        WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = [t2].[Level1_Optional_Id]
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id00]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Contains_with_subquery_optional_navigation_and_constant_item(bool async)
    {
        await base.Contains_with_subquery_optional_navigation_and_constant_item(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE 1 IN (
    SELECT CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END
    FROM (
        SELECT DISTINCT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END IS NOT NULL AND (CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END = [l1].[OneToMany_Optional_Inverse3Id] OR (CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END IS NULL AND [l1].[OneToMany_Optional_Inverse3Id] IS NULL))
    ) AS [t0]
)
""");
    }

    public override async Task Null_reference_protection_complex_client_eval(bool async)
    {
        await base.Null_reference_protection_complex_client_eval(async);

        AssertSql(
"""
SELECT [t1].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [t2].[Id0] AS [Id00], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [t3].[Id] AS [Id0], [t3].[OneToOne_Required_PK_Date], [t3].[Level1_Optional_Id], [t3].[Level1_Required_Id], [t3].[Level2_Name], [t3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l4]
            WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t3] ON [l3].[Id] = CASE
            WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
        END
        WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = [t2].[Level1_Optional_Id]
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id00]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Multiple_SelectMany_with_navigation_and_explicit_DefaultIfEmpty(bool async)
    {
        await base.Multiple_SelectMany_with_navigation_and_explicit_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [l1].[Id] > 5
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
WHERE [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Contains_with_subquery_optional_navigation_scalar_distinct_and_constant_item(bool async)
    {
        await base.Contains_with_subquery_optional_navigation_scalar_distinct_and_constant_item(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE 1 IN (
    SELECT DISTINCT CAST(LEN([l1].[Level3_Name]) AS int)
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = [l1].[OneToMany_Optional_Inverse3Id] OR (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL AND [l1].[OneToMany_Optional_Inverse3Id] IS NULL))
)
""");
    }

    public override async Task GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened2(bool async)
    {
        await base.GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened2(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    INNER JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task SelectMany_with_nested_required_navigation_filter_and_explicit_DefaultIfEmpty(bool async)
    {
        await base.SelectMany_with_nested_required_navigation_filter_and_explicit_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [l1].[Id] > 5
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Required_Inverse3Id]
WHERE [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Required_navigation_on_a_subquery_with_complex_projection_and_First(bool async)
    {
        await base.Required_navigation_on_a_subquery_with_complex_projection_and_First(async);

        AssertSql(
"""
SELECT (
    SELECT TOP(1) [l3].[Name]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l1].[Id] = CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END
    INNER JOIN [Level1] AS [l2] ON [t0].[Level1_Required_Id] = [l2].[Id]
    LEFT JOIN [Level1] AS [l3] ON [t0].[Level1_Required_Id] = [l3].[Id]
    WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 7
""");
    }

    public override async Task SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany3(
        bool async)
    {
        await base.SelectMany_with_nested_navigations_explicit_DefaultIfEmpty_and_additional_joins_outside_of_SelectMany3(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[Level3_Optional_Id], [t1].[Level3_Required_Id], [t1].[Level4_Name], [t1].[OneToMany_Optional_Inverse4Id], [t1].[OneToMany_Required_Inverse4Id], [t1].[OneToOne_Optional_PK_Inverse4Id], [t2].[Id0], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Optional_Id], [l2].[Level3_Required_Id], [l2].[Level4_Name], [l2].[OneToMany_Optional_Inverse4Id], [l2].[OneToMany_Required_Inverse4Id], [l2].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[OneToMany_Required_Inverse4Id]
INNER JOIN (
    SELECT [t3].[Id] AS [Id0], [t3].[OneToOne_Required_PK_Date], [t3].[Level1_Optional_Id], [t3].[Level1_Required_Id], [t3].[Level2_Name], [t3].[OneToMany_Optional_Inverse2Id], [t3].[OneToMany_Required_Inverse2Id], [t3].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l3]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l3].[Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON CASE
    WHEN [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t1].[Id]
END = CASE
    WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id0]
END
""");
    }

    public override async Task Required_navigation_on_a_subquery_with_First_in_predicate(bool async)
    {
        await base.Required_navigation_on_a_subquery_with_First_in_predicate(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 7 AND (
    SELECT TOP(1) [l2].[Name]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l1].[Id] = CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END
    LEFT JOIN [Level1] AS [l2] ON [t0].[Level1_Required_Id] = [l2].[Id]
    WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END) = N'L1 02'
""");
    }

    public override async Task Manually_created_left_join_propagates_nullability_to_navigations(bool async)
    {
        await base.Manually_created_left_join_propagates_nullability_to_navigations(async);

        AssertSql(
"""
SELECT [l2].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Level1_Optional_Id], [t].[Level1_Required_Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
LEFT JOIN [Level1] AS [l2] ON [t0].[Level1_Required_Id] = [l2].[Id]
WHERE [l2].[Name] <> N'L3 02' OR [l2].[Name] IS NULL
""");
    }

    public override async Task
        SelectMany_with_nested_navigations_and_explicit_DefaultIfEmpty_followed_by_Select_required_navigation_using_different_navs(
            bool async)
    {
        await base
            .SelectMany_with_nested_navigations_and_explicit_DefaultIfEmpty_followed_by_Select_required_navigation_using_different_navs(
                async);

        AssertSql(
"""
SELECT [l3].[Id], [l3].[Date], [l3].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[OneToMany_Optional_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END = [l3].[Id]
""");
    }

    public override async Task Optional_navigation_propagates_nullability_to_manually_created_left_join1(bool async)
    {
        await base.Optional_navigation_propagates_nullability_to_manually_created_left_join1(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [t1].[Id] AS [Id0], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Required_Id], [t1].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l1].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    WHERE [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [t].[Level1_Required_Id] = CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
""");
    }

    public override async Task GroupJoin_on_left_side_being_a_subquery(bool async)
    {
        await base.GroupJoin_on_left_side_being_a_subquery(async);

        AssertSql(
"""
@__p_0='2'

SELECT TOP(@__p_0) [l].[Id], [t].[Level2_Name] AS [Brand]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
ORDER BY [t].[Level2_Name], [l].[Id]
""");
    }

    public override async Task
        SelectMany_with_nested_navigations_and_explicit_DefaultIfEmpty_followed_by_Select_required_navigation_using_same_navs(
            bool async)
    {
        await base
            .SelectMany_with_nested_navigations_and_explicit_DefaultIfEmpty_followed_by_Select_required_navigation_using_same_navs(
                async);

        AssertSql(
"""
SELECT [l7].[Id], [l7].[Date], [l7].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Required_Id], [l2].[OneToMany_Required_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = CASE
    WHEN [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN (
    SELECT [l3].[Id], [l3].[Level2_Required_Id], [l3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l3]
    WHERE [l3].[Level2_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t2] ON [t1].[Level3_Required_Id] = CASE
    WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
END
LEFT JOIN (
    SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Required_Id], [l4].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l4]
    WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t3] ON [t2].[Level2_Required_Id] = CASE
    WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
END
LEFT JOIN (
    SELECT [l5].[Level2_Required_Id], [l5].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l5]
    WHERE [l5].[Level2_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t4] ON CASE
    WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
END = [t4].[OneToMany_Required_Inverse3Id]
LEFT JOIN (
    SELECT [l6].[Id], [l6].[OneToOne_Required_PK_Date], [l6].[Level1_Required_Id], [l6].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l6]
    WHERE [l6].[OneToOne_Required_PK_Date] IS NOT NULL AND [l6].[Level1_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t5] ON [t4].[Level2_Required_Id] = CASE
    WHEN [t5].[OneToOne_Required_PK_Date] IS NOT NULL AND [t5].[Level1_Required_Id] IS NOT NULL AND [t5].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t5].[Id]
END
LEFT JOIN [Level1] AS [l7] ON CASE
    WHEN [t5].[OneToOne_Required_PK_Date] IS NOT NULL AND [t5].[Level1_Required_Id] IS NOT NULL AND [t5].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t5].[Id]
END = [l7].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL
""");
    }

    public override async Task Required_navigation_on_a_subquery_with_First_in_projection(bool async)
    {
        await base.Required_navigation_on_a_subquery_with_First_in_projection(async);

        AssertSql(
"""
SELECT (
    SELECT TOP(1) [l2].[Name]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l1].[Id] = CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END
    LEFT JOIN [Level1] AS [l2] ON [t0].[Level1_Required_Id] = [l2].[Id]
    WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 7
""");
    }

    public override async Task GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened(bool async)
    {
        await base.GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    INNER JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened3(bool async)
    {
        await base.GroupJoin_with_complex_subquery_with_joins_does_not_get_flattened3(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Required_Id]
""");
    }

    public override async Task GroupJoin_on_a_subquery_containing_another_GroupJoin_projecting_inner(bool async)
    {
        await base.GroupJoin_on_a_subquery_containing_another_GroupJoin_projecting_inner(async);

        AssertSql(
"""
@__p_0='2'

SELECT [l2].[Name]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [t0].[Level1_Optional_Id]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [t].[Level1_Optional_Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
    ORDER BY [l].[Id]
) AS [t1]
LEFT JOIN [Level1] AS [l2] ON [t1].[Level1_Optional_Id] = [l2].[Id]
ORDER BY [t1].[Id]
""");
    }

    public override async Task SelectMany_with_navigation_filter_paging_and_explicit_DefaultIfEmpty(bool async)
    {
        await base.SelectMany_with_navigation_filter_paging_and_explicit_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM (
        SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id], ROW_NUMBER() OVER(PARTITION BY [l0].[OneToMany_Required_Inverse2Id] ORDER BY CASE
            WHEN [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l0].[Id]
        END) AS [row]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l0].[Id] > 5
    ) AS [t]
    WHERE [t].[row] <= 3
) AS [t0] ON [l].[Id] = [t0].[OneToMany_Required_Inverse2Id]
WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Select_join_subquery_containing_filter_and_distinct(bool async)
    {
        await base.Select_join_subquery_containing_filter_and_distinct(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT DISTINCT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END > 2
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task Optional_navigation_propagates_nullability_to_manually_created_left_join2(bool async)
    {
        await base.Optional_navigation_propagates_nullability_to_manually_created_left_join2(async);

        AssertSql(
"""
SELECT [t0].[Level3_Name] AS [Name1], [t1].[Level2_Name] AS [Name2]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [t2].[Id] AS [Id0], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = [t2].[Level1_Optional_Id]
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id0]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task GroupJoin_on_a_subquery_containing_another_GroupJoin_projecting_outer(bool async)
    {
        await base.GroupJoin_on_a_subquery_containing_another_GroupJoin_projecting_outer(async);

        AssertSql(
"""
@__p_0='2'

SELECT [t2].[Level2_Name]
FROM (
    SELECT TOP(@__p_0) [l].[Id]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Id], [l0].[Date], [l0].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
    ORDER BY [l].[Id]
) AS [t1]
LEFT JOIN (
    SELECT [t3].[Level1_Optional_Id], [t3].[Level2_Name]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l2].[Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON [t1].[Id] = [t2].[Level1_Optional_Id]
ORDER BY [t1].[Id]
""");
    }

    public override async Task Where_multiple_nav_prop_optional_required(bool async)
    {
        await base.Where_multiple_nav_prop_optional_required(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[Level3_Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
WHERE [t0].[Level3_Name] <> N'L3 05' OR [t0].[Level3_Name] IS NULL
""");
    }

    public override async Task Join_navigation_non_key_join(bool async)
    {
        await base.Join_navigation_non_key_join(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END AS [Id2], [t].[Level2_Name] AS [Name2], [t0].[Id] AS [Id1], [t0].[Name] AS [Name1]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
INNER JOIN (
    SELECT [l1].[Id], [l1].[Name], [t1].[Level2_Name]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l2].[Level1_Optional_Id], [l2].[Level2_Name]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l1].[Id] = [t1].[Level1_Optional_Id]
) AS [t0] ON [t].[Level2_Name] = [t0].[Level2_Name]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Select_nav_prop_reference_optional3(bool async)
    {
        await base.Select_nav_prop_reference_optional3(async);

        AssertSql(
"""
SELECT [l1].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Optional_Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Include11(bool async)
    {
        await base.Include11(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [t1].[Id], [t1].[Level2_Optional_Id], [t1].[Level2_Required_Id], [t1].[Level3_Name], [t1].[OneToMany_Optional_Inverse3Id], [t1].[OneToMany_Required_Inverse3Id], [t1].[OneToOne_Optional_PK_Inverse3Id], [t2].[Id], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id], [t3].[Id], [t3].[Level2_Optional_Id], [t3].[Level2_Required_Id], [t3].[Level3_Name], [t3].[OneToMany_Optional_Inverse3Id], [t3].[OneToMany_Required_Inverse3Id], [t3].[OneToOne_Optional_PK_Inverse3Id], [t4].[Id], [t4].[Level3_Optional_Id], [t4].[Level3_Required_Id], [t4].[Level4_Name], [t4].[OneToMany_Optional_Inverse4Id], [t4].[OneToMany_Required_Inverse4Id], [t4].[OneToOne_Optional_PK_Inverse4Id], [t5].[Id], [t5].[Level3_Optional_Id], [t5].[Level3_Required_Id], [t5].[Level4_Name], [t5].[OneToMany_Optional_Inverse4Id], [t5].[OneToMany_Required_Inverse4Id], [t5].[OneToOne_Optional_PK_Inverse4Id], [t6].[Id], [t6].[Level2_Optional_Id], [t6].[Level2_Required_Id], [t6].[Level3_Name], [t6].[OneToMany_Optional_Inverse3Id], [t6].[OneToMany_Required_Inverse3Id], [t6].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t1].[OneToOne_Optional_PK_Inverse3Id]
LEFT JOIN (
    SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l3]
    WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON [l].[Id] = [t2].[OneToOne_Optional_PK_Inverse2Id]
LEFT JOIN (
    SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Optional_Inverse3Id], [l4].[OneToMany_Required_Inverse3Id], [l4].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l4]
    WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t3] ON CASE
    WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
END = [t3].[Level2_Optional_Id]
LEFT JOIN (
    SELECT [l5].[Id], [l5].[Level3_Optional_Id], [l5].[Level3_Required_Id], [l5].[Level4_Name], [l5].[OneToMany_Optional_Inverse4Id], [l5].[OneToMany_Required_Inverse4Id], [l5].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l5]
    WHERE [l5].[Level3_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t4] ON CASE
    WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
END = [t4].[Level3_Optional_Id]
LEFT JOIN (
    SELECT [l6].[Id], [l6].[Level3_Optional_Id], [l6].[Level3_Required_Id], [l6].[Level4_Name], [l6].[OneToMany_Optional_Inverse4Id], [l6].[OneToMany_Required_Inverse4Id], [l6].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l6]
    WHERE [l6].[Level3_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t5] ON CASE
    WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
END = [t5].[OneToOne_Optional_PK_Inverse4Id]
LEFT JOIN (
    SELECT [l7].[Id], [l7].[Level2_Optional_Id], [l7].[Level2_Required_Id], [l7].[Level3_Name], [l7].[OneToMany_Optional_Inverse3Id], [l7].[OneToMany_Required_Inverse3Id], [l7].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l7]
    WHERE [l7].[Level2_Required_Id] IS NOT NULL AND [l7].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t6] ON CASE
    WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
END = [t6].[OneToOne_Optional_PK_Inverse3Id]
""");
    }

    public override async Task Include18_3_3(bool async)
    {
        await base.Include18_3_3(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id], [t1].[Id], [t1].[Level2_Optional_Id], [t1].[Level2_Required_Id], [t1].[Level3_Name], [t1].[OneToMany_Optional_Inverse3Id], [t1].[OneToMany_Required_Inverse3Id], [t1].[OneToOne_Optional_PK_Inverse3Id]
FROM (
    SELECT DISTINCT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
) AS [t0]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level2_Optional_Id]
""");
    }

    public override async Task
        String_include_multiple_derived_collection_navigation_with_same_name_and_different_type_nested_also_includes_partially_matching_navigation_chains(
            bool async)
    {
        await base
            .String_include_multiple_derived_collection_navigation_with_same_name_and_different_type_nested_also_includes_partially_matching_navigation_chains(
                async);

        AssertSql(
"""
SELECT [i].[Id], [i].[Discriminator], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1], [i].[Name], [i0].[Id], [i0].[DifferentTypeCollection_InheritanceDerived1Id], [i0].[DifferentTypeReference_InheritanceDerived1Id], [i0].[InheritanceDerived2Id], [i0].[Name], [i0].[SameTypeCollection_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived2Id], [t].[Id], [t].[DifferentTypeReference_InheritanceDerived2Id], [t].[InheritanceDerived2Id], [t].[Name], [t].[Id0], [t].[InheritanceLeaf2Id], [t].[Name0]
FROM [InheritanceOne] AS [i]
LEFT JOIN [InheritanceLeafOne] AS [i0] ON [i].[Id] = [i0].[DifferentTypeCollection_InheritanceDerived1Id]
LEFT JOIN (
    SELECT [i1].[Id], [i1].[DifferentTypeReference_InheritanceDerived2Id], [i1].[InheritanceDerived2Id], [i1].[Name], [i2].[Id] AS [Id0], [i2].[InheritanceLeaf2Id], [i2].[Name] AS [Name0]
    FROM [InheritanceLeafTwo] AS [i1]
    LEFT JOIN [InheritanceTwo] AS [i2] ON [i1].[Id] = [i2].[InheritanceLeaf2Id]
) AS [t] ON [i].[Id] = [t].[InheritanceDerived2Id]
ORDER BY [i].[Id], [i0].[Id], [t].[Id]
""");
    }

    public override async Task Join_navigation_nested(bool async)
    {
        await base.Join_navigation_nested(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END AS [Id3], [t1].[Id] AS [Id1]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
INNER JOIN (
    SELECT [l2].[Id], [t3].[Id] AS [Id1], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = [t2].[Level1_Required_Id]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = [t3].[Level2_Optional_Id]
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Projection_select_correct_table_from_subquery_when_materialization_is_not_required(bool async)
    {
        await base.Projection_select_correct_table_from_subquery_when_materialization_is_not_required(async);

        AssertSql(
"""
@__p_0='3'

SELECT TOP(@__p_0) [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Name] = N'L1 03'
ORDER BY CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
""");
    }

    public override async Task Projection_select_correct_table_with_anonymous_projection_in_subquery(bool async)
    {
        await base.Projection_select_correct_table_with_anonymous_projection_in_subquery(async);

        AssertSql(
"""
@__p_0='3'

SELECT TOP(@__p_0) [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
INNER JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
INNER JOIN (
    SELECT [t2].[Level2_Required_Id], [t2].[Level3_Name]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l2].[Id] = CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t2] ON CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END = CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END
    WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON [l1].[Id] = [t1].[Level2_Required_Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Name] = N'L1 03' AND [t1].[Level3_Name] = N'L3 08'
ORDER BY [l1].[Id]
""");
    }

    public override async Task Include14(bool async)
    {
        await base.Include14(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t1].[Id], [t1].[Level2_Optional_Id], [t1].[Level2_Required_Id], [t1].[Level3_Name], [t1].[OneToMany_Optional_Inverse3Id], [t1].[OneToMany_Required_Inverse3Id], [t1].[OneToOne_Optional_PK_Inverse3Id], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[OneToOne_Optional_PK_Inverse2Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t1].[Level2_Optional_Id]
""");
    }

    public override async Task Key_equality_using_property_method_nested2(bool async)
    {
        await base.Key_equality_using_property_method_nested2(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] = 7
""");
    }

    public override async Task Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access_subquery(bool async)
    {
        await base.Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access_subquery(async);

        AssertSql(
"""
@__p_0='10'

SELECT [l3].[Name]
FROM (
    SELECT TOP(@__p_0) [t1].[Level1_Required_Id] AS [Level1_Required_Id0], CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END AS [c]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t0] ON CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [t0].[Level2_Required_Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
) AS [t2]
LEFT JOIN [Level1] AS [l3] ON [t2].[Level1_Required_Id0] = [l3].[Id]
ORDER BY [t2].[c]
""");
    }

    public override async Task Where_complex_predicate_with_with_nav_prop_and_OrElse1(bool async)
    {
        await base.Where_complex_predicate_with_with_nav_prop_and_OrElse1(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END AS [Id2]
FROM [Level1] AS [l]
CROSS JOIN (
    SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0]
LEFT JOIN (
    SELECT [l2].[Level1_Optional_Id], [l2].[Level2_Name]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [l].[Id] = [t1].[Level1_Optional_Id]
LEFT JOIN [Level1] AS [l3] ON [t0].[Level1_Required_Id] = [l3].[Id]
WHERE [t1].[Level2_Name] = N'L2 01' OR [l3].[Name] <> N'Bar' OR [l3].[Name] IS NULL
""");
    }

    public override async Task OrderBy_nav_prop_reference_optional(bool async)
    {
        await base.OrderBy_nav_prop_reference_optional(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
ORDER BY [t].[Level2_Name], [l].[Id]
""");
    }

    public override async Task Include18(bool async)
    {
        await base.Include18(async);

        AssertSql(
"""
@__p_0='10'

SELECT [t].[Id], [t].[Date], [t].[Name], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [l].[Date], [l].[Name]
    FROM [Level1] AS [l]
    ORDER BY [l].[Id]
) AS [t]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [t].[Id] = [t0].[OneToOne_Optional_PK_Inverse2Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t].[Id] = [t1].[Level1_Optional_Id]
ORDER BY [t].[Id]
""");
    }

    public override async Task String_include_multiple_derived_navigation_with_same_name_and_different_type(bool async)
    {
        await base.String_include_multiple_derived_navigation_with_same_name_and_different_type(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[Discriminator], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1], [i].[Name], [i0].[Id], [i0].[DifferentTypeCollection_InheritanceDerived1Id], [i0].[DifferentTypeReference_InheritanceDerived1Id], [i0].[InheritanceDerived2Id], [i0].[Name], [i0].[SameTypeCollection_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived2Id], [i1].[Id], [i1].[DifferentTypeReference_InheritanceDerived2Id], [i1].[InheritanceDerived2Id], [i1].[Name]
FROM [InheritanceOne] AS [i]
LEFT JOIN [InheritanceLeafOne] AS [i0] ON [i].[Id] = [i0].[DifferentTypeReference_InheritanceDerived1Id]
LEFT JOIN [InheritanceLeafTwo] AS [i1] ON [i].[Id] = [i1].[DifferentTypeReference_InheritanceDerived2Id]
""");
    }

    public override async Task Include3(bool async)
    {
        await base.Include3(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[OneToOne_Optional_PK_Inverse2Id]
""");
    }

    public override async Task Join_navigation_key_access_optional(bool async)
    {
        await base.Join_navigation_key_access_optional(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id], [l1].[Id] AS [Id1]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Optional_Id] = [l1].[Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Id1]
""");
    }

    public override async Task Optional_navigation_inside_method_call_translated_to_join(bool async)
    {
        await base.Optional_navigation_inside_method_call_translated_to_join(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] IS NOT NULL AND [t].[Level2_Name] LIKE N'L%'
""");
    }

    public override async Task Multi_level_navigation_with_same_navigation_compared_to_null(bool async)
    {
        await base.Multi_level_navigation_with_same_navigation_compared_to_null(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[OneToMany_Optional_Inverse3Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Required_Id] = [l3].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND ([l3].[Name] <> N'L1 07' OR [l3].[Name] IS NULL) AND [l3].[Id] IS NOT NULL
""");
    }

    public override async Task Multi_level_navigation_compared_to_null(bool async)
    {
        await base.Multi_level_navigation_compared_to_null(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[OneToMany_Optional_Inverse3Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Required_Id] = [l3].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [l3].[Id] IS NOT NULL
""");
    }

    public override async Task Where_multiple_nav_prop_reference_optional_compared_to_null1(bool async)
    {
        await base.Where_multiple_nav_prop_reference_optional_compared_to_null1(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
WHERE [t0].[Level2_Required_Id] IS NULL OR [t0].[OneToMany_Required_Inverse3Id] IS NULL
""");
    }

    public override async Task Where_complex_predicate_with_with_nav_prop_and_OrElse3(bool async)
    {
        await base.Where_complex_predicate_with_with_nav_prop_and_OrElse3(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l2].[Level2_Optional_Id], [l2].[Level3_Name]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level2_Optional_Id]
WHERE [t].[Level2_Name] <> N'L2 05' OR [t].[Level2_Name] IS NULL OR [t1].[Level3_Name] = N'L3 05'
""");
    }

    public override async Task Include2(bool async)
    {
        await base.Include2(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Join_navigation_in_inner_selector(bool async)
    {
        await base.Join_navigation_in_inner_selector(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END AS [Id2], [t0].[Id] AS [Id1]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
INNER JOIN (
    SELECT [l1].[Id], [t1].[Id] AS [Id0], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Required_Id], [t1].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l1].[Id] = [t1].[Level1_Optional_Id]
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Select_nav_prop_reference_optional1(bool async)
    {
        await base.Select_nav_prop_reference_optional1(async);

        AssertSql(
"""
SELECT [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task SelectMany_navigation_comparison3(bool async)
    {
        await base.SelectMany_navigation_comparison3(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END AS [Id2]
FROM [Level1] AS [l]
CROSS JOIN (
    SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [l].[Id] = [t1].[Level1_Optional_Id]
WHERE CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END = CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END OR (CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END IS NULL AND CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END IS NULL)
""");
    }

    public override async Task Select_multiple_nav_prop_reference_required2(bool async)
    {
        await base.Select_multiple_nav_prop_reference_required2(async);

        AssertSql(
"""
SELECT [l3].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Required_Id] = [l3].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Include12(bool async)
    {
        await base.Include12(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
""");
    }

    public override async Task Join_navigation_in_outer_selector_translated_to_extra_join(bool async)
    {
        await base.Join_navigation_in_outer_selector_translated_to_extra_join(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
INNER JOIN (
    SELECT [t1].[Id] AS [Id0], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Required_Id], [t1].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l1].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    WHERE [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
""");
    }

    public override async Task Where_multiple_nav_prop_reference_optional_member_compared_to_value(bool async)
    {
        await base.Where_multiple_nav_prop_reference_optional_member_compared_to_value(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Optional_Id], [l1].[Level3_Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
WHERE [t0].[Level3_Name] <> N'L3 05' OR [t0].[Level3_Name] IS NULL
""");
    }

    public override async Task Result_operator_nav_prop_reference_optional_Sum(bool async)
    {
        await base.Result_operator_nav_prop_reference_optional_Sum(async);

        AssertSql(
"""
SELECT COALESCE(SUM([t].[Level1_Required_Id]), 0)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Key_equality_using_property_method_nested(bool async)
    {
        await base.Key_equality_using_property_method_nested(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 7
""");
    }

    public override async Task Explicit_GroupJoin_in_subquery_with_unrelated_projection4(bool async)
    {
        await base.Explicit_GroupJoin_in_subquery_with_unrelated_projection4(async);

        AssertSql(
"""
@__p_0='20'

SELECT TOP(@__p_0) [t1].[Id]
FROM (
    SELECT DISTINCT [l].[Id]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
    WHERE [t0].[Level2_Name] <> N'Foo' OR [t0].[Level2_Name] IS NULL
) AS [t1]
ORDER BY [t1].[Id]
""");
    }

    public override async Task SelectMany_with_string_based_Include1(bool async)
    {
        await base.SelectMany_with_string_based_Include1(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
""");
    }

    public override async Task SelectMany_nested_navigation_property_required(bool async)
    {
        await base.SelectMany_nested_navigation_property_required(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
INNER JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
""");
    }

    public override async Task Key_equality_using_property_method_required(bool async)
    {
        await base.Key_equality_using_property_method_required(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END > 7
""");
    }

    public override async Task Optional_navigation_inside_method_call_translated_to_join_keeps_original_nullability(bool async)
    {
        await base.Optional_navigation_inside_method_call_translated_to_join_keeps_original_nullability(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE DATEADD(day, CAST(10.0E0 AS int), [t].[OneToOne_Required_PK_Date]) > '2000-02-01T00:00:00.0000000'
""");
    }

    public override async Task Include4(bool async)
    {
        await base.Include4(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToOne_Optional_PK_Inverse3Id]
""");
    }

    public override async Task Explicit_GroupJoin_in_subquery_with_unrelated_projection3(bool async)
    {
        await base.Explicit_GroupJoin_in_subquery_with_unrelated_projection3(async);

        AssertSql(
"""
SELECT DISTINCT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
WHERE [t0].[Level2_Name] <> N'Foo' OR [t0].[Level2_Name] IS NULL
""");
    }

    public override async Task Include18_1_1(bool async)
    {
        await base.Include18_1_1(async);

        AssertSql(
"""
@__p_0='10'

SELECT [t0].[Id], [t0].[Date], [t0].[Name], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [l].[Date], [l].[Name], [t].[Level2_Name]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Level1_Required_Id], [l0].[Level2_Name]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
    ORDER BY [t].[Level2_Name]
) AS [t0]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Id] = [t1].[Level1_Optional_Id]
ORDER BY [t0].[Level2_Name]
""");
    }

    public override async Task Optional_navigation_projected_into_DTO(bool async)
    {
        await base.Optional_navigation_projected_into_DTO(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Name], CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END, CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END, [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Join_navigation_nested2(bool async)
    {
        await base.Join_navigation_nested2(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END AS [Id3], [t1].[Id] AS [Id1]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
INNER JOIN (
    SELECT [l2].[Id], [t3].[Id] AS [Id1], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = [t2].[Level1_Required_Id]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = [t3].[Level2_Optional_Id]
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Key_equality_using_property_method_and_member_expression1(bool async)
    {
        await base.Key_equality_using_property_method_and_member_expression1(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 7
""");
    }

    public override async Task Navigation_key_access_optional_comparison(bool async)
    {
        await base.Navigation_key_access_optional_comparison(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[OneToOne_Optional_PK_Inverse2Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] > 5
""");
    }

    public override async Task SelectMany_navigation_property(bool async)
    {
        await base.SelectMany_navigation_property(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
""");
    }

    public override async Task Join_with_orderby_on_inner_sequence_navigation_non_key_join(bool async)
    {
        await base.Join_with_orderby_on_inner_sequence_navigation_non_key_join(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END AS [Id2], [t].[Level2_Name] AS [Name2], [t0].[Id] AS [Id1], [t0].[Name] AS [Name1]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
INNER JOIN (
    SELECT [l1].[Id], [l1].[Name], [t1].[Level2_Name]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l2].[Level1_Optional_Id], [l2].[Level2_Name]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l1].[Id] = [t1].[Level1_Optional_Id]
) AS [t0] ON [t].[Level2_Name] = [t0].[Level2_Name]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Correlated_subquery_doesnt_project_unnecessary_columns_in_top_level_join(bool async)
    {
        await base.Correlated_subquery_doesnt_project_unnecessary_columns_in_top_level_join(async);

        AssertSql(
"""
SELECT [l].[Name] AS [Name1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id], [l1].[Id] AS [Id1]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Optional_Id] = [l1].[Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Id1]
WHERE EXISTS (
    SELECT 1
    FROM [Level1] AS [l3]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l3].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    WHERE [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t1].[Level1_Required_Id] = [l].[Id])
""");
    }

    public override async Task SelectMany_navigation_property_and_projection(bool async)
    {
        await base.SelectMany_navigation_property_and_projection(async);

        AssertSql(
"""
SELECT [t].[Level2_Name]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
""");
    }

    public override async Task Order_by_key_of_navigation_similar_to_projected_gets_optimized_into_FK_access(bool async)
    {
        await base.Order_by_key_of_navigation_similar_to_projected_gets_optimized_into_FK_access(async);

        AssertSql(
"""
SELECT [l3].[Id], [l3].[Date], [l3].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Required_Id] = [l3].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
ORDER BY CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
""");
    }

    public override async Task Navigations_compared_to_each_other2(bool async)
    {
        await base.Navigations_compared_to_each_other2(async);

        AssertSql(
"""
SELECT [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[OneToMany_Required_Inverse2Id] = [l1].[Id]
LEFT JOIN [Level1] AS [l2] ON [t].[OneToOne_Optional_PK_Inverse2Id] = [l2].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND ([l1].[Id] = [l2].[Id] OR ([l1].[Id] IS NULL AND [l2].[Id] IS NULL))
""");
    }

    public override async Task SelectMany_with_navigation_and_explicit_DefaultIfEmpty(bool async)
    {
        await base.SelectMany_with_navigation_and_explicit_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Explicit_GroupJoin_in_subquery_with_multiple_result_operator_distinct_count_materializes_main_clause(
        bool async)
    {
        await base.Explicit_GroupJoin_in_subquery_with_multiple_result_operator_distinct_count_materializes_main_clause(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
WHERE (
    SELECT COUNT(*)
    FROM (
        SELECT DISTINCT [l0].[Id], [l0].[Date], [l0].[Name]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[Date], [l1].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            LEFT JOIN (
                SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
                FROM [Level1] AS [l2]
                WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
            ) AS [t] ON [l1].[Id] = CASE
                WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
            END
            WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l0].[Id] = [t0].[Level1_Optional_Id]
    ) AS [t1]) > 4
""");
    }

    public override async Task Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access2(bool async)
    {
        await base.Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access2(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
ORDER BY CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
""");
    }

    public override async Task SelectMany_with_string_based_Include2(bool async)
    {
        await base.SelectMany_with_string_based_Include2(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [t1].[Id], [t1].[Level3_Optional_Id], [t1].[Level3_Required_Id], [t1].[Level4_Name], [t1].[OneToMany_Optional_Inverse4Id], [t1].[OneToMany_Required_Inverse4Id], [t1].[OneToOne_Optional_PK_Inverse4Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Optional_Id], [l2].[Level3_Required_Id], [l2].[Level4_Name], [l2].[OneToMany_Optional_Inverse4Id], [l2].[OneToMany_Required_Inverse4Id], [l2].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level3_Required_Id]
""");
    }

    public override async Task Method_call_on_optional_navigation_translates_to_null_conditional_properly_for_arguments(bool async)
    {
        await base.Method_call_on_optional_navigation_translates_to_null_conditional_properly_for_arguments(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] = N'' OR ([t].[Level2_Name] IS NOT NULL AND LEFT([t].[Level2_Name], LEN([t].[Level2_Name])) = [t].[Level2_Name])
""");
    }

    public override async Task SelectMany_subquery_with_custom_projection(bool async)
    {
        await base.SelectMany_subquery_with_custom_projection(async);

        AssertSql(
"""
@__p_0='1'

SELECT TOP(@__p_0) [t].[Name]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Level2_Name] AS [Name], [l0].[OneToMany_Optional_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
ORDER BY [l].[Id]
""");
    }

    public override async Task Join_with_navigations_in_the_result_selector1(bool async)
    {
        await base.Join_with_navigations_in_the_result_selector1(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id0], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [l].[Id] = [t1].[Level1_Optional_Id]
""");
    }

    public override async Task
        String_include_multiple_derived_navigation_with_same_name_and_different_type_nested_also_includes_partially_matching_navigation_chains(
            bool async)
    {
        await base
            .String_include_multiple_derived_navigation_with_same_name_and_different_type_nested_also_includes_partially_matching_navigation_chains(
                async);

        AssertSql(
"""
SELECT [i].[Id], [i].[Discriminator], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1], [i].[Name], [i0].[Id], [i0].[DifferentTypeCollection_InheritanceDerived1Id], [i0].[DifferentTypeReference_InheritanceDerived1Id], [i0].[InheritanceDerived2Id], [i0].[Name], [i0].[SameTypeCollection_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived2Id], [i1].[Id], [i1].[DifferentTypeReference_InheritanceDerived2Id], [i1].[InheritanceDerived2Id], [i1].[Name], [i2].[Id], [i2].[InheritanceLeaf2Id], [i2].[Name]
FROM [InheritanceOne] AS [i]
LEFT JOIN [InheritanceLeafOne] AS [i0] ON [i].[Id] = [i0].[DifferentTypeReference_InheritanceDerived1Id]
LEFT JOIN [InheritanceLeafTwo] AS [i1] ON [i].[Id] = [i1].[DifferentTypeReference_InheritanceDerived2Id]
LEFT JOIN [InheritanceTwo] AS [i2] ON [i1].[Id] = [i2].[InheritanceLeaf2Id]
ORDER BY [i].[Id], [i0].[Id], [i1].[Id]
""");
    }

    public override async Task Key_equality_navigation_converted_to_FK(bool async)
    {
        await base.Key_equality_navigation_converted_to_FK(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] = 1
""");
    }

    public override async Task Include10(bool async)
    {
        await base.Include10(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id], [t2].[Id], [t2].[Level2_Optional_Id], [t2].[Level2_Required_Id], [t2].[Level3_Name], [t2].[OneToMany_Optional_Inverse3Id], [t2].[OneToMany_Required_Inverse3Id], [t2].[OneToOne_Optional_PK_Inverse3Id], [t3].[Id], [t3].[Level3_Optional_Id], [t3].[Level3_Required_Id], [t3].[Level4_Name], [t3].[OneToMany_Optional_Inverse4Id], [t3].[OneToMany_Required_Inverse4Id], [t3].[OneToOne_Optional_PK_Inverse4Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToOne_Optional_PK_Inverse3Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [l].[Id] = [t1].[OneToOne_Optional_PK_Inverse2Id]
LEFT JOIN (
    SELECT [l3].[Id], [l3].[Level2_Optional_Id], [l3].[Level2_Required_Id], [l3].[Level3_Name], [l3].[OneToMany_Optional_Inverse3Id], [l3].[OneToMany_Required_Inverse3Id], [l3].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l3]
    WHERE [l3].[Level2_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t2] ON CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END = [t2].[Level2_Optional_Id]
LEFT JOIN (
    SELECT [l4].[Id], [l4].[Level3_Optional_Id], [l4].[Level3_Required_Id], [l4].[Level4_Name], [l4].[OneToMany_Optional_Inverse4Id], [l4].[OneToMany_Required_Inverse4Id], [l4].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l4]
    WHERE [l4].[Level3_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t3] ON CASE
    WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
END = [t3].[OneToOne_Optional_PK_Inverse4Id]
""");
    }

    public override async Task Where_navigation_property_to_collection_of_original_entity_type(bool async)
    {
        await base.Where_navigation_property_to_collection_of_original_entity_type(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[OneToMany_Required_Inverse2Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND (
    SELECT COUNT(*)
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] IS NOT NULL AND [l1].[Id] = [l2].[OneToMany_Optional_Inverse2Id]) > 0
""");
    }

    public override async Task Where_multiple_nav_prop_reference_optional_compared_to_null2(bool async)
    {
        await base.Where_multiple_nav_prop_reference_optional_compared_to_null2(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Optional_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Optional_Id] = [l3].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [l3].[Id] IS NULL
""");
    }

    public override async Task SelectMany_navigation_property_and_filter_after(bool async)
    {
        await base.SelectMany_navigation_property_and_filter_after(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END <> 6 OR CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END IS NULL
""");
    }

    public override async Task Correlated_nested_subquery_doesnt_project_unnecessary_columns_in_top_level(bool async)
    {
        await base.Correlated_nested_subquery_doesnt_project_unnecessary_columns_in_top_level(async);

        AssertSql(
"""
SELECT DISTINCT [l].[Name]
FROM [Level1] AS [l]
WHERE EXISTS (
    SELECT 1
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND EXISTS (
        SELECT 1
        FROM [Level1] AS [l2]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l2].[Id] = CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Optional_Inverse3Id], [l4].[OneToMany_Required_Inverse3Id], [l4].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l4]
            WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t1] ON CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END = CASE
            WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
        END
        WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL))
""");
    }

    public override async Task OrderBy_nav_prop_reference_optional_via_DefaultIfEmpty(bool async)
    {
        await base.OrderBy_nav_prop_reference_optional_via_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
ORDER BY [t0].[Level2_Name], [l].[Id]
""");
    }

    public override async Task Where_complex_predicate_with_with_nav_prop_and_OrElse2(bool async)
    {
        await base.Where_complex_predicate_with_with_nav_prop_and_OrElse2(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Required_Id], [l1].[Level3_Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
WHERE [t0].[Level3_Name] = N'L3 05' OR [t].[Level2_Name] <> N'L2 05' OR [t].[Level2_Name] IS NULL
""");
    }

    public override async Task Optional_navigation_inside_nested_method_call_translated_to_join(bool async)
    {
        await base.Optional_navigation_inside_nested_method_call_translated_to_join(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] IS NOT NULL AND UPPER([t].[Level2_Name]) LIKE N'L%'
""");
    }

    public override async Task Multiple_SelectMany_calls(bool async)
    {
        await base.Multiple_SelectMany_calls(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
INNER JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
""");
    }

    public override async Task Navigation_inside_method_call_translated_to_join(bool async)
    {
        await base.Navigation_inside_method_call_translated_to_join(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Required_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE [t].[Level2_Name] IS NOT NULL AND [t].[Level2_Name] LIKE N'L%'
""");
    }

    public override async Task Include5(bool async)
    {
        await base.Include5(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToOne_Optional_PK_Inverse3Id]
""");
    }

    public override async Task String_include_multiple_derived_collection_navigation_with_same_name_and_same_type(bool async)
    {
        await base.String_include_multiple_derived_collection_navigation_with_same_name_and_same_type(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[Discriminator], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1], [i].[Name], [i0].[Id], [i0].[DifferentTypeCollection_InheritanceDerived1Id], [i0].[DifferentTypeReference_InheritanceDerived1Id], [i0].[InheritanceDerived2Id], [i0].[Name], [i0].[SameTypeCollection_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived2Id], [i1].[Id], [i1].[DifferentTypeCollection_InheritanceDerived1Id], [i1].[DifferentTypeReference_InheritanceDerived1Id], [i1].[InheritanceDerived2Id], [i1].[Name], [i1].[SameTypeCollection_InheritanceDerived1Id], [i1].[SameTypeReference_InheritanceDerived1Id], [i1].[SameTypeReference_InheritanceDerived2Id]
FROM [InheritanceOne] AS [i]
LEFT JOIN [InheritanceLeafOne] AS [i0] ON [i].[Id] = [i0].[SameTypeCollection_InheritanceDerived1Id]
LEFT JOIN [InheritanceLeafOne] AS [i1] ON [i].[Id] = [i1].[InheritanceDerived2Id]
ORDER BY [i].[Id], [i0].[Id]
""");
    }

    public override async Task Navigations_compared_to_each_other4(bool async)
    {
        await base.Navigations_compared_to_each_other4(async);

        AssertSql(
"""
SELECT [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND EXISTS (
    SELECT 1
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL AND CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END IS NOT NULL AND (CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END = [l2].[OneToMany_Optional_Inverse4Id] OR (CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END IS NULL AND [l2].[OneToMany_Optional_Inverse4Id] IS NULL)))
""");
    }

    public override async Task Null_check_removal_applied_recursively(bool async)
    {
        await base.Null_check_removal_applied_recursively(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
LEFT JOIN (
    SELECT [l2].[Level3_Optional_Id], [l2].[Level4_Name]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level3_Optional_Id]
WHERE [t1].[Level4_Name] = N'L4 01'
""");
    }

    public override async Task SelectMany_where_with_subquery(bool async)
    {
        await base.SelectMany_where_with_subquery(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Required_Inverse2Id]
WHERE EXISTS (
    SELECT 1
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = [l1].[OneToMany_Required_Inverse3Id])
""");
    }

    public override async Task Result_operator_nav_prop_reference_optional_Max(bool async)
    {
        await base.Result_operator_nav_prop_reference_optional_Max(async);

        AssertSql(
"""
SELECT MAX([t].[Level1_Required_Id])
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Complex_navigations_with_predicate_projected_into_anonymous_type(bool async)
    {
        await base.Complex_navigations_with_predicate_projected_into_anonymous_type(async);

        AssertSql(
"""
SELECT [l].[Name], CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
END AS [Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t1].[Level2_Optional_Id]
WHERE (CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
END OR (CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END IS NULL AND CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
END IS NULL)) AND (CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
END <> 7 OR CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
END IS NULL)
""");
    }

    public override async Task GroupJoin_without_DefaultIfEmpty(bool async)
    {
        await base.GroupJoin_without_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[Date], [l0].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task Key_equality_two_conditions_on_same_navigation(bool async)
    {
        await base.Key_equality_two_conditions_on_same_navigation(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 1 OR CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 2
""");
    }

    public override async Task Select_multiple_nav_prop_reference_required(bool async)
    {
        await base.Select_multiple_nav_prop_reference_required(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
""");
    }

    public override async Task Project_collection_navigation_count(bool async)
    {
        await base.Project_collection_navigation_count(async);

        AssertSql(
"""
SELECT [l].[Id], (
    SELECT COUNT(*)
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = [l1].[OneToMany_Optional_Inverse3Id] OR (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL AND [l1].[OneToMany_Optional_Inverse3Id] IS NULL))) AS [Count]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Include18_2(bool async)
    {
        await base.Include18_2(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Date], [t0].[Name], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT DISTINCT [l].[Id], [l].[Date], [l].[Name]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Level1_Required_Id], [l0].[Level2_Name]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
    WHERE [t].[Level2_Name] <> N'Foo' OR [t].[Level2_Name] IS NULL
) AS [t0]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Id] = [t1].[Level1_Optional_Id]
""");
    }

    public override async Task Include13(bool async)
    {
        await base.Include13(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Include_with_optional_navigation(bool async)
    {
        await base.Include_with_optional_navigation(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] <> N'L2 05' OR [t].[Level2_Name] IS NULL
""");
    }

    public override async Task GroupJoin_on_right_side_being_a_subquery(bool async)
    {
        await base.GroupJoin_on_right_side_being_a_subquery(async);

        AssertSql(
"""
@__p_0='2'

SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END AS [Id], [t0].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT TOP(@__p_0) [l1].[Id], [l1].[Name]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l2].[Level1_Optional_Id], [l2].[Level2_Name]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l1].[Id] = [t1].[Level1_Optional_Id]
    ORDER BY [t1].[Level2_Name]
) AS [t0] ON [t].[Level1_Optional_Id] = [t0].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Multiple_SelectMany_with_string_based_Include(bool async)
    {
        await base.Multiple_SelectMany_with_string_based_Include(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [t1].[Id], [t1].[Level3_Optional_Id], [t1].[Level3_Required_Id], [t1].[Level4_Name], [t1].[OneToMany_Optional_Inverse4Id], [t1].[OneToMany_Required_Inverse4Id], [t1].[OneToOne_Optional_PK_Inverse4Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
INNER JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Optional_Id], [l2].[Level3_Required_Id], [l2].[Level4_Name], [l2].[OneToMany_Optional_Inverse4Id], [l2].[OneToMany_Required_Inverse4Id], [l2].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level3_Required_Id]
""");
    }

    public override async Task Where_navigation_property_to_collection2(bool async)
    {
        await base.Where_navigation_property_to_collection2(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND (
    SELECT COUNT(*)
    FROM [Level1] AS [l3]
    WHERE [l3].[Level2_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END IS NOT NULL AND (CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END = [l3].[OneToMany_Optional_Inverse3Id] OR (CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END IS NULL AND [l3].[OneToMany_Optional_Inverse3Id] IS NULL))) > 0
""");
    }

    public override async Task GroupJoin_in_subquery_with_client_projection(bool async)
    {
        await base.GroupJoin_in_subquery_with_client_projection(async);

        AssertSql(
"""
SELECT [l].[Name]
FROM [Level1] AS [l]
WHERE (
    SELECT COUNT(*)
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[Date], [l1].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        LEFT JOIN (
            SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l2]
            WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l1].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l0].[Id] = [t0].[Level1_Optional_Id]) > 7 AND [l].[Id] < 3
""");
    }

    public override async Task Query_source_materialization_bug_4547(bool async)
    {
        await base.Query_source_materialization_bug_4547(async);

        AssertSql(
"""
SELECT [l2].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
INNER JOIN [Level1] AS [l2] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = (
    SELECT TOP(1) CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id1]
    END
    FROM [Level1] AS [l3]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l3].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    LEFT JOIN (
        SELECT [l5].[Id], [l5].[Date], [l5].[Name], [t3].[Id] AS [Id0], [t3].[OneToOne_Required_PK_Date], [t3].[Level1_Optional_Id], [t3].[Level1_Required_Id], [t3].[Level2_Name], [t3].[OneToMany_Optional_Inverse2Id], [t3].[OneToMany_Required_Inverse2Id], [t3].[OneToOne_Optional_PK_Inverse2Id], [t4].[Id] AS [Id1], [t4].[Level2_Optional_Id], [t4].[Level2_Required_Id], [t4].[Level3_Name], [t4].[OneToMany_Optional_Inverse3Id], [t4].[OneToMany_Required_Inverse3Id], [t4].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l5]
        LEFT JOIN (
            SELECT [l6].[Id], [l6].[OneToOne_Required_PK_Date], [l6].[Level1_Optional_Id], [l6].[Level1_Required_Id], [l6].[Level2_Name], [l6].[OneToMany_Optional_Inverse2Id], [l6].[OneToMany_Required_Inverse2Id], [l6].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l6]
            WHERE [l6].[OneToOne_Required_PK_Date] IS NOT NULL AND [l6].[Level1_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t3] ON [l5].[Id] = CASE
            WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
        END
        LEFT JOIN (
            SELECT [l7].[Id], [l7].[Level2_Optional_Id], [l7].[Level2_Required_Id], [l7].[Level3_Name], [l7].[OneToMany_Optional_Inverse3Id], [l7].[OneToMany_Required_Inverse3Id], [l7].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l7]
            WHERE [l7].[Level2_Required_Id] IS NOT NULL AND [l7].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t4] ON CASE
            WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
        END = CASE
            WHEN [t4].[Level2_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t4].[Id]
        END
        WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t4].[Level2_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t2] ON CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END = [t2].[Level2_Optional_Id]
    WHERE [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id1]
    END)
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Join_navigation_deeply_nested_required(bool async)
    {
        await base.Join_navigation_deeply_nested_required(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t4].[Level3_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t4].[Id2]
END AS [Id4], [t4].[Level4_Name] AS [Name4], [l].[Id] AS [Id1], [l].[Name] AS [Name1]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t1].[Id] AS [Id2], [t1].[Level3_Required_Id], [t1].[Level4_Name], [t1].[OneToMany_Required_Inverse4Id], [l5].[Name] AS [Name0]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[Level2_Required_Id], [l2].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t0] ON CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[Level3_Required_Id], [l3].[Level4_Name], [l3].[OneToMany_Required_Inverse4Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[Level3_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse4Id] IS NOT NULL
    ) AS [t1] ON CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END = CASE
        WHEN [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t1].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t2] ON [t1].[Level3_Required_Id] = CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l6].[Id], [l6].[OneToOne_Required_PK_Date], [l6].[Level1_Required_Id], [l6].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l6]
        WHERE [l6].[OneToOne_Required_PK_Date] IS NOT NULL AND [l6].[Level1_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [t2].[Level2_Required_Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    LEFT JOIN [Level1] AS [l5] ON CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END = [l5].[Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t4] ON [l].[Name] = [t4].[Name0]
""");
    }

    public override async Task Where_predicate_on_optional_reference_navigation(bool async)
    {
        await base.Where_predicate_on_optional_reference_navigation(async);

        AssertSql(
"""
@__p_0='3'

SELECT TOP(@__p_0) [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Required_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE [t].[Level2_Name] = N'L2 03'
ORDER BY [l].[Id]
""");
    }

    public override async Task Navigations_compared_to_each_other3(bool async)
    {
        await base.Navigations_compared_to_each_other3(async);

        AssertSql(
"""
SELECT [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND EXISTS (
    SELECT 1
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = [l1].[OneToMany_Optional_Inverse3Id] OR (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL AND [l1].[OneToMany_Optional_Inverse3Id] IS NULL)))
""");
    }

    public override async Task Where_multiple_nav_prop_reference_optional_compared_to_null5(bool async)
    {
        await base.Where_multiple_nav_prop_reference_optional_compared_to_null5(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
LEFT JOIN (
    SELECT [l2].[Level3_Required_Id], [l2].[OneToMany_Required_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level3_Required_Id]
WHERE [t1].[Level3_Required_Id] IS NULL OR [t1].[OneToMany_Required_Inverse4Id] IS NULL
""");
    }

    public override async Task Where_on_multilevel_reference_in_subquery_with_outer_projection(bool async)
    {
        await base.Where_on_multilevel_reference_in_subquery_with_outer_projection(async);

        AssertSql(
"""
@__p_0='0'
@__p_1='10'

SELECT [t0].[Level3_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[OneToMany_Required_Inverse3Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Required_Id] = [l3].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [l3].[Name] = N'L1 03'
ORDER BY [t0].[Level2_Required_Id]
OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY
""");
    }

    public override async Task Projection_select_correct_table_in_subquery_when_materialization_is_not_required_in_multiple_joins(
        bool async)
    {
        await base.Projection_select_correct_table_in_subquery_when_materialization_is_not_required_in_multiple_joins(async);

        AssertSql(
"""
@__p_0='3'

SELECT TOP(@__p_0) [l1].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
INNER JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
INNER JOIN (
    SELECT [t2].[Level2_Required_Id], [t2].[Level3_Name]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l2].[Id] = CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t2] ON CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END = CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END
    WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON [l1].[Id] = [t1].[Level2_Required_Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Name] = N'L1 03' AND [t1].[Level3_Name] = N'L3 08'
ORDER BY [l1].[Id]
""");
    }

    public override async Task Include6(bool async)
    {
        await base.Include6(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToOne_Optional_PK_Inverse3Id]
""");
    }

    public override async Task Include7(bool async)
    {
        await base.Include7(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToOne_Optional_PK_Inverse2Id]
""");
    }

    public override async Task Member_doesnt_get_pushed_down_into_subquery_with_result_operator(bool async)
    {
        await base.Member_doesnt_get_pushed_down_into_subquery_with_result_operator(async);

        AssertSql(
"""
SELECT (
    SELECT [t1].[Level3_Name]
    FROM (
        SELECT DISTINCT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        LEFT JOIN (
            SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l2]
            WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t0] ON CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END = CASE
            WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t1]
    ORDER BY CASE
        WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
    END
    OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY)
FROM [Level1] AS [l]
WHERE [l].[Id] < 3
""");
    }

    public override async Task Correlated_nested_two_levels_up_subquery_doesnt_project_unnecessary_columns_in_top_level(bool async)
    {
        await base.Correlated_nested_two_levels_up_subquery_doesnt_project_unnecessary_columns_in_top_level(async);

        AssertSql(
"""
SELECT DISTINCT [l].[Name]
FROM [Level1] AS [l]
WHERE EXISTS (
    SELECT 1
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND EXISTS (
        SELECT 1
        FROM [Level1] AS [l2]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l2].[Id] = CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[Level3_Name], [l4].[OneToMany_Optional_Inverse3Id], [l4].[OneToMany_Required_Inverse3Id], [l4].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l4]
            WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t1] ON CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END = CASE
            WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id]
        END
        WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL))
""");
    }

    public override async Task Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access3(bool async)
    {
        await base.Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access3(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
ORDER BY CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
""");
    }

    public override async Task Select_nav_prop_reference_optional2(bool async)
    {
        await base.Select_nav_prop_reference_optional2(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Accessing_optional_property_inside_result_operator_subquery(bool async)
    {
        await base.Accessing_optional_property_inside_result_operator_subquery(async);

        AssertSql(
"""
@__names_0='["Name1","Name2"]' (Size = 4000)

SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE NOT EXISTS (
    SELECT 1
    FROM OPENJSON(@__names_0) WITH ([value] nvarchar(max) '$') AS [n]
    WHERE [n].[value] = [t].[Level2_Name] OR ([n].[value] IS NULL AND [t].[Level2_Name] IS NULL))
""");
    }

    public override async Task Where_multiple_nav_prop_reference_optional_compared_to_null4(bool async)
    {
        await base.Where_multiple_nav_prop_reference_optional_compared_to_null4(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Optional_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Optional_Id] = [l3].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [l3].[Id] IS NOT NULL
""");
    }

    public override async Task Multi_include_with_groupby_in_subquery(bool async)
    {
        await base.Multi_include_with_groupby_in_subquery(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[Date], [t1].[Name], [t1].[Id0], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id], [t].[Name], [t3].[Id], [t3].[Level2_Optional_Id], [t3].[Level2_Required_Id], [t3].[Level3_Name], [t3].[OneToMany_Optional_Inverse3Id], [t3].[OneToMany_Required_Inverse3Id], [t3].[OneToOne_Optional_PK_Inverse3Id]
FROM (
    SELECT [l].[Name]
    FROM [Level1] AS [l]
    GROUP BY [l].[Name]
) AS [t]
LEFT JOIN (
    SELECT [t0].[Id], [t0].[Date], [t0].[Name], [t0].[Id0], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
    FROM (
        SELECT [l0].[Id], [l0].[Date], [l0].[Name], [t2].[Id] AS [Id0], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id], ROW_NUMBER() OVER(PARTITION BY [l0].[Name] ORDER BY [l0].[Id]) AS [row]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t2] ON [l0].[Id] = [t2].[Level1_Optional_Id]
    ) AS [t0]
    WHERE [t0].[row] <= 1
) AS [t1] ON [t].[Name] = [t1].[Name]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t3] ON CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id0]
END = [t3].[OneToMany_Optional_Inverse3Id]
ORDER BY [t].[Name], [t1].[Id], [t1].[Id0]
""");
    }

    public override async Task Join_navigation_deeply_nested_non_key_join(bool async)
    {
        await base.Join_navigation_deeply_nested_non_key_join(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t1].[Id]
END AS [Id4], [t1].[Level4_Name] AS [Name4], [t2].[Id] AS [Id1], [t2].[Name] AS [Name1]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Required_Id], [l2].[Level4_Name], [l2].[OneToMany_Required_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = CASE
    WHEN [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t1].[Id]
END
INNER JOIN (
    SELECT [l3].[Id], [l3].[Name], [t5].[Level4_Name]
    FROM [Level1] AS [l3]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Required_Id], [l4].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l3].[Id] = [t3].[Level1_Required_Id]
    LEFT JOIN (
        SELECT [l5].[Id], [l5].[Level2_Optional_Id], [l5].[Level2_Required_Id], [l5].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l5]
        WHERE [l5].[Level2_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t4] ON CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END = [t4].[Level2_Optional_Id]
    LEFT JOIN (
        SELECT [l6].[Id], [l6].[Level3_Required_Id], [l6].[Level4_Name], [l6].[OneToMany_Required_Inverse4Id]
        FROM [Level1] AS [l6]
        WHERE [l6].[Level3_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse4Id] IS NOT NULL
    ) AS [t5] ON CASE
        WHEN [t4].[Level2_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t4].[Id]
    END = CASE
        WHEN [t5].[Level3_Required_Id] IS NOT NULL AND [t5].[OneToMany_Required_Inverse4Id] IS NOT NULL THEN [t5].[Id]
    END
) AS [t2] ON [t1].[Level4_Name] = [t2].[Level4_Name]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [t1].[Level3_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse4Id] IS NOT NULL
""");
    }

    public override async Task Join_navigation_in_outer_selector_translated_to_extra_join_nested(bool async)
    {
        await base.Join_navigation_in_outer_selector_translated_to_extra_join_nested(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END AS [Id3]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
INNER JOIN (
    SELECT [t3].[Id] AS [Id1], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END
""");
    }

    public override async Task Where_nav_prop_reference_optional2(bool async)
    {
        await base.Where_nav_prop_reference_optional2(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] = N'L2 05' OR [t].[Level2_Name] <> N'L2 42' OR [t].[Level2_Name] IS NULL
""");
    }

    public override async Task GroupJoin_client_method_on_outer(bool async)
    {
        await base.GroupJoin_client_method_on_outer(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[Date], [l0].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task Navigations_compared_to_each_other5(bool async)
    {
        await base.Navigations_compared_to_each_other5(async);

        AssertSql(
"""
SELECT [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t1].[OneToOne_Optional_PK_Inverse3Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND EXISTS (
    SELECT 1
    FROM [Level1] AS [l3]
    WHERE [l3].[Level3_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse4Id] IS NOT NULL AND CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END IS NOT NULL AND (CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END = [l3].[OneToMany_Optional_Inverse4Id] OR (CASE
        WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
    END IS NULL AND [l3].[OneToMany_Optional_Inverse4Id] IS NULL)))
""");
    }

    public override async Task Include_multiple_collections_on_same_level(bool async)
    {
        await base.Include_multiple_collections_on_same_level(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[OneToMany_Required_Inverse2Id]
ORDER BY [l].[Id], [t].[Id]
""");
    }

    public override async Task Key_equality_using_property_method_required2(bool async)
    {
        await base.Key_equality_using_property_method_required2(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] > 7
""");
    }

    public override async Task Select_optional_navigation_property_string_concat(bool async)
    {
        await base.Select_optional_navigation_property_string_concat(async);

        AssertSql(
"""
SELECT COALESCE([l].[Name], N'') + N' ' + COALESCE(CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Level2_Name]
    ELSE N'NULL'
END, N'')
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l0].[Id] > 5
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
""");
    }

    public override async Task Join_navigation_in_outer_selector_translated_to_extra_join_nested2(bool async)
    {
        await base.Join_navigation_in_outer_selector_translated_to_extra_join_nested2(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END AS [Id3], [l4].[Id] AS [Id1]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Optional_Id] = [l3].[Id]
INNER JOIN [Level1] AS [l4] ON [l3].[Id] = [l4].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Include19(bool async)
    {
        await base.Include19(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id], [t1].[Id0], [t1].[OneToOne_Required_PK_Date0], [t1].[Level1_Optional_Id0], [t1].[Level1_Required_Id0], [t1].[Level2_Name0], [t1].[OneToMany_Optional_Inverse2Id0], [t1].[OneToMany_Required_Inverse2Id0], [t1].[OneToOne_Optional_PK_Inverse2Id0]
FROM (
    SELECT DISTINCT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id] AS [Id0], [t0].[OneToOne_Required_PK_Date] AS [OneToOne_Required_PK_Date0], [t0].[Level1_Optional_Id] AS [Level1_Optional_Id0], [t0].[Level1_Required_Id] AS [Level1_Required_Id0], [t0].[Level2_Name] AS [Level2_Name0], [t0].[OneToMany_Optional_Inverse2Id] AS [OneToMany_Optional_Inverse2Id0], [t0].[OneToMany_Required_Inverse2Id] AS [OneToMany_Required_Inverse2Id0], [t0].[OneToOne_Optional_PK_Inverse2Id] AS [OneToOne_Optional_PK_Inverse2Id0]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l].[Id] = [t0].[OneToOne_Optional_PK_Inverse2Id]
) AS [t1]
""");
    }

    public override async Task SelectMany_navigation_property_and_filter_before(bool async)
    {
        await base.SelectMany_navigation_property_and_filter_before(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
WHERE [l].[Id] = 1
""");
    }

    public override async Task Subquery_with_Distinct_Skip_FirstOrDefault_without_OrderBy(bool async)
    {
        await base.Subquery_with_Distinct_Skip_FirstOrDefault_without_OrderBy(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Key], (
    SELECT [t1].[Level3_Name]
    FROM (
        SELECT DISTINCT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l0].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        LEFT JOIN (
            SELECT [l2].[Id], [l2].[Level2_Optional_Id], [l2].[Level2_Required_Id], [l2].[Level3_Name], [l2].[OneToMany_Optional_Inverse3Id], [l2].[OneToMany_Required_Inverse3Id], [l2].[OneToOne_Optional_PK_Inverse3Id]
            FROM [Level1] AS [l2]
            WHERE [l2].[Level2_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse3Id] IS NOT NULL
        ) AS [t0] ON CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END = CASE
            WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t1]
    ORDER BY (SELECT 1)
    OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY) AS [Subquery]
FROM [Level1] AS [l]
WHERE [l].[Id] < 3
""");
    }

    public override async Task Include1(bool async)
    {
        await base.Include1(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Comparing_collection_navigation_on_optional_reference_to_null(bool async)
    {
        await base.Comparing_collection_navigation_on_optional_reference_to_null(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NULL OR [t].[Level1_Required_Id] IS NULL OR [t].[OneToMany_Required_Inverse2Id] IS NULL
""");
    }

    public override async Task Where_nav_prop_reference_optional2_via_DefaultIfEmpty(bool async)
    {
        await base.Where_nav_prop_reference_optional2_via_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [t2].[Level1_Optional_Id], [t2].[Level2_Name]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [l].[Id] = [t1].[Level1_Optional_Id]
WHERE [t0].[Level2_Name] = N'L2 05' OR [t1].[Level2_Name] <> N'L2 42' OR [t1].[Level2_Name] IS NULL
""");
    }

    public override async Task Result_operator_nav_prop_reference_optional_Average_with_identity_selector(bool async)
    {
        await base.Result_operator_nav_prop_reference_optional_Average_with_identity_selector(async);

        AssertSql(
"""
SELECT AVG(CAST([t].[Level1_Required_Id] AS float))
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Optional_navigation_inside_property_method_translated_to_join(bool async)
    {
        await base.Optional_navigation_inside_property_method_translated_to_join(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] = N'L2 01'
""");
    }

    public override async Task
        Optional_navigation_inside_nested_method_call_translated_to_join_keeps_original_nullability_also_for_arguments(bool async)
    {
        await base
            .Optional_navigation_inside_nested_method_call_translated_to_join_keeps_original_nullability_also_for_arguments(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE DATEADD(day, CAST(CAST(CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END AS float) AS int), DATEADD(day, CAST(15.0E0 AS int), [t].[OneToOne_Required_PK_Date])) > '2002-02-01T00:00:00.0000000'
""");
    }

    public override async Task Key_equality_using_property_method_and_member_expression3(bool async)
    {
        await base.Key_equality_using_property_method_and_member_expression3(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] = 7
""");
    }

    public override async Task Include_reference_with_groupby_in_subquery(bool async)
    {
        await base.Include_reference_with_groupby_in_subquery(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[Date], [t1].[Name], [t1].[Id0], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT [l].[Name]
    FROM [Level1] AS [l]
    GROUP BY [l].[Name]
) AS [t]
LEFT JOIN (
    SELECT [t0].[Id], [t0].[Date], [t0].[Name], [t0].[Id0], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
    FROM (
        SELECT [l0].[Id], [l0].[Date], [l0].[Name], [t2].[Id] AS [Id0], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id], ROW_NUMBER() OVER(PARTITION BY [l0].[Name] ORDER BY [l0].[Id]) AS [row]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t2] ON [l0].[Id] = [t2].[Level1_Optional_Id]
    ) AS [t0]
    WHERE [t0].[row] <= 1
) AS [t1] ON [t].[Name] = [t1].[Name]
""");
    }

    public override async Task SelectMany_navigation_comparison2(bool async)
    {
        await base.SelectMany_navigation_comparison2(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
END AS [Id2]
FROM [Level1] AS [l]
CROSS JOIN (
    SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0]
LEFT JOIN [Level1] AS [l2] ON [t0].[Level1_Optional_Id] = [l2].[Id]
WHERE [l].[Id] = [l2].[Id]
""");
    }

    public override async Task Key_equality_using_property_method_and_member_expression2(bool async)
    {
        await base.Key_equality_using_property_method_and_member_expression2(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 7
""");
    }

    public override async Task Where_navigation_property_to_collection(bool async)
    {
        await base.Where_navigation_property_to_collection(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
WHERE (
    SELECT COUNT(*)
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END = [l1].[OneToMany_Optional_Inverse3Id] OR (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL AND [l1].[OneToMany_Optional_Inverse3Id] IS NULL))) > 0
""");
    }

    public override async Task Select_multiple_nav_prop_optional_required(bool async)
    {
        await base.Select_multiple_nav_prop_optional_required(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
""");
    }

    public override async Task Where_nav_prop_reference_optional1(bool async)
    {
        await base.Where_nav_prop_reference_optional1(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] IN (N'L2 05', N'L2 07')
""");
    }

    public override async Task SelectMany_navigation_property_with_another_navigation_in_subquery(bool async)
    {
        await base.SelectMany_navigation_property_with_another_navigation_in_subquery(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id], [t].[Level2_Optional_Id], [t].[Level2_Required_Id], [t].[Level3_Name], [t].[OneToMany_Optional_Inverse3Id], [t].[OneToMany_Required_Inverse3Id], [t].[OneToOne_Optional_PK_Inverse3Id], [l0].[OneToMany_Optional_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t] ON CASE
        WHEN [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l0].[Id]
    END = [t].[Level2_Optional_Id]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[OneToMany_Optional_Inverse2Id]
""");
    }

    public override async Task Join_navigations_in_inner_selector_translated_without_collision(bool async)
    {
        await base.Join_navigations_in_inner_selector_translated_without_collision(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END AS [Id2], [t0].[Id] AS [Id1], CASE
    WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id1]
END AS [Id3]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
INNER JOIN (
    SELECT [l1].[Id], [t1].[Id] AS [Id0], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Required_Id], [t1].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l1].[Id] = [t1].[Level1_Optional_Id]
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
INNER JOIN (
    SELECT [t4].[Id] AS [Id1], [t4].[Level2_Required_Id], [t4].[OneToMany_Required_Inverse3Id], [t5].[Id] AS [Id2], [t5].[OneToOne_Required_PK_Date] AS [OneToOne_Required_PK_Date0], [t5].[Level1_Required_Id] AS [Level1_Required_Id0], [t5].[OneToMany_Required_Inverse2Id] AS [OneToMany_Required_Inverse2Id0]
    FROM [Level1] AS [l3]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Required_Id], [l4].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [l3].[Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    LEFT JOIN (
        SELECT [l5].[Id], [l5].[Level2_Optional_Id], [l5].[Level2_Required_Id], [l5].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l5]
        WHERE [l5].[Level2_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t4] ON CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END = CASE
        WHEN [t4].[Level2_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t4].[Id]
    END
    LEFT JOIN (
        SELECT [l6].[Id], [l6].[OneToOne_Required_PK_Date], [l6].[Level1_Required_Id], [l6].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l6]
        WHERE [l6].[OneToOne_Required_PK_Date] IS NOT NULL AND [l6].[Level1_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t5] ON [t4].[Level2_Optional_Id] = CASE
        WHEN [t5].[OneToOne_Required_PK_Date] IS NOT NULL AND [t5].[Level1_Required_Id] IS NOT NULL AND [t5].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t5].[Id]
    END
    WHERE [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t4].[Level2_Required_Id] IS NOT NULL AND [t4].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t2] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t2].[OneToOne_Required_PK_Date0] IS NOT NULL AND [t2].[Level1_Required_Id0] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id0] IS NOT NULL THEN [t2].[Id2]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Optional_navigation_inside_nested_method_call_translated_to_join_keeps_original_nullability(bool async)
    {
        await base.Optional_navigation_inside_nested_method_call_translated_to_join_keeps_original_nullability(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE DATEADD(month, CAST(2 AS int), DATEADD(day, CAST(15.0E0 AS int), DATEADD(day, CAST(10.0E0 AS int), [t].[OneToOne_Required_PK_Date]))) > '2002-02-01T00:00:00.0000000'
""");
    }

    public override async Task Result_operator_nav_prop_reference_optional_Average(bool async)
    {
        await base.Result_operator_nav_prop_reference_optional_Average(async);

        AssertSql(
"""
SELECT AVG(CAST([t].[Level1_Required_Id] AS float))
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task GroupJoin_in_subquery_with_client_projection_nested2(bool async)
    {
        await base.GroupJoin_in_subquery_with_client_projection_nested2(async);

        AssertSql(
"""
SELECT [l].[Name]
FROM [Level1] AS [l]
WHERE (
    SELECT COUNT(*)
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[Date], [l1].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        LEFT JOIN (
            SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l2]
            WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l1].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l0].[Id] = [t0].[Level1_Optional_Id]
    WHERE (
        SELECT COUNT(*)
        FROM [Level1] AS [l3]
        LEFT JOIN (
            SELECT [l4].[Id], [l4].[Date], [l4].[Name], [t2].[Id] AS [Id0], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l4]
            LEFT JOIN (
                SELECT [l5].[Id], [l5].[OneToOne_Required_PK_Date], [l5].[Level1_Optional_Id], [l5].[Level1_Required_Id], [l5].[Level2_Name], [l5].[OneToMany_Optional_Inverse2Id], [l5].[OneToMany_Required_Inverse2Id], [l5].[OneToOne_Optional_PK_Inverse2Id]
                FROM [Level1] AS [l5]
                WHERE [l5].[OneToOne_Required_PK_Date] IS NOT NULL AND [l5].[Level1_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse2Id] IS NOT NULL
            ) AS [t2] ON [l4].[Id] = CASE
                WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
            END
            WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t1] ON [l3].[Id] = [t1].[Level1_Optional_Id]) > 7) > 4 AND [l].[Id] < 2
""");
    }

    public override async Task Complex_navigations_with_predicate_projected_into_anonymous_type2(bool async)
    {
        await base.Complex_navigations_with_predicate_projected_into_anonymous_type2(async);

        AssertSql(
"""
SELECT [t0].[Level3_Name] AS [Name], [l4].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN [Level1] AS [l3] ON [t1].[Level1_Required_Id] = [l3].[Id]
LEFT JOIN [Level1] AS [l4] ON [t1].[Level1_Optional_Id] = [l4].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND ([l3].[Id] = [l4].[Id] OR ([l3].[Id] IS NULL AND [l4].[Id] IS NULL)) AND ([l4].[Id] <> 7 OR [l4].[Id] IS NULL)
""");
    }

    public override async Task Select_subquery_with_client_eval_and_navigation1(bool async)
    {
        await base.Select_subquery_with_client_eval_and_navigation1(async);

        AssertSql(
"""
SELECT (
    SELECT TOP(1) [l2].[Name]
    FROM [Level1] AS [l1]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l1].[Id] = CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END
    LEFT JOIN [Level1] AS [l2] ON [t0].[Level1_Required_Id] = [l2].[Id]
    WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
    END)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task String_include_multiple_derived_collection_navigation_with_same_name_and_different_type(bool async)
    {
        await base.String_include_multiple_derived_collection_navigation_with_same_name_and_different_type(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[Discriminator], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1], [i].[Name], [i0].[Id], [i0].[DifferentTypeCollection_InheritanceDerived1Id], [i0].[DifferentTypeReference_InheritanceDerived1Id], [i0].[InheritanceDerived2Id], [i0].[Name], [i0].[SameTypeCollection_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived2Id], [i1].[Id], [i1].[DifferentTypeReference_InheritanceDerived2Id], [i1].[InheritanceDerived2Id], [i1].[Name]
FROM [InheritanceOne] AS [i]
LEFT JOIN [InheritanceLeafOne] AS [i0] ON [i].[Id] = [i0].[DifferentTypeCollection_InheritanceDerived1Id]
LEFT JOIN [InheritanceLeafTwo] AS [i1] ON [i].[Id] = [i1].[InheritanceDerived2Id]
ORDER BY [i].[Id], [i0].[Id]
""");
    }

    public override async Task Include_reference_and_project_into_anonymous_type(bool async)
    {
        await base.Include_reference_and_project_into_anonymous_type(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name], [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Explicit_GroupJoin_in_subquery_with_scalar_result_operator(bool async)
    {
        await base.Explicit_GroupJoin_in_subquery_with_scalar_result_operator(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
WHERE (
    SELECT COUNT(*)
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[Date], [l1].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        LEFT JOIN (
            SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l2]
            WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t] ON [l1].[Id] = CASE
            WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
        END
        WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t0] ON [l0].[Id] = [t0].[Level1_Optional_Id]) > 4
""");
    }

    public override async Task Nav_rewrite_doesnt_apply_null_protection_for_function_arguments(bool async)
    {
        await base.Nav_rewrite_doesnt_apply_null_protection_for_function_arguments(async);

        AssertSql(
"""
SELECT [t].[Level1_Required_Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToOne_Optional_PK_Inverse2Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Result_operator_nav_prop_reference_optional_Min(bool async)
    {
        await base.Result_operator_nav_prop_reference_optional_Min(async);

        AssertSql(
"""
SELECT MIN([t].[Level1_Required_Id])
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Entries_for_detached_entities_are_removed(bool async)
    {
        await base.Entries_for_detached_entities_are_removed(async);

        AssertSql(
"""
SELECT TOP(1) [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
ORDER BY CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
""");
    }

    public override async Task Navigation_inside_method_call_translated_to_join2(bool async)
    {
        await base.Navigation_inside_method_call_translated_to_join2(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND [t1].[Level2_Name] IS NOT NULL AND [t1].[Level2_Name] LIKE N'L%'
""");
    }

    public override async Task Select_nav_prop_reference_optional1_via_DefaultIfEmpty(bool async)
    {
        await base.Select_nav_prop_reference_optional1_via_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [t0].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task Where_multiple_nav_prop_reference_optional_compared_to_null3(bool async)
    {
        await base.Where_multiple_nav_prop_reference_optional_compared_to_null3(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
WHERE [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Navigations_compared_to_each_other1(bool async)
    {
        await base.Navigations_compared_to_each_other1(async);

        AssertSql(
"""
SELECT [t].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[OneToMany_Required_Inverse2Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND ([l1].[Id] = [l1].[Id] OR [l1].[Id] IS NULL)
""");
    }

    public override async Task Optional_navigation_in_subquery_with_unrelated_projection(bool async)
    {
        await base.Optional_navigation_in_subquery_with_unrelated_projection(async);

        AssertSql(
"""
@__p_0='15'

SELECT TOP(@__p_0) [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level2_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE [t].[Level2_Name] <> N'Foo' OR [t].[Level2_Name] IS NULL
ORDER BY [l].[Id]
""");
    }

    public override async Task Correlated_subquery_doesnt_project_unnecessary_columns_in_top_level(bool async)
    {
        await base.Correlated_subquery_doesnt_project_unnecessary_columns_in_top_level(async);

        AssertSql(
"""
SELECT DISTINCT [l].[Name]
FROM [Level1] AS [l]
WHERE EXISTS (
    SELECT 1
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t].[Level1_Required_Id] = [l].[Id])
""");
    }

    public override async Task Select_subquery_with_client_eval_and_navigation2(bool async)
    {
        await base.Select_subquery_with_client_eval_and_navigation2(async);

        AssertSql(
"""
SELECT CASE
    WHEN (
        SELECT TOP(1) [l2].[Name]
        FROM [Level1] AS [l1]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l1].[Id] = CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END
        LEFT JOIN [Level1] AS [l2] ON [t0].[Level1_Required_Id] = [l2].[Id]
        WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ORDER BY CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END) = N'L1 02' AND (
        SELECT TOP(1) [l2].[Name]
        FROM [Level1] AS [l1]
        LEFT JOIN (
            SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Optional_Inverse2Id], [l3].[OneToMany_Required_Inverse2Id], [l3].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l3]
            WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l1].[Id] = CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END
        LEFT JOIN [Level1] AS [l2] ON [t0].[Level1_Required_Id] = [l2].[Id]
        WHERE [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ORDER BY CASE
            WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id]
        END) IS NOT NULL THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
""");
    }

    public override async Task Include18_1(bool async)
    {
        await base.Include18_1(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[Date], [t].[Name], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT DISTINCT [l].[Id], [l].[Date], [l].[Name]
    FROM [Level1] AS [l]
) AS [t]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [t].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task Include18_4(bool async)
    {
        await base.Include18_4(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[Date], [t].[Name], [t0].[Id], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT DISTINCT [l].[Id], [l].[Date], [l].[Name]
    FROM [Level1] AS [l]
) AS [t]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [t].[Id] = [t0].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t].[Id] = [t1].[Level1_Optional_Id]
""");
    }

    public override async Task Result_operator_nav_prop_reference_optional_Average_without_selector(bool async)
    {
        await base.Result_operator_nav_prop_reference_optional_Average_without_selector(async);

        AssertSql(
"""
SELECT AVG(CAST([t].[Level1_Required_Id] AS float))
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
""");
    }

    public override async Task Order_by_key_of_anonymous_type_projected_navigation_doesnt_get_optimized_into_FK_access_subquery(
        bool async)
    {
        await base.Order_by_key_of_anonymous_type_projected_navigation_doesnt_get_optimized_into_FK_access_subquery(async);

        AssertSql(
"""
@__p_0='10'

SELECT TOP(@__p_0) [t1].[Level2_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
ORDER BY CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
""");
    }

    public override async Task Where_nav_prop_reference_optional1_via_DefaultIfEmpty(bool async)
    {
        await base.Where_nav_prop_reference_optional1_via_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [t2].[Level1_Optional_Id], [t2].[Level2_Name]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[Level2_Name], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [l].[Id] = [t1].[Level1_Optional_Id]
WHERE [t0].[Level2_Name] = N'L2 05' OR [t1].[Level2_Name] = N'L2 07'
""");
    }

    public override async Task Join_navigation_key_access_required(bool async)
    {
        await base.Join_navigation_key_access_required(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id], [l1].[Id] AS [Id1]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Id1]
""");
    }

    public override async Task Include_with_all_method_include_gets_ignored(bool async)
    {
        await base.Include_with_all_method_include_gets_ignored(async);

        AssertSql(
"""
SELECT CASE
    WHEN NOT EXISTS (
        SELECT 1
        FROM [Level1] AS [l]
        WHERE [l].[Name] = N'Foo' AND [l].[Name] IS NOT NULL) THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END
""");
    }

    public override async Task Include18_3_2(bool async)
    {
        await base.Include18_3_2(async);

        AssertSql(
"""
@__p_0='10'

SELECT [t0].[Id], [t0].[Date], [t0].[Name], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [l].[Date], [l].[Name], [t].[Level2_Name]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Level1_Required_Id], [l0].[Level2_Name]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
    ORDER BY [t].[Level2_Name]
) AS [t0]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Id] = [t1].[Level1_Optional_Id]
ORDER BY [t0].[Level2_Name]
""");
    }

    public override async Task Select_nav_prop_reference_optional2_via_DefaultIfEmpty(bool async)
    {
        await base.Select_nav_prop_reference_optional2_via_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access1(bool async)
    {
        await base.Order_by_key_of_projected_navigation_doesnt_get_optimized_into_FK_access1(async);

        AssertSql(
"""
SELECT [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
ORDER BY CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
""");
    }

    public override async Task Where_complex_predicate_with_with_nav_prop_and_OrElse4(bool async)
    {
        await base.Where_complex_predicate_with_with_nav_prop_and_OrElse4(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
LEFT JOIN (
    SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Level2_Optional_Id] = CASE
    WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
END
LEFT JOIN (
    SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Optional_Id], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l3]
    WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t2] ON [t0].[Level2_Required_Id] = CASE
    WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
END
LEFT JOIN [Level1] AS [l4] ON [t2].[Level1_Optional_Id] = [l4].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND ([t1].[Level2_Name] <> N'L2 05' OR [t1].[Level2_Name] IS NULL OR [l4].[Name] = N'L1 05')
""");
    }

    public override async Task String_include_multiple_derived_navigation_with_same_name_and_same_type(bool async)
    {
        await base.String_include_multiple_derived_navigation_with_same_name_and_same_type(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[Discriminator], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1], [i].[Name], [i0].[Id], [i0].[DifferentTypeCollection_InheritanceDerived1Id], [i0].[DifferentTypeReference_InheritanceDerived1Id], [i0].[InheritanceDerived2Id], [i0].[Name], [i0].[SameTypeCollection_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived1Id], [i0].[SameTypeReference_InheritanceDerived2Id], [i1].[Id], [i1].[DifferentTypeCollection_InheritanceDerived1Id], [i1].[DifferentTypeReference_InheritanceDerived1Id], [i1].[InheritanceDerived2Id], [i1].[Name], [i1].[SameTypeCollection_InheritanceDerived1Id], [i1].[SameTypeReference_InheritanceDerived1Id], [i1].[SameTypeReference_InheritanceDerived2Id]
FROM [InheritanceOne] AS [i]
LEFT JOIN [InheritanceLeafOne] AS [i0] ON [i].[Id] = [i0].[SameTypeReference_InheritanceDerived1Id]
LEFT JOIN [InheritanceLeafOne] AS [i1] ON [i].[Id] = [i1].[SameTypeReference_InheritanceDerived2Id]
""");
    }

    public override async Task Select_subquery_with_client_eval_and_multi_level_navigation(bool async)
    {
        await base.Select_subquery_with_client_eval_and_multi_level_navigation(async);

        AssertSql(
"""
SELECT (
    SELECT TOP(1) [l3].[Name]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[OneToOne_Required_PK_Date], [l4].[Level1_Optional_Id], [l4].[Level1_Required_Id], [l4].[Level2_Name], [l4].[OneToMany_Optional_Inverse2Id], [l4].[OneToMany_Required_Inverse2Id], [l4].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[OneToOne_Required_PK_Date] IS NOT NULL AND [l4].[Level1_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t1] ON [l2].[Id] = CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END
    LEFT JOIN (
        SELECT [l5].[Id], [l5].[Level2_Optional_Id], [l5].[Level2_Required_Id], [l5].[Level3_Name], [l5].[OneToMany_Optional_Inverse3Id], [l5].[OneToMany_Required_Inverse3Id], [l5].[OneToOne_Optional_PK_Inverse3Id]
        FROM [Level1] AS [l5]
        WHERE [l5].[Level2_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t2] ON CASE
        WHEN [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t1].[Id]
    END = CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l6].[Id], [l6].[OneToOne_Required_PK_Date], [l6].[Level1_Optional_Id], [l6].[Level1_Required_Id], [l6].[Level2_Name], [l6].[OneToMany_Optional_Inverse2Id], [l6].[OneToMany_Required_Inverse2Id], [l6].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l6]
        WHERE [l6].[OneToOne_Required_PK_Date] IS NOT NULL AND [l6].[Level1_Required_Id] IS NOT NULL AND [l6].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t3] ON [t2].[Level2_Required_Id] = CASE
        WHEN [t3].[OneToOne_Required_PK_Date] IS NOT NULL AND [t3].[Level1_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t3].[Id]
    END
    LEFT JOIN [Level1] AS [l3] ON [t3].[Level1_Required_Id] = [l3].[Id]
    WHERE [t1].[OneToOne_Required_PK_Date] IS NOT NULL AND [t1].[Level1_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ORDER BY CASE
        WHEN [t2].[Level2_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t2].[Id]
    END)
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL
""");
    }

    public override async Task Optional_navigation_take_optional_navigation(bool async)
    {
        await base.Optional_navigation_take_optional_navigation(async);

        AssertSql(
"""
@__p_0='10'

SELECT [t1].[Level3_Name]
FROM (
    SELECT TOP(@__p_0) [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id], CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END AS [c]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
    ORDER BY CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
) AS [t0]
LEFT JOIN (
    SELECT [l1].[Level2_Optional_Id], [l1].[Level3_Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END = [t1].[Level2_Optional_Id]
ORDER BY [t0].[c]
""");
    }

    public override async Task SelectMany_nested_navigation_property_optional_and_projection(bool async)
    {
        await base.SelectMany_nested_navigation_property_optional_and_projection(async);

        AssertSql(
"""
SELECT [t0].[Level3_Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
INNER JOIN (
    SELECT [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
""");
    }

    public override async Task Include18_3(bool async)
    {
        await base.Include18_3(async);

        AssertSql(
"""
@__p_0='10'

SELECT [t0].[Id], [t0].[Date], [t0].[Name], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [l].[Date], [l].[Name], [t].[Level2_Name]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Level1_Required_Id], [l0].[Level2_Name]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
    ORDER BY [t].[Level2_Name]
) AS [t0]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Id] = [t1].[Level1_Optional_Id]
ORDER BY [t0].[Level2_Name]
""");
    }

    public override async Task Navigation_with_same_navigation_compared_to_null(bool async)
    {
        await base.Navigation_with_same_navigation_compared_to_null(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[OneToMany_Required_Inverse2Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND ([l1].[Name] <> N'L1 07' OR [l1].[Name] IS NULL) AND [l1].[Id] IS NOT NULL
""");
    }

    public override async Task Explicit_GroupJoin_in_subquery_with_unrelated_projection(bool async)
    {
        await base.Explicit_GroupJoin_in_subquery_with_unrelated_projection(async);

        AssertSql(
"""
@__p_0='15'

SELECT TOP(@__p_0) [l].[Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Level1_Optional_Id], [t].[Level2_Name]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
WHERE [t0].[Level2_Name] <> N'Foo' OR [t0].[Level2_Name] IS NULL
ORDER BY [l].[Id]
""");
    }

    public override async Task Navigation_key_access_required_comparison(bool async)
    {
        await base.Navigation_key_access_required_comparison(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] > 5
""");
    }

    public override async Task SelectMany_navigation_comparison1(bool async)
    {
        await base.SelectMany_navigation_comparison1(async);

        AssertSql(
"""
SELECT [l].[Id] AS [Id1], [l0].[Id] AS [Id2]
FROM [Level1] AS [l]
CROSS JOIN [Level1] AS [l0]
WHERE [l].[Id] = [l0].[Id]
""");
    }

    public override async Task GroupJoin_in_subquery_with_client_result_operator(bool async)
    {
        await base.GroupJoin_in_subquery_with_client_result_operator(async);

        AssertSql(
"""
SELECT [l].[Name]
FROM [Level1] AS [l]
WHERE (
    SELECT COUNT(*)
    FROM (
        SELECT DISTINCT [l0].[Id], [l0].[Date], [l0].[Name]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[Date], [l1].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            LEFT JOIN (
                SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
                FROM [Level1] AS [l2]
                WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
            ) AS [t] ON [l1].[Id] = CASE
                WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
            END
            WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l0].[Id] = [t0].[Level1_Optional_Id]
    ) AS [t1]) > 7 AND [l].[Id] < 3
""");
    }

    public override async Task Include18_3_1(bool async)
    {
        await base.Include18_3_1(async);

        AssertSql(
"""
@__p_0='10'

SELECT [t0].[Id], [t0].[Date], [t0].[Name], [t1].[Id], [t1].[OneToOne_Required_PK_Date], [t1].[Level1_Optional_Id], [t1].[Level1_Required_Id], [t1].[Level2_Name], [t1].[OneToMany_Optional_Inverse2Id], [t1].[OneToMany_Required_Inverse2Id], [t1].[OneToOne_Optional_PK_Inverse2Id]
FROM (
    SELECT TOP(@__p_0) [l].[Id], [l].[Date], [l].[Name], [t].[Level2_Name]
    FROM [Level1] AS [l]
    LEFT JOIN (
        SELECT [l0].[Level1_Required_Id], [l0].[Level2_Name]
        FROM [Level1] AS [l0]
        WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l].[Id] = [t].[Level1_Required_Id]
    ORDER BY [t].[Level2_Name]
) AS [t0]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1] ON [t0].[Id] = [t1].[Level1_Optional_Id]
ORDER BY [t0].[Level2_Name]
""");
    }

    public override async Task Key_equality_two_conditions_on_same_navigation2(bool async)
    {
        await base.Key_equality_two_conditions_on_same_navigation2(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END
LEFT JOIN [Level1] AS [l1] ON [t].[Level1_Required_Id] = [l1].[Id]
WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l1].[Id] IN (1, 2)
""");
    }

    public override async Task Entity_equality_empty(bool async)
    {
        await base.Entity_equality_empty(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 0
""");
    }

    public override async Task Member_pushdown_with_multiple_collections(bool async)
    {
        await base.Member_pushdown_with_multiple_collections(async);

        AssertSql(
"""
SELECT (
    SELECT TOP(1) [l0].[Level3_Name]
    FROM [Level1] AS [l0]
    WHERE [l0].[Level2_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse3Id] IS NOT NULL AND (
        SELECT TOP(1) CASE
            WHEN [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l1].[Id]
        END
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l1].[OneToMany_Optional_Inverse2Id]
        ORDER BY CASE
            WHEN [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l1].[Id]
        END) IS NOT NULL AND ((
        SELECT TOP(1) CASE
            WHEN [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l2].[Id]
        END
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l2].[OneToMany_Optional_Inverse2Id]
        ORDER BY CASE
            WHEN [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l2].[Id]
        END) = [l0].[OneToMany_Optional_Inverse3Id] OR ((
        SELECT TOP(1) CASE
            WHEN [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l2].[Id]
        END
        FROM [Level1] AS [l2]
        WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [l2].[OneToMany_Optional_Inverse2Id]
        ORDER BY CASE
            WHEN [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [l2].[Id]
        END) IS NULL AND [l0].[OneToMany_Optional_Inverse3Id] IS NULL))
    ORDER BY CASE
        WHEN [l0].[Level2_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [l0].[Id]
    END)
FROM [Level1] AS [l]
""");
    }

    public override async Task Join_flattening_bug_4539(bool async)
    {
        await base.Join_flattening_bug_4539(async);

        AssertSql(
"""
SELECT [t0].[Id0], [t0].[OneToOne_Required_PK_Date], [t0].[Level1_Optional_Id], [t0].[Level1_Required_Id], [t0].[Level2_Name], [t0].[OneToMany_Optional_Inverse2Id], [t0].[OneToMany_Required_Inverse2Id], [t0].[OneToOne_Optional_PK_Inverse2Id], [l4].[Id], [l4].[Date], [l4].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
CROSS JOIN (
    SELECT [t2].[Level1_Required_Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t1]
INNER JOIN [Level1] AS [l4] ON [t1].[Level1_Required_Id] = [l4].[Id]
""");
    }

    public override async Task Multi_level_include_with_short_circuiting(bool async)
    {
        await base.Multi_level_include_with_short_circuiting(async);

        AssertSql(
"""
SELECT [f].[Name], [f].[LabelDefaultText], [f].[PlaceholderDefaultText], [m].[DefaultText], [m0].[DefaultText], [t].[Text], [t].[ComplexNavigationStringDefaultText], [t].[LanguageName], [t].[Name], [t].[CultureString], [t0].[Text], [t0].[ComplexNavigationStringDefaultText], [t0].[LanguageName], [t0].[Name], [t0].[CultureString]
FROM [Fields] AS [f]
LEFT JOIN [MultilingualStrings] AS [m] ON [f].[LabelDefaultText] = [m].[DefaultText]
LEFT JOIN [MultilingualStrings] AS [m0] ON [f].[PlaceholderDefaultText] = [m0].[DefaultText]
LEFT JOIN (
    SELECT [g].[Text], [g].[ComplexNavigationStringDefaultText], [g].[LanguageName], [l].[Name], [l].[CultureString]
    FROM [Globalizations] AS [g]
    LEFT JOIN [Languages] AS [l] ON [g].[LanguageName] = [l].[Name]
) AS [t] ON [m].[DefaultText] = [t].[ComplexNavigationStringDefaultText]
LEFT JOIN (
    SELECT [g0].[Text], [g0].[ComplexNavigationStringDefaultText], [g0].[LanguageName], [l0].[Name], [l0].[CultureString]
    FROM [Globalizations] AS [g0]
    LEFT JOIN [Languages] AS [l0] ON [g0].[LanguageName] = [l0].[Name]
) AS [t0] ON [m0].[DefaultText] = [t0].[ComplexNavigationStringDefaultText]
ORDER BY [f].[Name], [m].[DefaultText], [m0].[DefaultText], [t].[Text], [t].[Name], [t0].[Text]
""");
    }

    public override async Task Select_multiple_nav_prop_reference_optional(bool async)
    {
        await base.Select_multiple_nav_prop_reference_optional(async);

        AssertSql(
"""
SELECT CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
""");
    }

    public override async Task Where_multiple_nav_prop_reference_optional_member_compared_to_null(bool async)
    {
        await base.Where_multiple_nav_prop_reference_optional_member_compared_to_null(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [l1].[Level2_Optional_Id], [l1].[Level3_Name]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Optional_Id]
WHERE [t0].[Level3_Name] IS NOT NULL
""");
    }

    public override async Task GroupJoin_in_subquery_with_client_projection_nested1(bool async)
    {
        await base.GroupJoin_in_subquery_with_client_projection_nested1(async);

        AssertSql(
"""
SELECT [l].[Name]
FROM [Level1] AS [l]
WHERE (
    SELECT COUNT(*)
    FROM (
        SELECT TOP(10) [l0].[Id], [t0].[Id] AS [Id0], [t0].[Id0] AS [Id00]
        FROM [Level1] AS [l0]
        LEFT JOIN (
            SELECT [l1].[Id], [l1].[Date], [l1].[Name], [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id]
            FROM [Level1] AS [l1]
            LEFT JOIN (
                SELECT [l2].[Id], [l2].[OneToOne_Required_PK_Date], [l2].[Level1_Optional_Id], [l2].[Level1_Required_Id], [l2].[Level2_Name], [l2].[OneToMany_Optional_Inverse2Id], [l2].[OneToMany_Required_Inverse2Id], [l2].[OneToOne_Optional_PK_Inverse2Id]
                FROM [Level1] AS [l2]
                WHERE [l2].[OneToOne_Required_PK_Date] IS NOT NULL AND [l2].[Level1_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse2Id] IS NOT NULL
            ) AS [t] ON [l1].[Id] = CASE
                WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
            END
            WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL
        ) AS [t0] ON [l0].[Id] = [t0].[Level1_Optional_Id]
        WHERE (
            SELECT COUNT(*)
            FROM [Level1] AS [l3]
            LEFT JOIN (
                SELECT [l4].[Id], [l4].[Date], [l4].[Name], [t2].[Id] AS [Id0], [t2].[OneToOne_Required_PK_Date], [t2].[Level1_Optional_Id], [t2].[Level1_Required_Id], [t2].[Level2_Name], [t2].[OneToMany_Optional_Inverse2Id], [t2].[OneToMany_Required_Inverse2Id], [t2].[OneToOne_Optional_PK_Inverse2Id]
                FROM [Level1] AS [l4]
                LEFT JOIN (
                    SELECT [l5].[Id], [l5].[OneToOne_Required_PK_Date], [l5].[Level1_Optional_Id], [l5].[Level1_Required_Id], [l5].[Level2_Name], [l5].[OneToMany_Optional_Inverse2Id], [l5].[OneToMany_Required_Inverse2Id], [l5].[OneToOne_Optional_PK_Inverse2Id]
                    FROM [Level1] AS [l5]
                    WHERE [l5].[OneToOne_Required_PK_Date] IS NOT NULL AND [l5].[Level1_Required_Id] IS NOT NULL AND [l5].[OneToMany_Required_Inverse2Id] IS NOT NULL
                ) AS [t2] ON [l4].[Id] = CASE
                    WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
                END
                WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL
            ) AS [t1] ON [l3].[Id] = [t1].[Level1_Optional_Id]) > 7
        ORDER BY [l0].[Id]
    ) AS [t3]) > 4 AND [l].[Id] < 2
""");
    }

    public override async Task Key_equality_when_sentinel_ef_property(bool async)
    {
        await base.Key_equality_when_sentinel_ef_property(async);

        AssertSql(
"""
SELECT [l].[Id], [l].[Date], [l].[Name]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[Level1_Optional_Id]
WHERE CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = 0
""");
    }

    public override async Task Project_shadow_properties1(bool async)
    {
        await base.Project_shadow_properties1(async);

        AssertSql();
    }

    public override async Task Project_shadow_properties2(bool async)
    {
        await base.Project_shadow_properties2(async);

        AssertSql();
    }

    public override async Task Project_shadow_properties3(bool async)
    {
        await base.Project_shadow_properties3(async);

        AssertSql();
    }

    public override async Task Project_shadow_properties4(bool async)
    {
        await base.Project_shadow_properties4(async);

        AssertSql();
    }

    public override async Task Project_shadow_properties9(bool async)
    {
        await base.Project_shadow_properties9(async);

        AssertSql();
    }

    public override async Task GroupJoin_on_a_subquery_containing_another_GroupJoin_projecting_outer_with_client_method(bool async)
    {
        await base.GroupJoin_on_a_subquery_containing_another_GroupJoin_projecting_outer_with_client_method(async);

        AssertSql();
    }

    public override async Task GroupJoin_client_method_in_OrderBy(bool async)
    {
        await AssertTranslationFailedWithDetails(
            () => base.GroupJoin_client_method_in_OrderBy(async),
            CoreStrings.QueryUnableToTranslateMethod(
                "Microsoft.EntityFrameworkCore.Query.ComplexNavigationsQueryTestBase<Microsoft.EntityFrameworkCore.Query.ComplexNavigationsSharedTypeQuerySqlServerFixture>",
                "ClientMethodNullableInt"));

        AssertSql();
    }

    public override async Task GroupJoin_with_subquery_on_inner(bool async)
    {
        await base.GroupJoin_with_subquery_on_inner(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
OUTER APPLY (
    SELECT TOP(10) [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [l0].[Id] AS [Id0], CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END AS [c]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [t].[Level1_Optional_Id] AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END > 0
    ORDER BY CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
) AS [t0]
""");
    }

    public override async Task GroupJoin_with_subquery_on_inner_and_no_DefaultIfEmpty(bool async)
    {
        await base.GroupJoin_with_subquery_on_inner_and_no_DefaultIfEmpty(async);

        AssertSql(
"""
SELECT [l].[Id]
FROM [Level1] AS [l]
CROSS APPLY (
    SELECT TOP(10) [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [l0].[Id] AS [Id0], CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END AS [c]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[Level2_Name], [l1].[OneToMany_Optional_Inverse2Id], [l1].[OneToMany_Required_Inverse2Id], [l1].[OneToOne_Optional_PK_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [l].[Id] = [t].[Level1_Optional_Id] AND CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END > 0
    ORDER BY CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
) AS [t0]
""");
    }

    public override async Task Level4_Include(bool async)
    {
        await base.Level4_Include(async);

        AssertSql();
    }

    public override async Task Include17(bool async)
    {
        await base.Include17(async);

        AssertSql();
    }

    public override async Task Multiple_required_navigations_with_Include(bool async)
    {
        await base.Multiple_required_navigations_with_Include(async);

        AssertSql();
    }

    public override async Task Multiple_required_navigation_using_multiple_selects_with_Include(bool async)
    {
        await base.Multiple_required_navigation_using_multiple_selects_with_Include(async);

        AssertSql();
    }

    public override async Task Multiple_required_navigation_with_string_based_Include(bool async)
    {
        await base.Multiple_required_navigation_with_string_based_Include(async);

        AssertSql();
    }

    public override async Task Multiple_required_navigation_using_multiple_selects_with_string_based_Include(bool async)
    {
        await base.Multiple_required_navigation_using_multiple_selects_with_string_based_Include(async);

        AssertSql();
    }

    public override async Task Optional_navigation_with_Include(bool async)
    {
        await base.Optional_navigation_with_Include(async);

        AssertSql();
    }

    public override async Task Project_shadow_properties8(bool async)
    {
        await base.Project_shadow_properties8(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[InheritanceLeaf2Id]
FROM [InheritanceTwo] AS [i]
""");
    }

    public override async Task Project_shadow_properties10(bool async)
    {
        await base.Project_shadow_properties10(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[DifferentTypeReference_InheritanceDerived2Id], [i].[InheritanceDerived2Id]
FROM [InheritanceLeafTwo] AS [i]
""");
    }

    public override async Task Project_shadow_properties7(bool async)
    {
        await base.Project_shadow_properties7(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1]
FROM [InheritanceOne] AS [i]
WHERE [i].[Discriminator] = N'InheritanceDerived2'
""");
    }

    public override async Task Project_shadow_properties6(bool async)
    {
        await base.Project_shadow_properties6(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1]
FROM [InheritanceOne] AS [i]
WHERE [i].[Discriminator] = N'InheritanceDerived1'
""");
    }

    public override async Task String_include_multiple_derived_navigations_complex(bool async)
    {
        await base.String_include_multiple_derived_navigations_complex(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[InheritanceLeaf2Id], [i].[Name], [i0].[Id], [i0].[Discriminator], [i0].[InheritanceBase2Id], [i0].[InheritanceBase2Id1], [i0].[Name], [i1].[Id], [i1].[DifferentTypeCollection_InheritanceDerived1Id], [i1].[DifferentTypeReference_InheritanceDerived1Id], [i1].[InheritanceDerived2Id], [i1].[Name], [i1].[SameTypeCollection_InheritanceDerived1Id], [i1].[SameTypeReference_InheritanceDerived1Id], [i1].[SameTypeReference_InheritanceDerived2Id], [i2].[Id], [i2].[DifferentTypeReference_InheritanceDerived2Id], [i2].[InheritanceDerived2Id], [i2].[Name], [t].[Id], [t].[Discriminator], [t].[InheritanceBase2Id], [t].[InheritanceBase2Id1], [t].[Name], [t].[Id0], [t].[DifferentTypeCollection_InheritanceDerived1Id], [t].[DifferentTypeReference_InheritanceDerived1Id], [t].[InheritanceDerived2Id], [t].[Name0], [t].[SameTypeCollection_InheritanceDerived1Id], [t].[SameTypeReference_InheritanceDerived1Id], [t].[SameTypeReference_InheritanceDerived2Id], [t].[Id1], [t].[DifferentTypeCollection_InheritanceDerived1Id0], [t].[DifferentTypeReference_InheritanceDerived1Id0], [t].[InheritanceDerived2Id0], [t].[Name1], [t].[SameTypeCollection_InheritanceDerived1Id0], [t].[SameTypeReference_InheritanceDerived1Id0], [t].[SameTypeReference_InheritanceDerived2Id0]
FROM [InheritanceTwo] AS [i]
LEFT JOIN [InheritanceOne] AS [i0] ON [i].[Id] = [i0].[InheritanceBase2Id]
LEFT JOIN [InheritanceLeafOne] AS [i1] ON [i0].[Id] = [i1].[DifferentTypeCollection_InheritanceDerived1Id]
LEFT JOIN [InheritanceLeafTwo] AS [i2] ON [i0].[Id] = [i2].[InheritanceDerived2Id]
LEFT JOIN (
    SELECT [i3].[Id], [i3].[Discriminator], [i3].[InheritanceBase2Id], [i3].[InheritanceBase2Id1], [i3].[Name], [i4].[Id] AS [Id0], [i4].[DifferentTypeCollection_InheritanceDerived1Id], [i4].[DifferentTypeReference_InheritanceDerived1Id], [i4].[InheritanceDerived2Id], [i4].[Name] AS [Name0], [i4].[SameTypeCollection_InheritanceDerived1Id], [i4].[SameTypeReference_InheritanceDerived1Id], [i4].[SameTypeReference_InheritanceDerived2Id], [i5].[Id] AS [Id1], [i5].[DifferentTypeCollection_InheritanceDerived1Id] AS [DifferentTypeCollection_InheritanceDerived1Id0], [i5].[DifferentTypeReference_InheritanceDerived1Id] AS [DifferentTypeReference_InheritanceDerived1Id0], [i5].[InheritanceDerived2Id] AS [InheritanceDerived2Id0], [i5].[Name] AS [Name1], [i5].[SameTypeCollection_InheritanceDerived1Id] AS [SameTypeCollection_InheritanceDerived1Id0], [i5].[SameTypeReference_InheritanceDerived1Id] AS [SameTypeReference_InheritanceDerived1Id0], [i5].[SameTypeReference_InheritanceDerived2Id] AS [SameTypeReference_InheritanceDerived2Id0]
    FROM [InheritanceOne] AS [i3]
    LEFT JOIN [InheritanceLeafOne] AS [i4] ON [i3].[Id] = [i4].[SameTypeReference_InheritanceDerived1Id]
    LEFT JOIN [InheritanceLeafOne] AS [i5] ON [i3].[Id] = [i5].[SameTypeReference_InheritanceDerived2Id]
) AS [t] ON [i].[Id] = [t].[InheritanceBase2Id1]
ORDER BY [i].[Id], [i0].[Id], [i1].[Id], [i2].[Id], [t].[Id], [t].[Id0]
""");
    }

    public override async Task Project_shadow_properties5(bool async)
    {
        await base.Project_shadow_properties5(async);

        AssertSql(
"""
SELECT [i].[Id], [i].[InheritanceBase2Id], [i].[InheritanceBase2Id1]
FROM [InheritanceOne] AS [i]
""");
    }

    public override async Task Multiple_required_navigation_using_multiple_selects_with_EF_Property_Include(bool async)
    {
        await base.Multiple_required_navigation_using_multiple_selects_with_EF_Property_Include(async);

        AssertSql();
    }

    public override async Task SelectMany_with_EF_Property_Include1(bool async)
    {
        await base.SelectMany_with_EF_Property_Include1(async);

        AssertSql(
"""
SELECT [t].[Id], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[Level2_Name], [t].[OneToMany_Optional_Inverse2Id], [t].[OneToMany_Required_Inverse2Id], [t].[OneToOne_Optional_PK_Inverse2Id], [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Optional_Id], [l0].[Level1_Required_Id], [l0].[Level2_Name], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id], [l0].[OneToOne_Optional_PK_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
LEFT JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[Level2_Required_Id]
""");
    }

    public override async Task Multiple_SelectMany_with_EF_Property_Include(bool async)
    {
        await base.Multiple_SelectMany_with_EF_Property_Include(async);

        AssertSql(
"""
SELECT [t0].[Id], [t0].[Level2_Optional_Id], [t0].[Level2_Required_Id], [t0].[Level3_Name], [t0].[OneToMany_Optional_Inverse3Id], [t0].[OneToMany_Required_Inverse3Id], [t0].[OneToOne_Optional_PK_Inverse3Id], [t1].[Id], [t1].[Level3_Optional_Id], [t1].[Level3_Required_Id], [t1].[Level4_Name], [t1].[OneToMany_Optional_Inverse4Id], [t1].[OneToMany_Required_Inverse4Id], [t1].[OneToOne_Optional_PK_Inverse4Id]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [l0].[Id], [l0].[OneToOne_Required_PK_Date], [l0].[Level1_Required_Id], [l0].[OneToMany_Optional_Inverse2Id], [l0].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    WHERE [l0].[OneToOne_Required_PK_Date] IS NOT NULL AND [l0].[Level1_Required_Id] IS NOT NULL AND [l0].[OneToMany_Required_Inverse2Id] IS NOT NULL
) AS [t] ON [l].[Id] = [t].[OneToMany_Optional_Inverse2Id]
INNER JOIN (
    SELECT [l1].[Id], [l1].[Level2_Optional_Id], [l1].[Level2_Required_Id], [l1].[Level3_Name], [l1].[OneToMany_Optional_Inverse3Id], [l1].[OneToMany_Required_Inverse3Id], [l1].[OneToOne_Optional_PK_Inverse3Id]
    FROM [Level1] AS [l1]
    WHERE [l1].[Level2_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse3Id] IS NOT NULL
) AS [t0] ON CASE
    WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
END = [t0].[OneToMany_Optional_Inverse3Id]
LEFT JOIN (
    SELECT [l2].[Id], [l2].[Level3_Optional_Id], [l2].[Level3_Required_Id], [l2].[Level4_Name], [l2].[OneToMany_Optional_Inverse4Id], [l2].[OneToMany_Required_Inverse4Id], [l2].[OneToOne_Optional_PK_Inverse4Id]
    FROM [Level1] AS [l2]
    WHERE [l2].[Level3_Required_Id] IS NOT NULL AND [l2].[OneToMany_Required_Inverse4Id] IS NOT NULL
) AS [t1] ON CASE
    WHEN [t0].[Level2_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t0].[Id]
END = [t1].[Level3_Required_Id]
""");
    }

    public override async Task Multiple_required_navigation_with_EF_Property_Include(bool async)
    {
        await base.Multiple_required_navigation_with_EF_Property_Include(async);

        AssertSql();
    }

    public override async Task GroupJoin_SelectMany_DefaultIfEmpty_with_predicate_using_closure(bool async)
    {
        await base.GroupJoin_SelectMany_DefaultIfEmpty_with_predicate_using_closure(async);

        AssertSql(
"""
@__prm_0='10'

SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END <> @__prm_0 OR CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL)
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task GroupJoin_SelectMany_with_predicate_using_closure(bool async)
    {
        await base.GroupJoin_SelectMany_with_predicate_using_closure(async);

        AssertSql(
"""
@__prm_0='10'

SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END <> @__prm_0 OR CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL)
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
""");
    }

    public override async Task GroupJoin_SelectMany_DefaultIfEmpty_with_predicate_using_closure_nested(bool async)
    {
        await base.GroupJoin_SelectMany_DefaultIfEmpty_with_predicate_using_closure_nested(async);

        AssertSql(
"""
@__prm1_0='10'
@__prm2_1='20'

SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2], CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END AS [Id3]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END <> @__prm1_0 OR CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL)
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [t3].[Id] AS [Id1], [t3].[Level2_Optional_Id], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL AND (CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END <> @__prm2_1 OR CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END IS NULL)
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END = [t1].[Level2_Optional_Id]
""");
    }

    public override async Task GroupJoin_SelectMany_with_predicate_using_closure_nested(bool async)
    {
        await base.GroupJoin_SelectMany_with_predicate_using_closure_nested(async);

        AssertSql(
"""
@__prm1_0='10'
@__prm2_1='20'

SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2], CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END AS [Id3]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END <> @__prm1_0 OR CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL)
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
INNER JOIN (
    SELECT [t3].[Id] AS [Id1], [t3].[Level2_Optional_Id], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL AND (CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END <> @__prm2_1 OR CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END IS NULL)
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END = [t1].[Level2_Optional_Id]
""");
    }

    public override async Task GroupJoin_SelectMany_DefaultIfEmpty_with_predicate_using_closure_nested_same_param(bool async)
    {
        await base.GroupJoin_SelectMany_DefaultIfEmpty_with_predicate_using_closure_nested_same_param(async);

        AssertSql(
"""
@__prm_0='10'

SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2], CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END AS [Id3]
FROM [Level1] AS [l]
LEFT JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END <> @__prm_0 OR CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL)
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
LEFT JOIN (
    SELECT [t3].[Id] AS [Id1], [t3].[Level2_Optional_Id], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL AND (CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END <> @__prm_0 OR CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END IS NULL)
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END = [t1].[Level2_Optional_Id]
""");
    }

    public override async Task GroupJoin_SelectMany_with_predicate_using_closure_nested_same_param(bool async)
    {
        await base.GroupJoin_SelectMany_with_predicate_using_closure_nested_same_param(async);

        AssertSql(
"""
@__prm_0='10'

SELECT [l].[Id] AS [Id1], CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END AS [Id2], CASE
    WHEN [t1].[Level2_Required_Id] IS NOT NULL AND [t1].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t1].[Id1]
END AS [Id3]
FROM [Level1] AS [l]
INNER JOIN (
    SELECT [t].[Id] AS [Id0], [t].[OneToOne_Required_PK_Date], [t].[Level1_Optional_Id], [t].[Level1_Required_Id], [t].[OneToMany_Required_Inverse2Id]
    FROM [Level1] AS [l0]
    LEFT JOIN (
        SELECT [l1].[Id], [l1].[OneToOne_Required_PK_Date], [l1].[Level1_Optional_Id], [l1].[Level1_Required_Id], [l1].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l1]
        WHERE [l1].[OneToOne_Required_PK_Date] IS NOT NULL AND [l1].[Level1_Required_Id] IS NOT NULL AND [l1].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t] ON [l0].[Id] = CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END
    WHERE [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL AND (CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END <> @__prm_0 OR CASE
        WHEN [t].[OneToOne_Required_PK_Date] IS NOT NULL AND [t].[Level1_Required_Id] IS NOT NULL AND [t].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t].[Id]
    END IS NULL)
) AS [t0] ON [l].[Id] = [t0].[Level1_Optional_Id]
INNER JOIN (
    SELECT [t3].[Id] AS [Id1], [t3].[Level2_Optional_Id], [t3].[Level2_Required_Id], [t3].[OneToMany_Required_Inverse3Id]
    FROM [Level1] AS [l2]
    LEFT JOIN (
        SELECT [l3].[Id], [l3].[OneToOne_Required_PK_Date], [l3].[Level1_Required_Id], [l3].[OneToMany_Required_Inverse2Id]
        FROM [Level1] AS [l3]
        WHERE [l3].[OneToOne_Required_PK_Date] IS NOT NULL AND [l3].[Level1_Required_Id] IS NOT NULL AND [l3].[OneToMany_Required_Inverse2Id] IS NOT NULL
    ) AS [t2] ON [l2].[Id] = CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END
    LEFT JOIN (
        SELECT [l4].[Id], [l4].[Level2_Optional_Id], [l4].[Level2_Required_Id], [l4].[OneToMany_Required_Inverse3Id]
        FROM [Level1] AS [l4]
        WHERE [l4].[Level2_Required_Id] IS NOT NULL AND [l4].[OneToMany_Required_Inverse3Id] IS NOT NULL
    ) AS [t3] ON CASE
        WHEN [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t2].[Id]
    END = CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END
    WHERE [t2].[OneToOne_Required_PK_Date] IS NOT NULL AND [t2].[Level1_Required_Id] IS NOT NULL AND [t2].[OneToMany_Required_Inverse2Id] IS NOT NULL AND [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL AND (CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END <> @__prm_0 OR CASE
        WHEN [t3].[Level2_Required_Id] IS NOT NULL AND [t3].[OneToMany_Required_Inverse3Id] IS NOT NULL THEN [t3].[Id]
    END IS NULL)
) AS [t1] ON CASE
    WHEN [t0].[OneToOne_Required_PK_Date] IS NOT NULL AND [t0].[Level1_Required_Id] IS NOT NULL AND [t0].[OneToMany_Required_Inverse2Id] IS NOT NULL THEN [t0].[Id0]
END = [t1].[Level2_Optional_Id]
""");
    }

    private void AssertSql(params string[] expected)
        => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);
}
