1. It is impossible to use the GridListEditor for ViewTypes like LayoutView however is not that hard to support, sure it may require small breaking changes but the benefit is large. Yes we can use a custom user control instead and add it to as a view item. Is not a good solution as I cannot test it, or to say it better the testing cost makes me not even to think about it. On the other side all my Xaf apps have testing libs I can reuse. Give us your customer such flexiblity you did it with AdvBanded once I am certain it can be done with minimum changes. Not asking to support the layoutview just replace the GridView dependecies with a ColumnView, sounds super fair to me now is not, at the end is like this if GridView is other ColumnView means we used a custom one then do not run any code in this editor. There is only one Public property that requires breaking change the GridView, for events that send out a GridView just do not fire if GridView is null, we are in a custom listeditor at the end and if we require some of them we can do it ourselvse.

2. Here what will boost my productivity.\
     a) Create a user control add a grid.\
     b) Change the GridView to XafGridView, I am fine with modifying even the Designer.cs.\
     c) Use the Grid designer to configure the gridview.
     d) use that GridView instance in the my custom GridListEditor descendant

3. I wish for some feature while debugging I could remove the options model diff, I keep deleting my db cause it throws cause it does not find the views. Sure I delete the views but it should continue if not find them and not stop. My apps do not store the last navigated view in the db so i can remove the model from the bin. But deleting the DB at least for me now and with EF is expensive I have to restore records and takes long time. e.g. could be a commented conditional directive for debug that clear this thing?
4. I set launchBrowser False but it made other problems, not being able to see if the server stopped this I spent my time debugging why the Model does not change when I change it in the VS designer. Simply because the server did not stopped but only the win app. MiddleTier is really dangerous.
5. To debug Middle-tier issues I often switch between Rider and VS for verify is not an IDE shit. Both IDE use different setting by default I suggest we comment out one profile in the launchSettings.json to avoid such issues.  Note Rider behaviour is way better than VS it always prompt u on rerun if the apps (server,client) running already so to stopped them.

   ![image](https://github.com/eXpandFramework/eXpand/assets/159464/dc0be472-4385-491c-a62d-e327d0825c13)
3. How to show a toolbar when I add a user control into a dashboard, the documentation is for sure missing this information and without a toolbar I canot do much with my user control data.
4. Way to show a view does not cover the case where I have a usercontrol in a dashboard as per docs and want to show a view in a mdi child tab. ShowPopup is not an option, the only option i found by the Xaf book is to declare an action for just this but is hacky thinking IMHO, I would rather use a method for this like the showViewStrategy.SHowView or something else.
5. Debugging suffers with Xpo I can see the DefaultProperty value on hover
   ![image](https://github.com/eXpandFramework/eXpand/assets/159464/cf3c0f9a-c5b5-473b-91e3-c27e787685eb)
6. There is no doc for the FK/Navigation properties, specifically I asked you and you agreed with me: Since we use `PreFetchReferenceProperties` the FK property makes no sense however without them u get no FK and u also need to take care nullability
   ```
   public virtual Employee Manager{ get; set; }
   //FK
   public virtual Guid? ManagerId{ get; set; }
   ```
   no nullable FK and when u delete
   ```
   SqlException: The DELETE statement conflicted with the REFERENCE constraint "FK_Evaluations_Employees_ManagerID". The conflict occurred in database "OutlookInspired", table "dbo.Evaluations", column 'ManagerID'.
   ```
7. As I try to configure EF I had to reqork my import algoritm so not to spend 2min almost every db drop which is pretty often. So I have to repeat my self only by importing from a DBContext to an ObjectSpace instead of using 2 DBContext perforamnce wise I am 6 times slower. Imporving these numbers apparently will be 6 times faster when security. Note: I import from a DBContext to an IObjectSpace without security 
8. As I try to configure EF I had to reqork my import algoritm so not to spend 2min almost every db drop which is pretty often. So I have to repeat my self only by importing from a DBContext to an ObjectSpace instead of using 2 DBContext perforamnce wise I am 6 times slower. Imporving these numbers apparently will be 6 times faster when security. Note: I import from a DBContext to an IObjectSpace without security or middle tier. Note that improves speed by application.ObjectSpaceProvider.CreateObjectSpace() 15%
9. Add a note to the how to usecontrol that will say to use a detailview instead of a dashboard and add the controlitem there so u can have the detailview toolbar rendered.  
10. [9:25 AM] Apostolis Bekiaris (DevExpress)
    need some help here e.g. how to debug or whatever so I can move forward

System.Net.Http.HttpRequestException: Response status code does not indicate success: 500 (Internal Server Error).
   at System.Net.Http.HttpResponseMessage.EnsureSuccessStatusCode()
   at DevExpress.Data.Utils.AsyncDownloader`1.HttpLoadOptions.GetHttpContent(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.JsonResult`1..ctor(ExceptionDispatchInfo exceptionInfo, Stream stream, JsonSerializerOptions serializerOptions)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.<>c__6`2.<PostAsync>b__6_2(ExceptionDispatchInfo ex, Stream stream)
   at DevExpress.Data.Utils.AsyncDownloader`1.<>c__DisplayClass13_0.<LoadAsync>b__0(Object message, ExceptionDispatchInfo e, Stream s)
   at DevExpress.Data.Utils.AsyncDownloader`1.LoadAsyncCore(Uri uri, Func`4 handler, CancellationToken cancellationToken, PostMethodOptions options)
   at DevExpress.Data.Utils.AsyncDownloader`1.LoadAsync(Uri uri, Func`3 handler, CancellationToken cancellationToken, PostMethodOptions options)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.PostAsync[TData,TResult](Uri uri, TData data, CancellationToken cancellationToken)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.ExecuteQueryCore(Expression expression)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.RemoteQueryProvider.Execute[TResult](Expression expression)
