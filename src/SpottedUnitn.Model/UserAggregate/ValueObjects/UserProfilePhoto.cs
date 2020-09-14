using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.UserAggregate.ValueObjects
{
    public class UserProfilePhoto
    {
        public byte[] ProfilePhoto { get; private set; }

        private UserProfilePhoto()
        {
        }

        public UserProfilePhoto(byte[] profilePhoto)
        {
            this.ProfilePhoto = ValidateProfilePhoto(profilePhoto);
        }

        private static byte[] ValidateProfilePhoto(byte[] profilePhoto)
        {
            if ((profilePhoto?.Length ?? 0) == 0)
                throw UserException.InvalidProfilePhotoException(profilePhoto);

            return profilePhoto;
        }

        public override bool Equals(object obj)
        {
            UserProfilePhoto userProfilePhotoObj = obj as UserProfilePhoto;
            if (userProfilePhotoObj == null)
                return false;
            else
                return this == userProfilePhotoObj;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ProfilePhoto);
        }

        public static bool operator ==(UserProfilePhoto e1, UserProfilePhoto e2)
        {
            return e1.ProfilePhoto == e2.ProfilePhoto;
        }

        public static bool operator !=(UserProfilePhoto e1, UserProfilePhoto e2)
        {
            return !(e1 == e2);
        }
    }
}
