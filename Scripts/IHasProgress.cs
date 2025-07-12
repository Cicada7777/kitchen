using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress 
{
    public event EventHandler<OnPorgressChangedEventArgs> OnProgressChanged;
    public class OnPorgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
