﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth
{
    public interface IUserContext<T>
    {
        public void SetUserContext(T user);
    }
}
