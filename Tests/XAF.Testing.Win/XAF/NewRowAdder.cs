using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using XAF.Testing.XAF;

namespace XAF.Testing.Win.XAF{
    public class NewRowAdder : INewRowAdder{
        public void AddNewRowAndCloneMembers(Frame frame, object existingObject) 
            => ((GridListEditor)frame.View.ToListView().Editor).GridView
                .AddNewRow(frame.View.ToCompositeView().CloneExistingObjectMembers(true, existingObject).ToArray());
    }
}