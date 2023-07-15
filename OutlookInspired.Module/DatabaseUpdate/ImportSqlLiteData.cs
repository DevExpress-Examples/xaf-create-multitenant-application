using DevExpress.DevAV;
using Microsoft.EntityFrameworkCore;
using OutlookInspired.Module.BusinessObjects;
using System.Reflection;
using Crest = OutlookInspired.Module.BusinessObjects.Crest;
using Customer = OutlookInspired.Module.BusinessObjects.Customer;
using CustomerCommunication = OutlookInspired.Module.BusinessObjects.CustomerCommunication;
using CustomerEmployee = OutlookInspired.Module.BusinessObjects.CustomerEmployee;
using CustomerStatus = OutlookInspired.Module.BusinessObjects.CustomerStatus;
using CustomerStore = OutlookInspired.Module.BusinessObjects.CustomerStore;
using Employee = OutlookInspired.Module.BusinessObjects.Employee;
using EmployeeDepartment = OutlookInspired.Module.BusinessObjects.EmployeeDepartment;
using EmployeeStatus = OutlookInspired.Module.BusinessObjects.EmployeeStatus;
using EmployeeTask = OutlookInspired.Module.BusinessObjects.EmployeeTask;
using EmployeeTaskFollowUp = OutlookInspired.Module.BusinessObjects.EmployeeTaskFollowUp;
using EmployeeTaskPriority = OutlookInspired.Module.BusinessObjects.EmployeeTaskPriority;
using EmployeeTaskStatus = OutlookInspired.Module.BusinessObjects.EmployeeTaskStatus;
using Evaluation = OutlookInspired.Module.BusinessObjects.Evaluation;
using Order = OutlookInspired.Module.BusinessObjects.Order;
using OrderItem = OutlookInspired.Module.BusinessObjects.OrderItem;
using PersonPrefix = OutlookInspired.Module.BusinessObjects.PersonPrefix;
using Picture = OutlookInspired.Module.BusinessObjects.Picture;
using Probation = OutlookInspired.Module.BusinessObjects.Probation;
using Product = OutlookInspired.Module.BusinessObjects.Product;
using ProductCatalog = OutlookInspired.Module.BusinessObjects.ProductCatalog;
using ProductCategory = OutlookInspired.Module.BusinessObjects.ProductCategory;
using ProductImage = OutlookInspired.Module.BusinessObjects.ProductImage;
using Quote = OutlookInspired.Module.BusinessObjects.Quote;
using QuoteItem = OutlookInspired.Module.BusinessObjects.QuoteItem;
using ShipmentCourier = OutlookInspired.Module.BusinessObjects.ShipmentCourier;
using ShipmentStatus = OutlookInspired.Module.BusinessObjects.ShipmentStatus;
using State = OutlookInspired.Module.BusinessObjects.State;
using StateEnum = OutlookInspired.Module.BusinessObjects.StateEnum;
using TaskAttachedFile = OutlookInspired.Module.BusinessObjects.TaskAttachedFile;

namespace OutlookInspired.Module.DatabaseUpdate{
    public static class ImportSqlLiteData{
        public static async Task ImportFromSqlLite(this OutlookInspiredEFCoreDbContext sqlServerContext){
            await using var sqLiteContext = new DevAVDb($"Data Source={typeof(OutlookInspiredEFCoreDbContext).Assembly.ExtractSqlLiteData()}");
            await sqlServerContext.ImportFrom(sqLiteContext);
            await sqlServerContext.SaveChangesAsync();
        }

        private static string ExtractSqlLiteData(this Assembly assembly) {
            var sqlLitePath = $"{Environment.CurrentDirectory}\\devav.sqlite3";
            if (!File.Exists(sqlLitePath)) {
                assembly.GetManifestResourceStream(name => name.EndsWith("devav.sqlite3"))
                    .SaveToFile(sqlLitePath);
            }

            return sqlLitePath;
        }