​[9:29 AM] Apostolis Bekiaris (DevExpress)
    it happens at 

GridControl.DataSource
=_objectSpace.GetObjects(GetObjectType())

i have already tried different type of view, just one column of type string on the Grid the same. I also manage to enumerate the DataSource in a foreach without issues and print the length of item Image(byte[]) properties
12. Provide an extenion like this in the start instead of the long non user friendly sheet, look how I can switch off middle-tier when it makes problems to me while I develop and until the support helps me. It is the same exact approach I use for constractur the app from my tests. At the end I want to test the app the full thing mocking etc are for others here we have Xaf and a typesinfo that depends on everything in place. I personally never mock is way too time consuming and not breaks all the time as you continue develop and change your app. You write the test you should never spent time to support it further.
   ```cs
       public static WinApplication BuildApplication(){
        var builder = WinApplication.CreateBuilder();
        builder.UseApplication<OutlookInspiredWindowsFormsApplication>();
        builder.AddModules();
        // builder.AddObjectSpaceProviders();
        builder.AddObjectSpaceProviders(options => options.UseSqlServer("Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=OutlookInspired"));
        // builder.AddSecurity();
        builder.AddBuildStep(application => application.DatabaseUpdateMode = DatabaseUpdateMode.Never);
        return builder.Build();
    }

   ```
