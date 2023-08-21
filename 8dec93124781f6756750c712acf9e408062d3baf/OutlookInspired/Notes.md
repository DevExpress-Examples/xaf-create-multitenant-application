1. Win platform is simple and people often, including myself, experiment there first. Default it to a client server is not a good idea IMHO and in many app domains  not valuable at all, on the contrary I would say. I would be way happier with the simple version and the wizard to generate additional commented lines for I can switch easier rather than having to compare what it generates for the middle-tier in a new project so to add it back to my simpler version.  I am one step away from switching to the simple version for development and when I feel ready only then I will switch back . In addition to all DI magic, the endless EF configurations, a newcomer has to deal with a client-server architecture, no need to say I feel pain.

   In the end that’s the reason why DI exists, it can make transition from one architecture to another by simply replacing a few services.
u get it all the time.

2. I used to drop the db all the time now I really scared cause
![image](https://github.com/eXpandFramework/eXpand/assets/159464/7a9814ad-1f1c-40fd-858b-745c58e43084)
  We need detailed doc with all possible ways to follow to understand what's wrong. I am fighting this for the hundredth time over 1 hr now.
2. EF migration is another pain, maybe unrelated here but I want to raise my concerns. At least in early application stages changing the model is done constantly. So we need a smart way to migrate the db automatically, even if it has risks to break things, we are developing after all and we can resurrect the patient, so there is no risk IMHO. In addition, in Xaf, production migrations are not allowed by default for either EF or XPO. Your suggestion with the InMemory is not a good option, because I lose my test data and using an Updater to code every kind of BS I experiment is way too time consuming and hard. Also EF core tools not installed. Furthermore InMemory is not recommended see <https://github.com/dotnet/efcore/issues/2166#issuecomment-1291815232>. They recommend testing in production db. They favor SqlLite with drawbacks though, the most important is not details erroring e.g.
SqlLite : Microsoft.Data.Sqlite.SqliteException : SQLite Error 19: 'FOREIGN KEY constraint failed'.
SqlServer: Microsoft.Data.SqlClient.SqlException : The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Employees_Probations_ProbationReasonId".

3. ImportData test needs 12 sec, running the same code in the updater takes 62 sec. You said that this demo is not about migration so we will not include it. In addition you said the performance I face is caused by the bulk insert of too many records and this is not a common scenario and you expect them to use the UI to make the inserts. However, what about those that will use the updater to insert a lot of data or an action? Or what about if you have many clients sharing the same Middle-Tier are these common enough cases?
4. var dbContext = ObjectSpace.ServiceProvider.GetRequiredService<OutlookInspiredEFCoreDbContext>();
        var proxy = dbContext.CreateProxy<ApplicationUser>();
        dbContext.Users.Add(proxy);
        dbContext.SaveChanges();
Will throw object saving prohibited exceptions, Max said I have to use a user with permissions, how? Or to say it better, why? Because I am in Updater, Security is not applied here.
5. The updater is called b4 login which is inconsistent with other Blazor or simple win
6. see this with en empty db I hit F5 and debugging both the client and the server. But because I have a long operation in the updater it gets real messy again. Note that my breakpoint is hit meaning that my long synchronous operation succeeded.

    [devenv_7L2WlFib1N.gif] (<https://devexpress.sharepoint.com/sites/XAF5/Shared> Documents/Squad Security/devenv_7L2WlFib1N.gif)<<https://teams.microsoft.com/l/message/19:35bcf1da713c4dc1955d2dd37af504da@thread.skype/1689811350183?tenantId=e4d60396-9352-4ae8-b84c-e69244584fa4&amp;groupId=d06f3b24-2515-4dd5-8bda-bd37c414c600&amp;parentMessageId=1687879380236&amp;teamName=App> Frameworks (UI, API, ORM)&amp;channelName=!Core-Security-ORM Squad&amp;createdTime=1689811350183&amp;allowXTenantAccess=false>
7. [Docs say](https://docs.devexpress.com/eXpressAppFramework/402958/business-model-design-orm/business-model-design-with-entity-framework-core/relationships-between-entities-in-code-and-ui)

   ```
   public virtual IList<Note> NotesCollection { get; set; } = new ObservableCollection<Note>();
   ```

   I did not use it at all no problems

8. Show toolip when hover over an editor in a headless adv banded grid. See this shot is not clear what the bold value is a tooltip could solve it.
   ![image](https://github.com/eXpandFramework/eXpand/assets/159464/0366098a-3c9b-4a1d-a7fe-545eb3bfed0c)
9. Check out this video, the actioncontainer makes me crazy. The user model is empty. However it only behaves after I reset the non existent differences
  ![OutlookInspired Win_q4gemTAH1u](https://github.com/eXpandFramework/eXpand/assets/159464/562ae83f-da9b-4e04-a9b5-d96eaf343a73)
10. Add something like Mode (Image,ImageAndCaption) to the ImageName attribute to speed development and not need to add extra properties just for this. There is a big cost there (delayed and so on)
11. following <https://docs.devexpress.com/eXpressAppFramework/114159/ui-construction/using-a-custom-control-that-is-not-integrated-by-default/how-to-show-a-custom-data-bound-control-in-an-xaf-view-winforms>

 it says I can use the CollectionSource but ObjectTypeName does not populate neither I can select it as a grid datasorce in the designer

