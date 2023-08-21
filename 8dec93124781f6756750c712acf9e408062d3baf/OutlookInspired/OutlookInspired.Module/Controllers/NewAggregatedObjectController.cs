// using DevExpress.ExpressApp;
// using DevExpress.ExpressApp.Editors;
// using DevExpress.ExpressApp.SystemModule;
// using OutlookInspired.Module.Services;
//
// namespace OutlookInspired.Module.Controllers{
//     public class NewAggregatedObjectController:ViewController<ListView>{
//         public NewAggregatedObjectController() => TargetViewNesting=Nesting.Nested;
//
//         protected override void OnActivated(){
//             base.OnActivated();
//             // if (((NestedFrame)Frame).ViewItem is PropertyEditor editor && editor.MemberInfo.IsAggregated){
//             //     Frame.GetController<NewObjectViewController>().ObjectCreated+=OnObjectCreated;    
//             // } 
//             
//         }
//
//         protected override void OnDeactivated(){
//             base.OnDeactivated();
//             Frame.GetController<NewObjectViewController>().ObjectCreated-=OnObjectCreated;
//         }
//
//         private void OnObjectCreated(object sender, ObjectCreatedEventArgs e) 
//             => ((PropertyEditor)((NestedFrame)Frame).ViewItem).MemberInfo
//                 .AssociatedMemberInfo.SetValue(e.CreatedObject,e.ObjectSpace.GetObject(Frame.ParentObject()));
//     }
// }