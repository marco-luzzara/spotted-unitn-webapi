using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.ShopAggregate.ValueObjects
{
    public class ShopCoverPicture
    {
        public byte[] CoverPicture { get; private set; }

        private ShopCoverPicture()
        {
        }

        public ShopCoverPicture(byte[] coverPicture)
        {
            this.CoverPicture = ValidateCoverPicture(coverPicture);
        }

        private static byte[] ValidateCoverPicture(byte[] coverPicture)
        {
            if (coverPicture == null)
                throw ShopException.InvalidCoverPictureException(coverPicture);

            return coverPicture;
        }

        public override bool Equals(object obj)
        {
            ShopCoverPicture coverPictureObj = obj as ShopCoverPicture;
            if (coverPictureObj == null)
                return false;
            else
                return this == coverPictureObj;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.CoverPicture);
        }

        public static bool operator ==(ShopCoverPicture e1, ShopCoverPicture e2)
        {
            return e1.CoverPicture == e2.CoverPicture;
        }

        public static bool operator !=(ShopCoverPicture e1, ShopCoverPicture e2)
        {
            return !(e1 == e2);
        }
    }
}
