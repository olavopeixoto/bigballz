namespace BigBallz.Helpers
{
    public class GravatarHelper
    {
        /// <summary>
        /// Base URL for the Gravatar image
        /// </summary>
        private const string BaseUrl = "//www.gravatar.com/avatar/{0}?d={1}&s={2}&r={3}";

        #region Properties, very ugly
        /// <summary>
        /// Email Property
        /// </summary>
        string theEmail;

        public string email
        {
            get { return theEmail; }
            set
            {
                theEmail = value;
            }
        }

        /// <summary>
        /// Rating Property
        /// </summary>
        public enum Rating
        {
            g, pg, r, x
        }

        private Rating theRating;

        public Rating rating
        {
            get { return theRating; }
            set
            {
                theRating = value;
            }
        }

        /// <summary>
        /// Icon Set Property
        /// </summary>
        public enum IconSet
        {
            identicon, monsterid, wavatar
        }

        private IconSet theIconSet;

        public IconSet iconset
        {
            get { return theIconSet; }
            set
            {
                theIconSet = value;
            }
        }

        /// <summary>
        /// Size Property
        /// </summary>
        int theSize = 50;
        public int size
        {
            get { return theSize; }
            set
            {
                theSize = value;
            }
        }
        #endregion

        /// <summary>
        /// Small MD5 Function
        /// </summary>
        /// <param name="theEmail"></param>
        /// <returns>Hash of the email address passed.</returns>
        public string MD5(string theEmail)
        {
            var md5Obj = new System.Security.Cryptography.MD5CryptoServiceProvider();

            var bytesToHash = System.Text.Encoding.ASCII.GetBytes(theEmail);

            bytesToHash = md5Obj.ComputeHash(bytesToHash);

            var strResult = "";

            foreach (var b in bytesToHash)
            {
                strResult += b.ToString("x2");
            }

            return strResult;
        }

        /// <summary>
        /// <a title="Update" href="http://lifeofajackass.com/2008/11/update/">Update</a> the Gravatar anytime an attribute is changed
        /// </summary>
        public string GetGravatarUrl()
        {
            //hash the email address
            string hE = MD5(theEmail);
            //format our url to the Gravatar
            return string.Format(BaseUrl, hE, iconset, size, rating);
        }
    }
}