using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

public enum IdentityErrorCode
{
    PasswordTooShort = 1001,
    PasswordRequiresNonAlphanumeric = 1002,
    PasswordRequiresDigit = 1003,
    PasswordRequiresLower = 1004,
    PasswordRequiresUpper = 1005,
    DuplicateUserName = 1006,
    DuplicateEmail = 1007,
    InvalidUserName = 1008,
    InvalidEmail = 1009
}