13. changing the record selection fast while in Middle-Tier sometimes leads to the right grid not rendering correctly. The behaviour is not reproducible without middletier/security.
    ![image](https://github.com/eXpandFramework/eXpand/assets/159464/6ed57ee0-8e90-4119-89fc-4f5a67b5f835)

14. forgot the app and went to bed a few hrs later 
    ![image](https://github.com/eXpandFramework/eXpand/assets/159464/3ca304dd-ad19-47f9-b030-fa13f40ef648)
    note the exception was popup without any action e.g. click etc, the app is functional though after I click OK no need to restart it. But it keeps throws the app after small inactivity e.g. a few secords
15. maybe because of the winexplorer gridview, I have now switch to layoutview so I do not pursue this anymore, that was on ther employers right frame (CustomerStores)
    ```
    System.Net.Http.HttpRequestException: Response status code does not indicate success: 500 (Internal Server Error).
   at System.Net.Http.HttpResponseMessage.EnsureSuccessStatusCode()
   at DevExpress.Data.Utils.AsyncDownloader`1.HttpLoadOptions.GetHttpContent(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.JsonResult`1..ctor(ExceptionDispatchInfo exceptionInfo, Stream stream, JsonSerializerOptions serializerOptions)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.<>c__6`2.<PostAsync>b__6_2(ExceptionDispatchInfo ex, Stream stream)
   at DevExpress.Data.Utils.AsyncDownloader`1.<>c__DisplayClass13_0.<LoadAsync>b__0(Object message, ExceptionDispatchInfo e, Stream s)
   at DevExpress.Data.Utils.AsyncDownloader`1.LoadAsyncCore(Uri uri, Func`4 handler, CancellationToken cancellationToken, PostMethodOptions options)
   at DevExpress.Data.Utils.AsyncDownloader`1.LoadAsync(Uri uri, Func`3 handler, CancellationToken cancellationToken, PostMethodOptions options)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.PostAsync[TData,TResult](Uri uri, TData data, CancellationToken cancellationToken)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.HttpRemoteRepository.ExecuteQueryCore(Expression expression)
   at DevExpress.EntityFrameworkCore.Security.MiddleTier.ClientServer.RemoteQueryProvider.Execute[TResult](Expression expression)
    ```
16. Designing the detailview layout in ME many times makes my head spin. There is a problem there, many times u get different layout in runtim e.g. empty spaces visible only in runtime for example in Customer_DetailView. The runtime renders an empty space with id `SizeableEditors` next the diff for reference I removed this from source and redising the view again by first removing all auto generated elements. Note that the only property is the Profile which was not sizeable when I first designed the view and later added the FieldSize(-1)
    ```
        <DetailView Id="Customer_DetailView">
      <Layout>
        <LayoutGroup Id="Main" RelativeSize="100">
          <LayoutGroup Id="SimpleEditors" RelativeSize="15.754082612872239" Direction="Vertical" Caption="SimpleEditors">
            <LayoutGroup Id="Autoaa46dc1b-cd69-4e20-8139-2d0449eb1350" ShowCaption="False" Caption="Autoaa46dc1b-cd69-4e20-8139-2d0449eb1350(3)" Direction="Horizontal" Index="0" RelativeSize="100" IsNewNode="True">
              <LayoutGroup Id="Autod977b1bb-e354-4416-90b4-ed7012122bf2" ShowCaption="False" Caption="Autod977b1bb-e354-4416-90b4-ed7012122bf2(4)" Index="0" RelativeSize="59.36811168258633" IsNewNode="True">
                <LayoutItem Id="Name" ViewItem="Name" Index="0" RelativeSize="19.51219512195122" IsNewNode="True" />
                <LayoutItem Id="Status" ViewItem="Status" Index="1" RelativeSize="14.634146341463415" IsNewNode="True" />
                <LayoutItem Id="Website" ViewItem="Website" Index="2" RelativeSize="14.634146341463415" IsNewNode="True" />
                <LayoutGroup Id="Auto93dc11cc-6d2f-4ce8-9335-2053748beebe" ShowCaption="False" Caption="Auto93dc11cc-6d2f-4ce8-9335-2053748beebe(9)" Direction="Horizontal" Index="3" RelativeSize="51.21951219512195" IsNewNode="True">
                  <LayoutGroup Id="Auto915490a6-7a8c-493f-92de-bc91b6e75fe1" ShowCaption="False" Caption="Auto915490a6-7a8c-493f-92de-bc91b6e75fe1(10)" Index="0" RelativeSize="52.103960396039604" IsNewNode="True">
                    <LayoutItem Id="HomeOfficeCity" ViewItem="HomeOfficeCity" Index="0" RelativeSize="35.714285714285715" IsNewNode="True" />
                    <LayoutItem Id="BillingAddressLine" ViewItem="BillingAddressLine" Index="1" RelativeSize="28.571428571428573" IsNewNode="True" />
                    <LayoutItem Id="Phone" ViewItem="Phone" Index="2" RelativeSize="35.714285714285715" IsNewNode="True" />
                  </LayoutGroup>
                  <LayoutGroup Id="Auto34418a08-ed4f-49b1-8310-5ddb2fa79bc5" ShowCaption="False" Caption="Auto34418a08-ed4f-49b1-8310-5ddb2fa79bc5(13)" Index="1" RelativeSize="47.896039603960396" IsNewNode="True">
                    <LayoutItem Id="HomeOfficeState" ViewItem="HomeOfficeState" Index="0" RelativeSize="35.714285714285715" IsNewNode="True" />
                    <LayoutItem Id="HomeOfficeZipCode" ViewItem="HomeOfficeZipCode" Index="1" RelativeSize="64.28571428571429" IsNewNode="True" />
                  </LayoutGroup>
                </LayoutGroup>
              </LayoutGroup>
              <LayoutGroup Id="Autof40be654-2023-4379-b99a-98feffe73a67" ShowCaption="False" Caption="Autof40be654-2023-4379-b99a-98feffe73a67(11)" Index="1" RelativeSize="40.63188831741367" Direction="Horizontal" IsNewNode="True">
                <LayoutItem Id="Profile" ViewItem="Profile" Index="0" RelativeSize="67.0886075949367" CaptionLocation="Top" IsNewNode="True" />
                <LayoutItem Id="Logo" ViewItem="Logo" Index="1" RelativeSize="32.91139240506329" ShowCaption="True" CaptionLocation="Top" SizeConstraintsType="Custom" MinSize="182, 54" MaxSize="182, 0" IsNewNode="True" />
              </LayoutGroup>
            </LayoutGroup>
            <LayoutGroup Id="Customer" Index="1" RelativeSize="49.10941475826972" Removed="True">
              <LayoutGroup Id="Customer_col1" RelativeSize="50">
                <LayoutItem Id="BillingAddressCity" RelativeSize="20" Index="0" />
                <LayoutItem Id="BillingAddressZipCode" RelativeSize="15" Index="1" />
                <LayoutItem Id="HomeOfficeZipCode" RelativeSize="8.571428571428571" Index="1" Removed="True" />
                <LayoutItem Id="HomeOfficeLatitude" Index="2" RelativeSize="15" />
                <LayoutItem Id="HomeOfficeLongitude" Index="3" RelativeSize="15" />
                <LayoutItem Id="HomeOfficeState" Index="3" RelativeSize="8.571428571428571" Removed="True" />
                <LayoutItem Id="BillingAddressState" Index="4" RelativeSize="35" />
                <LayoutItem Id="BillingAddressLine" RelativeSize="8.571428571428571" Removed="True" />
                <LayoutItem Id="HomeOfficeCity" RelativeSize="8.571428571428571" Removed="True" />
                <LayoutItem Id="HomeOfficeLine" RelativeSize="11.428571428571429" Removed="True" />
                <LayoutItem Id="Name" Removed="True" />
              </LayoutGroup>
              <LayoutGroup Id="Customer_col2" RelativeSize="50">
                <LayoutItem Id="Fax" RelativeSize="15" Index="2" />
                <LayoutItem Id="AnnualRevenue" RelativeSize="15" Index="3" />
                <LayoutItem Id="Website" RelativeSize="13.043478260869565" Index="3" Removed="True" />
                <LayoutItem Id="TotalStores" RelativeSize="15" Index="4" />
                <LayoutItem Id="TotalEmployees" RelativeSize="20" Index="5" />
                <LayoutItem Id="Profile" RelativeSize="15.384615384615385" Index="7" Removed="True" />
                <LayoutItem Id="Status" RelativeSize="10.344827586206897" Index="7" Removed="True" />
                <LayoutItem Id="Logo" RelativeSize="12.5" Index="9" Removed="True" />
                <LayoutItem Id="BillingAddressLatitude" RelativeSize="20" />
                <LayoutItem Id="BillingAddressLongitude" RelativeSize="15" />
                <LayoutItem Id="Phone" RelativeSize="8.571428571428571" Removed="True" />
              </LayoutGroup>
            </LayoutGroup>
          </LayoutGroup>
          <TabbedGroup Id="Tabs" RelativeSize="84.24591738712776">
            <LayoutGroup Id="Employees" RelativeSize="100">
              <LayoutItem Id="Employees" RelativeSize="100" />
            </LayoutGroup>
            <LayoutGroup Id="Orders" RelativeSize="100">
              <LayoutItem Id="Orders" RelativeSize="100" />
            </LayoutGroup>
            <LayoutGroup Id="Quotes" RelativeSize="100">
              <LayoutItem Id="Quotes" RelativeSize="100" />
            </LayoutGroup>
            <LayoutGroup Id="CustomerStores" RelativeSize="100">
              <LayoutItem Id="CustomerStores" RelativeSize="100" />
            </LayoutGroup>
          </TabbedGroup>
          
        </LayoutGroup>
      </Layout>
    </DetailView>

    ```
17. Please tell me how to continue develop now, what kind of stack trace is this? who can dive in?, thanks god I have my own custom xpand ME that can take me out of this SHIT. At least you should put a note on the stack trace for people to try the standalone editor instead.
   ![image](https://github.com/eXpandFramework/eXpand/assets/159464/8f6679c4-3636-4405-9a9f-648b1e04e2be)
18. Order Listview does not refresh this a native Xaf view, again this is not repro without MiddleTier
    ![image](https://github.com/eXpandFramework/eXpand/assets/159464/c9554a39-6739-4b53-a0c0-2a113b24cf1f)
19. I do not get the XafDefaultProperty when I do customer.ToString but Castle.Proxies.CustomerProxy see LabelPropertyEditor in the demo