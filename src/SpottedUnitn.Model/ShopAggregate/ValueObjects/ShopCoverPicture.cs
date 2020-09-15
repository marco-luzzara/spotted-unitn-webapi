using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpottedUnitn.Model.ShopAggregate.ValueObjects
{
    public class ShopCoverPicture
    {
        private int shopId;
        public int ShopId => this.shopId;

        private byte[] coverPicture;
        public byte[] CoverPicture => this.coverPicture.ToArray();

        protected ShopCoverPicture()
        {
        }

        public ShopCoverPicture(byte[] coverPicture)
        {
            this.SetCoverPicture(coverPicture);
        }

        private static byte[] ValidateCoverPicture(byte[] coverPicture)
        {
            if (coverPicture == null)
                throw ShopException.InvalidCoverPictureException(coverPicture);

            return coverPicture;
        }

        public void SetCoverPicture(byte[] coverPicture)
        {
            this.coverPicture = ValidateCoverPicture(coverPicture);
        }
    }
}
