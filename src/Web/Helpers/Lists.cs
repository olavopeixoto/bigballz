using System.Collections.Generic;

namespace System.Web.Mvc {
    public static class ListExtensions {

        public static SelectList ToSelectList<T>(this IEnumerable<T> collection,
                             string dataValueField, string dataTextField) {
            return new SelectList(collection, dataValueField, dataTextField);
        }
    }
}
