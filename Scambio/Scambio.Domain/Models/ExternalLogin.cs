using System;

namespace Scambio.Domain.Models
{
    public class ExternalLogin
    {
        private User _user;

        #region Navigation Properties

        public virtual User User
        {
            get { return _user; }
            set
            {
                _user = value;
                UserId = value.Id;
            }
        }

        #endregion

        #region Scalar Properties

        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual Guid UserId { get; set; }

        #endregion
    }
}