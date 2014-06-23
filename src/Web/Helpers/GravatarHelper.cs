using System.Linq;

namespace BigBallz.Helpers
{
    public class GravatarHelper
    {
        /// <summary>
        /// Base URL for the Gravatar image
        /// </summary>
        private const string BaseUrl = "//www.gravatar.com/avatar/{0}?d={1}&s={2}&r={3}";

        #region Properties, very ugly

        public string Email { get; set; }

        public GravatarRating Rating { get; set; }

        public IconSet Iconset { get; set; }

        public GravatarHelper()
        {
            Size = 50;
            Rating = GravatarRating.x;
            Iconset = IconSet.monsterid;
        }

        public int Size { get; set; }

        #endregion

        /// <summary>
        /// Small MD5 Function
        /// </summary>
        /// <param name="theEmail"></param>
        /// <returns>Hash of the email address passed.</returns>
        private string MD5(string theEmail)
        {
            var md5Obj = new System.Security.Cryptography.MD5CryptoServiceProvider();

            var bytesToHash = System.Text.Encoding.ASCII.GetBytes(theEmail);

            bytesToHash = md5Obj.ComputeHash(bytesToHash);

            return bytesToHash.Aggregate("", (current, b) => current + b.ToString("x2"));
        }

        /// <summary>
        /// <a title="Update" href="http://lifeofajackass.com/2008/11/update/">Update</a> the Gravatar anytime an attribute is changed
        /// </summary>
        public string GetGravatarUrl()
        {
            //hash the email address
            var hE = MD5(Email);
            //format our url to the Gravatar
            return string.Format(BaseUrl, hE, Iconset, Size, Rating);
        }
    }

    /// <summary>
    /// Icon Set Property
    /// </summary>
    public enum IconSet
    {
        identicon, monsterid, wavatar
    }

    /// <summary>
    /// Rating Property
    /// </summary>
    public enum GravatarRating
    {
        g, pg, r, x
    }
}