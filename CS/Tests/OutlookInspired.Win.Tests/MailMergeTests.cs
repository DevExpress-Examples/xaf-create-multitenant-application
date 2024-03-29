﻿using System.Reactive.Linq;
using NUnit.Framework;
using OutlookInspired.Tests.Common;
using OutlookInspired.Tests.Services;
using TestBase = OutlookInspired.Win.Tests.Common.TestBase;

namespace OutlookInspired.Win.Tests{
    [Apartment(ApartmentState.STA)]
    [Order(30)]
    public class MailMergeTests : TestBase{
        [RetryTestCaseSource(nameof(EmployeeVariants),MaxTries=MaxTries)]
        [Category(Tests)]
        public async Task Employee(string user, string view, string viewVariant) 
            => await StartTest(user, application => application.AssertEmployeeMailMerge(view, viewVariant));
    }
}