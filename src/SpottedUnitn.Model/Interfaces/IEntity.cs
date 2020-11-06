using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Model.Interfaces
{
    public interface IEntity<T>
    {
        public T Id { get; }
    }
}
