using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T>
{
    T Build();
}