        static async Task ImportFrom(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => await sqlServerContext.ImportCrest( sqliteContext)
                // .Concat(sqlServerContext.ImportState(sqliteContext))
                // .Concat(sqlServerContext.ImportCustomer(sqliteContext))
                // .Concat(sqlServerContext.ImportCustomerStore(sqliteContext))
                // .Concat(sqlServerContext.ImportPicture(sqliteContext))
                // .Concat(sqlServerContext.ImportProbation(sqliteContext))
                // .Concat(sqlServerContext.ImportEmployee(sqliteContext))
                // .Concat(sqlServerContext.ImportEvaluation( sqliteContext))
                // .Concat(sqlServerContext.ImportCustomerEmployee(sqliteContext))
                // .Concat(sqlServerContext.ImportCustomerCommunication(sqliteContext))
                // .Concat(sqlServerContext.ImportEmployeeTasks( sqliteContext))
                // .Concat(sqlServerContext.ImportTaskAttachedFiles(sqliteContext))
                // .Concat(sqlServerContext.ImportProduct(sqliteContext))
                // .Concat(sqlServerContext.ImportProductImages(sqliteContext))
                // .Concat(sqlServerContext.ImportProductCatalog(sqliteContext))
                // .Concat(sqlServerContext.ImportOrder(sqliteContext))
                // .Concat(sqlServerContext.ImportOrderItem(sqliteContext))
                // .Concat(sqlServerContext.ImportQuote(sqliteContext))
                // .Concat(sqlServerContext.ImportQuoteItem(sqliteContext))
                .ToArrayAsync();

        static IAsyncEnumerable<Employee> ImportEmployee(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Employees.AsAsyncEnumerable()
                .SelectAwait(async employee => {
                    var proxy = sqlServerContext.CreateProxy<Employee>();
                    proxy.IdInt64 = employee.Id;
                    proxy.Picture = await sqlServerContext.Pictures.FindSqlLiteObjectAsync(employee.Picture?.Id??0);
                    proxy.AddressState = (StateEnum)employee.AddressState;
                    proxy.Department = (EmployeeDepartment)employee.Department;
                    proxy.Email=employee.Email;
                    proxy.Prefix = (PersonPrefix)employee.Prefix;
                    proxy.Skype = employee.Skype;
                    proxy.Status = (EmployeeStatus)employee.Status;
                    proxy.Title = employee.Title;
                    proxy.AddressLatitude=employee.AddressLatitude;
                    proxy.AddressLongitude=employee.AddressLongitude;
                    proxy.BirthDate=employee.BirthDate;
                    proxy.FirstName=employee.FirstName;
                    proxy.FullName=employee.FullName;
                    proxy.HireDate=employee.HireDate;
                    proxy.HomePhone=employee.HomePhone;
                    proxy.LastName = employee.LastName;
                    proxy.MobilePhone=employee.MobilePhone;
                    proxy.PersonalProfile=employee.PersonalProfile;
                    proxy.ProbationReason = await sqlServerContext.Probations.FindSqlLiteObjectAsync(employee.ProbationReason?.Id??0);
                    return proxy;
                });

        private static Task<T> FindSqlLiteObjectAsync<T>(this DbSet<T> dbSet, long id) where T:MigrationBaseObject 
            => dbSet.FirstOrDefaultAsync(migrationBaseObject => migrationBaseObject.IdInt64 == id);

        static IAsyncEnumerable<Evaluation> ImportEvaluation(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Evaluations.AsAsyncEnumerable()
                .SelectAwait(async evaluation => {
                    var proxy = (await sqlServerContext.Evaluations.AddAsync(sqlServerContext.CreateProxy<Evaluation>())).Entity;
                    proxy.IdInt64 = evaluation.Id;
                    proxy.Employee = await sqlServerContext.Employees.FindSqlLiteObjectAsync(evaluation.Employee.Id);
                    proxy.CreatedBy = await sqlServerContext.Employees.FindSqlLiteObjectAsync(evaluation.CreatedBy.Id);
                    return proxy;
                });

        static IAsyncEnumerable<Order> ImportOrder(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Orders.AsAsyncEnumerable()
                .SelectAwait(async order => {
                    var proxy = (await sqlServerContext.Orders.AddAsync(sqlServerContext.CreateProxy<Order>())).Entity;
                    proxy.IdInt64 = order.Id;
                    proxy.Employee = await sqlServerContext.Employees.FindSqlLiteObjectAsync(order.Employee.Id);
                    proxy.Customer = await sqlServerContext.Customers.FindSqlLiteObjectAsync(order.Customer.Id);
                    proxy.PaymentTotal=order.PaymentTotal;
                    proxy.ShippingAmount=order.ShippingAmount;
                    proxy.RefundTotal = order.RefundTotal;
                    proxy.TotalAmount = order.TotalAmount;
                    proxy.Comments = order.Comments;
                    proxy.Store = await sqlServerContext.CustomerStores.FindSqlLiteObjectAsync(order.Store.Id);
                    proxy.InvoiceNumber=order.InvoiceNumber;
                    proxy.OrderDate=order.OrderDate;
                    proxy.OrderTerms = order.OrderTerms;
                    proxy.SaleAmount = order.SaleAmount;
                    proxy.ShipDate = order.ShipDate;
                    proxy.ShipmentCourier = (ShipmentCourier)order.ShipmentCourier;
                    proxy.ShipmentStatus = (ShipmentStatus)order.ShipmentStatus;
                    proxy.PONumber = order.PONumber;
                    return proxy;
                });

        static IAsyncEnumerable<Product> ImportProduct(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Products.AsAsyncEnumerable()
                .SelectAwait(async product => {
                    var proxy = (await sqlServerContext.Products.AddAsync(sqlServerContext.CreateProxy<Product>())).Entity;
                    proxy.IdInt64 = product.Id;
                    proxy.Category=(ProductCategory)product.Category;
                    proxy.Name = product.Name;
                    proxy.Description=product.Description;
                    proxy.Available = product.Available;
                    proxy.Backorder=product.Backorder;
                    proxy.Barcode = product.Barcode;
                    proxy.Cost=product.Cost;
                    proxy.Engineer = await sqlServerContext.Employees.FindSqlLiteObjectAsync(product.Engineer.Id);
                    proxy.Image=product.Image;
                    proxy.Manufacturing=product.Manufacturing;
                    proxy.Support = await sqlServerContext.Employees.FindSqlLiteObjectAsync(product.Support.Id);
                    proxy.Weight = product.Weight;
                    proxy.ConsumerRating = product.ConsumerRating;
                    proxy.CurrentInventory=product.CurrentInventory;
                    proxy.PrimaryImage = await sqlServerContext.Pictures.FindSqlLiteObjectAsync(product.PrimaryImage.Id);
                    proxy.ProductionStart=product.ProductionStart;
                    proxy.RetailPrice = product.RetailPrice;
                    proxy.SalePrice=product.SalePrice;
                    return proxy;
                });

        static IAsyncEnumerable<ProductImage> ImportProductImages(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.ProductImages.AsAsyncEnumerable()
                .SelectAwait(async image => {
                    var proxy = (await sqlServerContext.ProductImages.AddAsync(sqlServerContext.CreateProxy<ProductImage>())).Entity;
                    proxy.IdInt64 = image.Id;
                    proxy.Product = await sqlServerContext.Products.FindSqlLiteObjectAsync(image.Product.Id);
                    proxy.Picture = await sqlServerContext.Pictures.FindSqlLiteObjectAsync(image.Picture.Id);
                    return proxy;
                });

        static IAsyncEnumerable<ProductCatalog> ImportProductCatalog(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.ProductCatalogs.AsAsyncEnumerable()
                .SelectAwait(async catalog => {
                    var proxy = (await sqlServerContext.ProductCatalogs.AddAsync(sqlServerContext.CreateProxy<ProductCatalog>())).Entity;
                    proxy.IdInt64 = catalog.Id;
                    proxy.Product = await sqlServerContext.Products.FindSqlLiteObjectAsync(catalog.Product.Id);
                    proxy.PDF = catalog.PDF;
                    return proxy;
                });

        static IAsyncEnumerable<OrderItem> ImportOrderItem(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.OrderItems.AsAsyncEnumerable()
                .SelectAwait(async item => {
                    var proxy = (await sqlServerContext.OrderItems.AddAsync(sqlServerContext.CreateProxy<OrderItem>())).Entity;
                    proxy.IdInt64 = item.Id;
                    proxy.ProductPrice = item.ProductPrice;
                    proxy.Discount = item.Discount;
                    proxy.Order = await sqlServerContext.Orders.FindSqlLiteObjectAsync(item.Order.Id);
                    proxy.Product = await sqlServerContext.Products.FindSqlLiteObjectAsync(item.Product.Id);
                    proxy.Total = item.Total;
                    proxy.ProductUnits = item.ProductUnits;
                    return proxy;
                });

        static IAsyncEnumerable<Quote> ImportQuote(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Quotes.AsAsyncEnumerable()
                .SelectAwait(async quote => {
                    var proxy = (await sqlServerContext.Quotes.AddAsync(sqlServerContext.CreateProxy<Quote>())).Entity;
                    proxy.IdInt64 = quote.Id;
                    proxy.CustomerStore = await sqlServerContext.CustomerStores.FindSqlLiteObjectAsync(quote.CustomerStore.Id);
                    proxy.Total = quote.Total;
                    proxy.Employee = await sqlServerContext.Employees.FindSqlLiteObjectAsync(quote.Employee.Id);
                    proxy.ShippingAmount=quote.ShippingAmount;
                    proxy.Date=quote.Date;
                    proxy.Customer = await sqlServerContext.Customers.FindSqlLiteObjectAsync(quote.Customer.Id);
                    proxy.Number=quote.Number;
                    proxy.Opportunity=quote.Opportunity;
                    proxy.SubTotal=quote.SubTotal;
                    return proxy;
                });

        static IAsyncEnumerable<QuoteItem> ImportQuoteItem(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.QuoteItems.AsAsyncEnumerable()
                .SelectAwait(async item => {
                    var proxy = (await sqlServerContext.QuoteItems.AddAsync(sqlServerContext.CreateProxy<QuoteItem>())).Entity;
                    proxy.IdInt64 = item.Id;
                    proxy.Total = item.Total;
                    proxy.Quote = await sqlServerContext.Quotes.FindSqlLiteObjectAsync(item.Quote.Id);
                    proxy.Discount = item.Discount;
                    proxy.ProductUnits = item.ProductUnits;
                    proxy.Product = await sqlServerContext.Products.FindSqlLiteObjectAsync(item.Product.Id);
                    proxy.ProductPrice = item.ProductPrice;
                    return proxy;
                });

        static IAsyncEnumerable<CustomerEmployee> ImportCustomerEmployee(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.CustomerEmployees.AsAsyncEnumerable()
                .SelectAwait(async employee => {
                    var proxy = (await sqlServerContext.CustomerEmployees.AddAsync(sqlServerContext.CreateProxy<CustomerEmployee>())).Entity;
                    proxy.IdInt64 = employee.Id;
                    proxy.Prefix = (PersonPrefix)employee.Prefix;
                    proxy.FirstName = employee.FirstName;
                    proxy.FullName = employee.FullName;
                    proxy.Picture = await sqlServerContext.Pictures.FindSqlLiteObjectAsync(employee.Picture.Id);
                    proxy.Email = employee.Email;
                    proxy.LastName = employee.LastName;
                    proxy.Customer = await sqlServerContext.Customers.FindSqlLiteObjectAsync(employee.Id);
                    proxy.Position = employee.Position;
                    proxy.CustomerStore = await sqlServerContext.CustomerStores.FindSqlLiteObjectAsync(employee.CustomerStore?.Id??0);
                    proxy.MobilePhone = employee.MobilePhone;
                    proxy.IsPurchaseAuthority = employee.IsPurchaseAuthority;
                    return proxy;
                });

        static IAsyncEnumerable<CustomerCommunication> ImportCustomerCommunication(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.CustomerCommunications.AsAsyncEnumerable()
                .SelectAwait(async communication => {
                    var proxy = (await sqlServerContext.CustomerCommunications.AddAsync(sqlServerContext.CreateProxy<CustomerCommunication>())).Entity;
                    proxy.IdInt64 = communication.Id;
                    proxy.Employee = await sqlServerContext.Employees.FindSqlLiteObjectAsync(communication.Employee.Id);
                    proxy.CustomerEmployee = await sqlServerContext.CustomerEmployees.FindSqlLiteObjectAsync(communication.CustomerEmployee.Id);
                    proxy.Date = communication.Date;
                    proxy.Purpose = communication.Purpose;
                    proxy.Type = communication.Type;
                    return proxy;
                });

        static IAsyncEnumerable<Probation> ImportProbation(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Probations.AsAsyncEnumerable()
                .SelectAwait(async probation => {
                    var proxy = (await sqlServerContext.Probations.AddAsync(sqlServerContext.CreateProxy<Probation>())).Entity;
                    proxy.IdInt64 = probation.Id;
                    proxy.Reason = probation.Reason;
                    return proxy;
                });

        static IAsyncEnumerable<Picture> ImportPicture(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Pictures.AsAsyncEnumerable()
                .SelectAwait(async picture => {
                    var proxy = (await sqlServerContext.Pictures.AddAsync(sqlServerContext.CreateProxy<Picture>())).Entity;
                    proxy.IdInt64 = picture.Id;
                    proxy.Data = picture.Data;
                    return proxy;
                });

        static IAsyncEnumerable<EmployeeTask> ImportEmployeeTasks(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.EmployeeTasks.AsAsyncEnumerable()
                .SelectAwait(async task => {
                    var employeeTask = (await sqlServerContext.EmployeeTasks.AddAsync(sqlServerContext.CreateProxy<EmployeeTask>())).Entity;
                    employeeTask.IdInt64=task.Id;
                    employeeTask.CustomerEmployee = await sqlServerContext.CustomerEmployees.FindSqlLiteObjectAsync(task.CustomerEmployee?.Id??0);
                    employeeTask.Status = (EmployeeTaskStatus)task.Status;
                    employeeTask.Category = task.Category;
                    employeeTask.Completion=task.Completion;
                    employeeTask.Description = task.Description;
                    employeeTask.Owner = await sqlServerContext.Employees.FindSqlLiteObjectAsync(task.Owner.Id);
                    employeeTask.Predecessors=task.Predecessors;
                    employeeTask.Priority = (EmployeeTaskPriority)task.Priority;
                    employeeTask.Private=task.Private;
                    employeeTask.Reminder = task.Reminder;
                    employeeTask.Subject = task.Subject;
                    employeeTask.AssignedEmployee = await sqlServerContext.Employees.FindSqlLiteObjectAsync(task.AssignedEmployee.Id);
                    employeeTask.DueDate=task.DueDate;
                    employeeTask.FollowUp = (EmployeeTaskFollowUp)task.FollowUp;
                    employeeTask.ParentId=task.ParentId;
                    employeeTask.StartDate=task.StartDate;
                    employeeTask.AttachedCollectionsChanged=task.AttachedCollectionsChanged;
                    employeeTask.ReminderDateTime=task.ReminderDateTime;
                    employeeTask.RtfTextDescription=task.RtfTextDescription;
                    return employeeTask;
                    // return task.AssignedEmployees.Do(employee => employeeTask.AssignedEmployees.Add(employees[employee.Id]))
                    // .IgnoreElements().To(employeeTask).Concat(employeeTask);
                });

        static IAsyncEnumerable<TaskAttachedFile> ImportTaskAttachedFiles(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.TaskAttachedFiles.AsAsyncEnumerable()
                .SelectAwait(async file => {
                    var proxy = (await sqlServerContext.TaskAttachedFiles.AddAsync(sqlServerContext.CreateProxy<TaskAttachedFile>())).Entity;
                    proxy.IdInt64 = file.Id;
                    proxy.Name = file.Name;
                    proxy.EmployeeTask = await sqlServerContext.EmployeeTasks.FindSqlLiteObjectAsync(file.EmployeeTask.Id);
                    proxy.Content = file.Content;
                    return proxy;
                });

        static IAsyncEnumerable<Customer> ImportCustomer(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            =>  sqliteContext.Customers.AsAsyncEnumerable()
                .SelectAwait(async customer => {
                    var proxy = (await sqlServerContext.Customers.AddAsync(sqlServerContext.CreateProxy<Customer>())).Entity;
                    proxy.IdInt64 = customer.Id;
                    proxy.Logo=customer.Logo;
                    proxy.Profile=customer.Profile;
                    proxy.Fax=customer.Fax;
                    proxy.Name=customer.Name;
                    proxy.Phone = customer.Phone;
                    proxy.Status = (CustomerStatus)customer.Status;
                    proxy.Website = customer.Website;
                    proxy.AnnualRevenue=customer.AnnualRevenue;
                    proxy.TotalEmployees=customer.TotalEmployees;
                    proxy.TotalStores=customer.TotalStores;
                    proxy.BillingAddressCity = customer.BillingAddressCity;
                    proxy.BillingAddressLatitude=customer.BillingAddressLatitude;
                    proxy.BillingAddressLine = customer.BillingAddressLine;
                    proxy.BillingAddressLongitude = customer.BillingAddressLongitude;
                    proxy.BillingAddressState = Enum.Parse<StateEnum>(customer.BillingAddressState.ToString());
                    proxy.BillingAddressZipCode = customer.BillingAddressZipCode;
                    proxy.HomeOfficeCity = customer.HomeOfficeCity;
                    proxy.HomeOfficeLatitude=customer.HomeOfficeLatitude;
                    proxy.HomeOfficeLine = customer.HomeOfficeLine;
                    proxy.HomeOfficeLongitude = customer.HomeOfficeLongitude;
                    proxy.HomeOfficeState = Enum.Parse<StateEnum>(customer.HomeOfficeState.ToString());
                    proxy.HomeOfficeZipCode = customer.HomeOfficeZipCode;
                    return proxy;
                });

        static IAsyncEnumerable<CustomerStore> ImportCustomerStore(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.CustomerStores.AsAsyncEnumerable()
                .SelectAwait(async store => {
                    var proxy = (await sqlServerContext.CustomerStores.AddAsync(sqlServerContext.CreateProxy<CustomerStore>())).Entity;
                    proxy.IdInt64 = store.Id;
                    proxy.Customer = await sqlServerContext.Customers.FindSqlLiteObjectAsync(store.Customer.Id);
                    proxy.Crest = await sqlServerContext.Crests.FindSqlLiteObjectAsync(store.Crest.Id);
                    proxy.Phone=store.Phone;
                    proxy.Fax = store.Fax;
                    proxy.Location = store.Location;
                    proxy.AddressCity=store.Address_City;
                    proxy.AddressLatitude = store.Address_Latitude;
                    proxy.AddressLongitude = store.Address_Longitude;
                    proxy.AddressState = Enum.Parse<StateEnum>(store.Address_State.ToString());
                    proxy.AddressLine = store.AddressLine;
                    proxy.AddressZipCode = store.Address_ZipCode;
                    proxy.AnnualSales = store.AnnualSales;
                    proxy.SquereFootage = store.SquereFootage;
                    proxy.TotalEmployees = store.TotalEmployees;
                    proxy.AddressZipCode = store.Address_ZipCode;
                    return proxy;
                });

        static IAsyncEnumerable<State> ImportState(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.States.AsAsyncEnumerable()
                .SelectAwait(async state => {
                    var proxy = (await sqlServerContext.States.AddAsync(sqlServerContext.CreateProxy<State>())).Entity;
                    proxy.IdInt64 = ((long)Enum.Parse<StateEnum>(state.ShortName.ToString()));
                    proxy.ShortName = Enum.Parse<StateEnum>(state.ShortName.ToString());
                    proxy.LargeFlag=state.Flag48px;
                    proxy.SmallFlag=state.Flag24px;
                    proxy.LongName=state.LongName;
                    return proxy;
                });

        static IAsyncEnumerable<Crest> ImportCrest(this OutlookInspiredEFCoreDbContext sqlServerContext, DevAVDb sqliteContext) 
            => sqliteContext.Crests.AsAsyncEnumerable()
                .SelectAwait(async crest => {
                    var proxy = (await sqlServerContext.Crests.AddAsync(sqlServerContext.CreateProxy<Crest>())).Entity;
                    proxy.IdInt64 = crest.Id;
                    proxy.LargeImage = crest.LargeImage;
                    proxy.SmallImage = crest.SmallImage;
                    return proxy;
                });
    }
}