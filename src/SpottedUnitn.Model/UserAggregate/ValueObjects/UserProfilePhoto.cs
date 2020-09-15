using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpottedUnitn.Model.UserAggregate.ValueObjects
{
    public class UserProfilePhoto
    {
        private int userId;
        public int UserId => this.userId;

        private byte[] profilePhoto;
        public byte[] ProfilePhoto => this.profilePhoto.ToArray();

        protected UserProfilePhoto()
        {
        }

        public UserProfilePhoto(byte[] profilePhoto)
        {
            this.SetProfilePhoto(profilePhoto);
        }

        public void SetProfilePhoto(byte[] profilePhoto)
        {
            this.profilePhoto = ValidateProfilePhoto(profilePhoto);
        }

        private static byte[] ValidateProfilePhoto(byte[] profilePhoto)
        {
            if ((profilePhoto?.Length ?? 0) == 0)
                throw UserException.InvalidProfilePhotoException(profilePhoto);

            return profilePhoto;
        }
    }
}
