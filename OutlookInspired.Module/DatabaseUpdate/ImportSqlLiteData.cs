using DevExpress.DevAV;
using OutlookInspired.Module.BusinessObjects;
using System.Reflection;
using DevExpress.ExpressApp;
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
        public static async Task ImportFromSqlLite(this IObjectSpace objectSpace){
            await using var sqLiteContext = new DevAVDb($"Data Source={typeof(OutlookInspiredEFCoreDbContext).Assembly.ExtractSqlLiteData()}");
            await objectSpace.ImportFrom(sqLiteContext);
            objectSpace.CommitChanges();
        }

        private static string ExtractSqlLiteData(this Assembly assembly) {
            var sqlLitePath = $"{Environment.CurrentDirectory}\\devav.sqlite3";
            if (!File.Exists(sqlLitePath)) {
                assembly.GetManifestResourceStream(name => name.EndsWith("devav.sqlite3"))
                    .SaveToFile(sqlLitePath);
            }

            return sqlLitePath;
        }

        static async Task ImportFrom(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => await objectSpace.ImportCrest( sqliteContext)
                .Concat(objectSpace.ImportState(sqliteContext))
                .Concat(objectSpace.ImportCustomer(sqliteContext))
                .Concat(objectSpace.ImportCustomerStore(sqliteContext))
                .Concat(objectSpace.ImportPicture(sqliteContext))
                .Concat(objectSpace.ImportProbation(sqliteContext))
                .Concat(objectSpace.ImportEmployee(sqliteContext))
                .Concat(objectSpace.ImportEvaluation( sqliteContext))
                .Concat(objectSpace.ImportCustomerEmployee(sqliteContext))
                .Concat(objectSpace.ImportCustomerCommunication(sqliteContext))
                .Concat(objectSpace.ImportEmployeeTasks( sqliteContext))
                .Concat(objectSpace.ImportTaskAttachedFiles(sqliteContext))
                .Concat(objectSpace.ImportProduct(sqliteContext))
                .Concat(objectSpace.ImportProductImages(sqliteContext))
                .Concat(objectSpace.ImportProductCatalog(sqliteContext))
                .Concat(objectSpace.ImportOrder(sqliteContext))
                .Concat(objectSpace.ImportOrderItem(sqliteContext))
                .Concat(objectSpace.ImportQuote(sqliteContext))
                .Concat(objectSpace.ImportQuoteItem(sqliteContext))
                .ToArrayAsync();

        static IAsyncEnumerable<Employee> ImportEmployee(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Employees.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var employee = objectSpace.CreateObject<Employee>();
                    employee.IdInt64 = sqlLite.Id;
                    employee.Picture = objectSpace.FindSqlLiteObject<Picture>( sqlLite.Picture?.Id);
                    employee.AddressState = (StateEnum)sqlLite.AddressState;
                    employee.Department = (EmployeeDepartment)sqlLite.Department;
                    employee.Email=sqlLite.Email;
                    employee.Prefix = (PersonPrefix)sqlLite.Prefix;
                    employee.Skype = sqlLite.Skype;
                    employee.Status = (EmployeeStatus)sqlLite.Status;
                    employee.Title = sqlLite.Title;
                    employee.AddressLatitude=sqlLite.AddressLatitude;
                    employee.AddressLongitude=sqlLite.AddressLongitude;
                    employee.BirthDate=sqlLite.BirthDate;
                    employee.FirstName=sqlLite.FirstName;
                    employee.FullName=sqlLite.FullName;
                    employee.HireDate=sqlLite.HireDate;
                    employee.HomePhone=sqlLite.HomePhone;
                    employee.LastName = sqlLite.LastName;
                    employee.MobilePhone=sqlLite.MobilePhone;
                    employee.PersonalProfile=sqlLite.PersonalProfile;
                    employee.ProbationReason = objectSpace.FindSqlLiteObject<Probation>(sqlLite.ProbationReason?.Id);
                    return employee;
                });

        private static T FindSqlLiteObject<T>(this IObjectSpace objectSpace, long? id) where T:MigrationBaseObject 
            => objectSpace.FindObject<T>(migrationBaseObject => id==migrationBaseObject.IdInt64,true);

        static IAsyncEnumerable<Evaluation> ImportEvaluation(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Evaluations.AsAsyncEnumerable()
                .Select( sqlLite => {
                    var evaluation = objectSpace.CreateObject<Evaluation>();
                    evaluation.IdInt64 = sqlLite.Id;
                    evaluation.Employee = objectSpace.FindSqlLiteObject<Employee>(sqlLite.Employee.Id);
                    evaluation.CreatedBy = objectSpace.FindSqlLiteObject<Employee>(sqlLite.CreatedBy.Id);
                    return evaluation;
                });

        static IAsyncEnumerable<Order> ImportOrder(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Orders.AsAsyncEnumerable()
                .Select( sqlLite => {
                    var order = objectSpace.CreateObject<Order>();
                    order.IdInt64 = sqlLite.Id;
                    order.Employee = objectSpace.FindSqlLiteObject<Employee>(sqlLite.Employee.Id);
                    order.Customer = objectSpace.FindSqlLiteObject<Customer>(sqlLite.Customer.Id);
                    order.PaymentTotal=sqlLite.PaymentTotal;
                    order.ShippingAmount=sqlLite.ShippingAmount;
                    order.RefundTotal = sqlLite.RefundTotal;
                    order.TotalAmount = sqlLite.TotalAmount;
                    order.Comments = sqlLite.Comments;
                    order.Store = objectSpace.FindSqlLiteObject<CustomerStore>(sqlLite.Store.Id);
                    order.InvoiceNumber=sqlLite.InvoiceNumber;
                    order.OrderDate=sqlLite.OrderDate;
                    order.OrderTerms = sqlLite.OrderTerms;
                    order.SaleAmount = sqlLite.SaleAmount;
                    order.ShipDate = sqlLite.ShipDate;
                    order.ShipmentCourier = (ShipmentCourier)sqlLite.ShipmentCourier;
                    order.ShipmentStatus = (ShipmentStatus)sqlLite.ShipmentStatus;
                    order.PONumber = sqlLite.PONumber;
                    return order;
                });

        static IAsyncEnumerable<Product> ImportProduct(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Products.AsAsyncEnumerable()
                .Select( sqlLite => {
                    var product = objectSpace.CreateObject<Product>();
                    product.IdInt64 = sqlLite.Id;
                    product.Category=(ProductCategory)sqlLite.Category;
                    product.Name = sqlLite.Name;
                    product.Description=sqlLite.Description;
                    product.Available = sqlLite.Available;
                    product.Backorder=sqlLite.Backorder;
                    product.Barcode = sqlLite.Barcode;
                    product.Cost=sqlLite.Cost;
                    product.Engineer = objectSpace.FindSqlLiteObject<Employee>(sqlLite.Engineer.Id);
                    product.Image=sqlLite.Image;
                    product.Manufacturing=sqlLite.Manufacturing;
                    product.Support = objectSpace.FindSqlLiteObject<Employee>(sqlLite.Support.Id);
                    product.Weight = sqlLite.Weight;
                    product.ConsumerRating = sqlLite.ConsumerRating;
                    product.CurrentInventory=sqlLite.CurrentInventory;
                    product.PrimaryImage = objectSpace.FindSqlLiteObject<Picture>(sqlLite.PrimaryImage.Id);
                    product.ProductionStart=sqlLite.ProductionStart;
                    product.RetailPrice = sqlLite.RetailPrice;
                    product.SalePrice=sqlLite.SalePrice;
                    return product;
                });

        static IAsyncEnumerable<ProductImage> ImportProductImages(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.ProductImages.AsAsyncEnumerable()
                .Select( sqlLite => {
                    var productImage = objectSpace.CreateObject<ProductImage>();
                    productImage.IdInt64 = sqlLite.Id;
                    productImage.Product = objectSpace.FindSqlLiteObject<Product>(sqlLite.Product.Id);
                    productImage.Picture = objectSpace.FindSqlLiteObject<Picture>(sqlLite.Picture.Id);
                    return productImage;
                });

        static IAsyncEnumerable<ProductCatalog> ImportProductCatalog(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.ProductCatalogs.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var productCatalog = objectSpace.CreateObject<ProductCatalog>();
                    productCatalog.IdInt64 = sqlLite.Id;
                    productCatalog.Product = objectSpace.FindSqlLiteObject<Product>(sqlLite.Product.Id);
                    productCatalog.PDF = sqlLite.PDF;
                    return productCatalog;
                });

        static IAsyncEnumerable<OrderItem> ImportOrderItem(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.OrderItems.AsAsyncEnumerable()
                .Select( sqlLite => {
                    var orderItem = objectSpace.CreateObject<OrderItem>();
                    orderItem.IdInt64 = sqlLite.Id;
                    orderItem.ProductPrice = sqlLite.ProductPrice;
                    orderItem.Discount = sqlLite.Discount;
                    orderItem.Order = objectSpace.FindSqlLiteObject<Order>(sqlLite.Order.Id);
                    orderItem.Product = objectSpace.FindSqlLiteObject<Product>(sqlLite.Product.Id);
                    orderItem.Total = sqlLite.Total;
                    orderItem.ProductUnits = sqlLite.ProductUnits;
                    return orderItem;
                });

        static IAsyncEnumerable<Quote> ImportQuote(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Quotes.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var quote = objectSpace.CreateObject<Quote>();
                    quote.IdInt64 = sqlLite.Id;
                    quote.CustomerStore = objectSpace.FindSqlLiteObject<CustomerStore>(sqlLite.CustomerStore.Id);
                    quote.Total = sqlLite.Total;
                    quote.Employee = objectSpace.FindSqlLiteObject<Employee>(sqlLite.Employee.Id);
                    quote.ShippingAmount=sqlLite.ShippingAmount;
                    quote.Date=sqlLite.Date;
                    quote.Customer = objectSpace.FindSqlLiteObject<Customer>(sqlLite.Customer.Id);
                    quote.Number=sqlLite.Number;
                    quote.Opportunity=sqlLite.Opportunity;
                    quote.SubTotal=sqlLite.SubTotal;
                    return quote;
                });

        static IAsyncEnumerable<QuoteItem> ImportQuoteItem(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.QuoteItems.AsAsyncEnumerable()
                .Select( sqlLite => {
                    var quoteItem = objectSpace.CreateObject<QuoteItem>();
                    quoteItem.IdInt64 = sqlLite.Id;
                    quoteItem.Total = sqlLite.Total;
                    quoteItem.Quote = objectSpace.FindSqlLiteObject<Quote>(sqlLite.Quote.Id);
                    quoteItem.Discount = sqlLite.Discount;
                    quoteItem.ProductUnits = sqlLite.ProductUnits;
                    quoteItem.Product = objectSpace.FindSqlLiteObject<Product>(sqlLite.Product.Id);
                    quoteItem.ProductPrice = sqlLite.ProductPrice;
                    return quoteItem;
                });

        static IAsyncEnumerable<CustomerEmployee> ImportCustomerEmployee(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.CustomerEmployees.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var customerEmployee = objectSpace.CreateObject<CustomerEmployee>();
                    customerEmployee.IdInt64 = sqlLite.Id;
                    customerEmployee.Prefix = (PersonPrefix)sqlLite.Prefix;
                    customerEmployee.FirstName = sqlLite.FirstName;
                    customerEmployee.FullName = sqlLite.FullName;
                    customerEmployee.Picture = objectSpace.FindSqlLiteObject<Picture>(sqlLite.Picture.Id);
                    customerEmployee.Email = sqlLite.Email;
                    customerEmployee.LastName = sqlLite.LastName;
                    customerEmployee.Customer = objectSpace.FindSqlLiteObject<Customer>(sqlLite.Id);
                    customerEmployee.Position = sqlLite.Position;
                    customerEmployee.CustomerStore = objectSpace.FindSqlLiteObject<CustomerStore>(sqlLite.CustomerStore.Id);
                    customerEmployee.MobilePhone = sqlLite.MobilePhone;
                    customerEmployee.IsPurchaseAuthority = sqlLite.IsPurchaseAuthority;
                    return customerEmployee;
                });

        static IAsyncEnumerable<CustomerCommunication> ImportCustomerCommunication(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.CustomerCommunications.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var customerCommunication = objectSpace.CreateObject<CustomerCommunication>();
                    customerCommunication.IdInt64 = sqlLite.Id;
                    customerCommunication.Employee = objectSpace.FindSqlLiteObject<Employee>(sqlLite.Employee.Id);
                    customerCommunication.CustomerEmployee = objectSpace.FindSqlLiteObject<CustomerEmployee>(sqlLite.CustomerEmployee.Id);
                    customerCommunication.Date = sqlLite.Date;
                    customerCommunication.Purpose = sqlLite.Purpose;
                    customerCommunication.Type = sqlLite.Type;
                    return customerCommunication;
                });

        static IAsyncEnumerable<Probation> ImportProbation(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Probations.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var probation = objectSpace.CreateObject<Probation>();
                    probation.IdInt64 = sqlLite.Id;
                    probation.Reason = sqlLite.Reason;
                    return probation;
                });

        static IAsyncEnumerable<Picture> ImportPicture(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Pictures.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var picture = objectSpace.CreateObject<Picture>();
                    picture.IdInt64 = sqlLite.Id;
                    picture.Data = sqlLite.Data;
                    return picture;
                });

        static IAsyncEnumerable<EmployeeTask> ImportEmployeeTasks(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.EmployeeTasks.AsAsyncEnumerable()
                .SelectMany(sqlLite => {
                    var task = objectSpace.CreateObject<EmployeeTask>();
                    task.IdInt64=sqlLite.Id;
                    task.CustomerEmployee = objectSpace.FindSqlLiteObject<CustomerEmployee>(sqlLite.CustomerEmployee?.Id);
                    task.Status = (EmployeeTaskStatus)sqlLite.Status;
                    task.Category = sqlLite.Category;
                    task.Completion=sqlLite.Completion;
                    task.Description = sqlLite.Description;
                    task.Owner = objectSpace.FindSqlLiteObject<Employee>(sqlLite.Owner.Id);
                    task.Predecessors=sqlLite.Predecessors;
                    task.Priority = (EmployeeTaskPriority)sqlLite.Priority;
                    task.Private=sqlLite.Private;
                    task.Reminder = sqlLite.Reminder;
                    task.Subject = sqlLite.Subject;
                    task.AssignedEmployee = objectSpace.FindSqlLiteObject<Employee>(sqlLite.AssignedEmployee?.Id);
                    task.DueDate=sqlLite.DueDate;
                    task.FollowUp = (EmployeeTaskFollowUp)sqlLite.FollowUp;
                    task.ParentId=sqlLite.ParentId;
                    task.StartDate=sqlLite.StartDate;
                    task.AttachedCollectionsChanged=sqlLite.AttachedCollectionsChanged;
                    task.ReminderDateTime=sqlLite.ReminderDateTime;
                    task.RtfTextDescription=sqlLite.RtfTextDescription;
            
                    return sqlLite.AssignedEmployees.Do(employee => task.AssignedEmployees.Add(objectSpace.FindSqlLiteObject<Employee>(employee.Id)))
                        .To(task).ToAsyncEnumerable();
                });

        static IAsyncEnumerable<TaskAttachedFile> ImportTaskAttachedFiles(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.TaskAttachedFiles.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var taskAttachedFile = objectSpace.CreateObject<TaskAttachedFile>();
                    taskAttachedFile.IdInt64 = sqlLite.Id;
                    taskAttachedFile.Name = sqlLite.Name;
                    taskAttachedFile.EmployeeTask = objectSpace.FindSqlLiteObject<EmployeeTask>(sqlLite.EmployeeTask.Id);
                    taskAttachedFile.Content = sqlLite.Content;
                    return taskAttachedFile;
                });

        static IAsyncEnumerable<Customer> ImportCustomer(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            =>  sqliteContext.Customers.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var customer = objectSpace.CreateObject<Customer>();
                    customer.IdInt64 = sqlLite.Id;
                    customer.Logo=sqlLite.Logo;
                    customer.Profile=sqlLite.Profile;
                    customer.Fax=sqlLite.Fax;
                    customer.Name=sqlLite.Name;
                    customer.Phone = sqlLite.Phone;
                    customer.Status = (CustomerStatus)sqlLite.Status;
                    customer.Website = sqlLite.Website;
                    customer.AnnualRevenue=sqlLite.AnnualRevenue;
                    customer.TotalEmployees=sqlLite.TotalEmployees;
                    customer.TotalStores=sqlLite.TotalStores;
                    customer.BillingAddressCity = sqlLite.BillingAddressCity;
                    customer.BillingAddressLatitude=sqlLite.BillingAddressLatitude;
                    customer.BillingAddressLine = sqlLite.BillingAddressLine;
                    customer.BillingAddressLongitude = sqlLite.BillingAddressLongitude;
                    customer.BillingAddressState = Enum.Parse<StateEnum>(sqlLite.BillingAddressState.ToString());
                    customer.BillingAddressZipCode = sqlLite.BillingAddressZipCode;
                    customer.HomeOfficeCity = sqlLite.HomeOfficeCity;
                    customer.HomeOfficeLatitude=sqlLite.HomeOfficeLatitude;
                    customer.HomeOfficeLine = sqlLite.HomeOfficeLine;
                    customer.HomeOfficeLongitude = sqlLite.HomeOfficeLongitude;
                    customer.HomeOfficeState = Enum.Parse<StateEnum>(sqlLite.HomeOfficeState.ToString());
                    customer.HomeOfficeZipCode = sqlLite.HomeOfficeZipCode;
                    return customer;
                });

        static IAsyncEnumerable<CustomerStore> ImportCustomerStore(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.CustomerStores.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var store = objectSpace.CreateObject<CustomerStore>();
                    store.IdInt64 = sqlLite.Id;
                    store.Customer = objectSpace.FindSqlLiteObject<Customer>(sqlLite.Customer.Id);
                    store.Crest = objectSpace.FindSqlLiteObject<Crest>(sqlLite.Crest.Id);
                    store.Phone=sqlLite.Phone;
                    store.Fax = sqlLite.Fax;
                    store.Location = sqlLite.Location;
                    store.AddressCity=sqlLite.Address_City;
                    store.AddressLatitude = sqlLite.Address_Latitude;
                    store.AddressLongitude = sqlLite.Address_Longitude;
                    store.AddressState = Enum.Parse<StateEnum>(sqlLite.Address_State.ToString());
                    store.AddressLine = sqlLite.AddressLine;
                    store.AddressZipCode = sqlLite.Address_ZipCode;
                    store.AnnualSales = sqlLite.AnnualSales;
                    store.SquereFootage = sqlLite.SquereFootage;
                    store.TotalEmployees = sqlLite.TotalEmployees;
                    store.AddressZipCode = sqlLite.Address_ZipCode;
                    return store;
                });

        static IAsyncEnumerable<State> ImportState(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.States.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var state = objectSpace.CreateObject<State>();
                    state.IdInt64 = ((long)Enum.Parse<StateEnum>(sqlLite.ShortName.ToString()));
                    state.ShortName = Enum.Parse<StateEnum>(sqlLite.ShortName.ToString());
                    state.LargeFlag=sqlLite.Flag48px;
                    state.SmallFlag=sqlLite.Flag24px;
                    state.LongName=sqlLite.LongName;
                    return state;
                });

        static IAsyncEnumerable<Crest> ImportCrest(this IObjectSpace objectSpace, DevAVDb sqliteContext) 
            => sqliteContext.Crests.AsAsyncEnumerable()
                .Select(sqlLite => {
                    var crest = objectSpace.CreateObject<Crest>();
                    crest.IdInt64 = sqlLite.Id;
                    crest.CityName = sqlLite.CityName;
                    crest.LargeImage = sqlLite.LargeImage;
                    crest.SmallImage = sqlLite.SmallImage;
                    return crest;
                });
    }
}