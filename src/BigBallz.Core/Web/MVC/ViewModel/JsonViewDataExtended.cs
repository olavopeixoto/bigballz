namespace BigBallz.Core.Web.MVC
{
    public class JsonViewDataExtended<TModel> : JsonViewData
    {
        public TModel Object
        {
            get;
            set;
        }
    }
}
