// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore;

public class AspNetIdentityCustomTypesIntKeySqliteTest
    : AspNetIdentityCustomTypesIntKeyTestBase<AspNetIdentityCustomTypesIntKeySqliteTest.AspNetIdentityCustomTypesIntKeySqliteFixture>
{
    public AspNetIdentityCustomTypesIntKeySqliteTest(AspNetIdentityCustomTypesIntKeySqliteFixture fixture)
        : base(fixture)
    {
    }

    public class AspNetIdentityCustomTypesIntKeySqliteFixture : AspNetIdentityFixtureBase
    {
        public TestSqlLoggerFactory TestSqlLoggerFactory
            => (TestSqlLoggerFactory)ListLoggerFactory;

        protected override ITestStoreFactory TestStoreFactory
            => SqliteTestStoreFactory.Instance;

        protected override string StoreName
            => "AspNetCustomTypesIntKeyIdentity";
    }
}
